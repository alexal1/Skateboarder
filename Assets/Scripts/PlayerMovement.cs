using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour {

    private const string TriggerPush = "Push";
    private const string TriggerJump = "Jump";
    private const float HorizontalForceCoefficient = 0.02f;
    private const float VerticalForceCoefficient = 0.02f;
    private const float MaxHorizontalForce = 20f;
    private const float MaxVerticalForce= 20f;
    private const float Tolerance = 0.0001f;
    private const int GroundLayerMask = 1 << 8;
    private SwipesDelegate _swipes;
    private Rigidbody2D _rb;
    private Animator _animator;
    private Action _doOnPushed;
    private Action _doOnJumped;
    private Collider2D _collider;

    // Use this for initialization
    private void Start() {
        _swipes = new SwipesDelegate(HandleSwipeStart, HandleSwipeEnd);
        _swipes.Start();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = gameObject.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    private void Update() {
        _swipes.Update();
        
        UpdateFlags();
    }

    private void UpdateFlags() {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, 8.0f, GroundLayerMask);

        var isFalling = _rb.velocity.y < 0;
        var isGroundDetected = hit.collider != null;
        var isGrounded = _collider.IsTouchingLayers(GroundLayerMask);
        var isStopped = Math.Abs(_rb.velocity.x) < Tolerance;
        
        _animator.SetBool("IsFalling", isFalling);
        _animator.SetBool("IsGroundDetected", isGroundDetected);
        _animator.SetBool("IsGrounded", isGrounded);
        _animator.SetBool("IsFallingOrGrounded", isFalling || isGrounded);
        _animator.SetBool("IsStopped", isStopped);
    }

    private void HandleSwipeStart(SwipeDirection swipeDirection) {
        switch (swipeDirection) {
            case SwipeDirection.Left:
                _animator.SetTrigger(TriggerPush);
                break;

            case SwipeDirection.Top:
                _animator.SetTrigger(TriggerJump);
                break;
        }
    }

    private void HandleSwipeEnd(SwipeDirection swipeDirection, float swipeVelocity) {
        switch (swipeDirection) {
            case SwipeDirection.Left:
                _doOnPushed = () => {
                    var horizontalForce = Math.Min(swipeVelocity * HorizontalForceCoefficient, MaxHorizontalForce);
                    _rb.AddForce(new Vector2(horizontalForce, 0), ForceMode2D.Impulse);
                };
                break;

            case SwipeDirection.Top:
                _doOnJumped = () => {
                    var verticalForce = Math.Min(swipeVelocity * VerticalForceCoefficient, MaxVerticalForce);
                    _rb.AddForce(new Vector2(0, verticalForce), ForceMode2D.Impulse);
                };
                break;
        }
    }

    public void OnPushed() {
        if (_doOnPushed != null) {
            _doOnPushed();
        }
        _doOnPushed = null;
    }

    public void OnJumped() {
        if (_doOnJumped != null) {
            _doOnJumped();
        }
        _doOnJumped = null;
    }

    public void OnGrounded() {
        _animator.ResetTrigger(TriggerJump);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Coin")) {
            Destroy(other.gameObject);
        }
    }

}