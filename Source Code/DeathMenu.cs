using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour {
    // Text fields for Death Stats
    public Text levelText, totalXPText, armorText, weaponText, coinsText, xpText;

    // Logic
    public RectTransform xpBar;
    private bool loaded = false;
    private Animator anim;

    protected void Awake() {
        anim = GetComponent<Animator>();
    }

    protected void Update() {
        // check if the menu wasn't already shown somehow
        if (!loaded) {
            
            // if the player is dead
            if(GameManager.instance.player.hitPoint == 0) {
                updateMenu();

                // update the variable to keep track the menu was shown
                loaded = true;
            }
        }
        
    }

    // Update menu text fields
    public void updateMenu() {

        // Death stats
        levelText.text = GameManager.instance.getCurrentLevel().ToString();
        totalXPText.text = GameManager.instance.experience.ToString();

        // set armor stat
        switch (GameManager.instance.armorLevel) {
            case 0:
                weaponText.text = "Bronze";
                weaponText.color = Color.black;
                break;
            case 1:
                weaponText.text = "Silver";
                weaponText.color = Color.grey;
                break;
            case 2:
                weaponText.text = "Gold";
                weaponText.color = Color.yellow;
                break;
            case 3:
                weaponText.text = "Diamond";
                weaponText.color = Color.cyan;
                break;
        }

        // set weapon stat
        switch (GameManager.instance.weapon.weaponLevel) {
            case 0:
                weaponText.text = "Bronze";
                weaponText.color = Color.black;
                break;
            case 1:
                weaponText.text = "Silver";
                weaponText.color = Color.grey;
                break;
            case 2:
                weaponText.text = "Gold";
                weaponText.color = Color.yellow;
                break;
            case 3:
                weaponText.text = "Diamond";
                weaponText.color = Color.cyan;
                break;
        }

        coinsText.text = GameManager.instance.inventory.coins.ToString();

        // XP Bar
        int currentLevel = GameManager.instance.getCurrentLevel();

        // check if max level was achieved
        if (currentLevel == GameManager.instance.xpTable.Count) {
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

    public void onRetryClick() {

        // reactivate the player object
        GameObject[] GO = GameObject.FindObjectsOfType<GameObject>(true);
        foreach (var item in GO) {
            if (item.tag == "Fighter" && item.name == "Player") {
                item.SetActive(true);
                GameManager.instance.player.lastImmune = Time.time;
                break;
            }
        }

        // reset progress
        GameManager.instance.resetProgress();

        // reset triggers
        anim.ResetTrigger("Show");
        anim.SetTrigger("Hide");

        // Go to Main Dungeon
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainDungeon");

    }

    public void onMainMenuClick() {
        // reactivate the player object
        GameObject[] GO = GameObject.FindObjectsOfType<GameObject>(true);
        foreach (var item in GO) {
            if (item.tag == "Fighter" && item.name == "Player") {
                item.SetActive(true);
                GameManager.instance.player.lastImmune = Time.time;

                // reset progress
                GameManager.instance.resetProgress();
                item.SetActive(false);
                //break;
            }

        }

        //GameManager.instance
        // reset progress
        // GameManager.instance.resetProgress();
        

        // reset triggers
        anim.ResetTrigger("Show");
        anim.SetTrigger("Hide");

        // Go to Start Menu
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartMenu");
    }

    public void onQuitClick() {
        // reactivate the player object
        GameObject[] GO = GameObject.FindObjectsOfType<GameObject>(true);
        foreach (var item in GO) {
            if (item.tag == "Fighter" && item.name == "Player") {
                item.SetActive(true);
                GameManager.instance.player.lastImmune = Time.time;
                break;
            }
        }
        // reset progress
        GameManager.instance.resetProgress();

        // reset triggers
        anim.ResetTrigger("Show");
        anim.SetTrigger("Hide");

        // Close the game
        Application.Quit();
    }
}
