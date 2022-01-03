using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAround : MonoBehaviour
{
    public float _duration = 2f;
    public float _angle = 90f;
    private CharacterController _characterController;
    private float _currentAngle = 0.0f; // in deg
    private int _direction = 1;
    private float _currentTime;

    public AnimationCurve _angleAnimationCurve;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _currentTime += _direction * Time.fixedDeltaTime / _duration;

        if (_currentTime > 1 || _currentTime < 0)
        {
            _direction *= -1;
        }

        float angle = _angleAnimationCurve.Evaluate(_currentTime) * _angle * 0.5f;
        _characterController.setManualOrientation(angle);
    }
}
