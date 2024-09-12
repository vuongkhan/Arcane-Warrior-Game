using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Appear : MonoBehaviour
{
    public Vector2 spawnPosision;
    public GameObject _gameobject;
    public void OnTriggerEnter2D(Collider2D collider) 
    {
        Destroy(gameObject);
        Instantiate(_gameobject, spawnPosision, Quaternion.identity);
    }

}
