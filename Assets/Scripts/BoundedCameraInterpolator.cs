using UnityEngine;

public class BoundedCameraInterpolator : CameraInterpolator {

	private float _maxOffsetY = Mathf.Infinity;

	public BoundedCameraInterpolator() {
		var middleground = GameObject.Find("Middleground");
		if (middleground == null) {
			Debug.Log("Cannot find Middleground!");
			return;
		}
		var middlegroundSprite = middleground.GetComponent<SpriteRenderer>();
		var middlegroundTransform = middleground.transform;
		var availableHalfHeight = middlegroundSprite.size.y * middlegroundTransform.localScale.y / 2f;
		if (Camera.main == null) {
			Debug.Log("Camera is null!");
			return;
		}
		var cameraHalfHeight = Camera.main.orthographicSize;
		_maxOffsetY = availableHalfHeight - cameraHalfHeight;
	}
	
	public override float GetCameraY(float groundY) {
		var cameraY = base.GetCameraY(groundY);
		if (cameraY > _maxOffsetY) {
			cameraY = _maxOffsetY;
		}
		else if (cameraY < -_maxOffsetY) {
			cameraY = -_maxOffsetY;
		}
		return cameraY;
	}
	
}