using UnityEngine;

public class CameraMovement : MonoBehaviour {

    private Transform _player;
	private CameraInterpolator _cameraInterpolator;

    // Use this for initialization
    void Start () {
        _player = GameObject.Find("Player").transform;
	    _cameraInterpolator = new BoundedCameraInterpolator();
	}
	
	// Update is called once per frame
	void Update () {
		var hit = Physics2D.Raycast(_player.position, Vector2.down, Mathf.Infinity, Config.ForegroundLayerMask);
		var hitY = hit.collider != null ? _cameraInterpolator.GetCameraY(hit.point.y) : transform.position.y;
		transform.position = new Vector3(_player.position.x, hitY, transform.position.z);
    }

}