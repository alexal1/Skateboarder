using UnityEngine;

public class DecorationToCameraAdjustment : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (Camera.main == null) {
			Debug.Log("Camera is null!");
			return;
		}
		var cameraHeight = Camera.main.orthographicSize * 2;
		var cameraWidth = cameraHeight * Screen.width / Screen.height;

		var sprite = GetComponent<SpriteRenderer>();
		var spriteHeight = sprite.size.y;
		var spriteWidth = sprite.size.x;

		float localScale;
		if (cameraWidth / cameraHeight > 1f) {
			localScale = cameraWidth / spriteWidth;
		}
		else {
			localScale = cameraHeight / spriteHeight;
		}
		
		transform.localScale = new Vector3(localScale, localScale, localScale);
	}
	
}