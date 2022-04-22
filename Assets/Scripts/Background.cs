using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    [Header("Speed")]
    public float SpeedWalk = 2;
    public float backgroundSize;

    private Transform _transform;
    private Transform _cameraTransform;
    private Transform[] _layers;
    private float viewZone = 10;
    private int leftIndex;
    private int rightIndex;


    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
        _cameraTransform = Camera.main.transform;
        _layers = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; ++i)
        {
            _layers[i] = transform.GetChild(i);
        }

        leftIndex = 0;
        rightIndex = _layers.Length - 1;
    }

    private void ScrollLeft()
    {
        _layers[rightIndex].position = Vector3.right * (_layers[leftIndex].position.x - backgroundSize);
        leftIndex = rightIndex;
        --rightIndex;

        if (rightIndex < 0)
        {
            rightIndex = _layers.Length - 1;
        }
    }

    private void ScrollRight()
    {
        _layers[leftIndex].position = Vector3.right * (_layers[rightIndex].position.x + backgroundSize);
        rightIndex = leftIndex;
        ++leftIndex;

        if (leftIndex == _layers.Length)
        {
            leftIndex = 0;
        }
    }

    private void ScrollDown()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _transform.Translate(new Vector3(-SpeedWalk, 0, 0) * Time.deltaTime);

        /*if (_cameraTransform.position.x < (_layers[leftIndex].transform.position.x + viewZone))
        {
            ScrollLeft();
        }*/


        if (_cameraTransform.position.x > (_layers[rightIndex].transform.position.x + viewZone))
        {
            ScrollRight();
        }
    }
}
