using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour {

	public GameObject[] roids;

	private float timer = 0f;
	private float xBound {
		get { return GameController.Instance.xBound; }
		set {}
	}
	private float yMin {
		get { return GameController.Instance.yMin; }
		set {}
	}
	private float yMax {
		get { return GameController.Instance.yMax; }
		set {}
	}

	void Start () {
		for (int x=0;x<20;x++) {
			NewAsteroid(x);
		}

		timer = GameController.Instance.interval;
	}
	
	void Update () {
		timer -= Time.deltaTime;
		
		if (timer < 0) {
			NewAsteroid();
			timer = GameController.Instance.interval;
		}

	}

	void NewAsteroid(float z = 0) {
		GameObject roid = roids[Random.Range(0,roids.Length)];
		var go = Instantiate(roid, transform.position, Quaternion.identity) as GameObject;
		go.transform.Translate(Random.Range(-xBound-1, xBound+1), Random.Range(yMin-1, yMax+1), Random.Range(-10,1)*z);
		go.transform.parent = transform;
	}

}
	