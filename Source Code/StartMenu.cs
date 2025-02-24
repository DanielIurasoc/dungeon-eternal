using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{

    public void onStartMenuButtonClick(bool start) {

        if (start) {
            // start button was clicked

            // reactivate the player object
            GameObject[] GO = GameObject.FindObjectsOfType<GameObject>(true);
            foreach (var item in GO) {
                if (item.tag == "Fighter" && item.name == "Player") {
                    if(!item.gameObject.activeInHierarchy)
                        item.SetActive(true);
                    GameManager.instance.player.lastImmune = Time.time;
                    break;
                }
            }

            UnityEngine.SceneManagement.SceneManager.LoadScene("MainDungeon");
        }
        else {
            // quit button was clicked
            // might not work and need replaced by another solution
            Application.Quit();
        }
    }
}
