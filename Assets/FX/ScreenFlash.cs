using UnityEngine;
using System.Collections;

public class ScreenFlash : MonoBehaviour {

	public float delay = 1f;
	public float fadeTime = .5f;
	public Color flashColor = Color.white;

	void Start () {
		var fade = new Texture2D(1,1);
		fade.SetPixel(0,0, Color.white);
		fade.Apply();
		var go = new GameObject();
		go.AddComponent<GUITexture>();
		go.guiTexture.texture = fade;
		go.guiTexture.pixelInset = new Rect(0,0,Screen.width, Screen.height);
		go.guiTexture.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f);
		Destroy (go, delay+fadeTime+1f);

		Go.from (go.guiTexture, fadeTime, new GoTweenConfig()
		       .colorProp("color", flashColor, false)
		       .setDelay(delay)
		       );
	}
}
