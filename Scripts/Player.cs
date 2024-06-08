using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool _isPlayer1;

    [SerializeField]
    private float _speed = 8f;
    private float originalspeed = 8f;

    [SerializeField]
    private GameObject _laserPrefab;

    private float _fireRate = 0.15f, _canFire = -1f;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private GameObject _shieldVisualazer;

    [SerializeField]
    private GameObject _leftEngineVisualazer, _rightEngineVisualazer, _thruster;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private SpawnManager _spawnManager;

    [SerializeField]
    private AudioClip _laserAudioClip;
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _clip;

    private bool TripleShot = false;
    private bool Shielded = false;

    private int _score;
    private UIManager _uiManager;
    private GameManager _gameManager;

    private Animator _anim;


    void Start()
    {

        try
        {
            _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
            _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
            _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        }
        catch
        {
            Debug.LogError("The Spwan or UI or Game Manager is NULL");
        }

        if (_gameManager._isCoopMode)
        {
            if (_isPlayer1) {
                transform.position = new Vector3(5, -3.3f, 0);
            }
            else{
                transform.position = new Vector3(-5, -3.3f, 0);                  
            }
        }
        else { transform.position = new Vector3(0, -3.3f, 0);}

        try
        {
            _anim = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip = _laserAudioClip;
        }
        catch
        {
            Debug.LogError("The Animator or the Audio Source is NULL");
        }
    }

    void Update()
    {
        if (!_gameManager._isCoopMode || _isPlayer1)
        {
            SingleMovement();
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            {
                FireLaser();
            }
        }
        else if (_gameManager._isCoopMode && !_isPlayer1)
        {
            CoopMovement();
            if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > _canFire)
            {
                FireLaser();
            }
        }
    }

    void CalculateMovement()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.9f, 5.9f), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void SingleMovement()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
        CalculateMovement();
    }

    void CoopMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
        CalculateMovement();
    }


    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        if (TripleShot)
        {
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(1.65f, 1.6f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
        }
        _audioSource.Play();
    }

    public void Damage()
    {
        if (Shielded)
        {
            _shieldVisualazer.SetActive(false);
            Shielded = false;
            return;
        }

        _lives--;
        _uiManager.UpdateLives(_lives,_isPlayer1, _gameManager._isCoopMode);

        switch (_lives)
        {
            case 0:
                GameOver();
                break;
            case 1:
                _rightEngineVisualazer.SetActive(true);
                break;
            case 2:
                _leftEngineVisualazer.SetActive(true);
                break;
            default:
                break;
        }
    }

    void GameOver()
    {
        _anim.SetTrigger("OnEnemyDeath");
        AudioSource.PlayClipAtPoint(_clip, transform.position);

        _thruster.SetActive(false);
        _leftEngineVisualazer.SetActive(false);
        _rightEngineVisualazer.SetActive(false);
        _speed = 0;

        _spawnManager.Dead();

        
        if (!_gameManager._isCoopMode)
        {
            _uiManager.CheckBestScore();
        }

        Destroy(this.gameObject,0.5f);
    }

    public void SetTripleShot(bool TripleShot)
    {
        this.TripleShot = TripleShot;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        this.TripleShot = false;
    }

    public void SetSpeed(int newSpeed)
    {
        _speed = newSpeed;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _speed = originalspeed;
    }

    public void SetShield(bool Shield)
    {
        _shieldVisualazer.SetActive(true);
        Shielded = Shield;
    }

    public void IncreaseScore()
    {
        _score += 10;
        _uiManager.UpdateScore(_score);
    }
}
