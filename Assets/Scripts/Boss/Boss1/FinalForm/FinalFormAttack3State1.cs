using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFormAttack3State : BaseState<FinalFormStateMachine.FinalFormState>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private GameObject[] _blastPrefab;
    private GameObject _blast;
    private float attackStartTime;
    private float projectileSpawnTime;
    private float spawnInterval = 0.2f;
    private int projectileCount = 0;
    private int maxProjectiles = 2;
    private FlipCharacter _flip;
    private EnemyAudioManager _audioManager;
    private float stateEndDelay = 1f;  
    private bool isWaitingToEnd = false;  
    private float waitStartTime;  

    public FinalFormAttack3State(StateMachine<FinalFormStateMachine.FinalFormState> stateMachine, Animator animator, Rigidbody2D rb, GameObject[] blastPrefab, FlipCharacter flip, EnemyAudioManager audioManager)
        : base(stateMachine, FinalFormStateMachine.FinalFormState.Attack3)
    {
        _animator = animator;
        _rb = rb;
        _blastPrefab = blastPrefab;
        _flip = flip;
        _audioManager = audioManager;
    }

    public override void EnterState()
    {
        _animator.SetBool("dubleattack", true);
        attackStartTime = Time.time;
        projectileSpawnTime = Time.time;
        projectileCount = 0;
        isWaitingToEnd = false;

        _audioManager.PlaySoundEffect("duble");
    }

    public override void ExitState()
    {
        _animator.SetBool("dubleattack", false);
    }

    public override void UpdateState()
    {
        if (!isWaitingToEnd)
        {
            if (projectileCount < maxProjectiles && Time.time >= projectileSpawnTime + spawnInterval)
            {
                SpawnProjectile();
                projectileSpawnTime = Time.time; 
            }
            if (projectileCount >= maxProjectiles)
            {
                _animator.SetBool("dubleattack", false);
                isWaitingToEnd = true;
                waitStartTime = Time.time; 
            }
        }
        else
        {
            if (Time.time >= waitStartTime + stateEndDelay)
            {
                StateMachine.QueueNextState(FinalFormStateMachine.FinalFormState.Attack4);
            }
        }
    }

    private void SpawnProjectile()
    {
        _blast = GameObject.Instantiate(_blastPrefab[1], _rb.position, Quaternion.identity);
        Rigidbody2D blastRb = _blast.GetComponent<Rigidbody2D>();

        if (blastRb != null)
        {
            Vector2 direction = _flip.IsFacingRight() ? Vector2.left : Vector2.right;
            blastRb.velocity = direction * 10f; 
        }


        projectileCount++;
    }



    public override FinalFormStateMachine.FinalFormState GetNextState()
    {
        return StateMachine.GetQueuedState();
    }

    public override void OnTriggerEnter2D(Collider2D other) { }
    public override void OnTriggerStay2D(Collider2D other) { }
    public override void OnTriggerExit2D(Collider2D other) { }
}
