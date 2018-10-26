using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private readonly TouchesProxy touches = new TouchesProxy();

    private readonly Action<Vector2> onTouchBegan = (Vector2 position) => {
        Debug.Log("Touch began");
    };

    private readonly Action<Vector2> onTouchMoved = (Vector2 position) => {
        Debug.Log("Touch moved");
    };

    private readonly Action onTouchFinished = () => {
        Debug.Log("Touch finished");
    };

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        touches.Update(onTouchBegan, onTouchMoved, onTouchFinished);
    }

}