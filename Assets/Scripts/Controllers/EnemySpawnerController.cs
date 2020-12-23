using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    [Header("Limits")]
    public float min_Y;
    public float max_Y;
    public float timer = 2f;

    [Header("Enemy prefabs")]
    public GameObject[] enemyPrefabs;

    [Header("Trash prefabs")]
    public GameObject[] trashPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("SpawnEnemy", timer);
    }

    // Update is called once per frame
    void Update()
    {


    }
    void SpawnEnemy()
    {
        float pos_y = Random.Range(min_Y, max_Y);
        Debug.Log(pos_y);
        Vector3 pos = transform.position;
        pos.y = pos_y;
        int EnemyIndex = Random.Range(0, enemyPrefabs.Length - 1);
        Instantiate<GameObject>(enemyPrefabs[EnemyIndex], pos, Quaternion.Euler(0f,0f,0f));
        Invoke("SpawnEnemy", timer);
    }
}
