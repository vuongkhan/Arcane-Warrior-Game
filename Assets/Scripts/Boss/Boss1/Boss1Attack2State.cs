using UnityEngine;

public class Boss1Attack2State : BaseState<Boss1StateMachine.Boss1State>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private float _pushForce = 10f;
    private WallCheck _wallCheck;
    private FlipCharacter _flip;
    private float chargeTime = 1f;
    private float _attackStartTime;
    public GameObject fireEffectPrefab;
    private Transform _pointPosition;
    private GameObject[] _effect;
    private bool isSpawn = true;
    private float damage = 15f;
    private EnemyAudioManager _audioManager;


    private bool hasDamaged = false; 
    public Boss1Attack2State(StateMachine<Boss1StateMachine.Boss1State> stateMachine, Animator animator, Rigidbody2D rb, WallCheck wallCheck, FlipCharacter flip, GameObject fireEffectPrefab, Transform pointPosition, GameObject[] effect, EnemyAudioManager audioManager)
        : base(stateMachine, Boss1StateMachine.Boss1State.Attack2)
    {
        _animator = animator;
        _rb = rb;
        _wallCheck = wallCheck;
        _flip = flip;
        _pointPosition = pointPosition;
        _effect = effect;
        _audioManager = audioManager;
    }

    public override void EnterState()
    {
        _animator.SetTrigger("Charge");
        _attackStartTime = Time.time;
        hasDamaged = false; 

    }

    public override void ExitState()
    {
        _animator.SetBool("kick", false);
        isSpawn = true;
    }

    public override void UpdateState()
    {
        Debug.Log(_flip.IsFacingRight());

        if (Time.time - _attackStartTime >= chargeTime)
        {
            if (isSpawn)
            {
                isSpawn = false;
                GameObject effectDust = GameObject.Instantiate(_effect[0], _rb.position, Quaternion.identity);
                FlipEffect(effectDust);
            }

            _animator.SetBool("kick", true);
                Vector2 attackPosition2D = _pointPosition.position;
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPosition2D, 0.5f);

                if (_flip.IsFacingRight())
                {
                    _rb.velocity = Vector2.left * _pushForce;
                }
                else
                {
                    _rb.velocity = Vector2.right * _pushForce;
                }

                foreach (Collider2D hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("Player"))
                    {
                 
                    _audioManager.PlaySoundEffect("kick");
                        HealthManager healthManager = hitCollider.gameObject.GetComponent<HealthManager>();

                        if (healthManager != null&& !hasDamaged)
                        {
                            healthManager.ReduceHealth(damage);
                            hasDamaged = true; 
                        }
                }
                    if (hitCollider.CompareTag("Wall"))
                    {
                        StateMachine.QueueNextState(Boss1StateMachine.Boss1State.Idle);
                        _flip.Flip();
                    }
                }
        }
    }

    private void FlipEffect(GameObject effect)
    {
        if (_flip.IsFacingRight())
        {
            Vector3 scale = effect.transform.localScale;
            scale.x *= -1;
            effect.transform.localScale = scale;
        }
    }

    public override Boss1StateMachine.Boss1State GetNextState()
    {
        return StateMachine.GetQueuedState();
    }

    public override void OnTriggerEnter2D(Collider2D other) { }
    public override void OnTriggerStay2D(Collider2D other) { }
    public override void OnTriggerExit2D(Collider2D other) { }
}
