using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {

	// Movement Variables
	private Vector2 _velocity;
	public Vector2 velocity {
		get { return _velocity; }
		set {
			_velocity = value;
	    }
	}

	public Vector2 maxTilt;
	public float speed, shipSpeed;

	// Materials
	public Material baseMat;
	public Material immuneMat;
	
	// Death effects and states
	public bool dead = false;
	public bool immune = false;
	public bool permImmune = false;
	public GameObject explosion;
	public ParticleSystem revive;
	public int lives = 3;
	private Vector3 origin;
	
	// Points
	public float points = 0f;
	public int lastLifeBonus = 0;
	public GameObject pointsParticle;
	public GameObject finalPointsParticle;

	// GUI Controls and Variables
	public GUISkin PointsSkin;
	public GUISkin TextSkin;
	private string deadText = "";
	public string[] deadTexts;
	
	public GameObject model;

	// Use this for initialization
	void Start () {

		if (GameObject.Find("ParamHolder")) {
			var paramHolder = GameObject.Find("ParamHolder").GetComponent<ParamHolder>();
			if (paramHolder.portChoice != "None") {
				gameObject.AddComponent<arduinoDriver>();
			} else {
				gameObject.AddComponent<inputDriver>();
			}
		} else {
			gameObject.AddComponent<inputDriver>();
		}
		
		origin = transform.position;	
		GameController.Instance.ship = this;
		
		StartCoroutine(Immunity());
	}
	
	// Update is called once per frame
	void Update () {

		if (!dead) {
			var movement = new Vector3(velocity.x, velocity.y, 0) * speed * Time.deltaTime;
			transform.Translate(movement, Space.World);

			var tilt = new Vector3();
			if (velocity.x > 0) {
				tilt.z = 360-(velocity.x*maxTilt.x);
			} else {		
				tilt.z = -(velocity.x*maxTilt.x);
			}
			if (velocity.y > 0) {
				tilt.x = 360-velocity.y*maxTilt.y;
			} else {		
				tilt.x = -velocity.y*maxTilt.y;
			}
			transform.eulerAngles = tilt;
			

			// Viewport boundaries
			if ((transform.position.x < -GameController.Instance.xBound)||(transform.position.x > GameController.Instance.xBound))
			{
				transform.position = new Vector3(Mathf.Clamp(transform.position.x, -GameController.Instance.xBound, GameController.Instance.xBound),
				                                 transform.position.y, transform.position.z);
			}
			if ((transform.position.y < -GameController.Instance.yMin)||(transform.position.x > GameController.Instance.yMax))
			{
				transform.position = new Vector3(transform.position.x,
				                                 Mathf.Clamp(transform.position.y, GameController.Instance.yMin, GameController.Instance.yMax),
				                                 transform.position.z);
			}

		}

		// Points Adding
		
		if (!immune && !dead) {
			points += Time.deltaTime * 25;
		}
		
		if ((points / (5000 * (lastLifeBonus + 1))) > 1) {
			lives++;
			lastLifeBonus++;
		}

	}

	IEnumerator Die() {
		dead = true;

		deadText = deadTexts[Random.Range(0, deadTexts.Length-1)];
		Instantiate(explosion, transform.position, transform.rotation);
		lives--;

		transform.Translate(-1000,-1000,-1000);

		if (lives > 0) {
			yield return new WaitForSeconds(2f);
			transform.position = origin;
			transform.rotation = Quaternion.identity;
			StartCoroutine(Immunity(3f));
		} else {
			var temp = Instantiate(finalPointsParticle, origin, Quaternion.identity) as GameObject;
			temp.GetComponent<TextMesh>().text = points.ToString("f0");
			points = 0f;
			lastLifeBonus = 0;
			lives = 3;
			StartCoroutine(Immunity());
		}
		dead = false;
	}

	public IEnumerator Immunity(float time = 0f) {
		if (!immune) ImmunityOn();

		if (time > 0f) {
			permImmune = false;
			yield return new WaitForSeconds(time);
			if (immune) ImmunityOff();
		} else {
			permImmune = true;
			return true;
		}
	}

	void ImmunityOn() {
		immune = true;
		model.renderer.material = immuneMat;
		revive.gameObject.SetActive(true);
	}

	void ImmunityOff() {
		immune = false;
		model.renderer.material = baseMat;
		revive.gameObject.SetActive(false);
		revive.Clear();
	}


	void OnTriggerEnter (Collider other) {
		if (!dead && !immune && other.name == "Death") {
			StartCoroutine(Die());
		}
		if (!dead && !immune && other.name == "Close") {
			BonusPoints(Vector3.Distance(other.transform.position, transform.position));
		}
	}
	
	void BonusPoints(float distance) {
		var bonusPoints = Mathf.Pow(((5-distance)*4), 2);
		points += bonusPoints;
		var temp = Instantiate(pointsParticle, transform.position, Quaternion.identity) as GameObject;
		temp.GetComponent<TextMesh>().text = "+"+bonusPoints.ToString("f0");
	}

	void OnGUI() {
		GUI.skin = PointsSkin;
		GUI.Label(new Rect(Screen.width-100, 10, 80, 45), points.ToString("f0"));
		GUI.Label(new Rect(15, 10, 80, 45), ""+lives.ToString());
		
		GUI.skin = TextSkin;
		if (dead) {
			GUI.Label(new Rect(Screen.width/2-100, 10, 200, 45), deadText);
		}
	}
}
