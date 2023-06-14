using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector3 _pointToMove;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private bool _isLookToDirection;

    public bool IsLookToDirection
    {
        get { return _isLookToDirection; }
        set { _isLookToDirection = value; }
    }

    [SerializeField] private bool _isNeedTurn;
    [SerializeField] private bool _isReachedPoint;
    [SerializeField] private float _currentAngleDifference;
    [SerializeField] private float _angleForMove;
    private float _angleForSpell;
    private float _normalAngle;
    [SerializeField] private bool _isGettingHit;
    [SerializeField] private bool _needCastSpell;

    public bool BlockMovement;
    [SerializeField] private bool _needAlwaysMove;

    private Rigidbody _rb;
    private Animator _animator;
    private Vector3 _directionToMove;
    private Vector3 _accumulatedForce;
    private float _timer;
    public GameObject AnimatingObjectToMove;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _angleForSpell = 0.05f;
        _normalAngle = _angleForMove;
    }

    private void Update()
    {
        if (_isGettingHit)
        {
            _timer += Time.deltaTime;
            if (_timer > 1f)
            {
                _isGettingHit = false;
                _timer = 0f;
            }
        }

        if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1) || _needAlwaysMove)
        {
            GetComponent<IPlayedRandomSound>().PlaySound();
            _pointToMove = GetVectorFromMouse.Take();

            if (_pointToMove == Vector3.zero)
            {
                _isNeedTurn = false;
                return;
            }

            _pointToMove.y = transform.position.y;

            _isReachedPoint = false;
            _isNeedTurn = true;

            AnimatingObjectToMove.transform.position = _pointToMove;
            
        }

        if (_isNeedTurn)
        {
            Turn();
        }

        if (_isLookToDirection)
        {
            if (!_isGettingHit)
            {
                _isNeedTurn = false;
            }
            MoveForward();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetHit();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            StopMovement();

        }
    }

    public void GetTurnToCastSpell(bool isNeedTurn, Vector3 pointWhereSpellFly)
    {
        _isNeedTurn = isNeedTurn;
        _pointToMove = pointWhereSpellFly;
        _needCastSpell = isNeedTurn;

        if (_isNeedTurn)
        {
            _angleForMove = _angleForSpell;

        }
        else
        {
            _angleForMove = _normalAngle;
        }
    }


    private void Turn()
    {
        Vector3 direction = _pointToMove - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion newRotation = Quaternion.RotateTowards(_rb.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            _rb.MoveRotation(newRotation);
        }

        _currentAngleDifference = Vector3.Angle(transform.forward, direction);

        if (!_isGettingHit || !_needCastSpell)
        {
            UpdateAnimatorState("Moving", false);
            _isLookToDirection = _currentAngleDifference <= _angleForMove;
        }
    }

    private void MoveForward()
    {

        if (_needCastSpell || BlockMovement)
        {
            return;
        }

        

        UpdateAnimatorState("Moving", true);
        float distanceToStop = 0.1f;
        _directionToMove = (_pointToMove - transform.position).normalized;
        _rb.MovePosition(_rb.position + (_directionToMove * _movementSpeed * Time.deltaTime));

        if (Vector3.Distance(transform.position, _pointToMove) <= distanceToStop)
        {
            _isNeedTurn = false;
            _isReachedPoint = true;
            _isGettingHit = false;
            _accumulatedForce = Vector3.zero;
            _isLookToDirection = false;
            UpdateAnimatorState("Moving", false);
        }
    }

    private void GetHit()
    {
        _isLookToDirection = true;
        _isGettingHit = true;
        _isNeedTurn = true;

        float hitForce = 10f;
        Vector3 hitDirection = Random.insideUnitSphere.normalized;
        hitDirection.y = 0f;
        _accumulatedForce += hitDirection * hitForce;
        _rb.AddForce(_accumulatedForce, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        if (_isGettingHit)
        {
            _accumulatedForce = Vector3.Lerp(_accumulatedForce, Vector3.zero, Time.deltaTime * 2f);
        }
    }

    public void UpdateAnimatorState(string animationName, bool value)
    {
        _animator.SetBool(animationName, value);
    }

    private void StopMovement()
    {
        UpdateAnimatorState("Moving", false);
        _isNeedTurn = false;
        _isLookToDirection = false;
    }


}
