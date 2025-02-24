using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EternalController : MonoBehaviour
{
    public static EternalController instance;

    public GameObject smallEnemy;
    public GameObject mediumEnemy;
    public GameObject largeEnemy;

    private BoxCollider2D smallBoxCollider;
    private BoxCollider2D mediumBoxCollider;
    private BoxCollider2D largeBoxCollider;

    private float lastWaveSpawned;

    public bool stopSpawning;

    public int smallEnemiesCount;
    public int mediumEnemiesCount;
    public int largeEnemiesCount;

    private Vector3 enemiesSpawn;
    private void Start()
    {
        // make the instance a Singleton
        if (EternalController.instance != null) {
            Destroy(gameObject);
            return;
        }
        else {
            // find the enemies spawn position
            enemiesSpawn = GameObject.Find("EnemySpawnpoint").transform.position;

            // get the enemies size
            smallBoxCollider = smallEnemy.GetComponent<BoxCollider2D>();
            mediumBoxCollider = mediumEnemy.GetComponent<BoxCollider2D>();
            largeBoxCollider = largeEnemy.GetComponent<BoxCollider2D>();

            // starting wave number of enemies
            smallEnemiesCount = 4;
            mediumEnemiesCount = 1;
            largeEnemiesCount = 0;

            lastWaveSpawned = 0;
            stopSpawning = false;

            // spawn the first wave
            StartCoroutine(printMsgAndSpawn());
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // check if the trigger to stop spawning was set
        if (!stopSpawning) {

            if (Time.time - lastWaveSpawned > 15.0f) {
                // save the time
                lastWaveSpawned = Time.time;

                // increase the wave difficulty
                smallEnemiesCount++;
                mediumEnemiesCount++;
                largeEnemiesCount++;

                // spawn another wave of enemies
                StartCoroutine(printMsgAndSpawn());
            }
        }
    }

    public void spawnEnemies(int smallCount, int mediumCount, int largeCount) {
        // spawn small enemies
        Vector3 randomPos;
        RaycastHit2D valid;

        for (int i = 0; i < smallCount; i++) {
            randomPos = Vector3.zero;
            do {
                randomPos = getRandomPosition(enemiesSpawn);
                valid = Physics2D.BoxCast(new Vector2(randomPos.x, randomPos.y), smallBoxCollider.size, 0, new Vector2(randomPos.x, randomPos.y), 0.0f, LayerMask.GetMask("Actor", "Blocking"));
            } while (valid.collider != null);
            Instantiate(smallEnemy, randomPos, Quaternion.identity);

            // increment the enemies alive counter
            GameManager.instance.enemiesAlive++;
        }

        // spawn medium enemies
        for (int i = 0; i < mediumCount; i++) {
            randomPos = Vector3.zero;
            do {
                randomPos = getRandomPosition(enemiesSpawn);
                valid = Physics2D.BoxCast(new Vector2(randomPos.x, randomPos.y), mediumBoxCollider.size, 0, new Vector2(randomPos.x, randomPos.y), 0.0f, LayerMask.GetMask("Actor", "Blocking"));
            } while (valid.collider != null);
            Instantiate(mediumEnemy, randomPos, Quaternion.identity);

            // increment the enemies alive counter
            GameManager.instance.enemiesAlive++;
        }

        // spawn large enemies
        for (int i = 0; i < largeCount; i++) {
            randomPos = Vector3.zero;
            do {
                randomPos = getRandomPosition(enemiesSpawn);
                valid = Physics2D.BoxCast(new Vector2(randomPos.x, randomPos.y), largeBoxCollider.size, 0, Vector2.zero, 0, LayerMask.GetMask("Actor", "Blocking"));
            } while (valid.collider != null);
            Instantiate(largeEnemy, randomPos, Quaternion.identity);

            // increment the enemies alive counter
            GameManager.instance.enemiesAlive++;
        }

    }

    // get a random position from the given value withing +/-1.5f tolerance
    private Vector3 getRandomPosition(Vector3 position) {
        return new Vector3(position.x + Random.Range(-1.5f, 1.5f), position.y + Random.Range(-1.5f, 1.5f), position.z);
    }

    // method to print messages with delay to print the player on the incoming wave spawning
    IEnumerator printMsgAndSpawn() {
        GameManager.instance.showText("New wave incoming in 3...", 25, Color.red, GameManager.instance.player.transform.position, Vector3.up * 25, 1f);
        yield return new WaitForSeconds(1);

        GameManager.instance.showText("2...", 25, Color.red, GameManager.instance.player.transform.position, Vector3.up * 25, 1f);
        yield return new WaitForSeconds(1);

        GameManager.instance.showText("1...", 25, Color.red, GameManager.instance.player.transform.position, Vector3.up * 25, 1f);
        yield return new WaitForSeconds(1);

        GameManager.instance.showText("Wave spawned!", 25, Color.red, GameManager.instance.player.transform.position, Vector3.up * 25, 1f);
        yield return new WaitForSeconds(1);

        // call the spawn enemies method after the warning messages were printed
        spawnEnemies(smallEnemiesCount, mediumEnemiesCount, largeEnemiesCount);
        yield return new WaitForSeconds(1);
    }
}
