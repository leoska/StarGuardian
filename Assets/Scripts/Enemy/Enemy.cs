using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector2 timer_limits = new Vector2 (0.2f, 1f);
    public GameObject EnemyLaser;
    public float speed = 2f;
    public float bounx_x = -11f;
    public Vector2 y_limits = new Vector2(-4.5f, 4.5f);

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
        pos.x -= speed * Time.deltaTime;
        pos.y += _dir == VerticalDirection.Down ? -speed * Time.deltaTime : speed * Time.deltaTime;
        if (pos.y < y_limits.x)
        {
            _dir = VerticalDirection.Up;
            pos.y = y_limits.x;

        }
        else if (pos.y > y_limits.y)
        {
            _dir = VerticalDirection.Down;
            pos.y = y_limits.y;
        }
        transform.position = pos;
        if (pos.x < bounx_x)
            Destroy(gameObject);

    }
    void Shoot()
    {
        Quaternion rotation = transform.rotation;
        rotation.eulerAngles = new Vector3(0f, 0f, 180f);
        Instantiate<GameObject>(EnemyLaser, new Vector3(transform.position.x - 0.8f, transform.position.y, 0), rotation);
        float timer = Random.Range(timer_limits.x, timer_limits.y);
        Invoke("Shoot", timer);
    }


    public enum VerticalDirection
    {
        Up = 0,
        Down = 1,
    }
}
