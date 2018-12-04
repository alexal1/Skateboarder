using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour {

    private const float HorizontalForceCoefficient = 0.5f;
    private const float VerticalForceCoefficient = 0.0625f;
    private const float MaxVerticalForce= 25f;
    private const float Tolerance = 0.0001f;
    private SwipesDelegate _swipes;
    private Rigidbody2D _rb;
    private Animator _animator;
    private Action _doOnPushed;
    private Action _doOnJumped;
    private Collider2D _collider;
    private bool _isGrounded;

    // Use this for initialization
    private void Start() {
        _swipes = new SwipesDelegate(OnSwipeChanged, OnSwipeReleased);
        _swipes.Start();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = gameObject.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    private void Update() {
        UpdateFlags();
        _swipes.Update();
    }

    private void UpdateFlags() {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, 8.0f, Config.ForegroundLayerMask);

        var isFalling = _rb.velocity.y < 0;
        var isGroundDetected = hit.collider != null;
        _isGrounded = _collider.IsTouchingLayers(Config.ForegroundLayerMask);
        var isStopped = Math.Abs(_rb.velocity.x) < Tolerance;
        
        _animator.SetBool("IsFalling", isFalling);
        _animator.SetBool("IsGroundDetected", isGroundDetected);
        _animator.SetBool("IsGrounded", _isGrounded);
        _animator.SetBool("IsFallingOrGrounded", isFalling || _isGrounded);
        _animator.SetBool("IsStopped", isStopped);
    }

    private void OnSwipeChanged(SwipeDirection swipeDirection, float swipeMagnitude) {
        switch (swipeDirection) {
            case SwipeDirection.Right:
                if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("PrePush")) {
                    _animator.SetTrigger("PrePush");
                }
                break;

            case SwipeDirection.Bottom:
                if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("PreJump")) {
                    _animator.SetTrigger("PreJump");
                }
                break;
        }
    }

    private void OnSwipeReleased(SwipeDirection swipeDirection, float swipeMagnitude) {
        switch (swipeDirection) {
            case SwipeDirection.Right:
                _animator.SetTrigger("Push");
                if (_isGrounded) {
                    var velocity = Mathf.Max(_rb.velocity.x, 10f);
                    var horizontalForce = swipeMagnitude / velocity * HorizontalForceCoefficient;
                    _rb.AddForce(new Vector2(horizontalForce, 0), ForceMode2D.Impulse);
                }
                break;

            case SwipeDirection.Bottom:
                _animator.SetTrigger("Jump");
                if (_isGrounded) {
                    var verticalForce = Math.Min(swipeMagnitude * VerticalForceCoefficient, MaxVerticalForce);
                    _rb.AddForce(new Vector2(0, verticalForce), ForceMode2D.Impulse);
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Coin")) {
            Destroy(other.gameObject);
        }
    }

}