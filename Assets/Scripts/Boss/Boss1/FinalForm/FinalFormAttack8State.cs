using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFormAttack8State : BaseState<FinalFormStateMachine.FinalFormState>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private Transform _playerTransform;
    private WallCheck _wallCheck;
    private GameObject[] _blastPrefab;
    private GameObject _blast;
    private Transform _pointAttackFinal;
    private float attackStartTime;
    private bool hasAttack = false; 
    private bool isAtWall = false; 
    private float projectileSpawnTime;
    private EnemyAudioManager _audioManager;

    public FinalFormAttack8State(StateMachine<FinalFormStateMachine.FinalFormState> stateMachine, Animator animator, Rigidbody2D rb, Transform playerTransform, WallCheck wallCheck, GameObject[] blastPrefab, Transform pointAttackFinal, EnemyAudioManager audioManager)
        : base(stateMachine, FinalFormStateMachine.FinalFormState.Attack8)
    {
        _animator = animator;
        _rb = rb;
        _playerTransform = playerTransform;
        _wallCheck = wallCheck;
        _blastPrefab = blastPrefab;
        _audioManager = audioManager;
        _pointAttackFinal = pointAttackFinal;
    }

    public override void EnterState()
    {
        _audioManager.PlaySoundEffect("hra");
        attackStartTime = Time.time;
        hasAttack = false;
        isAtWall = false; 
    }

    public override void ExitState()
    {
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
                _animator.SetTrigger("chargeattack8");
                isAtWall = true; 
            }
        }
        else
        {
            if (!hasAttack && Time.time >= attackStartTime + 2f) 
            {
                _animator.SetBool("finalkame", true);
                hasAttack = true;
                SpawnProjectile();
                projectileSpawnTime = Time.time;
            }
            if (hasAttack && Time.time >= projectileSpawnTime + 3f)
            {
                _animator.SetBool("finalkame", false);
                GameObject.Destroy(_blast);
                StateMachine.QueueNextState(FinalFormStateMachine.FinalFormState.Attack1);
            }
        }
    }

    private void SpawnProjectile()
    {
        _blast = GameObject.Instantiate(_blastPrefab[0], _pointAttackFinal.position, Quaternion.identity);

    }


    public override FinalFormStateMachine.FinalFormState GetNextState()
    {
        return StateMachine.GetQueuedState();
    }

    public override void OnTriggerEnter2D(Collider2D other) { }
    public override void OnTriggerStay2D(Collider2D other) { }
    public override void OnTriggerExit2D(Collider2D other) { }
}
