using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;

public class arduinoDriver : MonoBehaviour {

	SerialPort stream = null;
	ParamHolder paramHolder;
	string PortChoice = "None";

	public Vector2 movement = new Vector2(32, 32);
	public int badFramesLeft = 0;
	public int badFramesRight = 0;
	private List<Vector2> velocities;
	private ShipController ship;

	void Start () {
		ship = GetComponent<ShipController>();
		velocities = new List<Vector2>();

		if (GameObject.Find("ParamHolder")) {
			paramHolder = GameObject.Find("ParamHolder").GetComponent<ParamHolder>() as ParamHolder;
			PortChoice = paramHolder.portChoice;
		}
		
		if (PortChoice == "None") {
			Destroy(GetComponent<arduinoDriver>());
		} else {		
			Debug.Log("Opening port "+PortChoice);
			//Set the port and the baud rate (9600, is standard on most devices)
			stream = new SerialPort(PortChoice, 9600);
			//stream.ReadBufferSize = 2;
	        //stream.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);		
			stream.ReadTimeout = 2;
			stream.Open(); //Open the Serial Stream.
		}
	}

	// Update is called once per frame
	void Update () {
		bool _continue = true;
		
		if (stream != null)	
		{
			while (_continue)
			{
				try {
					int thebyte = stream.ReadByte();
					if (thebyte >= 128)
					{
						thebyte -= 128;
						movement.y = thebyte;
					}
					else
					{
						movement.x = thebyte;
					}
				}
				catch (TimeoutException) {
					_continue = false;
				}
				
			}

			if (movement.x != 127) {
				badFramesRight = 0;
			} else {
				movement.x = 0f;
				badFramesRight++;
			}	
			if (movement.y != 127) {
				badFramesLeft = 0;
			} else {
				movement.y = 0f;
				badFramesLeft++;
			}
			// Waiting for input?
			if (ship.permImmune && badFramesLeft == 0 && badFramesRight == 0) {
				StartCoroutine(ship.Immunity(2f));
			}

			float moveX = Mathf.Clamp(movement.x, 0, 64);
			float moveY = Mathf.Clamp(movement.y, 0, 64);
			
			var thisVelocity = new Vector2();
			
			thisVelocity.x = ((moveY - 32) - (moveX - 32)) / 32;
			thisVelocity.y = ((moveX - 32) + (moveY - 32)) / 32;
			
			if (velocities.Count >= 10)
			{
				velocities.RemoveAt(0);
			}
			velocities.Add(thisVelocity);
			
			var tempVel = Vector2.zero;
			
			for (var i = 0; i < velocities.Count; i++) {
				tempVel += velocities[i];
			}
			var velocity = tempVel / velocities.Count;


			ship.velocity = velocity;

			/*
			string value = stream.ReadLine(); //Read the information
			string[] vec2 = value.Split(','); //My arduino script returns a 3 part value (IE: 12,30,18)
			if(vec2[0] != "" && vec2[1] != "") //Check if all values are recieved
			{
				if (float.Parse(vec2[0]) != 127) {movement.x = float.Parse(vec2[0]);}
				if (float.Parse(vec2[1]) != 127) {movement.y = float.Parse(vec2[1]);}
				this.GetComponent("ShipController").SendMessage("Movement", movement);
				
				stream.BaseStream.Flush(); //Clear the serial information so we assure we get new information.
			}
			*/
		}	
	}
}
