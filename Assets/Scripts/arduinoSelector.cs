using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class arduinoSelector : MonoBehaviour {

	ArrayList ports = new ArrayList();
	string[] portsList;
	int curChoice = 0;
	public GameObject paramHolderPrefab;
	public GUISkin MyGUISkin;
	public Texture2D Logo;
	
	public GameObject shipPrefab;
	
	public int LogoSize = 600;
	
	// Use this for initialization
	void Start () {
		ports.Add("None (use WASD)");
		//ports.Add("COM1");
		//ports.Add("COM2");
		//ports.Add("COM3");
        ports.AddRange(SerialPort.GetPortNames());

		portsList = (string[])ports.ToArray( typeof( string ) );
		
		/*
		foreach(string port in ports) {
			Debug.Log("Ports: "+port);
		}
		*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI () {
		GUI.skin = MyGUISkin;
		
		GUI.Box(new Rect(Screen.width/2-LogoSize/2, Screen.height/20*13, LogoSize, LogoSize/2), Logo);
		
		GUI.Label(new Rect(Screen.width/2-75, 75, 100, 20), "Arduino port:");

		int height = ports.Count * 40;
		curChoice = GUI.SelectionGrid(new Rect(Screen.width/2-75, 100, 150, height), curChoice, portsList, 1);

		if (GUI.Button(new Rect(Screen.width/2-50, height+120, 100, 35), "START!")) {
			StartGame();
		}
	}
	
	void StartGame () {
		GameObject paramHolder = Instantiate(paramHolderPrefab, transform.position, transform.rotation) as GameObject;
		paramHolder.name = "ParamHolder";
		if (curChoice == 0) {
			paramHolder.SendMessage("SetChoice", "None");
		} else {
			paramHolder.SendMessage("SetChoice", ports[curChoice]);
		}
		Instantiate(shipPrefab, new Vector3(0.0f, -3.0f, -2.8f), Quaternion.identity);
		Destroy(gameObject.GetComponent("arduinoSelector"));
		//Object.DontDestroyOnLoad(paramHolder);
		//Object.DontDestroyOnLoad(GameObject.Find("Plane"));
		//Application.LoadLevel("Level");
	}
}
