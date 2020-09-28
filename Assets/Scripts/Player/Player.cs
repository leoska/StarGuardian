using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Speeds")]
    public float WalkSpeed = 3;
    public float WalkCooldown = 0.005f;

    private float _walkTime = 0;
    private Rigidbody2D _rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _walkTime -= Time.deltaTime;

        if (_walkTime <= 0)
        {
            _rigidbody2D.velocity = new Vector2(0f, 0f);
            _walkTime = 0;
        }
    }

    public void PlayerMove(Vector3 Directional)
    {
        if (Directional.magnitude > 0)
        {
            Directional.Normalize();
            _walkTime = WalkCooldown;

            if (Directional.magnitude != 0)
            {
                _rigidbody2D.velocity = Directional * WalkSpeed;
            }
        }
        else
        {
            _rigidbody2D.velocity = new Vector2(0f, 0f);
        }
    }

    private void UpMove()
    {

    }

    private void DownMove()
    {

    }
}
