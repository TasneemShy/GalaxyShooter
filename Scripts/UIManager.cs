using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private int _score, _bestScore;

    [SerializeField]
    private Text _scoreText, _bestScoreText;

    [SerializeField]
    private Image _liveImg;
    [SerializeField]
    private Image _player2LiveImg;

    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;

    private GameManager _gameManager;

    void Start()
    {
        _scoreText.text = "Score: " + 0;

        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);

        try
        {
            _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        }
        catch
        {
            Debug.LogError("The Game Manager is NULL");
        }

        if (!_gameManager._isCoopMode)
        {
            _bestScore = PlayerPrefs.GetInt("BestScore", 0);
            _bestScoreText.text = "Best: " + _bestScore;
        }

    }

    public void UpdateScore(int score)
    {
        _score = score;
        _scoreText.text = "Score: " + _score;
    }
   
    public void CheckBestScore()
    {
        if (_score > _bestScore)
        {
            _bestScore = _score;
            PlayerPrefs.SetInt("BestScore", _bestScore);
            _bestScoreText.text = "Best: " + _bestScore;
        }
    }

    public void UpdateLives(int currentLives, bool isPlayer1, bool isCoop)
    {
        if (isPlayer1 && !isCoop)
        {
            _liveImg.sprite = _liveSprites[currentLives];         
        }
        else if (isPlayer1 && isCoop){
            _player2LiveImg.sprite = _liveSprites[currentLives];          
        }
        else if (!isPlayer1 && isCoop)
        {
            _liveImg.sprite = _liveSprites[currentLives];
           
        }
        if (currentLives == 0)
            {
                EndingGame();
            }
    }

    public void EndingGame()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";

        }
    }

    public void ResumeGame()
    {
        _gameManager.ResumeGame();
    }

    public void BackToMainMenu()
    {
        _gameManager.ResumeGame();
        Destroy(GameObject.Find("Audio_Manager"));
        SceneManager.LoadScene(0);
    }
}
