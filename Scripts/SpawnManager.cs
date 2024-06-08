using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;

    private bool _alive = true;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (_alive)
        {
            float randomX = Random.Range(-9f, 9f);
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(randomX, 8f, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(3f);
        }

    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (_alive)
        {
            float randomX = Random.Range(-9f, 9f);
            int power = Random.Range(0, 3);
            Instantiate(powerups[power], new Vector3(randomX, 8f, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(5,11));
        }

    }

    public void Dead()
    {
        _alive = false;
    }
}
