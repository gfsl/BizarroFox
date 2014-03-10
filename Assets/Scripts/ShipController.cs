using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {

	// Movement Variables
	public Vector2 velocity;

	public Vector2 maxTilt;
	public Vector3 cameraTiltFactor;
	public float speed, shipSpeed;

	// Materials
	public Material baseMat;
	public Material immuneMat;
	
	// Death effects and states
	public bool dead = false;
	public bool immune = false;
	public bool permImmune = false;
	public GameObject explosion;
	public ParticleSystem revivefx;
	public ParticleSystem immunefx;
	private Vector3 origin;
	
	// Points
	public int lastLifeBonus = 0;

	public GameObject model;

	// Use this for initialization
	void Start () {
		origin = transform.position;	
		GameController.Instance.ship = this;
		
		StartCoroutine(Immunity());
	}
	
	// Update is called once per frame
	void Update () {

		var tilt = new Vector3();
		tilt.z = Mathf.Lerp (maxTilt.x, -maxTilt.x, (velocity.x+1)/2);
		tilt.x = Mathf.Lerp (maxTilt.y, -maxTilt.y, (velocity.y+1)/2);
		
		transform.eulerAngles = tilt;
		Camera.main.transform.localEulerAngles = 
			new Vector3(tilt.x * cameraTiltFactor.x, tilt.z * cameraTiltFactor.y, tilt.z * cameraTiltFactor.z);

		if (!dead) {
			var movement = new Vector3(velocity.x, velocity.y, 0) * speed * Time.deltaTime;
			transform.Translate(movement, Space.World);

			// Viewport boundaries
			if ((transform.position.x < -GameController.Instance.xBound)||(transform.position.x > GameController.Instance.xBound))
			{
				transform.position = new Vector3(Mathf.Clamp(transform.position.x, -GameController.Instance.xBound, GameController.Instance.xBound),
				                                 transform.position.y, transform.position.z);
			}
			if ((transform.position.y < -GameController.Instance.yMin)||(transform.position.y > GameController.Instance.yMax))
			{
				transform.position = new Vector3(transform.position.x,
				                                 Mathf.Clamp(transform.position.y, GameController.Instance.yMin, GameController.Instance.yMax),
				                                 transform.position.z);
			}

		}

		// Points Adding
		
		if (!immune && !dead) {
			GameController.Instance.points += Time.deltaTime * 25;
		}
		
//		if ((GameController.Instance.points / (5000 * (lastLifeBonus + 1))) > 1) {
//			GameController.Instance.lives++;
//			lastLifeBonus++;
//		}

	}

	IEnumerator Die() {
		dead = true;

		Instantiate(explosion, transform.position, transform.rotation);
		GameController.Instance.lives--;

		var lastPos = transform.position;
		transform.Translate(-1000,-1000,-1000);
		Go.to (Camera.main.transform, 0.5f, new GoTweenConfig()
		       .shake(new Vector3(1, 1, 0), GoShakeType.Position)
		       .shake(new Vector3(10, 10, 10), GoShakeType.Eulers)
		       );

		if (GameController.Instance.lives > 0) {
			var label = GameObject.Find ("Dead text").GetComponent<dfLabel>();
			label.Text = GameController.deadText;
			label.IsVisible = true;
			yield return new WaitForSeconds(2f);
			label.IsVisible = false;
			Instantiate(revivefx, lastPos, Quaternion.identity);
			yield return new WaitForSeconds(.3f);
			transform.position = lastPos;
			transform.rotation = Quaternion.identity;
			StartCoroutine(Immunity(3f));
		} else {
			var label = GameObject.Find ("Final points").GetComponent<dfLabel>();
			label.Text = GameController.Instance.points.ToString("f0");
			label.IsVisible = true;
			GameController.ResetGame();
			lastLifeBonus = 0;
			yield return new WaitForSeconds(5f);
			label.IsVisible = false;
			Instantiate(revivefx, origin, Quaternion.identity);
			yield return new WaitForSeconds(.3f);
			transform.position = origin;
			transform.rotation = Quaternion.identity;
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
			dead = false;
		} else {
			permImmune = true;
			return true;
		}
	}

	void ImmunityOn() {
		immune = true;
		model.renderer.material = immuneMat;
		immunefx.gameObject.SetActive(true);
	}

	void ImmunityOff() {
		immune = false;
		model.renderer.material = baseMat;
		immunefx.gameObject.SetActive(false);
		immunefx.Clear();
	}


	void OnTriggerEnter (Collider other) {
		if (!dead && !immune && other.name == "Rock") {
//			Debug.Break();
			StartCoroutine(Die());
		}
		if (!dead && other.name == "Close") {
//			other.transform.parent.GetComponent<AudioSource>().Play ();
//			if (!immune) BonusPoints(Vector3.Distance(other.transform.position, transform.position));
//			Go.to (Camera.main.transform, 0.66f, new GoTweenConfig()
//			       .shake(new Vector3(.15f, .05f, 0f), GoShakeType.Position)
//			       .shake(new Vector3(0f, 0f, 2.5f), GoShakeType.Eulers)
//			       );
		}
	}
	
//	void BonusPoints(float distance) {
//		var bonusPoints = Mathf.Pow(((5-distance)*4), 2);
//		points += bonusPoints;
//		var temp = Instantiate(pointsParticle, transform.position, Quaternion.identity) as GameObject;
//		temp.GetComponent<TextMesh>().text = "+"+bonusPoints.ToString("f0");
//	}

}
