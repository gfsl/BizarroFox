using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public static GameController Instance;

	public string PortChoice = "None";

	public float xBound;
	public float yMin, yMax;
	public float interval = 8f;

	public ShipController ship;
	public GameObject shipPrefab;
	public GameObject warningR;
	public GameObject warningL;

	public InputTypes InputType = InputTypes.Keyboard;
	public GameStates GameState = GameStates.TitleScreen;

	public dfPanel TitleScreen;
	public dfPanel GameScreen;

	public static string deadText {
		get {
			return Instance.deadTexts[Random.Range(0, Instance.deadTexts.Length-1)];
		}
	}
	public string[] deadTexts;

	public float points = 0f;
	public int lives = 3;

	void Awake () {
		if (Instance == null) {
			Instance = this;
		} else {
			Destroy(this);
		}
	}

	void Update () {
		if (Input.GetKeyDown ("escape")) {
			if (GameState == GameStates.TitleScreen) {
				Debug.Log ("Quit");
				Application.Quit();
			} else {
				Destroy (ship.gameObject);
				ResetGame();
				Instance.TitleScreen.IsVisible = true;
				Instance.GameScreen.IsVisible = false;
				GameState = GameStates.TitleScreen;
			}
		}
	}

	public static void StartGame() {
		if (!Application.isEditor) Screen.lockCursor = true;

		var _ship = Instantiate(Instance.shipPrefab, new Vector3(0.0f, -3.0f, -2.8f), Quaternion.identity) as GameObject;
		Instance.ship = _ship.GetComponent<ShipController>();
		switch (Instance.InputType)
		{
		case InputTypes.Arduino:
			_ship.AddComponent<arduinoDriver>();
			break;
		case InputTypes.Keyboard:
		default:
			_ship.AddComponent<keyboardDriver>();
			break;
		}
		Instance.TitleScreen.IsVisible = false;
		Instance.GameScreen.IsVisible = true;
		Instance.GameState = GameStates.Playing;
	}

	public static void ResetGame() {
		Instance.lives = 3;
		Instance.points = 0f;
	}

}
