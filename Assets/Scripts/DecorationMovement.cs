using UnityEngine;

public class DecorationMovement : MonoBehaviour {

	private const float MaxVelocity = 4f;
	private const float Acceleration = 0.0005f;
	private const int TilesCount = 2;
	private Camera _camera;
	private float _velocity = 0.1f;
	private float _width;
	private float _currentStep;
	private float _maxOffset;
	private float _currentOffset;

	// Use this for initialization
	void Start () {
		if (Camera.main == null) {
			Debug.Log("Camera is null!");
			return;
		}
		_camera = Camera.main;
		
		var spriteRenderer = GetComponent<SpriteRenderer>();
		_width = spriteRenderer.size.x * transform.localScale.x;
		
		spriteRenderer.size = new Vector2(spriteRenderer.size.x * TilesCount, spriteRenderer.size.y);
		_maxOffset = _width * (TilesCount - 1) / 2f;
	}
	
	// Update is called once per frame
	void Update() {
		if (_currentOffset <= -_maxOffset) {
			_currentOffset = _maxOffset;
		}
		else {
			_currentOffset -= _currentStep;
		}
		
		var cameraX = _camera.transform.position.x;
		transform.position = new Vector3(cameraX + _currentOffset, 0, 0);
		
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