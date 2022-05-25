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

    private int _topIndex;
    private int _downIndex;


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

        _downIndex = 0;
        _topIndex = _layers.Length - 1;
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
        _layers[_downIndex].position = Vector3.up * (_layers[_topIndex].position.y + backgroundSize);
        _topIndex = _downIndex;
        ++_downIndex;

        if (_downIndex == _layers.Length)
        {
            _downIndex = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _transform.Translate(new Vector3(0f, -SpeedWalk, 0f) * Time.deltaTime);

        /*if (_cameraTransform.position.x < (_layers[leftIndex].transform.position.x + viewZone))
        {
            ScrollLeft();
        }*/


        /*if (_cameraTransform.position.x > (_layers[rightIndex].transform.position.x + viewZone))
        {
            ScrollRight();
        }*/
        
        if (_cameraTransform.position.y > (_layers[_topIndex].transform.position.y + viewZone))
        {
            ScrollDown();
        }
    }
}
