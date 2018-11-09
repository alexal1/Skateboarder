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

    private const float SwipeIdleMagnitude = 10;
    private const float SwipeActionMagnitude = 100;
    private readonly Action<SwipeDirection> _handleSwipeStart;
    private readonly Action<SwipeDirection, float> _handleSwipeEnd;
    private TouchesDelegate _touches;
    private Vector2? _startTouch;
    private float _startTime;
    private SwipeDirection? _swipeDirection;

    public SwipesDelegate(Action<SwipeDirection> handleSwipeStart, Action<SwipeDirection, float> handleSwipeEnd) {
        _handleSwipeStart = handleSwipeStart;
        _handleSwipeEnd = handleSwipeEnd;
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
            _startTime = Time.time;
            _handleSwipeStart(SwipeDirection.Top);
        }
        else if (Input.GetKeyUp(KeyCode.W)) {
            var swipeTime = Time.time - _startTime;
            var swipeVelocity = SwipeActionMagnitude / swipeTime;
            _handleSwipeEnd(SwipeDirection.Top, swipeVelocity);
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            _startTime = Time.time;
            _handleSwipeStart(SwipeDirection.Right);
        }
        else if (Input.GetKeyUp(KeyCode.A)) {
            var swipeTime = Time.time - _startTime;
            var swipeVelocity = SwipeActionMagnitude / swipeTime;
            _handleSwipeEnd(SwipeDirection.Right, swipeVelocity);
        }
        else if (Input.GetKeyDown(KeyCode.S)) {
            _startTime = Time.time;
            _handleSwipeStart(SwipeDirection.Bottom);
        }
        else if (Input.GetKeyUp(KeyCode.S)) {
            var swipeTime = Time.time - _startTime;
            var swipeVelocity = SwipeActionMagnitude / swipeTime;
            _handleSwipeEnd(SwipeDirection.Bottom, swipeVelocity);
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            _startTime = Time.time;
            _handleSwipeStart(SwipeDirection.Left);
        }
        else if (Input.GetKeyUp(KeyCode.D)) {
            var swipeTime = Time.time - _startTime;
            var swipeVelocity = SwipeActionMagnitude / swipeTime;
            _handleSwipeEnd(SwipeDirection.Left, swipeVelocity);
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
            _handleSwipeEnd((SwipeDirection) _swipeDirection, swipeVelocity);
            OnTouchFinished();
        }
        else if (swipeMagnitude >= SwipeIdleMagnitude) {
            if (_swipeDirection == null) {
                _swipeDirection = GetDirection(swipe);
                _handleSwipeStart((SwipeDirection) _swipeDirection);
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