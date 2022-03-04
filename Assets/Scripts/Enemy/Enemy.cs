using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public Vector2 timer_limits = new Vector2 (0.2f, 1f);
    public GameObject EnemyLaser;
    public float speed = 2f;
    public float bounx_x = -11f;
    public Vector2 x_limits = new Vector2(-1.9f, 1.9f);

    private VerticalDirection _dir;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Shoot", 2f);
        _dir = (VerticalDirection) Random.Range(0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y -= speed * Time.deltaTime;
        pos.x += _dir == VerticalDirection.Left ? -speed * Time.deltaTime : speed * Time.deltaTime;
        if (pos.x < x_limits.x)
        {
            _dir = VerticalDirection.Right;
            pos.x = x_limits.x;

        }
        else if (pos.x > x_limits.y)
        {
            _dir = VerticalDirection.Left;
            pos.x = x_limits.y;
        }
        transform.position = pos;
        if (pos.y < bounx_x)
            Destroy(gameObject);

    }

    private void OnDestroy()
    {
        if (App.Instance.gameController.enemiesOnScreen.ContainsKey(gameObject.GetInstanceID()))
            App.Instance.gameController.enemiesOnScreen.Remove(gameObject.GetInstanceID());
    }

    void Shoot()
    {
        Quaternion rotation = transform.rotation;
        rotation.eulerAngles = new Vector3(0f, 0f, 270f);
        Instantiate<GameObject>(EnemyLaser, new Vector3(transform.position.x, transform.position.y - 0.8f, 0f), rotation);
        float timer = Random.Range(timer_limits.x, timer_limits.y);
        Invoke("Shoot", timer);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            App.Instance.gameController.GameOver();
        }
        
        if (collision.gameObject.tag == "FireZone")
        {
            App.Instance.gameController.enemiesOnScreen.Add(gameObject.GetInstanceID(), gameObject);
        }
    }

    public enum VerticalDirection
    {
        Left = 0,
        Right = 1,
    }
}
