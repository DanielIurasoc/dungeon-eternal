using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : Collidable
{
    public string targetScene;

    protected override void onCollide(Collider2D collider) {
        if(collider.name == "Player") {
            // check if the scene is a main level
            if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "EternalDungeon" &&
               UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "TreasureDungeon" &&
               UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "BossDungeon") {

                // find the coordinates of the position in front of the portal
                // considering the player position the middle between the portal and the wanted position
                // math formula is : pos = 2 * player position - portal position 
                float posX = 2 * GameManager.instance.player.transform.position.x - transform.position.x;
                float posY = 2 * GameManager.instance.player.transform.position.y - transform.position.y;
                GameManager.instance.lastPosition = new Vector3(posX, posY, GameManager.instance.player.transform.position.z);
            }

            // save the game state
            GameManager.instance.saveState();

            // load the target scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(targetScene);
        }
    }
}
