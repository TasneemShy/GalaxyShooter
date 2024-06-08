using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private bool _isGameOver = false;
    public bool _isCoopMode = false;
    public bool _isMainMenu = false;
    [SerializeField]
    private GameObject _pauseMenuPanel;
    private Animator _pauseAnim;

    private void Start()
    {
        try
        {
            if (SceneManager.GetActiveScene().name != "Main_Menu")
            {
                _pauseAnim = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
                _pauseAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
            }

        }
        catch
        {
            Debug.LogError("The Animator is NULL");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            if (_isCoopMode) { SceneManager.LoadScene(2); }
            else { SceneManager.LoadScene(1); }
            
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.P) && !_isMainMenu)
        {
            _pauseMenuPanel.SetActive(true);
            _pauseAnim.SetBool("isPaused", true);
            Time.timeScale = 0;

        }

    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void ResumeGame()
    {
        _pauseMenuPanel.SetActive(false);
        _pauseAnim.SetBool("isPaused", false);
        Time.timeScale = 1;
    }
}
