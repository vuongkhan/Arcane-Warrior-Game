using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    public GameObject explosionPrefab;
    private AudioManager _audioManager;
    void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")&& !other.CompareTag("Ground"))
        {
            _audioManager.PlaySoundEffect("explosion");
            GameObject explosion = Object.Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            BossHealthManager healthmanager = other.gameObject.GetComponent<BossHealthManager>();
            FinalFormHealthManager finalhealthmanager = other.gameObject.GetComponent<FinalFormHealthManager>();
            EnemyHealthManager enemyhealthmanager = other.gameObject.GetComponent<EnemyHealthManager>();
            Stone stoneHealthManager = other.gameObject.GetComponent<Stone>();
            RangeEnemyHealthManager rangeenemyHealthManager = other.gameObject.GetComponent<RangeEnemyHealthManager>();
            if (finalhealthmanager != null)
            {
                finalhealthmanager.ReduceHealth(damage);
            }
            if (healthmanager != null)
            {
                healthmanager.ReduceHealth(damage);
            }
            if (enemyhealthmanager != null)
            {
                enemyhealthmanager.ReduceHealth(damage);
            }
            if (stoneHealthManager != null)
            {
                stoneHealthManager.ReduceHealth(damage);
            }
            if (rangeenemyHealthManager != null)
            {
                rangeenemyHealthManager.ReduceHealth(damage);
            }
            Destroy(gameObject);
        }

    }
}
