using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [Header("Speeds")]
    [FormerlySerializedAs("WalkSpeed")] 
    public float walkSpeed = 3;
    [FormerlySerializedAs("WalkCooldown")] 
    public float walkCooldown = 0.005f;
    [FormerlySerializedAs("AttackCooldown")] 
    public float attackCooldown = 0.5f;
    private float _overheating;
    [FormerlySerializedAs("ScaleAnimationSpeed")] 
    public float scaleAnimationSpeed = 0.5f;

    [Header("Laser Game Object")]
    public GameObject laser;

    [Header("Rocket")]
    public GameObject rocket;
    [FormerlySerializedAs("RocketCooldown")] 
    public float rocketCooldown = 5f;
    private float _rocketTime = 0f;

    [FormerlySerializedAs("Shield")] [Header("Shield")]
    public GameObject shield;
    private float _shieldTime = 0f;
    private float _shieldCooldawn = 10f;

    [Header("Limits")]
    public Vector2 limit_y = new Vector2(-4.25f, 4.25f);
    public Vector2 limit_x = new Vector2(-7f, 7);
    
    [Header("Joystick")]
    public VariableJoystick variableJoystick;

    [Header("Dodge")]
    public float dodgeCooldown = 7f;
    public float maxDodgeDuration = 5f;

    private float _walkTime = 0f, _attackTime = 0f, _dodgeTime = 0f, _dodgeTimer = 0f;
    private Vector2 _scaleDodgeLimits = new Vector2(1f, 1.5f);
    private DodgeScaleStatus _scaleStatus;
    private Rigidbody2D _rigidbody2D;
    private Transform _transform;
    private BoxCollider2D _boxCollider2d;

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        _boxCollider2d = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_walkTime > 0)
            _walkTime -= Time.deltaTime;

        if (_attackTime > 0)
            _attackTime -= Time.deltaTime;

        if (_overheating > 0)
            _overheating -= Time.deltaTime;
        if (_overheating > 5f)
            attackCooldown = 5f;
        if (_overheating < 1f)
            attackCooldown = 0.3f;

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

            // Механика уворота
            if (_scaleStatus == DodgeScaleStatus.Increase)
            {
                transform.localScale = new Vector3(transform.localScale.x + scaleAnimationSpeed * Time.deltaTime, transform.localScale.y + scaleAnimationSpeed * Time.deltaTime, 0f);
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x - scaleAnimationSpeed * Time.deltaTime, transform.localScale.y - scaleAnimationSpeed * Time.deltaTime, 0f);
            }
            
            if (transform.localScale.x <= _scaleDodgeLimits.x)
                _scaleStatus = DodgeScaleStatus.Increase;
            if (transform.localScale.x >= _scaleDodgeLimits.y)
                _scaleStatus = DodgeScaleStatus.Decrease;

            if (_dodgeTimer <= 0f)
            {
                PlayerFinishDodge();
                _dodgeTimer = 0f;
            }
        }

        TouchMoveUpdate();
        //JoystickUpdate();
        UpdateKeyboard();
        
        // Механика стрельбы
        if (App.Instance.gameController.enemiesOnScreen.Count > 0)
        {
            PlayerAttack();
        }
    }
    
    // TODO: требует рефакторинга!!!
    private void UpdateKeyboard()
    {
        // Vertical Movement
        var moveUp = Input.GetKey(KeyCode.W);
        var moveDown = Input.GetKey(KeyCode.S);

        // Horizontal Movement
        var moveLeft = Input.GetKey(KeyCode.A);
        var moveRight = Input.GetKey(KeyCode.D);

        var fire = Input.GetKey(KeyCode.Space);

        var shield = Input.GetKey(KeyCode.Z);

        var rocketShell = Input.GetKey(KeyCode.X);

        var dodge = Input.GetKey(KeyCode.C);

        if (moveUp || moveDown || moveLeft || moveRight)
        {
            float xPosition = -Convert.ToInt32(moveLeft) + Convert.ToInt32(moveRight);
            float yPosition = -Convert.ToInt32(moveDown) + Convert.ToInt32(moveUp);
            var directional = new Vector3(xPosition, yPosition, 0);

            PlayerMove(directional);
        }

        if (shield)
        {
            PlayerShield();
        }

        if (rocketShell)
        {
            PlayerRocket();
        }

        if (dodge)
        {
            PlayerDodge();
        }
    }

    private void TouchMoveUpdate()
    {
        var inputMoveTouch = App.Instance.gameController.inputMoveTouch;
        var touchPos = new Vector3(inputMoveTouch.position.x, inputMoveTouch.position.y, 0f);
        Vector3 dir = (touchPos - transform.position).normalized;

        if (inputMoveTouch.touch)
        {
            _rigidbody2D.velocity = dir * walkSpeed * Time.deltaTime;
        
            transform.Translate(dir * walkSpeed * Time.deltaTime);
        }
        else
        {
            _rigidbody2D.velocity = new Vector2(0f, 0f);
        }
        
        Debug.DrawRay(transform.position, dir, Color.green);
    }
    
    // TODO: Отрефакторить. Не имеет смысла вообще использовать velocity -> лучше Vector3.Translate от transform.position
    private void PlayerMove(Vector3 directional)
    {
        if (directional.magnitude > 0)
        {
            directional.Normalize();
            _walkTime = walkCooldown;

            if (directional.magnitude != 0)
            {
                _rigidbody2D.velocity = directional * walkSpeed;
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
            Instantiate<GameObject>(laser, new Vector3(_transform.position.x, _transform.position.y + 0.8f , 0), rotation);
            // _overheating += 0.75f;
            _attackTime = attackCooldown;

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
        if (_shieldTime <=0 && !shield.activeSelf)
        {
            shield.SetActive(!shield.activeSelf);
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
            _rocketTime = rocketCooldown;
        }
    }

    public void PlayerDodge()
    {
        if (_dodgeTime<=0)
        {
            _boxCollider2d.enabled = false;
            _dodgeTime = dodgeCooldown;
            _dodgeTimer = maxDodgeDuration;
            _scaleStatus = DodgeScaleStatus.Increase;
        }
    }

    private void PlayerFinishDodge()
    {
        _boxCollider2d.enabled = true;
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    enum DodgeScaleStatus
    {
        Increase = 0,
        Decrease = 1
    }
}
