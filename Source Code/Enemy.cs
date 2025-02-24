using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    // Experience
    public int xpValue = 1;

    // Logic
    public float triggerLength = 1;
    public float chaseLenght = 5;
    private bool chasing;
    private bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;
    public Sprite[] enemySprites;

    // Hitbox
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];
    public ContactFilter2D filter;
    
    protected override void Start() {
        base.Start();

        // give a random sprite to the enemy
        GetComponent<SpriteRenderer>().sprite = enemySprites[Random.Range(0, enemySprites.Length)];

        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate() {
        // is the player in range?
        if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLenght) {
            if (Vector3.Distance(playerTransform.position, startingPosition) < triggerLength)
                chasing = true;

            if (chasing) {
                if (!collidingWithPlayer) {
                    // move enemy towards player
                    updateMotor((playerTransform.position - transform.position).normalized);
                }
            }
            else {
                // go back to starting position
                updateMotor(startingPosition - transform.position);
            }
        }
        else {
            // go back to starting position
            updateMotor(startingPosition - transform.position);
            chasing = false;
        }

        // check for overlaps
        collidingWithPlayer = false;

        // Collision work
        boxCollider.OverlapCollider(filter, hits);

        for (int i = 0; i < hits.Length; i++) {
            if (hits[i] == null)
                continue;

            if (hits[i].tag == "Fighter" && hits[i].name == "Player") {
                collidingWithPlayer = true;
            }

            // reset value
            hits[i] = null;
        }
    }

    protected override void death() {
        base.death();
        Destroy(gameObject);
        if(GameManager.instance.enemiesAlive > 0)
            GameManager.instance.enemiesAlive--;
        GameManager.instance.grantXP(xpValue);
        GameManager.instance.showText("+" + xpValue + " XP", 30, Color.magenta, transform.position, Vector3.up * 40, 1.0f);
    }

}
