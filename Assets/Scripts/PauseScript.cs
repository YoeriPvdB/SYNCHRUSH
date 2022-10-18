using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PauseScript : MonoBehaviour
{
    public GameObject PauseMenu;

    AudioScript _audioScript;

    [SerializeField] GameObject[] Buttons;

    bool[] buttonMoveStatus = { false, false, false };

    [SerializeField] Transform[] ogPos;

    [SerializeField] Vector2 ogGroupPos;

    //InputActionReference pauseAction;
    PlayerMovement _moveScript;
    Player2Script _p2Script;
    public bool isPaused;

    private void Start()
    {
        PauseMenu = GameObject.Find("Pause Menu");
        ogGroupPos = PauseMenu.transform.localPosition;
        
        _moveScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        _audioScript = GameObject.Find("SoundController").GetComponent<AudioScript>();

        if(SceneManager.GetActiveScene().name == "VersusModeScene" || SceneManager.GetActiveScene().name == "DiscoModeScene")
        {
            _p2Script = GameObject.FindGameObjectWithTag("Player 2").GetComponent<Player2Script>();
        }

        

        PauseMenu.SetActive(false);
    }

   
    public void PauseGame()
    {
        _moveScript.speed = 1;
        
        if(_p2Script != null)
        {
            _p2Script.p2Speed = 1;
        }
        isPaused = true;
        MoveGroup(PauseMenu);

        if (SceneManager.GetActiveScene().name == "VersusModeScene" || SceneManager.GetActiveScene().name == "DiscoModeScene")
        {
           GameObject.Find("Follow Object").GetComponent<VSCamScript>().canMove = false;
        }
    }

    public void UnPause()
    {
        PauseMenu.transform.localPosition = ogGroupPos;

        _moveScript.speed = _moveScript.normalSpeed;

        if (_p2Script != null)
        {
            _p2Script.p2Speed = 1300f;
        }

        //PauseMenu.transform.DOMoveX(PauseMenu.transform.position.x - 10f, 0.5f);

        
        PauseMenu.SetActive(false);

        isPaused = false;

        if (SceneManager.GetActiveScene().name == "VersusModeScene" || SceneManager.GetActiveScene().name == "DiscoModeScene")
        {
            GameObject.Find("Follow Object").GetComponent<VSCamScript>().canMove = true;
        }

    }


    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu Scene");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /*public void ButtonSelect(int currentButton)
    {
        // ogPos = Buttons[currentButton].transform.position;
       // Buttons[currentButton].transform.position = new Vector2(Buttons[currentButton].transform.position.x - 1f, Buttons[currentButton].transform.position.y);
        buttonMoveStatus[currentButton] = true;
        Buttons[currentButton].transform.DOMoveX(ogPos[currentButton].position.x + 1f, 0.3f);

    }

    public void ButtonOff(int currentButton)
    {
        
        if (buttonMoveStatus[currentButton] == true)
        {
            Buttons[currentButton].transform.DOMoveX(ogPos[currentButton].position.x - 1f, 0.3f);
            buttonMoveStatus[currentButton] = false;
        }

    }*/

    public void MoveGroup(GameObject group)
    {
        //PauseMenu.transform.position = ogGroupPos.position;
        //group.transform.position = new Vector2(ogPos[0].position.x - 10f, group.transform.position.y);
        PauseMenu.SetActive(true);
        PauseMenu.transform.DOLocalMoveX(ogGroupPos.x + 50f, 0.3f);

        EventSystem.current.SetSelectedGameObject(PauseMenu.transform.GetChild(0).gameObject);
    }


}
