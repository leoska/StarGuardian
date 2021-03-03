using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    public float speed = 1f;
    public float bounx_x = -11f;
    // Start is called before the first frame update
    void Start()
    {
        
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO: добавить коллизию для мусора
        if (collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            App.Instance.gameController.GameOver();
        }
    }
}
