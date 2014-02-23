using UnityEngine;
using System;
using System.Collections;
using System.IO.Ports;

public class arduinoDriver : MonoBehaviour {

	SerialPort stream = null;
	Vector2 movement = new Vector2(32, 32);
	ParamHolder paramHolder;
	string PortChoice = "None";
	
	void Start () {
		if (GameObject.Find("ParamHolder")) {
			paramHolder = GameObject.Find("ParamHolder").GetComponent("ParamHolder") as ParamHolder;
			PortChoice = paramHolder.portChoice;
		}
		
		if (PortChoice == "None") {
			Destroy(gameObject.GetComponent("arduinoDriver"));
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
			this.GetComponent("ShipController").SendMessage("arduinoMovement", movement);
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
