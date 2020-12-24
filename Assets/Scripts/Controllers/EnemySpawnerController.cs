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
        Invoke("SpawnTrash", timer*3);
    }

    // Update is called once per frame
    void Update()
    {


    }
    void SpawnEnemy()
    {
        float pos_y = Random.Range(min_Y, max_Y);
        Vector3 pos = transform.position;
        pos.y = pos_y;
        int EnemyIndex = Random.Range(0, enemyPrefabs.Length);
        Instantiate<GameObject>(enemyPrefabs[EnemyIndex], pos, Quaternion.Euler(0f,0f,0f));
        Invoke("SpawnEnemy", timer);
    }

    void SpawnTrash ()
    {
        float pos_y = Random.Range(min_Y, max_Y);
        Vector3 pos = transform.position;
        pos.y = pos_y;
        int TrashIndex = Random.Range(0, trashPrefabs.Length);
        Instantiate<GameObject>(trashPrefabs[TrashIndex], pos, Quaternion.Euler(0f, 0f, 0f));
        Invoke("SpawnTrash", timer*3);
    }
}
