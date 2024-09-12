using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Attack5State : BaseState<Boss1StateMachine.Boss1State>
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
    private bool Attacking=false;
    private bool isplayer = false;
    public Boss1Attack5State(StateMachine<Boss1StateMachine.Boss1State> stateMachine, Animator animator, Rigidbody2D rb, GameObject meteorPrefab, Transform pointUlti, FlipCharacter flipCharacter, GameObject[] effect, EnemyAudioManager audioManager, Transform playerTransform)
        : base(stateMachine, Boss1StateMachine.Boss1State.Attack5)
    {
        _animator = animator;
        _rb = rb;
        _meteorPrefab = meteorPrefab;
        _pointUlti = pointUlti;
        _flipCharacter = flipCharacter;
        _effect = effect;
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
        Vector3 abovePlayerPosition = new Vector3(_targetPosition.x, _targetPosition.y + 4.0f, _targetPosition.z);
        _rb.position = abovePlayerPosition;
        _hasTeleported = true;
        _animator.SetBool("punchdown", true);
        Vector2 diveDirection = Vector2.down * 2f; 
        _rb.velocity = diveDirection;
        Attacking= true;
    }
    private void attackPlayer()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(_rb.position, 1f);

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Player") &&!isplayer)
            {
                isplayer = true;
                _audioManager.PlaySoundEffect("kick");
                HealthManager healthmanager = collider.gameObject.GetComponent<HealthManager>();

                if (healthmanager != null)
                {
                    healthmanager.ReduceHealth(damage);
                }
                StateMachine.QueueNextState(Boss1StateMachine.Boss1State.Attack1);

            }
            else if (collider.CompareTag("Ground"))
            {
                StateMachine.QueueNextState(Boss1StateMachine.Boss1State.Attack2);
            }

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
