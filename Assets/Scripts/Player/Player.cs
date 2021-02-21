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
    public float ScaleAnimationSpeed = 0.5f;

    [Header("Laser Game Object")]
    public GameObject laser;

    [Header("Rocket")]
    public GameObject rocket;
    public float RocketCooldown = 5f;
    private float _rocketTime = 0f;

    [Header("Shield")]
    public GameObject Shield;
    private float _shieldTime = 0f;
    private float _shieldCooldawn = 10f;

    [Header("Limits")]
    public Vector2 limit_y = new Vector2(-4.25f, 4.25f);
    public Vector2 limit_x = new Vector2(-7f, 7);

    public float dodgeCooldown = 7f;
    public float maxDodgeDuration = 5f;

    private float _walkTime = 0f, _attackTime = 0f, _dodgeTime = 0f, _dodgeTimer = 0f;
    private Vector2 _scaleDodgeLimits = new Vector2(1f, 1.5f);
    private dodgeScaleStatus scaleStatus;
    private Rigidbody2D _rigidbody2D;
    private Transform _transform;
    private BoxCollider2D _boxCollider2d;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        _boxCollider2d = GetComponent<BoxCollider2D>();
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


        // TODO: отрефакторить по методам проверки
        if (_shieldTime > 0)
            _shieldTime -= Time.deltaTime;

        if (_rocketTime > 0)
            _rocketTime -= Time.deltaTime;

        if (_dodgeTime > 0)
            _dodgeTime -= Time.deltaTime;

        if (_dodgeTimer > 0)
        {
            _dodgeTimer -= Time.deltaTime;

            if (scaleStatus == dodgeScaleStatus.Increase)
            {
                transform.localScale = new Vector3(transform.localScale.x + ScaleAnimationSpeed * Time.deltaTime, transform.localScale.y + ScaleAnimationSpeed * Time.deltaTime, 0f);
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x - ScaleAnimationSpeed * Time.deltaTime, transform.localScale.y - ScaleAnimationSpeed * Time.deltaTime, 0f);
            }

            if (transform.localScale.x <= _scaleDodgeLimits.x)
                scaleStatus = dodgeScaleStatus.Increase;
            if (transform.localScale.x >= _scaleDodgeLimits.y)
                scaleStatus = dodgeScaleStatus.Decrease;

            if (_dodgeTimer <= 0f)
            {
                PlayerFinishDodge();
                _dodgeTimer = 0f;
            }
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
        if (_shieldTime <=0 && !Shield.activeSelf)
        {
            Shield.SetActive(!Shield.activeSelf);
        }
    }

    public void ShieldCooldawn ()
    {
        _shieldTime = _shieldCooldawn;
    }

    public void PlayerRocket ()
    {
        if (_rocketTime<=0)
        {
            Quaternion rotation = _transform.rotation;
            Instantiate<GameObject>(rocket, new Vector3(_transform.position.x + 0.8f, _transform.position.y, 1f), rotation);
            _rocketTime = RocketCooldown;
        }
    }

    public void PlayerDodge()
    {
        if (_dodgeTime<=0)
        {
            _boxCollider2d.enabled = false;
            _dodgeTime = dodgeCooldown;
            _dodgeTimer = maxDodgeDuration;
            scaleStatus = dodgeScaleStatus.Increase;
        }
    }

    private void PlayerFinishDodge()
    {
        _boxCollider2d.enabled = true;
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    enum dodgeScaleStatus
    {
        Increase = 0,
        Decrease = 1
    }
}
