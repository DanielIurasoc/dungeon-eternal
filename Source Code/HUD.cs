using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    // Text fields 
    public Text healthText, coinsText;

    public RectTransform healthBar;

    protected void Update() {

        // update texts
        healthText.text = GameManager.instance.player.hitPoint.ToString() + " / " + GameManager.instance.player.maxHitPoint.ToString();
        coinsText.text = GameManager.instance.inventory.coins.ToString();

        // update health bar
        float ratio = (float)GameManager.instance.player.hitPoint / (float)GameManager.instance.player.maxHitPoint;
        healthBar.localScale = new Vector3(ratio, 1.0f, 1.0f);
    }

    public void useHealthPotion() {
        if(GameManager.instance.inventory.healthPotions > 0) {
            GameManager.instance.player.heal(5);
            GameManager.instance.inventory.healthPotions--;
            GameManager.instance.showText("-1 Health Potion", 20, Color.red, GameManager.instance.player.transform.position, Vector3.up * 20, 1.0f);
        }
        else {
            GameManager.instance.showText("No Health Potions", 20, Color.red, GameManager.instance.player.transform.position, Vector3.up * 20, 1.0f);
        }
        
    }
}
