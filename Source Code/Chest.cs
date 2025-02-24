using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest;
    public int coinAmount = 5;
    
    // armors
    public int bronzeArmorAmount = 0;
    public int silverArmorAmount = 0;
    public int goldArmorAmount = 0;
    public int diamondArmorAmount = 0;

    // weapons
    public int bronzeWeaponAmount = 0;
    public int silverWeaponAmount = 0;
    public int goldWeaponAmount = 0;
    public int diamondWeaponAmount = 0;

    protected override void onCollect() {
        if (!collected) {
            base.onCollect();

            // update the sprite to show the empty chest
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            
            // print the rewards messages
            StartCoroutine(printMessages());

            // call the inventory changed method
            //GameManager.instance.onInventoryChange();
        }
    }

    IEnumerator printMessages() {
        
        // grant coins
        GameManager.instance.inventory.coins += coinAmount;
        GameManager.instance.showText("+" + coinAmount + " Coins!", 25, Color.yellow, GameManager.instance.player.transform.position, Vector3.up * 25, 1.0f);

        GameManager.instance.inventory.bronzeArmors += bronzeArmorAmount;
        GameManager.instance.inventory.silverArmors += silverArmorAmount;
        GameManager.instance.inventory.goldArmors += goldArmorAmount;
        GameManager.instance.inventory.diamondArmors += diamondArmorAmount;
        GameManager.instance.inventory.bronzeWeapons += bronzeWeaponAmount;
        GameManager.instance.inventory.silverWeapons += silverWeaponAmount;
        GameManager.instance.inventory.goldWeapons += goldWeaponAmount;
        GameManager.instance.inventory.diamondWeapons += diamondWeaponAmount;

        GameManager.instance.onInventoryChange();


        // Grant armors if there are any inside the chest
        // check if the chest contains any bronze armor
        if (bronzeArmorAmount > 0) {
            yield return new WaitForSeconds(1);
            //GameManager.instance.inventory.bronzeArmors += bronzeArmorAmount;
            GameManager.instance.showText("+" + bronzeArmorAmount + " Bronze Armors!", 25, Color.black, GameManager.instance.player.transform.position, Vector3.up * 25, 1.0f);

            // call the inventory changed method
            // GameManager.instance.onInventoryChange();
        }
        // check if the chest contains any silver armor
        if (silverArmorAmount > 0) {
            yield return new WaitForSeconds(1);
            //GameManager.instance.inventory.silverArmors += silverArmorAmount;
            GameManager.instance.showText("+" + silverArmorAmount + " Silver Armors!", 25, Color.grey, GameManager.instance.player.transform.position, Vector3.up * 25, 1.0f);

            // call the inventory changed method
            //GameManager.instance.onInventoryChange();
        }
        // check if the chest contains any gold armor
        if (goldArmorAmount > 0) {
            yield return new WaitForSeconds(1);
            //GameManager.instance.inventory.goldArmors += goldArmorAmount;
            GameManager.instance.showText("+" + goldArmorAmount + " Gold Armors!", 25, Color.yellow, GameManager.instance.player.transform.position, Vector3.up * 25, 1.0f);

            // call the inventory changed method
            //GameManager.instance.onInventoryChange();
        }
        // check if the chest contains any diamond armor
        if (diamondArmorAmount > 0) {
            yield return new WaitForSeconds(1);
            //GameManager.instance.inventory.diamondArmors += diamondArmorAmount;
            GameManager.instance.showText("+" + diamondArmorAmount + " Diamond Armors!", 25, Color.cyan, GameManager.instance.player.transform.position, Vector3.up * 25, 1.0f);

            // call the inventory changed method
            //GameManager.instance.onInventoryChange();
        }

        // Grant weapons if there are any inside the chest
        // check if the chest contains any bronze armor
        if (bronzeWeaponAmount > 0) {
            yield return new WaitForSeconds(1);
            //GameManager.instance.inventory.bronzeWeapons += bronzeWeaponAmount;
            GameManager.instance.showText("+" + bronzeWeaponAmount + " Bronze Weapons!", 25, Color.black, GameManager.instance.player.transform.position, Vector3.up * 25, 1.0f);

            // call the inventory changed method
            //GameManager.instance.onInventoryChange();
        }
        // check if the chest contains any silver armor
        if (silverWeaponAmount > 0) {
            yield return new WaitForSeconds(1);
            //GameManager.instance.inventory.silverWeapons += silverWeaponAmount;
            GameManager.instance.showText("+" + silverWeaponAmount + " Silver Weapons!", 25, Color.grey, GameManager.instance.player.transform.position, Vector3.up * 25, 1.0f);

            // call the inventory changed method
            //GameManager.instance.onInventoryChange();
        }
        // check if the chest contains any gold armor
        if (goldWeaponAmount > 0) {
            yield return new WaitForSeconds(1);
            //GameManager.instance.inventory.goldWeapons += goldWeaponAmount;
            GameManager.instance.showText("+" + goldWeaponAmount + " Gold Weapons!", 25, Color.yellow, GameManager.instance.player.transform.position, Vector3.up * 25, 1.0f);

            // call the inventory changed method
            //GameManager.instance.onInventoryChange();
        }
        // check if the chest contains any diamond armor
        if (diamondWeaponAmount > 0) {
            yield return new WaitForSeconds(1);
            //GameManager.instance.inventory.diamondWeapons += diamondWeaponAmount;
            GameManager.instance.showText("+" + diamondWeaponAmount + " Diamond Weapons!", 25, Color.cyan, GameManager.instance.player.transform.position, Vector3.up * 25, 1.0f);

            // call the inventory changed method
            //GameManager.instance.onInventoryChange();
        }

    }
}
