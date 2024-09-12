using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Ground : MonoBehaviour
{
    public float damage = 10f;
    private EnemyNormalAudioManager _enemyAudio;
    private bool isPlay = false;

    void Start()
    {
        _enemyAudio = FindObjectOfType<EnemyNormalAudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !isPlay)
        {
            _enemyAudio.PlaySoundEffect("dropdown");
            isPlay = true;
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
}
