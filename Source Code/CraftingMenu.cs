using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingMenu : MonoBehaviour
{
    // field give feedback after action
    public Text feedback;
    private float lastFeedbackTime = 0;

    // Fields for Crafting Menu
    public Text armorCostText, weaponCostText, potionCostText;
    private int currentArmorSelection = 0;
    private int currentWeaponSelection = 0;
    public Image armorSelectionSprite;
    public Image weaponSelectionSprite;
    public Image armorCostSprite;
    public Image weaponCostSprite;
    public Text armorSpriteCostText;
    public Text weaponSpriteCostText;

    // Text fields for Inventory
    public Text bronzeArmorAmount, bronzeWeaponAmount, silverArmorAmount, silverWeaponAmount;
    public Text goldArmorAmount, goldWeaponAmount, diamondArmorAmount, diamondWeaponAmount;
    public Text healthPotionAmount, coinsAmount;

    protected void Update() {
        updateMenu();
    }

    // Armor selection
    public void onArmorArrowClick(bool right) {
        if (right) {
            currentArmorSelection++;

            // check for end of array
            if (currentArmorSelection == GameManager.instance.armorSprites.Count)
                currentArmorSelection = 0;
        }
        else {
            currentArmorSelection--;

            // check for end of array
            if (currentArmorSelection < 0)
                currentArmorSelection = GameManager.instance.armorSprites.Count - 1;
        }

        if (currentArmorSelection > 0) {
            armorCostSprite.gameObject.SetActive(true);
            armorSpriteCostText.gameObject.SetActive(true);
            armorCostSprite.sprite = GameManager.instance.armorSprites[currentArmorSelection - 1];
        }
        else {
            armorCostSprite.gameObject.SetActive(false);
            armorSpriteCostText.gameObject.SetActive(false);
        }

        armorCostText.text = GameManager.instance.upgradePrices[currentArmorSelection].ToString() + " Coins";
        armorSelectionSprite.sprite = GameManager.instance.armorSprites[currentArmorSelection];
    }

    // Weapon selection
    public void onWeaponArrowClick(bool right) {
        if (right) {
            currentWeaponSelection++;

            // check for end of array
            if (currentWeaponSelection == GameManager.instance.weaponSprites.Count / 2)
                currentWeaponSelection = 0;
        }
        else {
            currentWeaponSelection--;

            // check for end of array
            if (currentWeaponSelection < 0)
                currentWeaponSelection = GameManager.instance.weaponSprites.Count / 2 - 1;
        }

        if (currentWeaponSelection > 0) {
            weaponCostSprite.gameObject.SetActive(true);
            weaponSpriteCostText.gameObject.SetActive(true);
            weaponCostSprite.sprite = GameManager.instance.weaponSprites[currentWeaponSelection - 1];
        }
        else {
            //weaponCostSprite.sprite = blankImage.sprite;
            weaponCostSprite.gameObject.SetActive(false);
            weaponSpriteCostText.gameObject.SetActive(false);
        }

        weaponCostText.text = GameManager.instance.upgradePrices[currentWeaponSelection].ToString() + " Coins";
        weaponSelectionSprite.sprite = GameManager.instance.weaponSprites[currentWeaponSelection];
    }

    // Update menu text fields
    public void updateMenu() {

        // inventory
        bronzeArmorAmount.text = " x" + GameManager.instance.inventory.bronzeArmors.ToString();
        silverArmorAmount.text = " x" + GameManager.instance.inventory.silverArmors.ToString();
        goldArmorAmount.text = " x" + GameManager.instance.inventory.goldArmors.ToString();
        diamondArmorAmount.text = " x" + GameManager.instance.inventory.diamondArmors.ToString();

        bronzeWeaponAmount.text = " x" + GameManager.instance.inventory.bronzeWeapons.ToString();
        silverWeaponAmount.text = " x" + GameManager.instance.inventory.silverWeapons.ToString();
        goldWeaponAmount.text = " x" + GameManager.instance.inventory.goldWeapons.ToString();
        diamondWeaponAmount.text = " x" + GameManager.instance.inventory.diamondWeapons.ToString();

        healthPotionAmount.text = " x " + GameManager.instance.inventory.healthPotions.ToString();
        coinsAmount.text = GameManager.instance.inventory.coins.ToString();

        // Crafting costs
        armorCostText.text = GameManager.instance.upgradePrices[currentArmorSelection].ToString() + " Coins";
        weaponCostText.text = GameManager.instance.upgradePrices[currentWeaponSelection].ToString() + " Coins";
        potionCostText.text = GameManager.instance.upgradePrices[GameManager.instance.upgradePrices.Count - 1].ToString() + " Coins";

        // Armor costs 
        if (currentArmorSelection > 0) {
            armorCostSprite.gameObject.SetActive(true);
            armorSpriteCostText.gameObject.SetActive(true);
            armorCostSprite.sprite = GameManager.instance.armorSprites[currentArmorSelection - 1];
        }
        else {
            armorCostSprite.gameObject.SetActive(false);
            armorSpriteCostText.gameObject.SetActive(false);
        }

        // weapon costs
        if (currentWeaponSelection > 0) {
            weaponCostSprite.gameObject.SetActive(true);
            weaponSpriteCostText.gameObject.SetActive(true);
            weaponCostSprite.sprite = GameManager.instance.weaponSprites[currentWeaponSelection - 1];
        }
        else {
            //weaponCostSprite.sprite = blankImage.sprite;
            weaponCostSprite.gameObject.SetActive(false);
            weaponSpriteCostText.gameObject.SetActive(false);
        }

        if (Time.time - lastFeedbackTime >= 3 || lastFeedbackTime == 0) {
            feedback.text = "";
        }

    }

    // Armor upgrade
    public void onCraftArmorClick() {

        // check if the player has all required resources to craft chosen armor
        if(GameManager.instance.inventory.coins >= GameManager.instance.upgradePrices[currentArmorSelection]) {
            switch (currentArmorSelection) {
                case 0:
                    GameManager.instance.inventory.bronzeArmors++;
                    GameManager.instance.inventory.coins -= GameManager.instance.upgradePrices[currentArmorSelection];

                    feedback.text = "Crafting successful!";
                    feedback.color = Color.green;
                    lastFeedbackTime = Time.time;
                    break;
                case 1:
                    if (GameManager.instance.inventory.bronzeArmors >= 3) {
                        GameManager.instance.inventory.bronzeArmors -= 3;
                        GameManager.instance.inventory.coins -= GameManager.instance.upgradePrices[currentArmorSelection];
                        GameManager.instance.inventory.silverArmors++;

                        feedback.text = "Crafting successful!";
                        feedback.color = Color.green;
                        lastFeedbackTime = Time.time;
                    }
                    else { 
                        feedback.text = "Not enough bronze armors!";
                        feedback.color = Color.red;
                        lastFeedbackTime = Time.time;
                    }
                    break;
                case 2:
                    if (GameManager.instance.inventory.silverArmors >= 3) {
                        GameManager.instance.inventory.silverArmors -= 3;
                        GameManager.instance.inventory.coins -= GameManager.instance.upgradePrices[currentArmorSelection];
                        GameManager.instance.inventory.goldArmors++;

                        feedback.text = "Crafting successful!";
                        feedback.color = Color.green;
                        lastFeedbackTime = Time.time;
                    }
                    else {
                        feedback.text = "Not enough silver armors!";
                        feedback.color = Color.red;
                        lastFeedbackTime = Time.time;
                    }
                    break;
                case 3:
                    if (GameManager.instance.inventory.goldArmors >= 3) {
                        GameManager.instance.inventory.goldArmors -= 3;
                        GameManager.instance.inventory.coins -= GameManager.instance.upgradePrices[currentArmorSelection];
                        GameManager.instance.inventory.diamondArmors++;
                        feedback.text = "Crafting successful!";
                        feedback.color = Color.green;
                    }
                    else {
                        feedback.text = "Not enough gold armors!";
                        feedback.color = Color.red;
                        lastFeedbackTime = Time.time;
                    }
                    break;
            }
        }
        else {
            feedback.text = "Not enough coins!";
            feedback.color = Color.red;
            lastFeedbackTime = Time.time;
        }

        // check if crafting was successful to call onInventoryChange() method
        if (feedback.text == "Crafting successful!")
            GameManager.instance.onInventoryChange();
    }

    // Weapon upgrade
    public void onCraftWeaponClick() {

        // check if the player has all required resources to craft chosen weapons
        if (GameManager.instance.inventory.coins >= GameManager.instance.upgradePrices[currentWeaponSelection]) {
            switch (currentWeaponSelection) {
                case 0:
                    GameManager.instance.inventory.bronzeWeapons++;
                    GameManager.instance.inventory.coins -= GameManager.instance.upgradePrices[currentWeaponSelection];

                    feedback.text = "Crafting successful!";
                    feedback.color = Color.green;
                    lastFeedbackTime = Time.time;
                    break;
                case 1:
                    if (GameManager.instance.inventory.bronzeWeapons >= 3) {
                        GameManager.instance.inventory.bronzeWeapons -= 3;
                        GameManager.instance.inventory.coins -= GameManager.instance.upgradePrices[currentWeaponSelection];
                        GameManager.instance.inventory.silverWeapons++;

                        feedback.text = "Crafting successful!";
                        feedback.color = Color.green;
                        lastFeedbackTime = Time.time;

                    }
                    else { 
                        feedback.text = "Not enough bronze weapons!";
                        feedback.color = Color.red;
                        lastFeedbackTime = Time.time;
                    }
                    break;
                case 2:
                    if (GameManager.instance.inventory.silverWeapons >= 3) {
                        GameManager.instance.inventory.silverWeapons -= 3;
                        GameManager.instance.inventory.coins -= GameManager.instance.upgradePrices[currentWeaponSelection];
                        GameManager.instance.inventory.goldWeapons++;

                        feedback.text = "Crafting successful!";
                        feedback.color = Color.green;
                        lastFeedbackTime = Time.time;
                    }
                    else { 
                        feedback.text = "Not enough silver weapons!";
                        feedback.color = Color.red;
                        lastFeedbackTime = Time.time;
                    }
                    break;
                case 3:
                    if (GameManager.instance.inventory.goldWeapons >= 3) {
                        GameManager.instance.inventory.goldWeapons -= 3;
                        GameManager.instance.inventory.coins -= GameManager.instance.upgradePrices[currentWeaponSelection];
                        GameManager.instance.inventory.diamondWeapons++;

                        feedback.text = "Crafting successful!";
                        feedback.color = Color.green;
                        lastFeedbackTime = Time.time;
                    }
                    else { 
                        feedback.text = "Not enough gold weapons!";
                        feedback.color = Color.red;
                        lastFeedbackTime = Time.time;
                    }
                    break;
            }
        }
        else {
            feedback.text = "Not enough coins!";
            feedback.color = Color.red;
            lastFeedbackTime = Time.time;
        }

        // check if crafting was successful to call onInventoryChange() method
        if (feedback.text == "Crafting successful!")
            GameManager.instance.onInventoryChange();
    }

    public void onCraftPotionClick() {
        // check if the player has all required resources to craft chosen weapons
        if (GameManager.instance.inventory.coins >= GameManager.instance.upgradePrices[GameManager.instance.upgradePrices.Count - 1]) {
            GameManager.instance.inventory.healthPotions++;
            GameManager.instance.inventory.coins -= GameManager.instance.upgradePrices[currentArmorSelection];

            feedback.text = "Crafting successful!";
            feedback.color = Color.green;
            lastFeedbackTime = Time.time;
        }
        else {
            feedback.text = "Not enough coins!";
            feedback.color = Color.red;
            lastFeedbackTime = Time.time;
        }
    }
}
