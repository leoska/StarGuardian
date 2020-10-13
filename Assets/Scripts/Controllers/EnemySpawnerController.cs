using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    [Header("Limits")]
    public float min_Y;
    public float max_Y;

    [Header("Enemy prefabs")]
    public GameObject[] enemyPrefabs;

    [Header("Trash prefabs")]
    public GameObject[] trashPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
