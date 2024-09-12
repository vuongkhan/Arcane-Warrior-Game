using System.Collections;
using UnityEngine;

public class FinalFormAttack5State : BaseState<FinalFormStateMachine.FinalFormState>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private GameObject[] _blastPrefab;
    private GameObject _blast;
    private EnemyAudioManager _audioManager;

    private bool canAttack = false; 
    private float delayTime = 2f; 
    private float elapsedTime = 0f; 

    public FinalFormAttack5State(StateMachine<FinalFormStateMachine.FinalFormState> stateMachine, Animator animator, Rigidbody2D rb, GameObject[] blastPrefab, FlipCharacter flip, EnemyAudioManager audioManager)
        : base(stateMachine, FinalFormStateMachine.FinalFormState.Attack5)
    {
        _animator = animator;
        _rb = rb;
        _blastPrefab = blastPrefab;
        _audioManager = audioManager;
    }

    public override void EnterState()
    {
        _animator.SetTrigger("attack5");
        elapsedTime = 0f; 
        canAttack = false; 
    }

    public override void UpdateState()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= delayTime && !canAttack)
        {
            canAttack = true; 
            SpawnProjectile();  
            StateMachine.QueueNextState(FinalFormStateMachine.FinalFormState.Attack2); 
        }
    }

    private void SpawnProjectile()
    {
        GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
        foreach (GameObject point in points)
        {
            Vector3 spawnPosition = new Vector3(point.transform.position.x, point.transform.position.y + 7.0f, point.transform.position.z);
            GameObject projectile = GameObject.Instantiate(_blastPrefab[3], spawnPosition, Quaternion.identity);
        }
    }

    public override void ExitState()
    {
    }

    public override FinalFormStateMachine.FinalFormState GetNextState()
    {
        return StateMachine.GetQueuedState();
    }

    public override void OnTriggerEnter2D(Collider2D other) { }
    public override void OnTriggerStay2D(Collider2D other) { }
    public override void OnTriggerExit2D(Collider2D other) { }
}
