using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine<EnemyStateMachine.EnemyState>
{
    [SerializeField] private float _moveSpeed = 5f;
    private Animator _animator;
    private Rigidbody2D _rb;
    private FlipCharacter _flip;
    [SerializeField] private Transform[] _patrolPoint;
    private CheckPlayer _checkPlayer;
    public float _Hp;
    public bool _canHurt=false;
    private EnemyNormalAudioManager _enemyAudio;

    public enum EnemyState
    {
        Patrol,
        Chase,
        Hit,
        Die,
        Attack
        
    }
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _checkPlayer = GetComponent<CheckPlayer>();
        _flip = GetComponent<FlipCharacter>();
        _enemyAudio = FindObjectOfType<EnemyNormalAudioManager>();

        RegisterState(EnemyState.Patrol, new PatrolState(this, _animator,_rb, _patrolPoint, _moveSpeed, _checkPlayer,_flip));
        RegisterState(EnemyState.Chase, new ChaseState(this, _animator, _rb, _moveSpeed, _checkPlayer, _flip));
        RegisterState(EnemyState.Hit, new HitState(this, _animator, _rb, _Hp, _enemyAudio));
        RegisterState(EnemyState.Die, new DieState(this, _animator, _rb, _Hp, _enemyAudio));
        RegisterState(EnemyState.Attack, new EnemyAttackState(this, _animator, _rb, _enemyAudio));
        SetInitialState(EnemyState.Patrol);
    }
}
