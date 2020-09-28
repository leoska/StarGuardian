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

    public void PlayerAttack()
    {
        if (_attackTime <= 0)
        {
            Quaternion rotation = _transform.rotation;
            rotation.z = 1f;
            Instantiate<GameObject>(laser, new Vector3(_transform.position.x + 0.8f, _transform.position.y , 0), rotation);
            _attackTime = AttackCooldown;
        }
    }

    private void UpMove()
    {

    }

    private void DownMove()
    {

    }
}
