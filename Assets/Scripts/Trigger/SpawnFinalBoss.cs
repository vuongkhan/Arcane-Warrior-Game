using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFinalBoss : MonoBehaviour
{
    public GameObject _gameobject;
    private EnemyAudioManager _audio;
    void Start()
    {
        _audio = FindObjectOfType<EnemyAudioManager>();
        _audio.PlaySoundEffect("bird-magic-skill");
    }
    void Update()
    {
        
    }
    private void Spawn()
    {
        Instantiate(_gameobject, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
