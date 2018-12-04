using System;
using UnityEngine;

public class TouchesDelegate {

    private readonly Action<Vector2> _onTouchBegan;
    private readonly Action<Vector2> _onTouchMoved;
    private readonly Action _onTouchFinished;

    public TouchesDelegate(Action<Vector2> onTouchBegan,
                        Action<Vector2> onTouchMoved,
                        Action onTouchFinished) {
        _onTouchBegan = onTouchBegan;
        _onTouchMoved = onTouchMoved;
        _onTouchFinished = onTouchFinished;
    }

    public void Update() {
#if UNITY_IOS || UNITY_ANDROID
        Touch touch;
        if (Input.touchCount == 1) {
            touch = Input.GetTouch(0);
        }
        else {
            return;
        }

        switch (touch.phase) {
            case TouchPhase.Began:
                _onTouchBegan(touch.position);
                break;

            case TouchPhase.Moved:
                _onTouchMoved(touch.position);
                break;
            
            case TouchPhase.Ended:
                _onTouchFinished();
                break;
            
            case TouchPhase.Canceled:
                _onTouchFinished();
                break;
        }
#elif UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0)) {
            _onTouchBegan(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0)) {
            _onTouchMoved(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0)) {
            _onTouchFinished();
        }
#endif
    }

}