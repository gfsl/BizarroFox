using UnityEngine;
using System.Collections;

public class keyboardDriver : MonoBehaviour {

	private ShipController ship;

	// Use this for initialization
	void Start () {
		if (GameController.Instance.InputType != InputTypes.Keyboard) {
			Destroy(this);
		}

		ship = GameController.Instance.ship;
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
