using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{


    private const float EPSILON = 0.1f;
    private Vector3 _velocity = Vector3.zero;
    [Range(1, 8)] public float _jumpForce = 3;
    [Range(0, 1)] public float _airControl = 0.2f;
    [Range(0, 1)] public float _floorControl = 1.0f;



    public float _gravityFactorJumpDown = 1.4f;
    public float _gravityFactorCancelJump = 1.6f;
    private bool _isGrounded = false;

    public float _groundCheckDistance = 0.4f;

    public float _groundCheckOffset = 1.0f;

    private Rigidbody _rigidBody;

    private bool _startJump = false;

    private Vector3 _horizontalMoveInput = Vector3.zero;

    public bool _isCustomGravity = false;

    public float _customGravity = 0;

    public float _manualOrientationAngle = 0; //deg

    public float _angle = 0;

    public void setManualOrientation(float angle)
    {
        _manualOrientationAngle = angle;
    }

    public bool isGrounded()
    {
        return _isGrounded;
    }

    public void setCustomGravityFactor(float customGravity)
    {
        _customGravity = customGravity;
        _isCustomGravity = true;
    }


    public void move(Vector3 horizontalMove)
    {
        _horizontalMoveInput = horizontalMove;
    }

    public void jump()
    {
        //add jump force to the player velocity
        if (_isGrounded)
        {
            _velocity += _jumpForce * Vector3.up;
            _startJump = true;
        }
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 groundCorrection = Vector3.zero;
        _isGrounded = false;

        if (!_startJump && _velocity.y < EPSILON)
        {
            RaycastHit hit;
            if (Physics.Raycast(_rigidBody.position + Vector3.up * _groundCheckOffset, Vector3.down, out hit, _groundCheckDistance + _groundCheckOffset, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                _isGrounded = true;
                groundCorrection = Vector3.down * (hit.distance - _groundCheckOffset);
            }
        }

        //change gravity factor for better jump
        float gravityFactor = 1.0f;

        if (_velocity.y < 0)
        {
            gravityFactor = _gravityFactorJumpDown;
        }
        else
        {
            if (_isCustomGravity)
            {
                gravityFactor = _customGravity;
                _isCustomGravity = false;
            }
            else
            {
                gravityFactor = _gravityFactorCancelJump;
            }

        }


        //apply gravity to the character
        if (!_isGrounded)
        {
            _velocity += Vector3.down * 9.81f * gravityFactor * Time.fixedDeltaTime;
        }
        else if (_velocity.y < EPSILON)
        {
            _velocity.y = 0;
        }

        float control = _floorControl;
        if (!_isGrounded)
        {
            control = _airControl;
        }

        Vector3 targetVelocity = new Vector3(_horizontalMoveInput.x, _velocity.y, _horizontalMoveInput.z);

        _velocity = Vector3.RotateTowards(_velocity, targetVelocity, control / (Mathf.PI * 2), control);

        Vector3 horizontalVelocity = Vector3.ProjectOnPlane(_velocity, transform.up);
        
        if (horizontalVelocity.magnitude > EPSILON)
        {
            _angle = Vector3.SignedAngle(new Vector3(1, 0, 0), horizontalVelocity, transform.up);
        }

        transform.rotation = Quaternion.Euler(0, _angle + _manualOrientationAngle, 0);

        _startJump = false;

        //mover character for better physics
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.MovePosition(_rigidBody.position + _velocity * Time.fixedDeltaTime + groundCorrection);

    }
}
