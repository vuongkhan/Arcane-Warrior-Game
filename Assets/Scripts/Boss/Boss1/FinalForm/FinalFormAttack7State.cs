using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFormAttack7State : BaseState<FinalFormStateMachine.FinalFormState>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private GameObject[] _blastPrefab;
    private GameObject _blast;
    private float attackStartTime;
    private float projectileSpawnTime;
    private float spawnInterval = 0.2f;
    private float attackDelay = 0.5f; 
    private int projectileCount = 0;
    private int maxProjectiles = 10; 
    private FlipCharacter _flip;
    private EnemyAudioManager _audioManager;
    private Transform _player; 

    public FinalFormAttack7State(StateMachine<FinalFormStateMachine.FinalFormState> stateMachine, Animator animator, Rigidbody2D rb, GameObject[] blastPrefab, FlipCharacter flip, EnemyAudioManager audioManager, Transform playerTransform)
        : base(stateMachine, FinalFormStateMachine.FinalFormState.Attack7)
    {
        _animator = animator;
        _rb = rb;
        _blastPrefab = blastPrefab;
        _flip = flip;
        _audioManager = audioManager;
        _player = playerTransform; 
    }

    public override void EnterState()
    {
        _animator.SetBool("dubleattack", true); 
        attackStartTime = Time.time;
        projectileSpawnTime = Time.time;
        projectileCount = 0;

        _audioManager.PlaySoundEffect("duble"); 
    }

    public override void ExitState()
    {
        _animator.SetBool("dubleattack", false);
    }

    public override void UpdateState()
    {
        if (Time.time >= attackStartTime + attackDelay)
        {
            if (projectileCount < maxProjectiles && Time.time >= projectileSpawnTime + spawnInterval)
            {
                SpawnProjectile();
                projectileSpawnTime = Time.time; 
            }
            if (projectileCount >= maxProjectiles)
            {
                StateMachine.QueueNextState(FinalFormStateMachine.FinalFormState.Attack8);
            }
        }
    }

    private void SpawnProjectile()
    {
        _blast = GameObject.Instantiate(_blastPrefab[4], _rb.position, Quaternion.identity);
        Rigidbody2D blastRb = _blast.GetComponent<Rigidbody2D>();

        if (blastRb != null)
        {
            Vector2 direction = (_player.position - (Vector3)_rb.position).normalized;
            blastRb.velocity = direction * 10f;
        }

        FlipEffect(_blast);

        projectileCount++;
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

    public override FinalFormStateMachine.FinalFormState GetNextState()
    {
        return StateMachine.GetQueuedState();
    }

    public override void OnTriggerEnter2D(Collider2D other) { }
    public override void OnTriggerStay2D(Collider2D other) { }
    public override void OnTriggerExit2D(Collider2D other) { }
}
