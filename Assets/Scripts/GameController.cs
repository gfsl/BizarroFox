using UnityEngine;
using System.Collections;

public enum InputTypes {
	Keyboard,
	Arduino
}

public class GameController : MonoBehaviour {

	public static GameController Instance;

	public string PortChoice = "None";

	public float xBound;
	public float yMin, yMax;
	public float interval = 8f;

	public ShipController ship;
	public GameObject warningR;
	public GameObject warningL;

	public InputTypes InputType = InputTypes.Keyboard;

	void Awake () {
		if (Instance == null) {
			Instance = this;
		} else {
			Destroy(this);
		}
	}

	void Update () {
		if (Input.GetKey ("escape")) {
			Application.Quit();
		}
	}
}
