using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAttack : MonoBehaviour
{
    private Animator _animator;
    public float damage;
    private EnemyAudioManager _audioManager;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioManager = FindObjectOfType<EnemyAudioManager>();
        _audioManager.PlayOneShotSoundEffect("chargegenkidama");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _audioManager.PlaySoundEffect("explode");
        if (collision.gameObject.CompareTag("Ground"))
        {
            _animator.SetTrigger("burn");


        }
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthManager healthmanager = collision.gameObject.GetComponent<HealthManager>();

            if (healthmanager != null)
            {
                healthmanager.ReduceHealth(damage);
            }
        }
    }
    private void Destroy()
    {
        _audioManager.StopOneShotSoundEffect();
        Destroy(gameObject);
    }
}
