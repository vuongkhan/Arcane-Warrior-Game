using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootState : BaseState<PlayerStateMachine.PlayerState>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private Transform _shootPosition;
    private float _bulletSpeed = 7f;
    private FlipCharacter _flip;
    private bool isHolding = false;
    private float holdTime;
    private GameObject[] _bulletType;
    private GameObject[] _effectCharge;
    private GameObject _currentChargeEffect;
    private bool isSpawnEffect = false;
    private Transform _dustPosition;
    private GameObject[] _dust;
    private GroundCheck _groundcheck;
    private AudioManager _audioManager;
    private string _nameSound;
    private HealthManager _healthManager;

    public ShootState(StateMachine<PlayerStateMachine.PlayerState> stateMachine, Animator animator, Rigidbody2D rb, Transform shootPosition, FlipCharacter flip, GameObject[] bulletType, GameObject[] effectCharge,
        Transform dustPosition, GameObject[] dust, GroundCheck groundCheck, AudioManager audioManager, HealthManager health)
        : base(stateMachine, PlayerStateMachine.PlayerState.Shoot)
    {
        _animator = animator;
        _rb = rb;
        _shootPosition = shootPosition;
        _bulletType = bulletType;
        _flip = flip;
        _effectCharge = effectCharge;
        _dustPosition = dustPosition;
        _dust = dust;
        _groundcheck = groundCheck;
        _audioManager = audioManager;
        _healthManager = health;
    }

    public override void EnterState()
    {
        _groundcheck.canMove = false;
        _audioManager.PlayOneShotSoundEffect("charge");
        isHolding = true;
        holdTime = 0f;
        _animator.SetTrigger("Shoot");
    }

    private void UpdateChargeEffect()
    {
        GameObject newChargeEffect = null;

        if (holdTime >= 2f && holdTime < 3f && !isSpawnEffect && _healthManager.GetCurrentMana() >= 15f)
        {
            isSpawnEffect = true;
            newChargeEffect = _effectCharge[0];
        }
        else if (holdTime >= 3f && isSpawnEffect && _healthManager.GetCurrentMana() >= 30f)
        {
            isSpawnEffect = false;
            newChargeEffect = _effectCharge[1];
        }

        if (newChargeEffect != null)
        {
            ReplaceChargeEffect(newChargeEffect);
        }

        if (_currentChargeEffect != null)
        {
            _currentChargeEffect.transform.position = _shootPosition.position;
        }
    }

    private void ReplaceChargeEffect(GameObject newChargeEffect)
    {
        if (newChargeEffect == _effectCharge[1])
        {
            Object.Destroy(_currentChargeEffect);
        }
        _currentChargeEffect = Object.Instantiate(newChargeEffect, _shootPosition.position, _shootPosition.rotation);
        FlipEffect(_currentChargeEffect);
    }

    private void Shoot()
    {
        if (holdTime >= 3f && _healthManager.GetCurrentMana() >= 30f)
        {
            _healthManager.ReduceMana(30f);
            GameObject currentDust = Object.Instantiate(_dust[0], _dustPosition.position, _dustPosition.rotation);
            FlipEffect(currentDust);
        }
        else if (holdTime >= 2f && _healthManager.GetCurrentMana() >= 15f)
        {
            _healthManager.ReduceMana(15f);
            GameObject currentDust = Object.Instantiate(_dust[1], _dustPosition.position, _dustPosition.rotation);
            FlipEffect(currentDust);
        }
        else
        {
            _healthManager.ReduceMana(10f);
            GameObject currentDust = Object.Instantiate(_dust[2], _dustPosition.position, _dustPosition.rotation);
            FlipEffect(currentDust);
        }
        var (bulletPrefab, _nameSound) = GetBulletPrefab();
        if (bulletPrefab != null)
        {
            _audioManager.PlaySoundEffect(_nameSound);
            _animator.SetTrigger("Fire");
            GameObject bullet = Object.Instantiate(bulletPrefab, _shootPosition.position, _shootPosition.rotation);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

            if (bulletRb != null)
            {
                Vector2 shootDirection = _flip.IsFacingRight() ? Vector2.right : Vector2.left;
                bulletRb.velocity = shootDirection * _bulletSpeed;
                FlipEffect(bullet);
            }
        }
        if (_currentChargeEffect != null)
        {
            Object.Destroy(_currentChargeEffect);
            _currentChargeEffect = null;
            isSpawnEffect = false;
        }
    }

    private void FlipEffect(GameObject effect)
    {
        if (!_flip.IsFacingRight())
        {
            Vector3 scale = effect.transform.localScale;
            scale.x *= -1;
            effect.transform.localScale = scale;
        }
    }

    private (GameObject, string) GetBulletPrefab()
    {
        if (holdTime >= 3f && _healthManager.GetCurrentMana() >= 30f)
        {
            return (_bulletType[2], "shoot3");
        }
        else if (holdTime >= 2f && _healthManager.GetCurrentMana() >= 15f)
        {
            return (_bulletType[1], "shoot2");
        }
        else
        {
            return (_bulletType[0], "shoot2");
        }
    }

    public override void ExitState()
    {
        _animator.SetBool("IsShooting", false);
        _groundcheck.canMove = true;
        _audioManager.StopOneShotSoundEffect();
    }

    public override void UpdateState()
    {
        if (isHolding)
        {
            holdTime += Time.deltaTime;
            UpdateChargeEffect();
            Debug.Log($"Hold Time: {holdTime}");
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            Debug.Log("Shot fired");
            isHolding = false;
            Shoot();
            StateMachine.QueueNextState(PlayerStateMachine.PlayerState.Idle);
        }
    }

    public override PlayerStateMachine.PlayerState GetNextState()
    {
        return StateMachine.GetQueuedState();
    }

    public override void OnTriggerEnter2D(Collider2D other) { }
    public override void OnTriggerStay2D(Collider2D other) { }
    public override void OnTriggerExit2D(Collider2D other) { }
}
