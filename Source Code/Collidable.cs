using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    public ContactFilter2D filter;
    private BoxCollider2D boxCollider;
    private Collider2D[] hits = new Collider2D[10];

    protected virtual void Start() {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update() {

        // Collision work
        boxCollider.OverlapCollider(filter, hits);

        for (int i = 0; i < hits.Length; i++) {
            if (hits[i] == null)
                continue;

            onCollide(hits[i]);

            // reset value
            hits[i] = null;
        }
    }

    protected virtual void onCollide(Collider2D collider) {
        // debug
        Debug.Log("onCollide to be implemented for " + this.name);
    }
}
