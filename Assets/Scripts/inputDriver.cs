using UnityEngine;
using System.Collections;

public class inputDriver : MonoBehaviour {

	private string PortChoice = "None";
	private ShipController ship;

	// Use this for initialization
	void Start () {
		if (GameObject.Find("ParamHolder")) {
			ParamHolder paramHolder = GameObject.Find("ParamHolder").GetComponent<ParamHolder>();
			PortChoice = paramHolder.portChoice;
		}
		
		if (PortChoice != "None") {
			Destroy(gameObject.GetComponent<inputDriver>());
		}

		ship = GetComponent<ShipController>();
	}

	void Update () {
		var move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		ship.velocity = move;

		// Waiting for input?
		if (ship.permImmune && Input.anyKey) {
			StartCoroutine(ship.Immunity(2f));
		}
	}
	
}
