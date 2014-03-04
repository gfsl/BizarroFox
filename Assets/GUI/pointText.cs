using UnityEngine;
using System.Collections;

public class pointText : MonoBehaviour {

	void Update () {
		transform.Translate(0, 3 * Time.deltaTime, 0);
		if (transform.position.y > 18) {
			Destroy(gameObject);
		}	
	}
}
