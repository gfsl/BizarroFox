// Movement Variables
private var movement : Vector2;
private var velocities : Array = new Array();
var velocity : Vector2;
var maxTilt : float = 100.0;
var speed : float = 10.0;
var shipSpeed : float = 5.0;
private var baseSpeed : float;
private var badFramesLeft : int = 0;
private var badFramesRight : int = 0;

// Viewport boundaries
var minY : float = 100.0;
var maxY : float = 100.0;
var maxX : float = 100.0;

// Spawning controls
var spawnZ : float = 85;
private var spawner : GameObject;
var roid: GameObject;
var interval : float = 10.0;
private var timer : float = -1;
private var baseInterval : float;

// Death effects and states
var dead : boolean = false;
var immune : boolean = false;
var explosion : GameObject;
var revive : GameObject;
var lives : int = 3;
private var Origin : Vector3;

// Boosting variables
//var cooldown : float = 0.0;
//var boosting : boolean = false;
//var speedUp : boolean = true;
//private var boostSpeed : float = 50.0;

// Points
var points : float = 0.0;
private var nextIncrease : float = 0.0;
var pointsParticle : GameObject;
var finalPointsParticle : GameObject;
var fireworksParticle : GameObject;
private var lastLifeBonus : int = 0;

// GUI Controls and Variables
var PointsSkin : GUISkin;
var TextSkin : GUISkin;
private var deadText : String = "";
var deadTexts : Array = new Array('DEDDED (>x<!)', 'OH NOES (/_\\)', 'AAARGH (ò_ó)', 'F#$@#! (¬_¬")');


function Awake () {
	Debug.Log("I started");
	if (GameObject.Find("ParamHolder")) {
		var paramHolder = GameObject.Find("ParamHolder").GetComponent("ParamHolder");
		if (paramHolder.portChoice != "None") {
			gameObject.AddComponent("arduinoDriver");
		} else {
			gameObject.AddComponent("inputDriver");
		}
	} else {
		gameObject.AddComponent("inputDriver");
	}

	// Initial spawns
	spawner = new GameObject();
	spawner.name = "Asteroids";
	spawner.transform.Translate(0,0,spawnZ);
	for (x=0; x<20; x++) {
		var newguy = Instantiate(roid, spawner.transform.position, Quaternion.identity);
		newguy.GetComponent("Asteroid").ship = this;
		newguy.transform.Translate(Random.Range(-maxX-1, maxX+1), Random.Range(-minY-1, maxY+1), Random.Range(-10,1)*x);
		newguy.transform.parent = spawner.transform;
	}
	
	baseSpeed = speed;
	baseInterval = interval;

	Origin = transform.position;	
	
	Remove(2);	
	
}

function Update () {

	// Movement
	
	if (!dead) {
		var thisX : float = velocity.x * speed * Time.deltaTime;
		var thisY : float = velocity.y * speed * Time.deltaTime;
				
		//Debug.Log("asdf X: "+thisX+" Y: "+thisY);
		
		if (thisX > 0) {
			transform.rotation.eulerAngles.z = 360-thisX*maxTilt;
		} else {		
			transform.rotation.eulerAngles.z = -thisX*maxTilt;
		}
		if (thisY > 0) {
			transform.rotation.eulerAngles.x = 360-thisY*maxTilt;
		} else {		
			transform.rotation.eulerAngles.x = -thisY*maxTilt;
		}
		
		/* 
		// Boosting code, doesn't work.
		
		if (cooldown > 0) {
			cooldown -= Time.deltaTime;
		}
		
		if (Input.GetButtonDown("Jump") && (cooldown <= 0)) {
			boosting = true;
			speedUp = true;
			cooldown = 10.0;
		}
		
		if (boosting) {
			DoBoost();
		}
		*/
		
		transform.Translate(thisX, thisY, 0, Space.World);
		
		// Viewport boundaries
		transform.position.x = Mathf.Clamp(transform.position.x, -maxX, maxX);
		transform.position.y = Mathf.Clamp(transform.position.y, minY, maxY);
	}

	// Asteroid Spawning
	
	timer -= (Time.deltaTime + Random.value);
	
	if (timer < 0) {
		var newguy = Instantiate(roid, spawner.transform.position, Quaternion.identity);
		newguy.GetComponent("Asteroid").ship = this;
		newguy.transform.Translate(Random.Range(-maxX-1, maxX+1), Random.Range(-minY-1, maxY+1), 0);
		newguy.transform.parent = spawner.transform;
		timer = interval;
	}
	
	// Points Adding

	if (!immune) {
		points += Time.deltaTime * 25;
	}
	
	if ((points / (5000 * (lastLifeBonus + 1))) > 1) {
		lives++;
		lastLifeBonus++;
	}

	if (points > nextIncrease && interval > 1 && interval != 10) {
		interval -= 1;
		nextIncrease += 1000;
		/*
		for (var i=0; i<(10-interval); i++) {
			var temp = Instantiate(fireworksParticle, spawner.transform.position, Quaternion.identity);
			if (i%2) {
				temp.transform.Translate(i*30, -3, -60);
			} else {
				temp.transform.Translate(-i*30, -3, -60);
			}
		}
		*/
	}
}


