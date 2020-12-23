﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float timer_min = 0.2f;
    public float timer_max = 1f;
    public GameObject EnemyLaser;
    public float speed = 2f;
    public float bounx_x = -11f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Shoot", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.x -= speed * Time.deltaTime;

        transform.position = pos;
        if (pos.x < bounx_x)
            Destroy(gameObject);
    }
    void Shoot()
    {
        Quaternion rotation = transform.rotation;
        rotation.eulerAngles = new Vector3(0f, 0f, 180f);
        Instantiate<GameObject>(EnemyLaser, new Vector3(transform.position.x - 0.8f, transform.position.y, 0), rotation);
        float timer = Random.Range(timer_min, timer_max);
        Invoke("Shoot", timer);
    }
}
