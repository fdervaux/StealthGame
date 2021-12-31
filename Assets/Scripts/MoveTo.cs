using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{

    public Transform _goal;
    private NavMeshAgent _agent;
    private CharacterController _characterController;
    private Rigidbody _rigidBody;

    public float _stopDistance = 2;
    public float _startFollowDistance = 3;

    private bool _followTarget = false;
    private bool _performedJump = false;

    private Vector3 _velocity = Vector3.zero;

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
    }


    private void FixedUpdate()
    {
        _agent.destination = _goal.position;

        float targetDistance = (_goal.position - transform.position).magnitude;

        if (targetDistance > _startFollowDistance)
        {
            _followTarget = true;
        }

        if (targetDistance < _stopDistance)
        {
            _followTarget = false;
        }

        if (!_performedJump)
        {
            if (_followTarget)
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
            else
            {
                _velocity = Vector3.zero;
            }
        }

        if (_agent.isOnOffMeshLink && !_performedJump)
        {
            _characterController.jump();
            _performedJump = true;

            _velocity = (_agent.currentOffMeshLinkData.endPos - _agent.currentOffMeshLinkData.startPos).normalized * _agent.speed;

            Debug.Log("jump");
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