function OnTriggerEnter (other : Collider) {
	if (!dead && !immune && other.name == "Death") {
		Die();
	}
	if (!dead && !immune && other.name == "Bonus") {
		BonusPoints(Vector3.Distance(other.transform.position, transform.position));
	}
}

function BonusPoints(distance : float) {
	var bonusPoints = Mathf.Pow(((5-distance)*4), 2);
	//Debug.Log("BONUS! "+bonusPoints+" ("+distance+")");
	points += bonusPoints;
	var temp = Instantiate(pointsParticle, transform.position, Quaternion.identity);
	temp.GetComponent("TextMesh").text = "+"+bonusPoints.ToString("f0");
	//temp.transform.parent = transform;
	//temp.transform.Translate(Vector3(0,0,-1));
}

function Die() {
	deadText = deadTexts[Random.Range(0, deadTexts.length-1)];
	dead = true;
	Instantiate(explosion, transform.position, transform.rotation);
	lives--;
	Debug.Log("Lives: "+lives);
	if (lives <= 0) {
		var temp = Instantiate(finalPointsParticle, Origin, Quaternion.identity);
		temp.GetComponent("TextMesh").text = points.ToString("f0");
		points = 0.0;
		lastLifeBonus = 0;
		nextIncrease = 0;
		lives = 3;
		Remove(8);
	} else {
		Remove(2);
	}
}

function Remove(time : int) {
	immune = true;
	transform.Translate(-1000,-1000,-1000);

	yield WaitForSeconds(time);

	var particle = Instantiate(revive, Origin, transform.rotation);
	dead = false;
	
	yield WaitForSeconds(0.2);
	transform.position = Origin;
	transform.rotation = Quaternion.identity;
	particle.transform.position = Origin;
	particle.transform.parent = transform;
	interval = baseInterval;
	
	yield WaitForSeconds(2.3);
	Destroy(particle);
	immune = false;
}

/*
// Speed boost code, doesn't work.
function DoBoost() {
		if (speedUp) {
			speed = Mathf.Lerp(baseSpeed, boostSpeed, Time.time*0.1);
		} else {
			speed = Mathf.Lerp(boostSpeed, baseSpeed, Time.time*0.4);
			if (speed == baseSpeed) {
				boosting = false;
			}
		}
		
		if (speed == boostSpeed) {
			speedUp = false;
		}
}
*/

function inputMovement (thisMove:Vector2) {
	velocity = thisMove;

	/*
	Debug.Log("X: "+thisMove.x+" Y: "+thisMove.y);
	if (thisMove.x != -1.0) {
		BroadcastMessage("WarningLeftOff");
	} else {
		BroadcastMessage("WarningLeftOn");
	}
	if (thisMove.x != 1.0) {
		BroadcastMessage("WarningRightOff");
	} else {
		BroadcastMessage("WarningRightOn");
	}
	*/
}

function arduinoMovement (thisMove:Vector2) {
	var moveX : float = 0.0;
	var moveY : float = 0.0;

	if (thisMove.x != 127) {
		movement.x = thisMove.x;
		badFramesRight = 0;
		BroadcastMessage("WarningRightOff");
	} else {
		badFramesRight++;
		if (badFramesRight > 12) { BroadcastMessage("WarningRightOn"); }
	}	
	if (thisMove.y != 127) {
		movement.y = thisMove.y;
		badFramesLeft = 0;
		BroadcastMessage("WarningLeftOff");
	} else {
		badFramesLeft++;
		if (badFramesLeft > 12) {  BroadcastMessage("WarningLeftOn"); }
	}
	
	moveX = Mathf.Clamp(movement.x, 0, 64);
	moveY = Mathf.Clamp(movement.y, 0, 64);
	
	var thisVelocity : Vector2;
	
	thisVelocity.x = ((moveY - 32) - (moveX - 32)) / 32;
	thisVelocity.y = ((moveX - 32) + (moveY - 32)) / 32;
	
	if (velocities.length >= 10)
	{
		velocities.shift();
	}
	velocities.push(thisVelocity);

	var tempVel : Vector2 = Vector2.zero;
	
	for (var i = 0; i < velocities.length; i++) {
		tempVel += velocities[i];
	}
	velocity = tempVel / velocities.length;
}

function OnGUI() {
	GUI.skin = PointsSkin;
	GUI.Label(Rect(Screen.width-100, 10, 80, 45), points.ToString("f0"));
	GUI.Label(Rect(15, 10, 80, 45), ""+lives);

	GUI.skin = TextSkin;
	if (dead) {
		GUI.Label(Rect(Screen.width/2-100, 10, 200, 45), deadText);
	}
}