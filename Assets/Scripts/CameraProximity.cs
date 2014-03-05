using UnityEngine;
using System.Collections;

public class CameraProximity : MonoBehaviour {

	public float time = 0.66f;
	public Vector3 shakePosition;
	public Vector3 shakeEulers;

	private AbstractGoTween _tween;

	void OnTriggerEnter (Collider other) {
		if (other.name == "Close") {
			Shake();
//			other.transform.parent.GetComponent<AudioSource>().Play ();
		}
	}
	
	void Shake() {

		if( _tween != null )
		{
			_tween.complete();
			_tween.destroy();
			_tween = null;
		}
		_tween = Go.to (Camera.main.transform.parent, time, new GoTweenConfig()
                .shake(shakePosition, GoShakeType.Position)
			       .shake(shakeEulers, GoShakeType.Eulers)
			       );

	}
}
