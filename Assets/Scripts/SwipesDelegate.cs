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

    private const float SwipeIdleMagnitude = 1;
    private const float SwipeDefaultMagnitude = 300;
    private readonly Action<SwipeDirection, float> _onSwipeChanged;
    private readonly Action<SwipeDirection, float> _onSwipeReleased;
    private TouchesDelegate _touches;
    private Vector2? _startTouch;
    private SwipeDirection _swipeDirection;
    private float _swipeMagnitude;

    public SwipesDelegate(Action<SwipeDirection, float> onSwipeChanged, Action<SwipeDirection, float> onSwipeReleased) {
        _onSwipeChanged = onSwipeChanged;
        _onSwipeReleased = onSwipeReleased;
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
            _onSwipeChanged(SwipeDirection.Bottom, SwipeDefaultMagnitude);
        }
        else if (Input.GetKeyUp(KeyCode.W)) {
            _onSwipeReleased(SwipeDirection.Bottom, SwipeDefaultMagnitude);
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            _onSwipeChanged(SwipeDirection.Left, SwipeDefaultMagnitude);
        }
        else if (Input.GetKeyUp(KeyCode.A)) {
            _onSwipeReleased(SwipeDirection.Left, SwipeDefaultMagnitude);
        }
        else if (Input.GetKeyDown(KeyCode.S)) {
            _onSwipeChanged(SwipeDirection.Top, SwipeDefaultMagnitude);
        }
        else if (Input.GetKeyUp(KeyCode.S)) {
            _onSwipeReleased(SwipeDirection.Top, SwipeDefaultMagnitude);
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            _onSwipeChanged(SwipeDirection.Right, SwipeDefaultMagnitude);
        }
        else if (Input.GetKeyUp(KeyCode.D)) {
            _onSwipeReleased(SwipeDirection.Right, SwipeDefaultMagnitude);
        }
    }

    private void OnTouchBegan(Vector2 position) {
        _startTouch = position;
    }

    private void OnTouchMoved(Vector2 position) {
        if (_startTouch == null) {
            return;
        }
        
        var swipe = position - (Vector2) _startTouch;
        _swipeDirection = GetDirection(swipe);
        _swipeMagnitude = swipe.magnitude;

        if (_swipeMagnitude > SwipeIdleMagnitude) {
            _onSwipeChanged(_swipeDirection, _swipeMagnitude);
        }
    }

    private void OnTouchFinished() {
        _startTouch = null;

        if (_swipeMagnitude > SwipeIdleMagnitude) {
            _onSwipeReleased(_swipeDirection, _swipeMagnitude);
        }
    }

}