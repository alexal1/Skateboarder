using System;
using UnityEngine;

public class DecorationMovement : MonoBehaviour {

	public int TilesCount;
	public float MaxVelocity;
	public float InitialVelocity;
	public float Acceleration;
	public float HeightMultiplier;

	private Camera _camera;
	private float _maxCameraOffsetY;
	private float _velocity;
	private float _currentStep;
	private float _currentOffset;
	private float _spriteWidth;
	private float _startOffset;
	private float _endOffset;

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

		_spriteWidth = spriteWidth * TilesCount * localScale;

		_maxCameraOffsetY = (HeightMultiplier - 1f) * cameraHeight / 2f;
		
		_startOffset = (_spriteWidth - cameraWidth) / 2f;
		_endOffset = _spriteWidth * 2f / TilesCount - (_spriteWidth + cameraWidth) / 2f;
	}
	
	void LateUpdate() {
		if (_currentOffset - _currentStep < _endOffset) {
			_currentOffset = _startOffset;
		}
		else {
			_currentOffset -= _currentStep;
		}
		
		var cameraX = _camera.transform.position.x;
		var spriteX = cameraX + _currentOffset;
		var spriteY = transform.position.y;
		spriteY = Math.Max(spriteY, _camera.transform.position.y - _maxCameraOffsetY);
		spriteY = Math.Min(spriteY, _camera.transform.position.y + _maxCameraOffsetY);
		transform.position = new Vector3(spriteX, spriteY, transform.position.z);
		
		IncrementVelocity();
	}

	private void IncrementVelocity() {
		if (_velocity < MaxVelocity) {
			_velocity += Acceleration;
		}
		_currentStep = GetStepByVelocity();
	}
	
	private float GetStepByVelocity() {
		return _spriteWidth * _velocity / 100f;
	}

}