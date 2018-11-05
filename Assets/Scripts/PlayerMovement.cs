using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour {

    private const float HorizontalForceCoefficient = 0.01f;
    private const float VerticalForceCoefficient = 0.01f;
    private const float Tolerance = 0.0001f;
    private const int GroundLayerMask = 1 << 8;
    private SwipesDelegate _swipes;
    private Rigidbody2D _rb;
    private Animator _animator;
    private Action _doOnPushed;
    private Action _doOnJumped;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _collider;

    // Use this for initialization
    private void Start() {
        _swipes = new SwipesDelegate(HandleSwipe);
        _swipes.Start();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _collider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update() {
        _swipes.Update();
        
        AdjustBounds();
        UpdateFlags();
    }

    private void AdjustBounds() {
        _collider.size = _spriteRenderer.sprite.bounds.size;
    }

    private void UpdateFlags() {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, 3.0f, GroundLayerMask);

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

    private void HandleSwipe(SwipeDirection swipeDirection, float swipeVelocity) {
        switch (swipeDirection) {
            case SwipeDirection.Left:
                _animator.SetTrigger("Push");
                _doOnPushed = () => {
                    var horizontalForce = swipeVelocity * HorizontalForceCoefficient;
                    _rb.AddForce(new Vector2(horizontalForce, 0), ForceMode2D.Impulse);
                };
                break;

            case SwipeDirection.Top:
                _animator.SetTrigger("Jump");
                _doOnJumped = () => {
                    var verticalForce = swipeVelocity * VerticalForceCoefficient;
                    _rb.AddForce(new Vector2(0, verticalForce), ForceMode2D.Impulse);
                };
                break;
        }
    }

    public void OnPushed() {
        _doOnPushed();
    }

    public void OnJumped() {
        _doOnJumped();
    }

}