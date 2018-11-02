using UnityEngine;
using System;

public class SwipesDelegate {
    
    private static SwipeDirection GetDirection(Vector2 swipe) {
        var angle = Vector2.SignedAngle(Vector2.right, swipe);
        
        if (-45 < angle && angle < 45) {
            return SwipeDirection.Right;
        }
        if (45 < angle && angle < 135) {
            return SwipeDirection.Top;
        }
        if (-135 < angle && angle < -45) {
            return SwipeDirection.Bottom;
        }
        return SwipeDirection.Left;
    }

    private const float SwipeIdleMagnitude = 20;
    private const float SwipeActionMagnitude = 100;
    private const float DefaultSwipeVelocity = 800;
    private readonly Action<SwipeDirection, float> _handleSwipe;
    private TouchesDelegate _touches;
    private Vector2? _startTouch;
    private float _startTime;
    private SwipeDirection? _swipeDirection;

    public SwipesDelegate(Action<SwipeDirection, float> handleSwipe) {
        _handleSwipe = handleSwipe;
    }
    
    // Use this for initialization
    public void Start() {
        _touches = new TouchesDelegate(OnTouchBegan, OnTouchMoved, OnTouchFinished);
    }

    // Update is called once per frame
    public void Update() {
        // Handle touches input
        _touches.Update();

        // Handle keyboard input
        if (Input.GetKeyDown(KeyCode.W)) {
            _handleSwipe(SwipeDirection.Top, DefaultSwipeVelocity);
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            _handleSwipe(SwipeDirection.Right, DefaultSwipeVelocity);
        }
        else if (Input.GetKeyDown(KeyCode.S)) {
            _handleSwipe(SwipeDirection.Bottom, DefaultSwipeVelocity);
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            _handleSwipe(SwipeDirection.Left, DefaultSwipeVelocity);
        }
    }

    private void OnTouchBegan(Vector2 position) {
        _startTouch = position;
        _startTime = Time.time;
    }

    private void OnTouchMoved(Vector2 position) {
        if (_startTouch == null) {
            return;
        }
        var swipe = position - (Vector2) _startTouch;
        var swipeMagnitude = swipe.magnitude;

        if (swipeMagnitude >= SwipeActionMagnitude && _swipeDirection != null) {
            var swipeTime = Time.time - _startTime;
            var swipeVelocity = swipeMagnitude / swipeTime;
            _handleSwipe((SwipeDirection) _swipeDirection, swipeVelocity);
            OnTouchFinished();
        }
        else if (swipeMagnitude >= SwipeIdleMagnitude) {
            if (_swipeDirection == null) {
                _swipeDirection = GetDirection(swipe);
            }
            else {
                if (GetDirection(swipe) != _swipeDirection) {
                    OnTouchFinished();
                }
            }
        }
    }

    private void OnTouchFinished() {
        _startTouch = null;
        _swipeDirection = null;
    }

}