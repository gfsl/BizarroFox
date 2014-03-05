using UnityEngine;
using System.Collections;

public class CameraProximityShake : MonoBehaviour {

	public float time = 0.66f;
	public Vector3 shakePosition;
	public Vector3 shakeEulers;

	private AbstractGoTween _tween;

	void OnTriggerEnter (Collider other) {
		if (other.name == "Close") {
			Shake();
		}
	}
	
	void Shake() {

		if( _tween != null )
		{
			_tween.complete();
			_tween.destroy();
			_tween = null;
		}
		_tween = Go.to (Camera.main.transform, time, new GoTweenConfig()
//		                .shake(shakePosition, GoShakeType.Position)
			       .shake(shakeEulers, GoShakeType.Eulers)
			       );

	}
}
