using UnityEngine;

public class BackgroundMovement : MonoBehaviour {

	private const float MaxVelocity = 10f;
	private const float Acceleration = 0.001f;
	private const int TilesCount = 2;
	private float _velocity = 0.1f;
	private float _cameraWidth;
	private Vector3 _currentStep;
	private float _maxOffset;

	// Use this for initialization
	void Start () {
		var spriteRenderer = GetComponent<SpriteRenderer>();
		
		if (Camera.main == null) {
			Debug.Log("Cannot obtain camera");
			return;
		}
		var cameraHeight = Camera.main.orthographicSize * 2f;
		_cameraWidth = cameraHeight / Screen.height * Screen.width;
		
		transform.localScale = new Vector3(
			_cameraWidth / spriteRenderer.bounds.size.x,
			cameraHeight / spriteRenderer.bounds.size.y,
			1
		);
		
		spriteRenderer.size = new Vector2(spriteRenderer.size.x * TilesCount, spriteRenderer.size.y);
		_maxOffset = _cameraWidth * (TilesCount - 1) / 2f;
	}
	
	// Update is called once per frame
	void Update() {
		if (transform.position.x <= -_maxOffset) {
			transform.position = new Vector3(_maxOffset, 0, 0);
		}
		else {
			transform.position -= _currentStep;
		}
		
		IncrementVelocity();
	}

	private void IncrementVelocity() {
		if (_velocity < MaxVelocity) {
			_velocity += Acceleration;
			_currentStep = GetStepByVelocity();
		}
	}
	
	private Vector3 GetStepByVelocity() {
		return new Vector3(_cameraWidth * _velocity / 100f, 0, 0);
	}

}