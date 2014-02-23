private var PortChoice : String = "None";

function Start () {
	if (GameObject.Find("ParamHolder")) {
		var paramHolder = GameObject.Find("ParamHolder").GetComponent("ParamHolder");
		PortChoice = paramHolder.portChoice;
	}
	
	if (PortChoice != "None") {
		Destroy(gameObject.GetComponent("inputDriver"));
	}
}

function Update () {
	var thisX : float = 0.0;
	var thisY : float = 0.0;

	thisX = Input.GetAxis("Horizontal");
	thisY = Input.GetAxis("Vertical");
	//Debug.Log("X: "+thisX+" Y: "+thisY);

	this.GetComponent("ShipController").SendMessage("inputMovement", new Vector2(thisX, thisY));
}