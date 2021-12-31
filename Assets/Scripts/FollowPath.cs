using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;
using System.Collections.Generic;

public class FollowPath : MonoBehaviour
{

    public List<Transform> _points = new List<Transform>();
    public float _speed = 4f;
    private NavMeshAgent _agent;
    private CharacterController _characterController;
    private Rigidbody _rigidBody;

    public float _stopDistance = 0.5f;
    private bool _performedJump = false;
    private Vector3 _velocity = Vector3.zero;

    private int _currentIndex = 0;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _characterController = GetComponent<CharacterController>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _agent.updateRotation = false;
        _agent.updatePosition = false;
        _agent.speed = _speed;
        if (_points.Count > 0)
            _agent.destination = _points[0].position;
    }

    private Vector3 getNextPoint()
    {

        _currentIndex++;

        if (_currentIndex == _points.Count)
        {
            _currentIndex = 0;
        }

        return _points[_currentIndex].position;
    }

    private void FixedUpdate()
    {

        float targetDistance = (_agent.destination - transform.position).magnitude;
        if (targetDistance < _stopDistance)
        {
            _agent.destination = getNextPoint();
        }

        if (!_performedJump)
        {
            if (_agent.enabled && _agent.hasPath && !_agent.pathPending)
            {
                _velocity = _agent.desiredVelocity;
            }
            else if (_agent.path.status == NavMeshPathStatus.PathPartial)
            {
                float distancePossiblePoint = (_agent.path.corners[_agent.path.corners.Length - 1] - transform.position).magnitude;

                if (distancePossiblePoint > _stopDistance)
                {
                    _velocity = _agent.desiredVelocity;
                }
                else
                {
                    _velocity = Vector3.zero;
                }
            }
        }

        if (_agent.isOnOffMeshLink && !_performedJump)
        {
            _characterController.jump();
            _performedJump = true;
            _velocity = (_agent.currentOffMeshLinkData.endPos - _agent.currentOffMeshLinkData.startPos).normalized * _speed;
        }

        if (!_agent.isOnOffMeshLink && _characterController.isGrounded())
        {
            _performedJump = false;
        }

        _characterController.move(_velocity);

        _agent.nextPosition = transform.position;
        if (!_performedJump && Vector3.Distance(_agent.nextPosition, transform.position) > 1.0f)
        {
            _agent.Warp(transform.position);

        }
    }
}