using System.Collections.Generic;
using UnityEngine;

public class EnemySmartMove : MonoBehaviour
{
    #region public values
    public Vector2 timer_limits = new Vector2 (0.2f, 1f);
    public GameObject EnemyLaser;
    public float speed = 2f;
    public float bounx_x = -11f;
    public Vector2 y_limits = new Vector2(-4.5f, 4.5f);
    public float widthSearchRect;
    public float timerStartDetect = 1f;
    #endregion

    #region private values
    private VerticalDirection _dir;
    private BoxCollider2D _boxCollider2d;
    private List<DetectionArea> _areasUpper;
    private List<DetectionArea> _areasBottom;
    private DetectionArea _areaForward;
    private float _heightSearchRect;
    private float _timer;
    private Vector3 _pointToMove;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Invoke("Shoot", 2f);
        _dir = (VerticalDirection) Random.Range(0, 2);
        _boxCollider2d = GetComponent<BoxCollider2D>();

        _heightSearchRect = _boxCollider2d.size.y;

        _timer = 0f;

        // Initialize detection areas
        _areasUpper = new List<DetectionArea>();
        _areasBottom = new List<DetectionArea>();

        _pointToMove = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 pos = transform.position;
        //pos.x -= speed * Time.deltaTime;
        //pos.y += _dir == VerticalDirection.Down ? -speed * Time.deltaTime : speed * Time.deltaTime;
        //if (pos.y < y_limits.x)
        //{
        //    _dir = VerticalDirection.Up;
        //    pos.y = y_limits.x;

        //}
        //else if (pos.y > y_limits.y)
        //{
        //    _dir = VerticalDirection.Down;
        //    pos.y = y_limits.y;
        //}
        //transform.position = pos;
        //if (pos.x < bounx_x)
        //    Destroy(gameObject);

        transform.position = Vector3.MoveTowards(transform.position, _pointToMove, speed * Time.deltaTime);

        _timer += Time.deltaTime;
        if (_timer >= timerStartDetect)
        {
            DetectionLasers();
            _timer = 0f;
        }

    }
    void Shoot()
    {
        Quaternion rotation = transform.rotation;
        rotation.eulerAngles = new Vector3(0f, 0f, 180f);
        Instantiate<GameObject>(EnemyLaser, new Vector3(transform.position.x - 0.8f, transform.position.y, 0), rotation);
        float timer = Random.Range(timer_limits.x, timer_limits.y);
        Invoke("Shoot", timer);
    }

    public void DetectionLasers()
    {
        string laserTag = "PlayerLaser";
        uint countTry = 3;

        float x1 = transform.position.x - widthSearchRect;
        float x2 = transform.position.x;
        float y1 = transform.position.y + _heightSearchRect / 2;
        float y2 = transform.position.y - _heightSearchRect / 2;
        float z = transform.position.z;

        // Calculate a 4 points for Forward Detection Area
        _areaForward.x1y1 = new Vector3(x1, y1, z);
        _areaForward.x1y2 = new Vector3(x1, y2, z);
        _areaForward.x2y1 = new Vector3(x2, y1, z);
        _areaForward.x2y2 = new Vector3(x2, y2, z);

        if (SearchGameObjectInArea(_areaForward, laserTag))
        {
            // If PlayerLaser detected in Forward Detection Area then to calculate next top and bottom areas
            float previousYTop = y1;
            float previousYBottom = y2;

            Debug.Log("DetectionLaser!!!");

            for (uint i = 0; i < countTry; ++i)
            {
                // Top Detection Area
                #region topDetectionArea
                Vector3 x1y1 = new Vector3(x1, previousYTop + _heightSearchRect, z);
                Vector3 x1y2 = new Vector3(x1, previousYTop, z);
                Vector3 x2y1 = new Vector3(x2, previousYTop + _heightSearchRect, z);
                Vector3 x2y2 = new Vector3(x2, previousYTop, z);

                previousYTop += _heightSearchRect;

                DetectionArea areaTop = new DetectionArea(x1y1, x1y2, x2y1, x2y2);
                #endregion

                // Bottom Detection Area
                #region bottomDetectionArea
                x1y1 = new Vector3(x1, previousYBottom, z);
                x1y2 = new Vector3(x1, previousYBottom - _heightSearchRect, z);
                x2y1 = new Vector3(x2, previousYBottom, z);
                x2y2 = new Vector3(x2, previousYBottom - _heightSearchRect, z);

                previousYBottom -= _heightSearchRect;
                DetectionArea areaBottom = new DetectionArea(x1y1, x1y2, x2y1, x2y2);
                #endregion

                _areasUpper.Add(areaTop);
                _areasBottom.Add(areaBottom);

                if (!SearchGameObjectInArea(areaTop, laserTag))
                {
                    _pointToMove = new Vector3(transform.position.x, areaTop.x1y1.y - _heightSearchRect / 2, 1);
                    break;
                }

                if (!SearchGameObjectInArea(areaBottom, laserTag))
                {
                    _pointToMove = new Vector3(transform.position.x, areaBottom.x1y2.y + _heightSearchRect / 2, 1);
                    break;
                }
                    
            }

            Invoke("ClearAreas", 0.7f);
        }
    }

    private void ClearAreas()
    {
        _areasUpper.Clear();
        _areasBottom.Clear();
    }

    private bool SearchGameObjectInArea(DetectionArea area, string tagObject)
    {
        var gameObjects = GameObject.FindGameObjectsWithTag(tagObject);

        foreach(var gmObject in gameObjects)
        {
            Vector3 gmPos = gmObject.transform.position;
            if (
                gmPos.x >= area.x1y1.x &&
                gmPos.x <= area.x2y1.x &&
                gmPos.y <= area.x1y1.y &&
                gmPos.y >= area.x1y2.y
                )
                return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        // Draw Detection Areas
        DrawGizmosArea(_areaForward, Color.yellow);

        foreach(DetectionArea area in _areasUpper)
            DrawGizmosArea(area, Color.blue);

        foreach (DetectionArea area in _areasBottom)
            DrawGizmosArea(area, Color.cyan);
    }

    private void DrawGizmosArea(DetectionArea area, Color color)
    {
        Gizmos.color = color;

        // Draw a rectangle
        Gizmos.DrawLine(area.x1y1, area.x2y1);
        Gizmos.DrawLine(area.x2y1, area.x2y2);
        Gizmos.DrawLine(area.x2y2, area.x1y2);
        Gizmos.DrawLine(area.x1y2, area.x1y1);
    }

    public enum VerticalDirection
    {
        Up = 0,
        Down = 1,
    }

    private struct DetectionArea
    {
        public Vector3 x1y1;
        public Vector3 x1y2;
        public Vector3 x2y1;
        public Vector3 x2y2;

        public DetectionArea(Vector3 _x1y1, Vector3 _x1y2, Vector3 _x2y1, Vector3 _x2y2)
        {
            x1y1 = _x1y1;
            x1y2 = _x1y2;
            x2y1 = _x2y1;
            x2y2 = _x2y2;
        }
    }
}
