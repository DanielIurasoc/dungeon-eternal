using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collidable {
    // Damage
    public int damage = 1;
    public float pushForce = 3;

    protected override void onCollide(Collider2D collider) {
        if (collider.tag == "Fighter" && collider.name == "Player") {
            // Create a damage object, and send it to player
            Damage dmg = new Damage {
                damageAmount = damage,
                origin = transform.position,
                pushForce = pushForce
            };

            collider.SendMessage("receiveDamage", dmg);
        }
    }
}

