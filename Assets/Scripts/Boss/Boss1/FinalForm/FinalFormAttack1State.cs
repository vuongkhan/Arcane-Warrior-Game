using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFormAttack1State : BaseState<FinalFormStateMachine.FinalFormState>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private Transform _playerTransform;
    private Vector2 _targetPosition;
    private WallCheck _wallCheck;
    private bool isClosePlayer = false;
    private float attackCooldown = 1.5f;
    private float attackStartTime;
    private bool isAttacked = false;
    private bool canBack = false;
    private float damage = 15f;
    private bool attacked;
    private EnemyAudioManager _audioManager;
    private float delayBeforeJumpAndAttack = 0.5f; 
    private float delayStartTime;
    private bool isDelaying = false; 
    private bool isPlayerHit = false;

    public FinalFormAttack1State(StateMachine<FinalFormStateMachine.FinalFormState> stateMachine, Animator animator, Rigidbody2D rb, Transform playerTransform, WallCheck wallCheck, EnemyAudioManager audioManager)
        : base(stateMachine, FinalFormStateMachine.FinalFormState.Attack1)
    {
        _animator = animator;
        _rb = rb;
        _playerTransform = playerTransform;
        _wallCheck = wallCheck;
        _audioManager = audioManager;
    }

    public override void EnterState()
    {
        _targetPosition = _playerTransform.position;
        isPlayerHit = false;
        _animator.SetFloat("Speed", 2);
    }

    public override void ExitState()
    {
        _animator.SetFloat("Speed", 0);
        isClosePlayer = false;
        canBack = false;
        isAttacked = false;
        isDelaying = false;
        isPlayerHit = false;
    }

    public override void UpdateState()
    {
        float distance = Mathf.Abs(_rb.position.x - _targetPosition.x);
        if (distance > 1 && !isClosePlayer)
        {
            Vector2 targetx = new Vector2(_targetPosition.x, _rb.position.y);
            Vector2 newPosition = Vector2.MoveTowards(_rb.position, targetx, 35f * Time.deltaTime);
            _rb.MovePosition(newPosition);
            _animator.SetFloat("Speed", 2);
        }
        else if (distance <= 1 && !isClosePlayer && !isAttacked)
        {
            _animator.SetTrigger("punch1");
            attackPlayer();
            _rb.velocity = Vector2.zero;
            attackStartTime = Time.time;
            isAttacked = true;
            _animator.SetFloat("Speed", 0);
            isClosePlayer = true;
        }
        if (isPlayerHit && isDelaying && (Time.time - delayStartTime >= delayBeforeJumpAndAttack))
        {
            JumpAndAttack();
            isDelaying = false;
        }

        if (isAttacked && (Time.time - attackStartTime > attackCooldown))
        {
            Debug.Log("Attack cooldown complete");
            isAttacked = false;
            canBack = true;
        }
        if (_wallCheck.distanceToWall > 1 && canBack)
        {
            _animator.SetBool("back", true);
            _animator.SetFloat("Speed", 2);
            Vector2 wallPosition = new Vector2(_wallCheck.wallObject.position.x, _rb.position.y);
            Vector2 direction = (wallPosition - _rb.position).normalized;
            _rb.velocity = direction * 10f;
        }
        else if (canBack)
        {
            _animator.SetBool("back", false);
            StateMachine.QueueNextState(FinalFormStateMachine.FinalFormState.Attack3);
        }
    }

    private void attackPlayer()
    {
        _audioManager.PlaySoundEffect("fightboss");

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(_rb.position, 1.5f);

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                HealthManager healthManager = collider.gameObject.GetComponent<HealthManager>();
                Rigidbody2D playerRb = collider.gameObject.GetComponent<Rigidbody2D>();

                if (healthManager != null)
                {
                    healthManager.ReduceHealth(damage);
                }

                if (playerRb != null)
                {
                    Vector2 knockbackVelocity = new Vector2(0, 15f);  
                    playerRb.velocity = knockbackVelocity;  
                }
                isPlayerHit = true;
                isDelaying = true;
                delayStartTime = Time.time;
            }
        }
    }

    private void JumpAndAttack()
    {
        StateMachine.QueueNextState(FinalFormStateMachine.FinalFormState.Attack6); 
    }

    public override FinalFormStateMachine.FinalFormState GetNextState()
    {
        return StateMachine.GetQueuedState();
    }

    public override void OnTriggerEnter2D(Collider2D other) { }
    public override void OnTriggerStay2D(Collider2D other) { }
    public override void OnTriggerExit2D(Collider2D other) { }
}
