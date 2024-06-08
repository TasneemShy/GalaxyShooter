using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 3f;
    private Player _player, _player2;
    private GameManager _gameManager;

    [SerializeField]
    private GameObject _laserPrefabs;
    private float _fireRate = 3f, _canFire = -1f;


    private Animator _anim;

    [SerializeField]
    private AudioClip _clip;

    [SerializeField]
    private AudioClip _laserClip;

    void Start()
    {
        try
        {
            _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
            _player = GameObject.Find("Player").GetComponent<Player>();

            if (_gameManager._isCoopMode)
            {
                _player2 = GameObject.Find("Player_2").GetComponent<Player>();
            }
        }
        catch
        {
            Debug.LogError("The Player is NULL");
        }

        try
        {
            _anim = GetComponent<Animator>();
        }
        catch
        {
            Debug.LogError("The Animator is NULL");
        }
    }

    void Update()
    {              
        CalculateMovement();

        if (Time.deltaTime > _canFire)
        {
            _canFire = Time.time + _fireRate;

            AudioSource.PlayClipAtPoint(_laserClip, transform.position);
            GameObject enemyLaser = Instantiate(_laserPrefabs, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }       

    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.5f && _player != null)
        {
                int lucky = Random.Range(1, 3);
                if(lucky == 1 || !_gameManager._isCoopMode) { _player.Damage(); }
                else { _player2.Damage();}            

            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_player != null && other.GetComponent<Player>().Equals(_player))
            {
                _player.Damage();
            }
            else if (_player2 != null && other.GetComponent<Player>().Equals(_player2))
            {
                _player2.Damage();
            }     
        }
        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
        
            if (_player != null)
            {
                _player.IncreaseScore();
            }
            /*
           if (_player != null && (!_gameManager._isCoopMode || _player._isPlayer1))
           {
               _player.IncreaseScore();
           }
           else if (_player2 != null && _gameManager._isCoopMode && !_player2._isPlayer1)
           {
               _player2.IncreaseScore();
           }*/
        }
        if (other.tag == "Laser" || other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 0.3f);
        }
    }
   
}
