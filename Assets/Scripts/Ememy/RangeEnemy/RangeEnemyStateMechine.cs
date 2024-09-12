using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyStateMachine : StateMachine<RangeEnemyStateMachine.RangeEnemy>
{
    [SerializeField] private float _moveSpeed = 5f;
    private Animator _animator;
    private Rigidbody2D _rb;
    private FlipCharacter _flip;
    [SerializeField] private Transform[] _patrolPoint;
    private CheckPlayer _checkPlayer;
    [SerializeField] private GameObject[] _fire;
    public float _Hp;
    public bool _canHurt = false;
    private EnemyNormalAudioManager _enemyAudio;

    public enum RangeEnemy
    {
        Patrol,
        Die,
        Attack,

    }
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _checkPlayer = GetComponent<CheckPlayer>();
        _flip = GetComponent<FlipCharacter>();
        _enemyAudio = FindObjectOfType<EnemyNormalAudioManager>();

       RegisterState(RangeEnemy.Patrol, new RangeEnemyPatrolState(this, _animator, _rb, _patrolPoint, _moveSpeed, _checkPlayer, _flip));
        RegisterState(RangeEnemy.Attack, new RangeEnemyAttackState(this, _animator, _rb, _checkPlayer, _fire, _enemyAudio));
        RegisterState(RangeEnemy.Die, new RangeEnemyDieState(this, _animator, _rb, _enemyAudio));
        SetInitialState(RangeEnemy.Patrol);
    }
}
