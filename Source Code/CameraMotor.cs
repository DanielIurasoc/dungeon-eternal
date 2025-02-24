using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    // lookAt = object the camera shall follow in game
    private Transform lookAt;

    // boundries for the player to move whitout triggering the camera to follow
    public float boundX = 0.25f;
    public float boundY = 0.10f;

    private void Start() {
        lookAt = GameObject.Find("Player").transform;
    }

    // LateUpdate() is called after Update and FixedUpdate(). To make sure to move the camera AFTER the player movement is registered
    private void LateUpdate() {
        // store the difference in camera movement
        Vector3 delta = Vector3.zero;

        // get horizontal delta between player and camera
        float deltaX = lookAt.position.x - transform.position.x;

        // get vertical delta between player and camera
        float deltaY = lookAt.position.y - transform.position.y;

        // check for right camera movement
        if (deltaX > boundX) {
            delta.x = deltaX - boundX;
        }
        // check for left camera movement
        else if (deltaX < -boundX) {
            delta.x = deltaX + boundX;
        }
        // check for up camera movement
        else if (deltaY > boundY) {
            delta.y = deltaY - boundY;
        }
        // check for down camera movement
        else if (deltaY < -boundY) {
            delta.y = deltaY + boundY;
        }

        // move camera
        transform.position += new Vector3(delta.x, delta.y, 0);
    }
}
