using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Trigger : Collidable
{
    // enemy and chest prefabs
    public GameObject smallEnemy;
    public GameObject mediumEnemy;
    public GameObject largeEnemy;
    public GameObject bossEnemy;
    public GameObject chest;

    // Logic
    public int triggerId;
    private bool trigger1Used = false, trigger2Used = false, trigger3Used = false;
    private bool chest1Spawned = false, chest2Spawned = false, chest3Spawned = false;

    protected override void onCollide(Collider2D collider) {
        if(collider.name == "Player") {
            // call spawnEnemies function if the trigger wasn't already used
            if((triggerId == 1 && trigger1Used != true) ||
               (triggerId == 2 && trigger2Used != true) ||
               (triggerId == 3 && trigger3Used != true)) {
                spawnEnemies(triggerId);
            }

            // update the lastPosition on collide with the trigger
            GameManager.instance.lastPosition = GameManager.instance.player.transform.position;
        }
    }

    protected override void Update() {
        base.Update();

        // check for enemies alive
        if(GameManager.instance.enemiesAlive == 0) {
            // no more enemies are left alive, victory chest can be spawned
            spawnVictoryChest(triggerId);
        }
    }

    private void spawnEnemies(int triggerId) {
        // position where the enemies should be spawned around
        Vector3 enemiesSpawn;
        switch(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name) {
            case "MainDungeon":
                switch (triggerId) {
                    case 1:
                        // first wave of enemies
                        // 5 small enemies, 1 medium enemy

                        // get enemies spawn position
                        enemiesSpawn = GameObject.Find("EnemySpawn_1").transform.position;

                        // spawn the small enemies
                        for (int i = 0; i < 5; i++) {
                            Instantiate(smallEnemy, getRandomPosition(enemiesSpawn), Quaternion.identity);
                        }

                        // spawn the medium enemy
                        Instantiate(mediumEnemy, getRandomPosition(enemiesSpawn), Quaternion.identity);

                        // update the GameManager counter
                        GameManager.instance.enemiesAlive += 6;

                        // make the trigger inactive
                        trigger1Used = true;
                        break;

                    case 2:
                        // second wave of enemies
                        // 1 small enemies, 5 medium enemy, 1 large enemy

                        // get enemies spwan position
                        enemiesSpawn = GameObject.Find("EnemySpawn_2").transform.position;

                        // spawn the small enemy
                        Instantiate(smallEnemy, getRandomPosition(enemiesSpawn), Quaternion.identity);

                        // spawn the medium enemies
                        for (int i = 0; i < 5; i++) {
                            Instantiate(mediumEnemy, getRandomPosition(enemiesSpawn), Quaternion.identity);
                        }

                        // spawn the large enemy
                        Instantiate(largeEnemy, getRandomPosition(enemiesSpawn), Quaternion.identity);

                        // update the GameManager counter
                        GameManager.instance.enemiesAlive += 7;

                        // make the trigger inactive
                        trigger2Used = true;
                        break;

                    case 3:
                        // third wave of enemies
                        // 1 small enemies, 2 medium enemy, 6 large enemy

                        // get enemies spwan position
                        enemiesSpawn = GameObject.Find("EnemySpawn_3").transform.position;

                        // spawn the small enemy
                        Instantiate(smallEnemy, getRandomPosition(enemiesSpawn), Quaternion.identity);

                        // spawn the medium enemies
                        for (int i = 0; i < 2; i++) {
                            Instantiate(mediumEnemy, getRandomPosition(enemiesSpawn), Quaternion.identity);
                        }

                        // spawn the large enemies
                        for (int i = 0; i < 6; i++) {
                            Instantiate(largeEnemy, getRandomPosition(enemiesSpawn), Quaternion.identity);
                        }

                        // update the GameManager counter
                        GameManager.instance.enemiesAlive += 9;

                        // make the trigger inactive
                        trigger3Used = true;
                        break;
                }

                break;
            case "BossDungeon":

                if(triggerId == 1) {
                    // get enemies spawn position
                    enemiesSpawn = GameObject.Find("BossSpawnpoint").transform.position;

                    // spawn the boss enemy
                    Instantiate(bossEnemy, enemiesSpawn, Quaternion.identity);

                    // update the GameManager counter
                    GameManager.instance.enemiesAlive++;

                    // make the trigger inactive
                    trigger1Used = true;
                }
                
                break;
        }
        
    }

    // get a random position from the given value withing +/-1.0f horizontal and +/-0.5f vertical tolerance
    private Vector3 getRandomPosition(Vector3 position) {
        return new Vector3(position.x + Random.Range(-1.0f, 1.0f), position.y + Random.Range(-0.5f, 0.5f), position.z);
    }

    // spawn the victory chest
    private void spawnVictoryChest(int triggerId) {

        switch (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name) {
            case "MainDungeon":

                // no enemies exist anymore, chest can be spanwned
                switch (triggerId) {
                    case 1:
                        // check if the chest wasn't already spawned somehow
                        if (chest1Spawned != true && trigger1Used == true) {

                            // chest reward rates
                            int[] bronzeRates1 = { 1, 2, 2, 2, 2, 3, 3, 3, 4 };
                            int[] silverRates1 = { 1, 1, 1, 2, 2, 3 };
                            int[] goldRates1 = { 0, 0, 1, 1, 1, 1, 2, 2 };
                            int[] diamondRates1 = { 0, 0, 0, 1 };

                            // find the desired Victory Chest game object
                            GameObject[] GO = GameObject.FindObjectsOfType<GameObject>(true);
                            foreach (var item in GO) {
                                if (!item.gameObject.activeInHierarchy) {
                                    if (item.tag == "VictoryChest" && item.name == "VChest1") {
                                        // get the chest object
                                        Chest chest1 = item.GetComponent<Chest>();

                                        // give chest properties
                                        chest1.coinAmount = 30;

                                        // bronze reward
                                        int rate = Random.Range(0, bronzeRates1.Length);
                                        if (rate % 2 == 0)
                                            chest1.bronzeArmorAmount = bronzeRates1[rate];
                                        else
                                            chest1.bronzeWeaponAmount = bronzeRates1[rate];

                                        // silver reward
                                        rate = Random.Range(0, silverRates1.Length);
                                        if (rate % 2 == 0)
                                            chest1.silverArmorAmount = silverRates1[rate];
                                        else
                                            chest1.silverWeaponAmount = silverRates1[rate];

                                        // gold reward
                                        rate = Random.Range(0, goldRates1.Length);
                                        if (rate % 2 == 0)
                                            chest1.goldArmorAmount = goldRates1[rate];
                                        else
                                            chest1.goldWeaponAmount = goldRates1[rate];

                                        // diamond reward
                                        rate = Random.Range(0, diamondRates1.Length);
                                        if (rate % 2 == 0)
                                            chest1.diamondArmorAmount = diamondRates1[rate];
                                        else
                                            chest1.diamondWeaponAmount = diamondRates1[rate];

                                        // make the object active
                                        item.SetActive(true);

                                        // keep track that chest was spawned
                                        chest1Spawned = true;
                                    }
                                }
                            }
                        }
                        break;
                    case 2:
                        // check if the chest wasn't already spawned somehow
                        if (chest2Spawned != true && trigger2Used == true) {

                            // chest reward rates
                            int[] bronzeRates2 = { 1, 1, 2, 2, 2, 2, 3, 3, 4 };
                            int[] silverRates2 = { 1, 1, 2, 2, 2, 2, 3 };
                            int[] goldRates2 = { 0, 1, 1, 2, 2, 2, 2, 3, 3 };
                            int[] diamondRates2 = { 0, 0, 0, 1, 1 };

                            // find the desired Victory Chest game object
                            GameObject[] GO = GameObject.FindObjectsOfType<GameObject>(true);
                            foreach (var item in GO) {
                                if (!item.gameObject.activeInHierarchy) {
                                    if (item.tag == "VictoryChest" && item.name == "VChest2") {
                                        // get the chest object
                                        Chest chest2 = item.GetComponent<Chest>();

                                        // give chest properties
                                        chest2.coinAmount = 30;

                                        // bronze reward
                                        int rate = Random.Range(0, bronzeRates2.Length);
                                        if (rate % 2 == 0)
                                            chest2.bronzeArmorAmount = bronzeRates2[rate];
                                        else
                                            chest2.bronzeWeaponAmount = bronzeRates2[rate];

                                        // silver reward
                                        rate = Random.Range(0, silverRates2.Length);
                                        if (rate % 2 == 0)
                                            chest2.silverArmorAmount = silverRates2[rate];
                                        else
                                            chest2.silverWeaponAmount = silverRates2[rate];

                                        // gold reward
                                        rate = Random.Range(0, goldRates2.Length);
                                        if (rate % 2 == 0)
                                            chest2.goldArmorAmount = goldRates2[rate];
                                        else
                                            chest2.goldWeaponAmount = goldRates2[rate];

                                        // diamond reward
                                        rate = Random.Range(0, diamondRates2.Length);
                                        if (rate % 2 == 0)
                                            chest2.diamondArmorAmount = diamondRates2[rate];
                                        else
                                            chest2.diamondWeaponAmount = diamondRates2[rate];

                                        // make the object active
                                        item.SetActive(true);

                                        // keep track that chest was spawned
                                        chest1Spawned = true;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        // check if the chest wasn't already spawned somehow
                        if (chest3Spawned != true && trigger3Used == true) {

                            // chest reward rates
                            int[] bronzeRates3 = { 1, 2, 2, 2, 3, 3, 3, 4, 4 };
                            int[] silverRates3 = { 1, 1, 2, 2, 3, 3 };
                            int[] goldRates3 = { 0, 1, 1, 1, 2, 2, 3, 4 };
                            int[] diamondRates3 = { 0, 0, 1, 1, 1 };

                            // find the desired game objects : Victory Chest, Victory NPC, the portals
                            GameObject[] GO = GameObject.FindObjectsOfType<GameObject>(true);
                            foreach (var item in GO) {
                                if (!item.gameObject.activeInHierarchy) {
                                    if (item.tag == "EternalPortal" || item.tag == "BossPortal" ||
                                        item.tag == "TreasurePortal" || item.tag == "VictoryNPC1") {
                                        item.SetActive(true);
                                    }
                                    // spawn the victory chest
                                    else if (item.tag == "VictoryChest" && item.name == "VChest3") {
                                        // get the chest object
                                        Chest chest3 = item.GetComponent<Chest>();

                                        // give chest properties
                                        chest3.coinAmount = 30;

                                        // bronze reward
                                        int rate = Random.Range(0, bronzeRates3.Length);
                                        if (rate % 2 == 0)
                                            chest3.bronzeArmorAmount = bronzeRates3[rate];
                                        else
                                            chest3.bronzeWeaponAmount = bronzeRates3[rate];

                                        // silver reward
                                        rate = Random.Range(0, silverRates3.Length);
                                        if (rate % 2 == 0)
                                            chest3.silverArmorAmount = silverRates3[rate];
                                        else
                                            chest3.silverWeaponAmount = silverRates3[rate];

                                        // gold reward
                                        rate = Random.Range(0, goldRates3.Length);
                                        if (rate % 2 == 0)
                                            chest3.goldArmorAmount = goldRates3[rate];
                                        else
                                            chest3.goldWeaponAmount = goldRates3[rate];

                                        // diamond reward
                                        rate = Random.Range(0, diamondRates3.Length);
                                        if (rate % 2 == 0)
                                            chest3.diamondArmorAmount = diamondRates3[rate];
                                        else
                                            chest3.diamondWeaponAmount = diamondRates3[rate];

                                        // make the object active
                                        item.SetActive(true);

                                        // keep track that chest was spawned
                                        chest3Spawned = true;
                                    }
                                }
                            }
                        }
                        break;
                }

                break;
            case "BossDungeon":
                if(triggerId == 1) {
                    if(chest1Spawned != true && trigger1Used == true) {
                        // chest reward rates
                        int[] bronzeRates1 = { 1, 2, 2, 2, 2, 3, 3, 3, 4 };
                        int[] silverRates1 = { 1, 1, 1, 2, 2, 3 };
                        int[] goldRates1 = { 0, 1, 1, 1, 2, 2, 2, 3 };
                        int[] diamondRates1 = { 0, 0, 1, 1, 2 };

                        // find the desired Victory Chest game object
                        GameObject[] GO = GameObject.FindObjectsOfType<GameObject>(true);
                        foreach (var item in GO) {
                            if (!item.gameObject.activeInHierarchy) {
                                if (item.tag == "VictoryChest" && item.name == "VChest") {
                                    // get the chest object
                                    Chest chest1 = item.GetComponent<Chest>();

                                    // give chest properties
                                    chest1.coinAmount = 100;

                                    // bronze reward
                                    int rate = Random.Range(0, bronzeRates1.Length);
                                    if (rate % 2 == 0)
                                        chest1.bronzeArmorAmount = bronzeRates1[rate];
                                    else
                                        chest1.bronzeWeaponAmount = bronzeRates1[rate];

                                    // silver reward
                                    rate = Random.Range(0, silverRates1.Length);
                                    if (rate % 2 == 0)
                                        chest1.silverArmorAmount = silverRates1[rate];
                                    else
                                        chest1.silverWeaponAmount = silverRates1[rate];

                                    // gold reward
                                    rate = Random.Range(0, goldRates1.Length);
                                    if (rate % 2 == 0)
                                        chest1.goldArmorAmount = goldRates1[rate];
                                    else
                                        chest1.goldWeaponAmount = goldRates1[rate];

                                    // diamond reward
                                    rate = Random.Range(0, diamondRates1.Length);
                                    if (rate % 2 == 0)
                                        chest1.diamondArmorAmount = diamondRates1[rate];
                                    else
                                        chest1.diamondWeaponAmount = diamondRates1[rate];

                                    // make the object active
                                    item.SetActive(true);

                                    // keep track that chest was spawned
                                    chest1Spawned = true;
                                }
                                else if (item.tag == "2ndLevelPortal") {
                                    item.SetActive(true);
                                }
                            }
                        }
                    }
                }

                break;
        }

    }
}
