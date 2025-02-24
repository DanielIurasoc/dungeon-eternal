using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Collidable
{
    private float lastShown1, lastShown2;

    protected override void Start() {
        base.Start();
        lastShown1 = 0;
        lastShown2 = 0;
    }

    protected override void Update() {
        base.Update();

        // check if the player is close to the NPC to show the tag name
        if(Vector3.Distance(transform.position, GameManager.instance.player.transform.position) < 1.5f) {
            // Wallace, Raymond, Horace

            // get the correct tag name
            string _name = "";
            if (tag == "TrainingNPC") {
                _name = "Greg";
            }
            else if(tag == "EternalNPC") {
                _name = "Edmund";
            }
            else if(tag == "VictoryNPC1") {
                _name = "Eugene";
            }
            
            // update the tag name as fast as possible, to make sure it is on top of the NPC
            if (Time.time - lastShown1 >= 0.00001f) {
                GameManager.instance.showText(_name, 25, Color.white, new Vector3(transform.position.x, transform.position.y + 0.15f, 0), Vector3.zero, 0.00001f);
                lastShown1 = Time.time;
            }
        }


    }
    protected override void onCollide(Collider2D collider) {

        // check to see which NPC it is
        if(tag == "TrainingNPC") {
            if(Time.time - lastShown2 > 3) {

                // create the messages wanted to show
                string[] messages = { 
                    "\nWelcome to Dungeon Eternal!\nTo move around, use W-A-S-D/up-left-down-right arrows", 
                    "\nTo attack press SPACE.\nFind chests to get crafting materials. Press the Hammer to upgrade your equipment and become stronger!", 
                    "Good luck, adventurer!" };

                // print the messages
                StartCoroutine(printMessages(messages, 3, 1.5f));

                // update the time
                lastShown2 = Time.time;
            }   
        }
        else if(tag == "EternalNPC") {
            if (Time.time - lastShown2 > 3) {

                // create the messages wanted to show
                string[] messages = {
                    "\nThis is the Eternal Dungeon, where you can fight endless waves of enemies",
                    "\nTo end the battle, go towards the portal\n A chest with rewards will spawn for you",
                    "Good luck, adventurer!" };

                // print the messages
                StartCoroutine(printMessages(messages, 3, 1.5f));

                // update the time
                lastShown2 = Time.time;
            }
        }
        else if (tag == "VictoryNPC1") {
            if (Time.time - lastShown2 > 3) {

                // create the messages wanted to show
                string[] messages = {
                    "\nCongratulations! You cleared all rooms!",
                    "\nThe portal to the right takes you to the Eternal Dungeon",
                    "\nThe portal to the left takes you to the Boss Dungeon",
                    "\nThere is a hidden portal in this room to a Treasure room, full of chests",
                    "Good luck, adventurer!" };

                // print the messages
                StartCoroutine(printMessages(messages, 4, 1.5f));

                // update the time
                lastShown2 = Time.time;
            }
        }
    }

    // method to show messages with delay
    IEnumerator printMessages(string[] msg, int count, float duration) {
        for (int i = 0; i < count; i++) {

            // print the message
            GameManager.instance.showText(msg[i], 20, Color.white, transform.position, Vector3.zero, duration);

            // wait for the desired duration between each message
            yield return new WaitForSeconds(duration);
        }
    }


}
