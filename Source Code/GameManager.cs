using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // create an instance of this class
    public static GameManager instance;
    public GameObject chest;

    // Resources
    public int chosenAvatar;
    public int chosenWeapon;
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<Sprite> weaponTypeSprites;
    public List<Sprite> armorSprites;
    public List<int> upgradePrices;
    public List<int> xpTable;

    // Player attributes
    public int[] armorHPStat = { 2, 4, 6, 10 };
    public float[] armorPushbackStat = { 0.2f, 0.4f, 0.7f, 1.0f };
    public Player player;
    public Weapon weapon;
    public int armorLevel = 0;
    
    // References
    public int enemiesAlive;
    public FloatingTextManager floatingTextManager;
    public SoundManager soundManager;
    public RectTransform hitpointBar;
    public GameObject hud;
    public GameObject pauseMenu;
    public GameObject craftingMenu;
    public GameObject deathMenu;
    public Vector3 lastPosition;

    // Logic
    public Inventory inventory;
    public int experience;

    private void Awake() {
        // make instance a Singleton
        if (GameManager.instance != null) {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(soundManager.gameObject);
            Destroy(hud);
            Destroy(pauseMenu);
            Destroy(craftingMenu);
            Destroy(deathMenu);
            //Destroy(eventSistem);
            return;
        }
        else {
            enemiesAlive = 0;
            armorLevel = 0;
            lastPosition = Vector3.zero;
        }
        instance = this;
        
        SceneManager.sceneLoaded += loadState;
        SceneManager.sceneLoaded += onSceneLoaded;
    }

    ///////////////////////// FLOATING TEXT /////////////////////////
    public void showText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) {
        floatingTextManager.show(msg, fontSize, color, position, motion, duration);
    }

    ///////////////////////// PLAY SOUND /////////////////////////
    public void playSound(string clip) {
        SoundManager.instance.playSound(clip);
    }

    ///////////////////////// SET WEAPON and ARMOR LEVEL /////////////////////////
    public void onInventoryChange() {
        // find the highest armor level the player can achieve
        int newArmorLevel = 0;
        if(inventory.diamondArmors >= 5) {
            newArmorLevel = 3;
        }
        else if(inventory.goldArmors >= 5) {
            newArmorLevel = 2;
        }
        else if (inventory.silverArmors >= 5) {
            newArmorLevel = 1;
        }
        else {
            newArmorLevel = 0;
        }
        // check if the armor needs upgrading
        if(armorLevel != newArmorLevel) {
            armorLevel = newArmorLevel;
            setArmorStats();
        }

        // find the highest weapon level the player can achieve
        int newWeaponLevel = 0;
        if (inventory.diamondWeapons >= 5) {
            newWeaponLevel = 3;
        }
        else if (inventory.goldWeapons >= 5) {
            newWeaponLevel = 2;
        }
        else if (inventory.silverWeapons >= 5) {
            newWeaponLevel = 1;
        }
        else {
            newWeaponLevel = 0;
        }

        // check if the weapon level has changed
        if (weapon.weaponLevel != newWeaponLevel) {
            weapon.weaponLevel = newWeaponLevel;
            weapon.setWeaponSprite(weapon.weaponLevel);
        }
    }

    ///////////////////////// ARMOR /////////////////////////
    public void setArmorStats() {
        if (armorLevel > 0) {
            player.maxHitPoint = player.maxHitPoint - armorHPStat[armorLevel - 1] + armorHPStat[armorLevel];
            player.pushRecoverySpeed = player.pushRecoverySpeed - armorPushbackStat[armorLevel - 1] + armorPushbackStat[armorLevel];
        }
        else {
            player.maxHitPoint += armorHPStat[armorLevel];
            player.pushRecoverySpeed += armorPushbackStat[armorLevel];
        }
    }

    ///////////////////////// HEALTHPOINT BAR /////////////////////////
    public void onHitpointChange() {
        float ratio = (float)player.hitPoint / (float)player.maxHitPoint;
        hitpointBar.localScale = new Vector3(ratio, 1.0f, 1.0f);
    }

    ///////////////////////// EXPERIENCE SYSTEM /////////////////////////
    public void grantXP(int xp) {
        int currentLevel = getCurrentLevel();
        experience += xp;

        // check if the player leveled up
        if (currentLevel < getCurrentLevel())
            onLevelUp();
    }

    public int getCurrentLevel() {
        int r = 0;
        int add = 0;
        while (experience >= add) {
            add += xpTable[r];
            r++;

            // check for max level
            if (r == xpTable.Count)
                return r;
        }

        return r;
    }

    public int getXpToLevel(int level) {
        int r = 0;
        int xp = 0;
        while (r < level) {
            xp += xpTable[r];
            r++;
        }
        return xp;
    }

    public void onLevelUp() {
        onHitpointChange();
        player.onLevelUp();
    }

    // find the correct spawnpoint for the player on loading a new scene
    public void onSceneLoaded(Scene s, LoadSceneMode mode) {
        // find the correct spawn point for the player character
        if(s.name != "StartMenu") {
            if(s.name != "EternalDungeon" && s.name != "TreasureDungeon" && s.name != "BossDungeon") {
                if(GameManager.instance.lastPosition != Vector3.zero) {
                    player.transform.position = GameManager.instance.lastPosition;
                }
                else
                    player.transform.position = GameObject.Find("SpawnPoint").transform.position;
            }
            else
                player.transform.position = GameObject.Find("SpawnPoint").transform.position;
        }

        // setup each different scene
        switch (s.name) {
            case "MainDungeon":
                // MainDungeon settings
                if(GameManager.instance.lastPosition != Vector3.zero) {
                    // make triggers inactive
                    GameObject.Find("Trigger_1").SetActive(false);
                    GameObject.Find("Trigger_2").SetActive(false);
                    GameObject.Find("Trigger_3").SetActive(false);
                }
                else {
                    GameObject.FindWithTag("EternalPortal").SetActive(false);
                    GameObject.FindWithTag("BossPortal").SetActive(false);
                    GameObject.FindWithTag("TreasurePortal").SetActive(false);
                    GameObject.FindWithTag("VictoryNPC1").SetActive(false);
                    //GameObject.FindWithTag("VictoryChest").SetActive(false);
                    //GameObject.FindWithTag("VictoryChest").SetActive(false);
                    //GameObject.FindWithTag("VictoryChest").SetActive(false);
                }

                GameObject.FindWithTag("VictoryChest").SetActive(false);
                GameObject.FindWithTag("VictoryChest").SetActive(false);
                GameObject.FindWithTag("VictoryChest").SetActive(false);

                /*// reactivate the HUD
                GameObject[] GO1 = GameObject.FindObjectsOfType<GameObject>(true);
                foreach (var item in GO1) {
                    if (item.name == "HUD") {
                        //foreach (var item1 in item.GetComponentsInChildren<GameObject>()) {
                        //    if(item1.name == "HUDContainer")
                        //}
                        
                        item.SetActive(true);
                    }
                    else if (item.name == "GameManager") {
                        item.SetActive(true);
                    }
                }*/

                break;
            case "TreasureDungeon":
                // TreasureDungeon settings

                int x, nrOfChests = 4;
                bool[] active = { false, false, false, false, false, false, false };

                // get 4 random chests as active
                for (int i = 0; i < nrOfChests; i++) {
                    do {
                        x = Random.Range(1, 7);
                    } while (active[x]);
                    active[x] = true;
                }


                GameObject[] GO = GameObject.FindObjectsOfType<GameObject>(true);
                GameObject[] go = new GameObject[7];
                int j = 0;
                foreach (var item in GO) {
                    if (item.tag == "TreasureChest") {
                        go[j] = item;
                        j++;
                    }
                }
                        
                //go = GameObject.FindGameObjectsWithTag("TreasureChest");
                int pos;
                for (int i = 0; i < active.Length; i++) {
                    // find the Game Object linked to the current chest
                    for (pos = 0; pos < go.Length; pos++) {
                        string _name = "TChest" + (i + 1).ToString();
                        if (go[pos].name == _name)
                            break;
                    }

                    if (!active[i]) {
                        // chest should not be active, make it inactive
                        go[pos].SetActive(false);  
                    }
                    else {
                        // chest should be active

                        // set chest contents
                        int[] bronzeRates = { 1, 2, 2, 2, 2, 3, 3, 3, 4 };
                        int[] silverRates = { 1, 1, 1, 2, 2, 3 };
                        int[] goldRates = { 0, 0, 1, 1, 1, 1, 2, 2 };
                        int[] diamondRates = { 0, 0, 0, 1, 1 };

                        Chest _chest = go[pos].GetComponent<Chest>();

                        _chest.coinAmount = Random.Range(25, 50);

                        _chest.bronzeArmorAmount = bronzeRates[Random.Range(0, bronzeRates.Length)];
                        _chest.silverArmorAmount = silverRates[Random.Range(0, silverRates.Length)];
                        _chest.goldArmorAmount = goldRates[Random.Range(0, goldRates.Length)];
                        _chest.diamondArmorAmount = diamondRates[Random.Range(0, diamondRates.Length)];

                        _chest.bronzeWeaponAmount = bronzeRates[Random.Range(0, bronzeRates.Length)];
                        _chest.silverWeaponAmount = silverRates[Random.Range(0, silverRates.Length)];
                        _chest.goldWeaponAmount = goldRates[Random.Range(0, goldRates.Length)];
                        _chest.diamondWeaponAmount = diamondRates[Random.Range(0, diamondRates.Length)];

                        // make object active
                        go[pos].SetActive(true);

                    }
                }

                break;
            case "EternalDungeon":
                // EternalDungeon settings
                GameObject.FindWithTag("VictoryChest").SetActive(false);
                break;
            case "BossDungeon":
                // BossDungeon settings
                GameObject.FindWithTag("VictoryChest").SetActive(false);
                GameObject.FindWithTag("2ndLevelPortal").SetActive(false);
                break;
        }
    }

    ///////////////////////// RESET PROGRESS /////////////////////////
    public void resetProgress() {
        //// reset inventory
        // reset coin amount
        inventory.coins = 0;

        // reset armors
        inventory.bronzeArmors = 0;
        inventory.silverArmors = 0;
        inventory.goldArmors = 0;
        inventory.diamondArmors = 0;

        // reset weapons
        inventory.bronzeWeapons = 0;
        inventory.silverWeapons = 0;
        inventory.goldWeapons = 0;
        inventory.diamondWeapons = 0;

        // reset health potions amount
        inventory.healthPotions = 0;

        // reset player health and experience
        player.maxHitPoint = 10;
        player.hitPoint = player.maxHitPoint;
        experience = 0;

        // reset avatar and weapon choices
        chosenAvatar = 0;
        chosenWeapon = 0;

        // reset last position
        lastPosition = Vector3.zero;

        onInventoryChange();
        onHitpointChange();
        saveState();
        showText("Progress was reset!", 20, Color.red, player.transform.position, Vector3.up, 2.5f);
    }

    ///////////////////////// SAVE AND LOAD /////////////////////////
    public void saveState() {
        string s = "";

        s += chosenAvatar.ToString() + "|";                 // 0

        // save inventory
        s += inventory.coins.ToString() + "|";              // 1
        s += inventory.healthPotions.ToString() + "|";      // 2

        s += inventory.bronzeArmors.ToString() + "|";       // 3
        s += inventory.silverArmors.ToString() + "|";       // 4
        s += inventory.goldArmors.ToString() + "|";         // 5
        s += inventory.diamondArmors.ToString() + "|";      // 6

        s += inventory.bronzeWeapons.ToString() + "|";      // 7
        s += inventory.silverWeapons.ToString() + "|";      // 8
        s += inventory.goldWeapons.ToString() + "|";        // 9
        s += inventory.diamondWeapons.ToString() + "|";     // 10

        // save player experience
        s += experience.ToString() + "|";                   // 11

        // save player health and max health
        s += player.hitPoint.ToString() + "|";              // 12
        s += player.maxHitPoint.ToString() + "|";           // 13

        // save weapon level and type
        s += weapon.weaponLevel.ToString() + "|";           // 14
        s += chosenWeapon.ToString() + "|";                 // 15

        // save armor level
        s += armorLevel.ToString();                         // 16

        PlayerPrefs.SetString("saveState", s);
    }

    // load game state method
    public void loadState(Scene s, LoadSceneMode mode) {

        SceneManager.sceneLoaded -= loadState;
        if (!PlayerPrefs.HasKey("saveState"))
            return;

        // split it into "0", "15", "20", "0"
        string[] data = PlayerPrefs.GetString("saveState").Split('|');

        chosenAvatar = int.Parse(data[0]);
        
        // load inventory
        inventory.coins = int.Parse(data[1]);
        inventory.healthPotions = int.Parse(data[2]);

        inventory.bronzeArmors = int.Parse(data[3]);
        inventory.goldArmors = int.Parse(data[4]);
        inventory.silverArmors = int.Parse(data[5]);
        inventory.diamondArmors = int.Parse(data[6]);

        inventory.bronzeWeapons = int.Parse(data[7]);
        inventory.silverWeapons = int.Parse(data[8]);
        inventory.goldWeapons = int.Parse(data[9]);
        inventory.diamondWeapons = int.Parse(data[10]);

        // player Experience and level
        experience = int.Parse(data[11]);
        if (getCurrentLevel() != 1)
            player.setLevel(getCurrentLevel());

        // player health and max health
        player.hitPoint = int.Parse(data[12]);
        player.maxHitPoint = int.Parse(data[13]);

        // weapon
        weapon.setWeaponLevel(int.Parse(data[14]));
        chosenWeapon = int.Parse(data[15]);

        // armor
        armorLevel = int.Parse(data[16]);
        //setArmorStats();

        onInventoryChange();
        onHitpointChange();
    }
}
