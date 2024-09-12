using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyAttackState : BaseState<RangeEnemyStateMachine.RangeEnemy>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private CheckPlayer _checkPlayer;
    private float _attackCooldown = 2f;
    private float _lastAttackTime;
    private GameObject[] _fire;
    private GameObject fireObject;
    private EnemyNormalAudioManager _enemyAudio;
    private bool _soundPlayed = false;

    public RangeEnemyAttackState(StateMachine<RangeEnemyStateMachine.RangeEnemy> stateMachine, Animator animator, Rigidbody2D rb, CheckPlayer checkPlayer, GameObject[] fire, EnemyNormalAudioManager enemyAudio)
        : base(stateMachine, RangeEnemyStateMachine.RangeEnemy.Attack)
    {
        _animator = animator;
        _rb = rb;
        _checkPlayer = checkPlayer;
        _fire = fire;
        _enemyAudio = enemyAudio;
    }

    public override void EnterState()
    {
        Debug.Log("Entering Attack State");
        _animator.SetTrigger("attack");
        if (!_soundPlayed)
        {
            _enemyAudio.PlaySoundEffect("large-monster");
            _soundPlayed = true;
        }
        _lastAttackTime = Time.time - _attackCooldown; 
    }

    public override void ExitState()
    {
        _soundPlayed = false;
    }

    public override void UpdateState()
    {
        if (_checkPlayer.IsPlayer())
        {
            if (Time.time >= _lastAttackTime + _attackCooldown)
            {
                Attack();
                _lastAttackTime = Time.time; 
            }
        }
        else
        {
            StateMachine.QueueNextState(RangeEnemyStateMachine.RangeEnemy.Patrol);
        }
    }

    private void Attack()
    {

        _animator.SetTrigger("attack");
        GameObject projectile = GameObject.Instantiate(_fire[0], _rb.position, Quaternion.identity);
        Vector2 shootDirection = _rb.transform.localScale.x > 0 ? Vector2.right : Vector2.left;  // Shoot based on facing direction
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.velocity = shootDirection * 5f;

        Debug.Log("Enemy shoots a projectile");
    }

    public override RangeEnemyStateMachine.RangeEnemy GetNextState()
    {
        return StateMachine.GetQueuedState();
    }

    public override void OnTriggerEnter2D(Collider2D other) { }
    public override void OnTriggerStay2D(Collider2D other) { }
    public override void OnTriggerExit2D(Collider2D other) { }
}
