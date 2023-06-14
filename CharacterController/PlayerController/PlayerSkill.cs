using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private bool _isAiming;
    [SerializeField]
    private bool _isWaitForCast;
    [SerializeField]
    private bool _isCasting;
    public GameObject Spell;
    public GameObject SpellHolder;
    private Animator _animator;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _isAiming = true;
        }
        if (Input.GetMouseButtonDown(1))
        {
            _isAiming = false;
        }

        if (Input.GetMouseButtonDown(0) && _isAiming)
        {
            _playerMovement.IsLookToDirection = false;
            var point = GetVectorFromMouse.Take();
            _playerMovement.GetTurnToCastSpell(true, point);
            _isWaitForCast = true;
        }

        if (_isWaitForCast && _playerMovement.IsLookToDirection)
        {
          

            _isCasting = true;
            _playerMovement.UpdateAnimatorState("Cast", true);

            _isWaitForCast = false;
            _playerMovement.IsLookToDirection = false;
            _playerMovement.GetTurnToCastSpell(false, Vector3.zero);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (_isCasting)
            {
                _isCasting = false;
                _playerMovement.UpdateAnimatorState("Cast", false);
                _animator.Play("Idle");
            }
        }
        _playerMovement.BlockMovement = _isCasting;
    }

  

    private void SpawnMagic()
    {
        var spell = Instantiate(Spell, SpellHolder.transform.position, transform.rotation);
        spell.SetActive(true);
        _isCasting = false;
    }

    public void ReturnToIdleAnimation()
    {
        _playerMovement.UpdateAnimatorState("Cast", false);
    }
}
