using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFormAttack2State : BaseState<FinalFormStateMachine.FinalFormState>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private Transform _playerTransform;
    private WallCheck _wallCheck;
    private GameObject[] _blastPrefab;
    private Transform _pointAttackFinal;
    private float attackStartTime;
    private bool hasAttack = false;
    private bool isAtWall = false;
    private float projectileSpawnTime;
    private EnemyAudioManager _audioManager;
    private GameObject[] _effect;
    private GameObject instantiatedEffect;
    private GameObject _blast;

    public FinalFormAttack2State(StateMachine<FinalFormStateMachine.FinalFormState> stateMachine, Animator animator, Rigidbody2D rb, Transform playerTransform, WallCheck wallCheck, GameObject[] blastPrefab, Transform pointAttackFinal, EnemyAudioManager audioManager, GameObject[] effect)
        : base(stateMachine, FinalFormStateMachine.FinalFormState.Attack2)
    {
        _animator = animator;
        _rb = rb;
        _playerTransform = playerTransform;
        _wallCheck = wallCheck;
        _blastPrefab = blastPrefab;
        _audioManager = audioManager;
        _pointAttackFinal = pointAttackFinal;
        _effect = effect;
    }

    public override void EnterState()
    {
        _audioManager.PlaySoundEffect("kamehameha");
        attackStartTime = Time.time;
        hasAttack = false;
        isAtWall = false;
    }

    public override void ExitState()
    {
        if (instantiatedEffect != null)
        {
            GameObject.Destroy(instantiatedEffect);
        }
        if (_blast != null)
        {
            GameObject.Destroy(_blast);
        }
    }

    public override void UpdateState()
    {
        if (!isAtWall)
        {
            if (_wallCheck.distanceToWall > 1)
            {
                _animator.SetBool("back", true);
                _animator.SetFloat("Speed", 2f);

                Vector2 wallPosition = new Vector2(_wallCheck.wallObject.position.x, _rb.position.y);
                Vector2 direction = (wallPosition - _rb.position).normalized;
                _rb.velocity = direction * 10f;
            }
            else
            {
                _animator.SetBool("back", false);
                _animator.SetFloat("Speed", 0f);
                _rb.velocity = Vector2.zero;
                _animator.SetTrigger("chargefinal");
                isAtWall = true;
                if (_effect != null && _effect.Length > 0)
                {
                    instantiatedEffect = GameObject.Instantiate(_effect[0], new Vector3(_rb.position.x + 0.1f, _rb.position.y + 0.5f, 0), Quaternion.identity);
                }
            }
        }
        else
        {
            if (!hasAttack && Time.time >= attackStartTime + 8f)
            {
                _animator.SetBool("finalkameha", true);
                hasAttack = true;
                SpawnProjectile();
                projectileSpawnTime = Time.time;
            }
            if (hasAttack && Time.time >= projectileSpawnTime + 2f)
            {
                _animator.SetBool("finalkameha", false);
                if (_blast != null)
                {
                    GameObject.Destroy(_blast);
                }
                StateMachine.QueueNextState(FinalFormStateMachine.FinalFormState.Attack3);
            }
        }
    }

    private void SpawnProjectile()
    {
        if (_blastPrefab != null && _blastPrefab.Length > 0)
        {
            _blast = GameObject.Instantiate(_blastPrefab[0], _pointAttackFinal.position, Quaternion.identity);
        }
    }

    public override FinalFormStateMachine.FinalFormState GetNextState()
    {
        return StateMachine.GetQueuedState();
    }

    public override void OnTriggerEnter2D(Collider2D other) { }
    public override void OnTriggerStay2D(Collider2D other) { }
    public override void OnTriggerExit2D(Collider2D other) { }
}
