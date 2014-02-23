
function Awake() {
	Timer();
}

function Update () {
}

function Timer() {
	yield WaitForSeconds(2);
	Destroy(gameObject);
}