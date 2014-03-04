using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public static GameController Instance;

	public float xBound;
	public float yMin, yMax;
	public float interval = 8f;

	public ShipController ship;

	void Awake () {
		if (Instance == null) {
			Instance = this;
		} else {
			Destroy(this);
		}
	}

}
