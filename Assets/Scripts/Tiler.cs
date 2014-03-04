using UnityEngine;
using System.Collections;

public class Tiler : MonoBehaviour {

	public float scrollSpeed = 0.04f;

	void Update () {
		float offset = Time.time * scrollSpeed;
		renderer.material.SetTextureOffset("_MainTex", new Vector2(0, -offset));
		renderer.material.SetTextureOffset("_Illum", new Vector2(0, -offset));
	}
}
