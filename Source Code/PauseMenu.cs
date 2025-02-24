using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    // Text fields for Character Stats
    public Text healthText, levelText, coinsText, xpText;

    // Text fields for Inventory
    public Text bronzeArmorAmount, bronzeWeaponAmount, silverArmorAmount, silverWeaponAmount;
    public Text goldArmorAmount, goldWeaponAmount, diamondArmorAmount, diamondWeaponAmount;
    public Text healthPotionAmount;

    // Logic
    private int currentAvatarSelection = 0;
    private int currentWeaponSelection = 0;
    public Image avatarSelectionSprite;
    public Image weaponSelectionSprite;
    public RectTransform xpBar;

    // Avatar selection
    public void onAvatarArrowClick(bool right) {
        if (right) {
            currentAvatarSelection++;

            // check for end of array
            if (currentAvatarSelection == GameManager.instance.playerSprites.Count)
                currentAvatarSelection = 0;

            onSelectionChanged();
        }
        else {
            currentAvatarSelection--;

            // check for end of array
            if (currentAvatarSelection < 0)
                currentAvatarSelection = GameManager.instance.playerSprites.Count - 1;

            onSelectionChanged();
        }
    }

    private void onSelectionChanged() {
        avatarSelectionSprite.sprite = GameManager.instance.playerSprites[currentAvatarSelection];
        GameManager.instance.player.swapSprite(currentAvatarSelection);
    }

    // Weapon selection
    public void onWeaponArrowClick(bool right) {
        if (right) {
            currentWeaponSelection++;

            // check for end of array
            if (currentWeaponSelection == GameManager.instance.weaponTypeSprites.Count)
                currentWeaponSelection = 0;

            onWeaponChanged();
        }
        else {
            currentWeaponSelection--;

            // check for end of array
            if (currentWeaponSelection < 0)
                currentWeaponSelection = GameManager.instance.weaponTypeSprites.Count - 1;

            onWeaponChanged();
        }
    }

    private void onWeaponChanged() {
        weaponSelectionSprite.sprite = GameManager.instance.weaponTypeSprites[currentWeaponSelection];
        GameManager.instance.chosenWeapon = currentWeaponSelection;
        GameManager.instance.weapon.setWeaponSprite(GameManager.instance.weapon.weaponLevel);
    }

    // Reset button action
    public void onResetClick() {
        GameManager.instance.resetProgress();
    }

    // Quit button action
    public void onQuitClick() {
        // save the progress made so far
        GameManager.instance.saveState();

        // Quit the game
        Application.Quit();
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

        // character stats
        healthText.text = GameManager.instance.player.hitPoint.ToString();
        levelText.text = GameManager.instance.getCurrentLevel().ToString();
        coinsText.text = GameManager.instance.inventory.coins.ToString();

        // XP Bar
        int currentLevel = GameManager.instance.getCurrentLevel();

        // check if max level was achieved
        if(currentLevel == GameManager.instance.xpTable.Count) {
            xpText.text = GameManager.instance.experience.ToString() + " Total XP points";
            xpBar.localScale = Vector3.one;
        }
        else {
            int previousLevelXp = GameManager.instance.getXpToLevel(currentLevel - 1);
            int currentLevelXp = GameManager.instance.getXpToLevel(currentLevel);

            int difference = currentLevelXp - previousLevelXp;
            int currentXpIntoLevel = GameManager.instance.experience - previousLevelXp;

            float completionRatio = (float)currentXpIntoLevel / (float)difference;

            xpText.text = currentXpIntoLevel.ToString() + " / " + difference.ToString();
            xpBar.localScale = new Vector3(completionRatio, 1.0f, 1.0f);
        }
    }
}
