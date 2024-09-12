using UnityEngine;

public class Attack2State : BaseState<PlayerStateMachine.PlayerState>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private float _attackDuration = 0.3f;
    private float _timer = 0f;

    private float attackRadius = 0.2f;
    private Transform _attackPosition;
    private int damage = 10;
    private AudioManager _audioManager;

    public Attack2State(StateMachine<PlayerStateMachine.PlayerState> stateMachine, Animator animator, Rigidbody2D rb, Transform attackPosition, AudioManager audioManager)
        : base(stateMachine, PlayerStateMachine.PlayerState.Attack2)
    {
        _animator = animator;
        _rb = rb;
        _attackPosition = attackPosition;
        _audioManager= audioManager;
    }

    public override void EnterState()
    {
        _audioManager.PlaySoundEffect("slash2");
        _animator.SetTrigger("Attack2");
        _timer = 0f;

        Vector2 attackPosition2D = (Vector2)_attackPosition.position;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPosition2D, attackRadius);

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                BossHealthManager enemybossHealthManager = collider.gameObject.GetComponent<BossHealthManager>();
                FinalFormHealthManager finalFormHealthManager = collider.gameObject.GetComponent<FinalFormHealthManager>();
                EnemyHealthManager enemyHealthManager = collider.gameObject.GetComponent<EnemyHealthManager>();
                RangeEnemyHealthManager rangeenemyHealthManager = collider.gameObject.GetComponent<RangeEnemyHealthManager>();
                Stone stoneHealthManager = collider.gameObject.GetComponent<Stone>();
                if (finalFormHealthManager != null)
                {
                    finalFormHealthManager.ReduceHealth(damage);
                }
                if (enemybossHealthManager != null)
                {
                    enemybossHealthManager.ReduceHealth(damage);
                }
                if (enemyHealthManager != null)
                {
                    enemyHealthManager.ReduceHealth(damage);
                }
                if (stoneHealthManager != null)
                {
                    stoneHealthManager.ReduceHealth(damage);
                }
                if (rangeenemyHealthManager != null)
                {
                    rangeenemyHealthManager.ReduceHealth(damage);
                }
            }
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Attack2 State");
    }

    public override void UpdateState()
    {
        _timer += Time.deltaTime;
        if (_timer >= _attackDuration)
        {
            StateMachine.QueueNextState(PlayerStateMachine.PlayerState.Idle);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StateMachine.QueueNextState(PlayerStateMachine.PlayerState.Attack3);
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
