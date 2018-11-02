using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    private Transform player;
    private Transform camera;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").transform;
        camera = GameObject.Find("MainCamera").transform;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 playerpos = player.position;
        playerpos.z = transform.position.z;
        transform.position = playerpos;
        Debug.Log(camera.position);
        Debug.Log(player.position);
        Debug.Log("----------");
	}
}
