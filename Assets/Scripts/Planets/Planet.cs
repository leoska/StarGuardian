using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public float speed;
    public float boundX = -11f;

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
        if (pos.x < boundX)
            Destroy(gameObject);
    }
}
