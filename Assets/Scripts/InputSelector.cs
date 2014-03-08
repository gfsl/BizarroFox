using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;

public class InputSelector : MonoBehaviour {

	public GameObject InputSelectorButtonPrefab;
	public dfControl ButtonPanel;

	List<string> ports = new List<string>();

	void Start () {
		ports.Add("Keyboard (WASD)");
		ports.Add("Test 1");
		ports.Add("Test 2");
		foreach (var port in SerialPort.GetPortNames()) {
			ports.Add ("Arduino ("+port+")");
		}

		foreach (var port in ports) {
			var btn = ButtonPanel.AddPrefab(InputSelectorButtonPrefab) as dfButton;
			btn.Text = port;
			btn.ButtonGroup = ButtonPanel;
			btn.Click += InputSelected;
			btn.gameObject.name = "Button (" + port + ")";
		}
	}

	void InputSelected(dfControl clicked, dfMouseEventArgs args) {
		Debug.Log ("Clicked " + clicked.name);
	}
}
