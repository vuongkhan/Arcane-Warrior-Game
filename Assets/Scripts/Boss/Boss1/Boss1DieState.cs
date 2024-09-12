using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1DieState : BaseState<Boss1StateMachine.Boss1State>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private GameObject[] _effect;
    private GameObject _finalForm;
    private float startTime;
    private bool hasSpawned = false;
    private bool hasChanged = false;
    private bool hasFinalSpawned = false;
    private float angryStartTime;
    private float effectChangeTime;
    private GameObject instantiatedEffect;
    private EnemyAudioManager _audioManager;

    public Boss1DieState(StateMachine<Boss1StateMachine.Boss1State> stateMachine, Animator animator, Rigidbody2D rb, GameObject[] effect, GameObject finalForm, EnemyAudioManager audioManager)
        : base(stateMachine, Boss1StateMachine.Boss1State.Die)
    {
        _animator = animator;
        _rb = rb;
        _effect = effect;
        _finalForm = finalForm;
        _audioManager= audioManager;
    }

    public override void EnterState()
    {
        _animator.SetTrigger("die");
        startTime = Time.time;
        hasSpawned = false;
        hasChanged = false;
        hasFinalSpawned = false;
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        if (!hasSpawned && Time.time >= startTime + 2f)
        {
            _animator.SetTrigger("angry");
            _audioManager.PlaySoundEffect("shout");

            hasSpawned = true;
            angryStartTime = Time.time;  
        }
        if (hasSpawned && !hasChanged && Time.time >= angryStartTime + 2f)
        {
            _audioManager.PlayOneShotSoundEffect("aura"); 
            instantiatedEffect = GameObject.Instantiate(_effect[2], _rb.position, Quaternion.identity); 
            hasChanged = true;
            effectChangeTime = Time.time; 
        }
        if (hasChanged && !hasFinalSpawned && Time.time >= effectChangeTime + 2f)
        {
            GameObject.Destroy(instantiatedEffect);
            GameObject.Destroy(StateMachine.gameObject);
            GameObject.Instantiate(_finalForm, _rb.position, Quaternion.identity); 
            hasFinalSpawned = true;
            _audioManager.StopOneShotSoundEffect();
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
