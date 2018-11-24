using UnityEngine;

public class DecorationMovement : MonoBehaviour {

	public int TilesCount;
	public float MaxVelocity;
	public float InitialVelocity;
	public float Acceleration;
	public float HeightMultiplier;
	public bool FollowCameraY;

	private Camera _camera;
	private float _velocity;
	private float _width;
	private float _currentStep;
	private float _maxOffset;
	private float _currentOffset;

	void Awake() {
		if (Camera.main == null) {
			Debug.Log("Camera is null!");
			return;
		}
		_camera = Camera.main;
		
		var cameraHeight = _camera.orthographicSize * 2;
		var cameraWidth = cameraHeight * Screen.width / Screen.height;

		_velocity = InitialVelocity;
		
		var spriteRenderer = GetComponent<SpriteRenderer>();
		var spriteHeight = spriteRenderer.size.y;
		var spriteWidth = spriteRenderer.size.x;
		spriteRenderer.size = new Vector2(spriteWidth * TilesCount, spriteHeight);
		
		var localScale = cameraHeight * HeightMultiplier / spriteHeight;
		transform.localScale = new Vector3(localScale, localScale, localScale);

		_width = cameraWidth * TilesCount;
		_maxOffset = cameraWidth * (TilesCount - 1) / 2f * HeightMultiplier;
	}
	
	void LateUpdate() {
		if (_currentOffset <= -_maxOffset) {
			_currentOffset = _maxOffset;
		}
		else {
			_currentOffset -= _currentStep;
		}
		
		var cameraX = _camera.transform.position.x;
		var decorationX = cameraX + _currentOffset;
		var decorationY = FollowCameraY ? _camera.transform.position.y : transform.position.y;
		transform.position = new Vector3(decorationX, decorationY, transform.position.z);
		
		IncrementVelocity();
	}

	private void IncrementVelocity() {
		if (_velocity < MaxVelocity) {
			_velocity += Acceleration;
		}
		_currentStep = GetStepByVelocity();
	}
	
	private float GetStepByVelocity() {
		return _width * _velocity / 100f;
	}

}