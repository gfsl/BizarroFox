var guiTextLPrefab : GUITexture;
var guiTextRPrefab : GUITexture;

private var guiTextL : GUITexture;
private var guiTextR : GUITexture;

function Awake () {
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

	guiTextL = Instantiate(guiTextLPrefab, new Vector3(0, 0.5, 0), Quaternion.identity);
	guiTextR = Instantiate(guiTextRPrefab, new Vector3(1, 0.5, 0), Quaternion.identity);
	guiTextL.enabled = false;
	guiTextR.enabled = false;
}

function Update () {
}

function WarningLeftOn () {
	guiTextL.enabled = true;
}

function WarningLeftOff () {
	guiTextL.enabled = false;
}

function WarningRightOn () {
	guiTextR.enabled = true;
}

function WarningRightOff () {
	guiTextR.enabled = false;
}