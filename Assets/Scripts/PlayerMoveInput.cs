using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveInput : MonoBehaviour
{

    public bool _is2dSideScroller = false;

    private InputAction _moveAction = null;
    private InputAction _JumpAcion = null;

    private CharacterController _characterController = null;

    public float _gravityFactorJumpUp = 1;

    private PlayerInput _PlayerInput = null;
    [Range(1, 10)] public float _speed = 7;



    private void Start()
    {

        _PlayerInput = GetComponent<PlayerInput>();
        _characterController = GetComponent<CharacterController>();


        _moveAction = _PlayerInput.actions.FindAction("Move", true);
        _JumpAcion = _PlayerInput.actions.FindAction("Jump", true);
    }

    public void OnJump()
    {
        _characterController.jump();
    }

    private void FixedUpdate() {
        //Retrieve player input
        Vector2 inputMove = _moveAction.ReadValue<Vector2>();
        float inputJump = _JumpAcion.ReadValue<float>();

        //get axis relative to the camera and the transform up vector
        Vector3 XAxis = Vector3.Cross(Camera.main.transform.forward, transform.up).normalized;
        Vector3 YAxis = Vector3.Cross(Camera.main.transform.right, transform.up).normalized;

        Vector3 move = -XAxis * inputMove.x * _speed;
        if (!_is2dSideScroller)
        {
            move += YAxis * inputMove.y * _speed;
        }

        _characterController.move(move);

        if (inputJump > 0.5f)
        {
            _characterController.setCustomGravityFactor(_gravityFactorJumpUp);
        }
    }



}