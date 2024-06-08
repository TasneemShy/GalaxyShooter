using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _anim;
    private GameManager _gameManager;
    public bool _isPlayer1;

    void Start()
    {    
        try
        {
            _anim = GetComponent<Animator>();
            _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        }
        catch
        {
            Debug.LogError("The Animator or Game Manager is NULL");
        }
    }

    void Update()
    {
        if (!_gameManager._isCoopMode || _isPlayer1)
        {
            Player1Anim();
        }
        else if (_gameManager._isCoopMode && !_isPlayer1)
        {
            Player2Anim();
        }
    }

    void Player1Anim()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _anim.SetBool("TurnLeft",true);
            _anim.SetBool("TurnRight",false);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _anim.SetBool("TurnRight",true);
            _anim.SetBool("TurnLeft",false);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            _anim.SetBool("TurnLeft",false);
            _anim.SetBool("TurnRight",false);
        }

    }

    void Player2Anim()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _anim.SetBool("TurnLeft",true);
            _anim.SetBool("TurnRight",false);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _anim.SetBool("TurnRight",true);
            _anim.SetBool("TurnLeft",false);
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            _anim.SetBool("TurnLeft",false);
            _anim.SetBool("TurnRight",false);
        }

    }
}
