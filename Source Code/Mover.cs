using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : Fighter
{
    protected BoxCollider2D boxCollider;
    protected RaycastHit2D hit;
    protected Vector3 moveDelta;
    protected float ySpeed = 0.75f;
    protected float xSpeed = 1.0f;
    private Vector3 scale;

    // runs at beginning of execution
    protected virtual void Start() {

        boxCollider = GetComponent<BoxCollider2D>();
        scale = GetComponent<Transform>().localScale;
    }

    protected virtual void updateMotor(Vector3 input) {
        // reset moveDelta
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        // swap sprite direction, wether you go right of left
        if (moveDelta.x > 0) {
            // flip the sprite to right
            transform.localScale = scale;
        }
        else if (moveDelta.x < 0) {
            // flip the sprite to the left
            transform.localScale = new Vector3(-1 * scale.x, scale.y, scale.z);
        }

        // Add push vector if there is one
        moveDelta += pushDirection;

        // Reduce push force every frame, based off recovery speed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        // check for vertical collision
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));

        // move the object if there were no collisions(cast a box on wanted position. If box is successfully casted - null returned - object can be moved)
        if (hit.collider == null) {
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }

        // check for horizontal collision
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));

        // move the object if there were no collisions(cast a box on wanted position. If box is successfully casted - null returned - object can be moved)
        if (hit.collider == null) {
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
    }
}
