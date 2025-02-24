using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    private SpriteRenderer spriteRenderer;
    public Animator anim;
    private float lastPlayed;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPlayed = 0;
    }

    protected override void receiveDamage(Damage dmg) {
        base.receiveDamage(dmg);
        GameManager.instance.onHitpointChange();
    }

    protected void FixedUpdate()
    {
        // GetAxisRaw returns
        // -1 : negative input key is pressed
        //  0 : no key pressed
        //  1 : positive input key is pressed

        // read horizontal movement
        float x = Input.GetAxisRaw("Horizontal");

        // read vertical movement
        float y = Input.GetAxisRaw("Vertical");

        // call move function
        updateMotor(new Vector3(x, y, 0));
        if(x != 0 || y != 0)
            if(Time.time - lastPlayed > 0.4f) {
                GameManager.instance.playSound("playerWalk");
                lastPlayed = Time.time;
            }
    }

    // for healing the player through level ups, healing fountain or healing potions
    public void heal(int healAmount) {
        // check if the player is already at full health
        if (hitPoint == maxHitPoint)
            return;
        hitPoint += healAmount;
        if (hitPoint > maxHitPoint)
            hitPoint = maxHitPoint;
        GameManager.instance.showText("+" + healAmount.ToString() + "HP", 25, Color.green, transform.position, Vector3.up * 30, 1.0f);
        GameManager.instance.onHitpointChange();

    }

    // grant stats on level up
    public void onLevelUp() {
        
        // increase the maximum health
        maxHitPoint++;

        // heal the player with 5 health
        if (hitPoint + 5 < maxHitPoint)
            hitPoint += 5;
        else
            hitPoint = maxHitPoint;
    }

    // to give the player the right stats for his level on load
    public void setLevel(int level) {
        for (int i = 0; i < level; i++) {
            onLevelUp();
        }
    }

    public void swapSprite(int skinId) {
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinId];
    }

    protected override void death() {
        GameManager.instance.playSound("playerDeath");
        anim.SetTrigger("Show");
        gameObject.SetActive(false);
    }
}
