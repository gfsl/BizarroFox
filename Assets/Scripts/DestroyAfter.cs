using UnityEngine;
using System.Collections;

public class DestroyAfter : MonoBehaviour {

	public float time = 2f;

	void Start () {
		Destroy (gameObject, time);
	}
}
