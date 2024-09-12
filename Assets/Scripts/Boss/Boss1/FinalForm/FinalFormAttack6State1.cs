using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFormAttack6State : BaseState<FinalFormStateMachine.FinalFormState>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private GameObject _meteorPrefab;
    private Transform _pointUlti;
    private FlipCharacter _flipCharacter;
    private GameObject[] _effect;
    private EnemyAudioManager _audioManager;
    private Transform _playerTransform;
    private Vector3 _targetPosition;
    private bool _hasTeleported = false;
    private float damage = 10;
    private bool Attacking = false;
    private bool isplayer = false;
    public FinalFormAttack6State(StateMachine<FinalFormStateMachine.FinalFormState> stateMachine, Animator animator, Rigidbody2D rb, Transform playerTransform, FlipCharacter flipCharacter, EnemyAudioManager audioManager)
        : base(stateMachine, FinalFormStateMachine.FinalFormState.Attack6)
    {
        _animator = animator;
        _rb = rb;
        _flipCharacter = flipCharacter;
        _audioManager = audioManager;
        _playerTransform = playerTransform;
    }

    public override void EnterState()
    {
        _targetPosition = _playerTransform.position;
        _hasTeleported = false;
        _audioManager.PlaySoundEffect("teleport");
    }

    public override void ExitState()
    {
        _animator.SetBool("punchdown", false);
        isplayer = false;
    }

    public override void UpdateState()
    {
        if (!_hasTeleported)
        {
            TeleportAbovePlayer();
        }
        if (Attacking)
        {
            attackPlayer();
        }
    }

    private void TeleportAbovePlayer()
    {
        Vector3 abovePlayerPosition = new Vector3(_targetPosition.x, _targetPosition.y + 1.0f, _targetPosition.z);
        _rb.position = abovePlayerPosition;
        _hasTeleported = true;
        _animator.SetBool("punchdown", true);
        Vector2 diveDirection = Vector2.down * 2f; 
        _rb.velocity = diveDirection;
        Attacking = true;
    }

    private void attackPlayer()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(_rb.position, 1f);

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Player") && !isplayer)
            {
                isplayer = true;
                _audioManager.PlaySoundEffect("kick");
                HealthManager healthManager = collider.gameObject.GetComponent<HealthManager>();
                Rigidbody2D playerRb = collider.gameObject.GetComponent<Rigidbody2D>(); 

                if (healthManager != null)
                {
                    healthManager.ReduceHealth(damage);
                }
                if (playerRb != null)
                {
                    Vector2 knockbackVelocity = new Vector2(0, -15f);  
                    playerRb.velocity = knockbackVelocity; 
                }
            }
            else if (collider.CompareTag("Ground"))
            {
                StateMachine.QueueNextState(FinalFormStateMachine.FinalFormState.Attack5);
            }
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
