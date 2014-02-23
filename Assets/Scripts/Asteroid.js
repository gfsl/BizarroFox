var ship : ShipController;
var minSize : float = 0.6;
var maxSize : float = 1.1;
private var rotX : int;
private var rotY : int;
private var rotZ : int;
private var rotSpeed : float;

function Awake () {
	transform.localScale *= Random.Range(minSize, maxSize);
	transform.Rotate(Random.Range(0, 359), Random.Range(0, 359), Random.Range(0, 359));
	rotSpeed = Random.value;
	if (Random.value > 0.5) {
		rotX = 1;
	} else {
		rotX = -1;
	}
	if (Random.value > 0.5) {
		rotY = 1;
	} else {
		rotY = -1;
	}
	if (Random.value > 0.5) {
		rotZ = 1;
	} else {
		rotZ = -1;
	}
}

function Update () {
	var speed : float = ship.speed * 2;

	if (transform.position.z < -15) {
		Destroy(gameObject);
	}	
	transform.Translate(0,0, -speed * Time.deltaTime, Space.World);
	transform.Rotate(rotSpeed*rotX, rotSpeed*rotY, rotSpeed*rotZ);
}