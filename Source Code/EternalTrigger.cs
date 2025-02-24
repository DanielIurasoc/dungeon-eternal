using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EternalTrigger : Collidable
{
    private bool triggerUsed = false;

    protected override void onCollide(Collider2D collider) {
        if (collider.name == "Player") {
            if(triggerUsed != true) {
                // stop spawning more enemies
                EternalController.instance.stopSpawning = true;

                // end the battle by spawning the rewards chest and killing the remaining enemies
                endBattle();
                killAllEnemies();

                // make the trigger inactive after being used
                triggerUsed = true;
            }
        }
    }

    private void endBattle() {
        // pre-determined rewards rates
        int[] bronzeRates = { 1, 2, 2, 2, 2, 3, 3, 3, 4 };
        int[] silverRates = { 1, 1, 1, 2, 2, 3 };
        int[] goldRates = { 0, 0, 1, 1, 1, 1, 2, 2 };
        int[] diamondRates = { 0, 0, 0, 1 };

        // find the number of enemies killed until this point
        int enemiesAlive = GameManager.instance.enemiesAlive;
        int totalSmallEnemies = EternalController.instance.smallEnemiesCount * (EternalController.instance.smallEnemiesCount + 1) / 2 - 6;
        int totalMediumEnemies = EternalController.instance.mediumEnemiesCount * (EternalController.instance.mediumEnemiesCount + 1) / 2;
        int totalLargeEnemies = EternalController.instance.largeEnemiesCount * (EternalController.instance.largeEnemiesCount + 1) / 2;
        int totalKilled = totalSmallEnemies + totalMediumEnemies + totalLargeEnemies - enemiesAlive;
        Debug.Log(totalSmallEnemies + " " + totalMediumEnemies + " " + totalLargeEnemies + " " + totalKilled);

        // calculate the rate of increase to the rewards based on the number of killed enemies
        int increaseRate;
        if (totalKilled < 10)
            increaseRate = 0;
        else if (totalKilled < 20)
            increaseRate = 1;
        else if (totalKilled < 30) 
            increaseRate = 2;
        else
            increaseRate = 3;

        // find the Victory chest game object
        GameObject[] GO = GameObject.FindObjectsOfType<GameObject>(true);
        foreach (var item in GO) {
            if (!item.gameObject.activeInHierarchy) {
                if (item.tag == "VictoryChest" && item.name == "VChest") {
                    // get the chest object
                    Chest _chest = item.GetComponent<Chest>();

                    // give chest properties
                    _chest.coinAmount = 30 * (increaseRate + 1);

                    _chest.bronzeArmorAmount = bronzeRates[Random.Range(0, bronzeRates.Length)] + increaseRate;
                    _chest.silverArmorAmount = silverRates[Random.Range(0, silverRates.Length)] + increaseRate;
                    _chest.goldArmorAmount = goldRates[Random.Range(0, goldRates.Length)] + increaseRate;
                    _chest.diamondArmorAmount = diamondRates[Random.Range(0, diamondRates.Length)] + increaseRate;

                    _chest.bronzeWeaponAmount = bronzeRates[Random.Range(0, bronzeRates.Length)] + increaseRate;
                    _chest.silverWeaponAmount = silverRates[Random.Range(0, silverRates.Length)] + increaseRate;
                    _chest.goldWeaponAmount = goldRates[Random.Range(0, goldRates.Length)] + increaseRate;
                    _chest.diamondWeaponAmount = diamondRates[Random.Range(0, diamondRates.Length)] + increaseRate;

                    // make the object active
                    item.SetActive(true);
                    break;
                }
            }
        }
    }

    public void killAllEnemies() {
        // destroy all remaining enemies
        GameObject[] GO = GameObject.FindGameObjectsWithTag("Fighter");
        foreach (var item in GO) {
            if (item.name != "Player")
                Destroy(item);
        }

        // reset the game manager counter
        GameManager.instance.enemiesAlive = 0;
    }
}
