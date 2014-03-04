using UnityEngine;
using System.Collections;

public class ParamHolder : MonoBehaviour {
	public string portChoice = "";

	void SetChoice(string choice) {
		portChoice = choice;
	}
	
	string GetChoice() {
		return portChoice;
	}
}
