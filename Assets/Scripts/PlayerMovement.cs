using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private readonly TouchesProxy touches = new TouchesProxy();
    private readonly float k = 0.1f;
    private Rigidbody2D rb;
    private Vector2 startTouch = new Vector2();
    private float startTime = 0;

    private void OnTouchBegan(Vector2 position) {
        startTouch = position;
        startTime = Time.time;
    }

    private void OnTouchMoved(Vector2 position) {
        float velocity;
        if (rb.velocity.x >= 0) {
            float distance = startTouch.x - position.x;
            float time = Time.time - startTime;
            velocity = distance / time;
        }
        else {
            rb.velocity = new Vector2();
            velocity = 0;
        }
        rb.AddForce(new Vector2(velocity * k, 0), ForceMode2D.Force);
    }

    private void OnTouchFinished() {}

    // Use this for initialization
    void Start() {
       rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        touches.Update(OnTouchBegan, OnTouchMoved, OnTouchFinished);
    }

}