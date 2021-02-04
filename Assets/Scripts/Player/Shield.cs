using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public Player Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyLaser")
        {
            Destroy(collision.gameObject);
            gameObject.SetActive(!gameObject.activeSelf);
            Player.ShieldCooldawn();
        }

    }
}
