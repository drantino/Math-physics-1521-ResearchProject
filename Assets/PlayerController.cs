using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private GameState _gameState;
    public int speed;
    private InputAction _move, _jump;
    public bool _grounded,_jumping,_wallLeft,_wallRight,_onConveyor;
    public Transform groundCheck;
    public float gravity,velocity,maxFallingSpeed,maxHorizontalSpeed,jumpForceHardCode,jumpForcePhysics,jumpForceCodePhysics;
    private Rigidbody2D _rb;
    private void Awake()
    {
        _gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
        _jump = InputSystem.actions.FindAction("Jump");
        _move = InputSystem.actions.FindAction("Move");
        _grounded = false;
        _jumping = false;
        _wallLeft = false;
        _wallRight = false;
        _onConveyor = false;
        _rb = GetComponent<Rigidbody2D>();  
    }
    private void Update()
    {
        if (_grounded && !_jumping && _jump.WasPressedThisFrame())
        {
            _jumping = true;

        }
        
    }
    private void FixedUpdate()
    {
        _grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("WhatIsGround"));
        float horizontalMovement;
        switch (_gameState.physics)
        {
            case GameState.PhysicsType.HardCoded:
                {
                    _rb.bodyType = RigidbodyType2D.Kinematic;

                    if(_jumping)
                    {
                        _jumping = false;
                        velocity = jumpForceHardCode;
                    }
                    
                    velocity += gravity;
                    if (velocity < maxFallingSpeed)
                    {
                        velocity = maxFallingSpeed;
                    }

                    //velocity down
                    if (_grounded && velocity <0)
                    {
                        velocity = 0;
                        //transform.position = new Vector3(transform.position.x,groundCheck.position.y +
                        //    (GetComponent<Renderer>().bounds.min.y),transform.position.y);
                    }

                    //horizontal movement
                    Vector2 movement = _move.ReadValue<Vector2>();
                    float hori = movement.x * speed * Time.deltaTime;

                    if(_wallLeft && hori < 0)
                    {
                        hori = 0;
                    }
                    if (_wallRight && hori > 0)
                    {
                        hori = 0;
                    }
                    if(_onConveyor)
                    {
                        hori -= (float)0.1;
                    }

                    transform.Translate(hori, velocity, 0, Space.Self);
                    break;
                }
            case GameState.PhysicsType.CodePhyics:
                {
                    _rb.bodyType = RigidbodyType2D.Dynamic;
                    horizontalMovement = _move.ReadValue<Vector2>().x;

                    _rb.position += new Vector2(horizontalMovement * maxHorizontalSpeed * Time.deltaTime,0); 

                    if(_jumping)
                    {
                        _jumping = false;
                        _rb.linearVelocityY = jumpForceCodePhysics;
                    }
                    if(_onConveyor)
                    {
                        //_rb.position += new Vector2(-5*Time.deltaTime,0);
                        _rb.AddForce(Vector2.left * 20);
                    }

                    break;
                }
            case GameState.PhysicsType.ForcePhysics:
                {
                    _rb.bodyType = RigidbodyType2D.Dynamic;
                    horizontalMovement = _move.ReadValue<Vector2>().x;
                    
                    if(horizontalMovement * _rb.linearVelocityX<maxHorizontalSpeed)
                    {
                        _rb.AddForce(Vector2.right*horizontalMovement*speed*3);
                    }
                    if(Mathf.Abs(_rb.linearVelocityX)>maxHorizontalSpeed)
                    {
                        _rb.linearVelocityX = Mathf.Sign(_rb.linearVelocityX) * maxHorizontalSpeed;
                    }
                    if(_jumping)
                    {
                        _jumping = false;
                        _rb.AddForce(Vector2.up*jumpForcePhysics, ForceMode2D.Impulse);
                    }
                    if(_onConveyor)
                    {
                        _rb.AddForce(Vector2.left * 20);
                    }

                    break;
                }
        }

    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WallLeft"))
        {
            _wallLeft = true;
        }
        if (collision.gameObject.CompareTag("WallRight"))
        {
            _wallRight = true;
        }
        if (collision.gameObject.CompareTag("ConveyorBelt"))
        {
            _onConveyor = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WallLeft"))
        {
            _wallLeft = false;
        }
        if (collision.gameObject.CompareTag("WallRight"))
        {
            _wallRight = false;
        }
        if (collision.gameObject.CompareTag("ConveyorBelt"))
        {
            _onConveyor = false;
        }
    }
    
}
