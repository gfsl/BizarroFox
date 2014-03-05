using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

//	public ShipController ship;
	public float minSize = 0.6f;
	public float maxSize = 1.1f;
	public Vector3 rot;
	public float rotSpeed;
	public float movSpeed;

	// Use this for initialization
	void Start () {
		transform.localScale *= Random.Range(minSize, maxSize);
		transform.Rotate(Random.Range(0, 359), Random.Range(0, 359), Random.Range(0, 359));

		rotSpeed = Random.value / 4;
		movSpeed = 1+(Random.value/2);
		rot = new Vector3();

		if (Random.value > 0.5f) {
			rot.x = 1;
		} else {
			rot.x = -1;
		}
		if (Random.value > 0.5f) {
			rot.y = 1;
		} else {
			rot.y = -1;
		}
		if (Random.value > 0.5f) {
			rot.z = 1;
		} else {
			rot.z = -1;
		}

		rot *= rotSpeed;
	
	}
	
	// Update is called once per frame
	void Update () {
//		float speed = ship.speed * movSpeed;
		float speed = 10f * movSpeed;

		if (transform.position.z < -15) {
			Destroy(gameObject);
		}	
		transform.Translate(0,0, -speed * Time.deltaTime, Space.World);
		transform.Rotate(rot);
	}

}
