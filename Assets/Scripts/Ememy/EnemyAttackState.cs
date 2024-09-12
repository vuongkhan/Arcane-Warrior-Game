using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : BaseState<EnemyStateMachine.EnemyState>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private EnemyNormalAudioManager _enemyAudio;

    private bool _isAttackComplete = false;  // Indicates whether the attack has completed
    private float _attackDuration = 1.5f;    // Duration of the attack animation
    private float _attackTimer = 0f;         // Timer to track attack time
    private const float _attackRadius = 1f;  // Radius for checking player collision
    private const float _attackDamage = 15f; // Damage to deal to the player

    public EnemyAttackState(StateMachine<EnemyStateMachine.EnemyState> stateMachine, Animator animator, Rigidbody2D rb, EnemyNormalAudioManager enemyAudio)
        : base(stateMachine, EnemyStateMachine.EnemyState.Attack)
    {
        _animator = animator;
        _rb = rb;
        _enemyAudio = enemyAudio;
    }

    public override void EnterState()
    {
        _animator.SetTrigger("attack");
        _attackTimer = 0f;
        _isAttackComplete = false;
        _enemyAudio.PlaySoundEffect("small-monster-attack-195712");
        attackPlayer();
    }

    public override void UpdateState()
    {
        if (!_isAttackComplete)
        {
            _attackTimer += Time.deltaTime;
            if (_attackTimer >= _attackDuration)
            {
                _isAttackComplete = true;
                StateMachine.QueueNextState(EnemyStateMachine.EnemyState.Chase);
            }
        }
    }

    public override void ExitState()
    {
    }

    public override EnemyStateMachine.EnemyState GetNextState()
    {
        return StateMachine.GetQueuedState();
    }
    private void attackPlayer()
    {
        Vector2 attackPosition = _rb.position;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPosition, _attackRadius);
        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                HealthManager healthManager = collider.GetComponent<HealthManager>();
                if (healthManager != null)
                {
                    healthManager.ReduceHealth(_attackDamage);
                }
            }
        }
    }
    public override void OnTriggerEnter2D(Collider2D other) { }
    public override void OnTriggerStay2D(Collider2D other) { }
    public override void OnTriggerExit2D(Collider2D other) { }

}
