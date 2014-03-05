using UnityEngine;
using System.Collections;

public class CameraTilt : MonoBehaviour {

	private Transform ship;
	public float factor = 0.1f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (GameController.Instance.ship != null) {
			ship = GameController.Instance.ship.transform;

			var tilt = new Vector3();
			if (ship.eulerAngles.z < 180) {
				tilt.z = ship.eulerAngles.z * factor;
			} else {
				tilt.z = 360 - ((360 - ship.eulerAngles.z) * factor);
			}
			transform.eulerAngles = tilt;
		} else {
			transform.eulerAngles = Vector3.zero;
		}
	}
}
