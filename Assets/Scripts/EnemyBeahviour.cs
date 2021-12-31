using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBeahviour : MonoBehaviour
{

    public enum EnemyBehaviourState
    {
        Normal,
        Suspicious,
        AlertStopped,
        Alert,
        PlayerInVision,
    }

    LookAround _lookAround;
    FollowPath _followPath;
    MoveTo _MoveTo;

    Vision _vision;

    private EnemyBehaviourState _currentState;
    private CharacterController _characterController;
    private NavMeshAgent _agent;

    public Color PlayerInVisionColor;
    public Color AlertColor;
    public Color SuspiciousColor;
    public Color NormalColor;

    public float _alertDuration = 8f; // in sec
    private float _alertTimeReamaining = 0f;
    public float SuspiciousDuration = 15f; // in sec
    private float SuspiciousTimeRemaining = 0f;

    public float AlertSpeed = 5;
    public float AlertStopSpeed = 1;
    public float SuspiciousSpeed = 4;
    public float NormalSpeed = 4;
    public float PlayerInVisionSpeed = 7;

    private void Awake()
    {
        _lookAround = GetComponent<LookAround>();
        _followPath = GetComponent<FollowPath>();
        _MoveTo = GetComponent<MoveTo>();
        _vision = GetComponentInChildren<Vision>();
        _characterController = GetComponent<CharacterController>();
        _agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentState = EnemyBehaviourState.Normal;
        _MoveTo.enabled = false;
        _followPath.enabled = true;
        _lookAround.enabled = false;
        _characterController.setManualOrientation(0);
        _vision.setColor(NormalColor);
        _agent.speed = NormalSpeed;

    }

    private void UpdateState()
    {
        if (_vision.playerIsVisble())
        {
            _currentState = EnemyBehaviourState.PlayerInVision;
            _vision.setColor(PlayerInVisionColor);
            _MoveTo.enabled = true;
            _followPath.enabled = false;
            _lookAround.enabled = false;
            _characterController.setManualOrientation(0);
            _agent.speed = PlayerInVisionSpeed;
            return;
        }

        if (_currentState == EnemyBehaviourState.PlayerInVision)
        {
            _vision.setColor(AlertColor);
            _currentState = EnemyBehaviourState.Alert;
            _alertTimeReamaining = _alertDuration;
            _MoveTo.enabled = true;
            _followPath.enabled = false;
            _lookAround.enabled = false;
            _agent.speed = AlertSpeed;
            return;
        }

        if (_currentState == EnemyBehaviourState.Alert)
        {
            _alertTimeReamaining -= Time.fixedDeltaTime;
            if (_alertTimeReamaining < 0)
            {
                _vision.setColor(AlertColor);
                _currentState = EnemyBehaviourState.AlertStopped;
                _alertTimeReamaining = _alertDuration;
                _MoveTo.enabled = false;
                _followPath.enabled = true;
                _lookAround.enabled = true;
                _agent.speed = AlertStopSpeed;
                
            }
            return;
        }

        if (_currentState == EnemyBehaviourState.AlertStopped)
        {
            _alertTimeReamaining -= Time.fixedDeltaTime;
            if (_alertTimeReamaining < 0)
            {
                _vision.setColor(SuspiciousColor);
                _currentState = EnemyBehaviourState.Suspicious;
                SuspiciousTimeRemaining = SuspiciousDuration;
                _MoveTo.enabled = false;
                _followPath.enabled = true;
                _lookAround.enabled = true;
                _agent.speed = SuspiciousSpeed;
            }
            return;
        }


        if (_currentState == EnemyBehaviourState.Suspicious)
        {
           
            SuspiciousTimeRemaining -= Time.fixedDeltaTime;
            if (SuspiciousTimeRemaining < 0)
            {
                _vision.setColor(NormalColor);
                _currentState = EnemyBehaviourState.Normal;
                _MoveTo.enabled = false;
                _followPath.enabled = true;
                _lookAround.enabled = false;
                _characterController.setManualOrientation(0);
                _agent.speed = NormalSpeed;
            }
            return;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateState();
    }
}
