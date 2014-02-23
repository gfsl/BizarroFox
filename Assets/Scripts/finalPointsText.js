
function Awake() {
	Timer();
}

function Update () {
}

function Timer() {
	yield WaitForSeconds(5);
	Destroy(gameObject);
}