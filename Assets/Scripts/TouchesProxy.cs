using System;
using UnityEngine;

public class TouchesProxy {

    public void Update(Action<Vector2> onTouchBegan,
                       Action<Vector2> onTouchMoved,
                       Action onTouchFinished) {
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
                onTouchBegan(touch.position);
                break;

            case TouchPhase.Moved:
                onTouchMoved(touch.position);
                break;

            default:
                onTouchFinished();
                break;
        }
#elif UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0)) {
            onTouchBegan(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0)) {
            onTouchMoved(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0)) {
            onTouchFinished();
        }
#endif
    }

}