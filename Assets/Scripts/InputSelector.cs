using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;

public class InputSelector : MonoBehaviour {

	public GameObject InputSelectorButtonPrefab;
	public dfControl ButtonPanel;

	Dictionary<object,string> inputs = new Dictionary<object,string>();

	void Start () {

		dfButton btn;

		btn = ButtonPanel.AddPrefab(InputSelectorButtonPrefab) as dfButton;
		btn.Text = btn.gameObject.name = "Keyboard (WASD)";
		btn.ButtonGroup = ButtonPanel;
		btn.Click += InputSelected;
		inputs.Add(btn, "Keyboard");

		foreach (var port in SerialPort.GetPortNames()) {
			btn = ButtonPanel.AddPrefab(InputSelectorButtonPrefab) as dfButton;
			btn.Text = btn.gameObject.name = "Arduino ("+port+")";
			btn.ButtonGroup = ButtonPanel;
			btn.Click += InputSelected;
			inputs.Add(btn, port);
		}
	}

	void InputSelected(dfControl clicked, dfMouseEventArgs args) {
		Debug.Log ("Selected " + inputs[clicked]);

		if (inputs[clicked] == "Keyboard") {
			GameController.Instance.InputType = InputTypes.Keyboard;
		} else {
			GameController.Instance.InputType = InputTypes.Arduino;
			GameController.Instance.PortChoice = inputs[clicked];
		}

		GameController.StartGame();
	}
}
