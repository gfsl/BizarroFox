using UnityEngine;
using System.Collections;

public class ParamHolder : MonoBehaviour {
	public string portChoice = "";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SetChoice(string choice) {
		portChoice = choice;
	}
	
	string GetChoice() {
		return portChoice;
	}
}
