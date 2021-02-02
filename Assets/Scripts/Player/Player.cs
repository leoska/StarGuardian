using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Speeds")]
    public float WalkSpeed = 3;
    public float WalkCooldown = 0.005f;
    public float AttackCooldown = 0.2f;

    [Header("Laser Game Object")]
    public GameObject laser;

    [Header("Shield")]
    public GameObject Shield;
    private float _shieldTime = 0f;
    private float _shieldCooldawn = 10f;

    [Header("Limits")]
    public Vector2 limit_y = new Vector2(-4.25f, 4.25f);
    public Vector2 limit_x = new Vector2(-7f, 7);

    private float _walkTime = 0f, _attackTime = 0f;
    private Rigidbody2D _rigidbody2D;
    private Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_walkTime > 0)
            _walkTime -= Time.deltaTime;

        if (_attackTime > 0)
            _attackTime -= Time.deltaTime;

        if (_walkTime <= 0)
        {
            _rigidbody2D.velocity = new Vector2(0f, 0f);
            _walkTime = 0;
        }

        if (_shieldTime > 0)
            _shieldTime -= Time.deltaTime;
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

            // Check Min and Max X and Y position
            Vector3 pos = _transform.position;
            bool hasChange = false;

            if (pos.y > limit_y.y)
            {
                pos.y = limit_y.y;
                hasChange = true;
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0f);
            }
            else if (pos.y < limit_y.x)
            {
                pos.y = limit_y.x;
                hasChange = true;
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0f);
            }

            if (pos.x > limit_x.y)
            {
                pos.x = limit_x.y;
                hasChange = true;
                _rigidbody2D.velocity = new Vector2(0f, _rigidbody2D.velocity.y);
            }
            else if (pos.x < limit_x.x)
            {
                pos.x = limit_x.x;
                hasChange = true;
                _rigidbody2D.velocity = new Vector2(0f, _rigidbody2D.velocity.y);
            }

            if (hasChange)
                _transform.position = pos;
        }
        else
        {
            _rigidbody2D.velocity = new Vector2(0f, 0f);
        }
    }

    public void PlayerAttack()
    {
        if (_attackTime <= 0)
        {
            Quaternion rotation = _transform.rotation;
            Instantiate<GameObject>(laser, new Vector3(_transform.position.x + 0.8f, _transform.position.y , 0), rotation);
            _attackTime = AttackCooldown;

            // Play the sound FX of shoot laser PEW PEW!!!
        }
    }

    private void UpMove()
    {

    }

    private void DownMove()
    {

    }

    public void PlayerShield ()
    {
        if (_shieldTime <=0)
        {
            Shield.SetActive(!Shield.activeSelf);
            _shieldTime = _shieldCooldawn;
        }
    }

}
