using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    // Public fields
    public int hitPoint = 10;
    public int maxHitPoint = 10;
    public float pushRecoverySpeed = 0.2f;

    // Immunity
    protected float immuneTime = 1.0f;
    public float lastImmune;

    // Push
    protected Vector3 pushDirection;

    // All fighters can receive damage and die
    protected virtual void receiveDamage(Damage dmg) {
        if (Time.time - lastImmune > immuneTime) {
            lastImmune = Time.time;
            hitPoint -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            if (name == "Player")
                GameManager.instance.playSound("enemyAttack");
            GameManager.instance.showText(dmg.damageAmount.ToString(), 15, Color.red, transform.position, Vector3.zero, 0.5f);

            if (hitPoint <= 0) {
                hitPoint = 0;
                death();
            }
        }
    }

    protected virtual void death() {
        GameManager.instance.playSound("enemyDeath");
    }
}
