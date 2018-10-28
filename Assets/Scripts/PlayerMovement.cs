using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private readonly TouchesProxy touches = new TouchesProxy();
    private readonly float k = 0.1f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 startTouch = new Vector2();
    private float startTime = 0;
    private bool isAnimationTriggered = false;

    private void OnTouchBegan(Vector2 position) {
        startTouch = position;
        startTime = Time.time;
    }

    private void OnTouchMoved(Vector2 position) {
        float swipeDistance = startTouch.x - position.x;

        if (swipeDistance > 0 && !isAnimationTriggered) {
            isAnimationTriggered = true;
            animator.SetTrigger("Push");
        }

        float swipeVelocity;
        if (rb.velocity.x >= 0) {
            float swipeTime = Time.time - startTime;
            swipeVelocity = swipeDistance / swipeTime;
        }
        else {
            rb.velocity = new Vector2();
            swipeVelocity = 0;
        }
        rb.AddForce(new Vector2(swipeVelocity * k, 0), ForceMode2D.Force);
    }

    private void OnTouchFinished() {
        isAnimationTriggered = false;
    }

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        touches.Update(OnTouchBegan, OnTouchMoved, OnTouchFinished);
    }

}