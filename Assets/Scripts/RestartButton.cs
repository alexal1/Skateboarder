using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour {

	private Transform _playerTransform;
	private Rigidbody2D _playerRigidbody;

	void Start() {
		GetComponent<Button>().onClick.AddListener(HandleClick);
		var player = GameObject.Find("Player");
		_playerTransform = player.transform;
		_playerRigidbody = player.GetComponent<Rigidbody2D>();
	}

	public void HandleClick() {
		_playerTransform.position = Vector3.zero;
		_playerRigidbody.velocity = Vector2.zero;
	}

}