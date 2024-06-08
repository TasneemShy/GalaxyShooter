using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private float _rotateSpeed = 8f;

    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;

    private void Start()
    {
        try
        { 
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        }
        catch
        {
            Debug.LogError("The Spawn Manager is NULL");
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.back * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
        }
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _spawnManager.StartSpawning();
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 0.3f);
    }
}
