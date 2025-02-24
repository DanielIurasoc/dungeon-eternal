using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable {

    // Damage struct
    public int[] damagePoint = { 1, 2, 3, 4 };
    public float[] pushForce = { 3.0f, 3.5f, 4.0f , 5.0f};

    // Upgrade
    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    // Swing weapon
    private float cooldown = 0.5f;
    private float lastSwing;
    private Animator anim;
    
    public void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Start() {
        base.Start();
        anim = GetComponent<Animator>();
    }

    protected override void Update() {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (Time.time - lastSwing > cooldown) {
                lastSwing = Time.time;
                swing();
            }
        }
    }

    protected override void onCollide(Collider2D collider) {
        if (collider.tag == "Fighter") {
            if (collider.name == "Player")
                return;

            // Create a new Damage object, and send it to the figher hit
            Damage dmg = new Damage {
                damageAmount = damagePoint[weaponLevel],
                origin = transform.position,
                pushForce = pushForce[weaponLevel]

            };

            // send the message with the damage to the hit fighter
            collider.SendMessage("receiveDamage", dmg);
            
        }
    }

    private void swing() {
        anim.SetTrigger("Swing");
        GameManager.instance.playSound("playerAttack");
    }

    public void upgradeWeapon() {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }

    public void setWeaponLevel(int level) {
        weaponLevel = level;
        setWeaponSprite(weaponLevel);
    }

    public void setWeaponSprite(int weaponLevel) {
        if (GameManager.instance.chosenWeapon == 0) {

            // short weapons, sprites 0, 1, 2, 3
            spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
        }
        else { 
            
            // long weapons, sprites 4, 5, 6, 7
            spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel + GameManager.instance.weaponSprites.Count / 2];
        }
    }
}
