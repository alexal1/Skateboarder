using UnityEngine;
using Object = UnityEngine.Object;

public class BoundedCameraInterpolator : CameraInterpolator {
	
	private static GameObject FindMiddleground() {
		foreach (var gameObject in Object.FindObjectsOfType<GameObject>()) {
			if (1 << gameObject.layer == Config.MiddlegroundLayerMask) {
				return gameObject;
			}
		}
		Debug.Log("Cannot find Middleground!");
		return null;
	}

	private float _maxOffsetY = Mathf.Infinity;

	public BoundedCameraInterpolator() {
		var middleground = FindMiddleground();
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