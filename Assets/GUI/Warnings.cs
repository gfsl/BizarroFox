using UnityEngine;
using System.Collections;

public class Warnings : MonoBehaviour {

	public GUITexture guiTextLPrefab;
	public GUITexture guiTextRPrefab;
	
	private GUITexture guiTextL;
	private GUITexture guiTextR;

	// Use this for initialization
	void Start () {
		/*
		guiTextL = new GUITexture();
		guiTextL.transform.position = new Vector3(0, 0.5, 0);
		guiTextL.texture = textureL;
		guiTextL.pixelInset = new Rect(0, -256, 512, 512);

		guiTextR = new GUITexture();
		guiTextR.transform.position = new Vector3(1, 0.5, 0);
		guiTextR.texture = textureR;
		guiTextR.pixelInset = new Rect(-512, -256, 512, 512);
		*/
		
		guiTextL = Instantiate(guiTextLPrefab, new Vector3(0f, 0.5f, 0f), Quaternion.identity) as GUITexture;
		guiTextR = Instantiate(guiTextRPrefab, new Vector3(1f, 0.5f, 0f), Quaternion.identity) as GUITexture;
		guiTextL.enabled = false;
		guiTextR.enabled = false;
	}

	void WarningLeftOn () {
		guiTextL.enabled = true;
	}
	
	void WarningLeftOff () {
		guiTextL.enabled = false;
	}
	
	void WarningRightOn () {
		guiTextR.enabled = true;
	}
	
	void WarningRightOff () {
		guiTextR.enabled = false;
	}
}
