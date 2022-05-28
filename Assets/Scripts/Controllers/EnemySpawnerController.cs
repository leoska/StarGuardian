﻿using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class EnemySpawnerController : MonoBehaviour
    {
        [Header("Limits")]
        public Vector2 range_limits = new Vector2(-1.9f, 1.9f);
        public float timer = 2f;
        public float timerTrash = 30f;

        [Header("Enemy prefabs")]
        public GameObject[] enemyPrefabs;

        [Header("Trash prefabs")]
        public GameObject[] trashPrefabs;

        // Start is called before the first frame update
        void Start()
        {
            Invoke("SpawnEnemy", timer);
            Invoke("SpawnTrash", timerTrash);
        }

        // Update is called once per frame
        void Update()
        {


        }
        void SpawnEnemy()
        {
            float pos_x = Random.Range(range_limits.x, range_limits.y);
            Vector3 pos = transform.position;
            pos.x = pos_x;
            int EnemyIndex = Random.Range(0, enemyPrefabs.Length);
            Instantiate<GameObject>(enemyPrefabs[EnemyIndex], pos, Quaternion.Euler(0f,0f,90f));
            Invoke("SpawnEnemy", timer);
        }

        void SpawnTrash ()
        {
            float pos_x = Random.Range(range_limits.x, range_limits.y);
            Vector3 pos = transform.position;
            pos.x = pos_x;
            int TrashIndex = Random.Range(0, trashPrefabs.Length);
            Instantiate<GameObject>(trashPrefabs[TrashIndex], pos, Quaternion.Euler(0f, 0f, 0f));
            Invoke("SpawnTrash", timerTrash);
        }
    }
}
