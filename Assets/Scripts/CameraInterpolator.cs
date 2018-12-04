using UnityEngine;

public class CameraInterpolator {

	private const float Tolerance = 0.1f;
	private const float CameraOffset = 2.5f;
	private const float N = 20f;
	private float _groundY;
	private float _cameraY;
	private float _step;

	public float GetCameraY(float groundY) {
		var groundDelta = Mathf.Abs(groundY - _groundY);
		var desiredCameraY = groundY + CameraOffset;
		
		if (groundDelta > Tolerance) {
			_groundY = groundY;
			_step = (desiredCameraY - _cameraY) / N;
		}

		if (Mathf.Abs(_cameraY + _step - desiredCameraY) < Mathf.Abs(_cameraY - desiredCameraY)) {
			_cameraY += _step;
		}

		return _cameraY;
	}
	
}