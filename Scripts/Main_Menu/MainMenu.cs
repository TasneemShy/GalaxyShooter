using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
  
    public void LoadSinglePlayerGame()
    {
        DontDestroyOnLoad(GameObject.Find("Audio_Manager"));
        SceneManager.LoadScene(1);
    }

    public void LoadCoopModeGame()
    {
        DontDestroyOnLoad(GameObject.Find("Audio_Manager"));
        SceneManager.LoadScene(2);
    }
}
