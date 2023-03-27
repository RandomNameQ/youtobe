using System.Collections;
using UnityEngine;

public class ShiftMovement : MonoBehaviour
{
    [SerializeField] private int _shiftRange;
    [SerializeField] private float _shiftSpeed;
    private bool _isTimer, _isDoublePress, isCoroutineRunning;
    private float _timer;

    private KeyCode _lastPressedKey;
    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (_isTimer)
        {
            _timer += Time.deltaTime;
            _isDoublePress = true;
            if (_timer > 0.2f)
            {
                _isTimer = false;
                _isDoublePress = false;
                _timer = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ShiftOnSide(KeyCode.S);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ShiftOnSide(KeyCode.D);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            ShiftOnSide(KeyCode.W);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            ShiftOnSide(KeyCode.A);
        }
    }

   
   
    
    private void ShiftOnSide(KeyCode currentKey)
    {
        
        _isTimer = true;
        if (_lastPressedKey == currentKey && _isDoublePress && !isCoroutineRunning)
        {
            
            Vector3 destination = GetDestinationPosition(currentKey);
            StartCoroutine(ShiftToPosition(destination));
        }
        _lastPressedKey = currentKey;
    }

    private Vector3 GetDestinationPosition(KeyCode key)
    {
        Vector3 direction = Vector3.zero;
        switch (key)
        {
            case KeyCode.S:
                direction = -transform.forward;
                break;

            case KeyCode.D:
                direction = transform.right;
                break;

            case KeyCode.W:
                direction = transform.forward;
                break;

            case KeyCode.A:
                direction = -transform.right;
                break;
        }
        return direction * _shiftRange;
    }

    private IEnumerator ShiftToPosition(Vector3 destination)
    {
        isCoroutineRunning = true;
        float elapsedTime = 0;
        while (elapsedTime < _shiftSpeed)
        {
            _characterController.SimpleMove(destination.normalized * _shiftRange);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isCoroutineRunning = false;
    }
}