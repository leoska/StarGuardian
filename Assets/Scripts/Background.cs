using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    [Header("Speed")]
    public float SpeedWalk = 2;

    private Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        _transform.Translate(new Vector3(-SpeedWalk, 0, 0) * Time.deltaTime);
    }
}
