using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour {

    private const float HorizontalForceCoefficient = 0.01f;
    private const float VerticalForceCoefficient = 0.01f;
    private SwipesDelegate _swipes;
    private Rigidbody2D _rb;
    private Animator _animator;
    private Action _doOnPushed;

    // Use this for initialization
    private void Start() {
        _swipes = new SwipesDelegate(HandleSwipe);
        _swipes.Start();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update() {
        _swipes.Update();
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
                var verticalForce = swipeVelocity * VerticalForceCoefficient;
                _rb.AddForce(new Vector2(0, verticalForce), ForceMode2D.Impulse);
                break;
        }
    }

    public void OnPushed() {
        _doOnPushed();
    }

}