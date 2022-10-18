using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class InputScript : MonoBehaviour
{
    PlayerMovement _moveScript;
    PlayerSwitch _switchScript;
    UIScript _uiScript;
    MenuScript _menuScript;
    Player2Script _p2Script;
    PauseScript _pauseScript;
    AudioScript _audioScript;
    VersusCheckScript _checkScript;
    VersusManagerScript _versusScript;
    public bool p1Input, p2Input;

    InputAction p1Callback;

    private void Awake()
    {
        _moveScript = GetComponent<PlayerMovement>();
        _pauseScript = GameObject.Find("Menu Canvas").GetComponent<PauseScript>();
        _switchScript = GetComponent<PlayerSwitch>();
        _uiScript = GameObject.Find("UI Handler").GetComponent<UIScript>();
        _menuScript = GetComponent<MenuScript>();

        if(SceneManager.GetActiveScene().name == "VersusModeScene" || SceneManager.GetActiveScene().name == "DiscoModeScene")
        {
            _p2Script = GameObject.Find("Player 2").GetComponent<Player2Script>();
            _checkScript = GameObject.Find("Level Manager").GetComponent<VersusCheckScript>();
            _versusScript = GameObject.Find("Level Manager").GetComponent<VersusManagerScript>();
        }
        
        _audioScript = GameObject.Find("SoundController").GetComponent<AudioScript>();
    }



    public void P1Input(InputAction.CallbackContext context)
    {
        //gameObject.scene.IsValid();

        if(context.performed && _pauseScript.isPaused == false)
        {
            if (_switchScript.playerChoice == PlayerSwitch.PlayerChoice.Player1)
            {
                _switchScript.StartJump();
            }
            else
            {
                _switchScript.StartBlast();
                print("we are blasting");
            }
        }
    }

    public void P2Input(InputAction.CallbackContext context)
    {
        //gameObject.scene.IsValid();
        if (context.performed && _pauseScript.isPaused == false)
        {
            if (_switchScript.playerChoice == PlayerSwitch.PlayerChoice.Player2)
            {
                _switchScript.StartJump();
            }
            else
            {
                _switchScript.StartBlast();
                
            }
            
        }
    }

    public void P3Input(InputAction.CallbackContext context)
    {
        if (context.performed && _pauseScript.isPaused == false)
        {
            if (_checkScript.t2Status != VersusCheckScript.T2Status.Blast)
            {
                if (_p2Script.p2Choice == Player2Script.P2Choice.P1)
                {
                    _audioScript.JumpAudio();
                    _p2Script.p2Jump = true;
                    // _p2Script.Swap();
                }
            }
            else
            {
                if (_checkScript.t2Dist > 10f)
                {
                    if (_p2Script.p2Choice == Player2Script.P2Choice.P1)
                    {
                        _audioScript.JumpAudio();
                        _p2Script.p2Jump = true;
                        // _p2Script.Swap();
                    }
                } else
                {
                    return;
                }
            }
        }
    }

    public void P4Input(InputAction.CallbackContext context)
    {
        if (context.performed && _pauseScript.isPaused == false)
        {
            if (_checkScript.t2Status != VersusCheckScript.T2Status.Blast)
            {
                if (_p2Script.p2Choice == Player2Script.P2Choice.P2)
                {
                    _audioScript.JumpAudio();
                    _p2Script.p2Jump = true;
                    // _p2Script.Swap();
                }
            }
            else
            {
                if (_checkScript.t2Dist > 10f)
                {
                    if (_p2Script.p2Choice == Player2Script.P2Choice.P2)
                    {
                        _audioScript.JumpAudio();
                        _p2Script.p2Jump = true;
                        // _p2Script.Swap();
                    }
                } else
                {
                    return;
                }
            }
        }
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
           
            if(_pauseScript.isPaused == false)
            {
                _pauseScript.PauseGame();
            } else
            {
                _pauseScript.UnPause();
            }
            
        }
    }

    public void VSP1Input(InputAction.CallbackContext context)
    {

        if (context.performed && _pauseScript.isPaused == false && _versusScript.canJump == true)
        {
            if (_checkScript.t1Status != VersusCheckScript.T1Status.Blast)
            {
                if (_switchScript.playerChoice == PlayerSwitch.PlayerChoice.Player1)
                {
                    _switchScript.StartJump();
                }
                else
                {
                    _switchScript.StartBlast();
                }
            } else
            {
                if(_checkScript.t1Dist > 10f)
                {
                    if (_switchScript.playerChoice == PlayerSwitch.PlayerChoice.Player1)
                    {
                        _switchScript.StartJump();
                    }
                    else
                    {
                        _switchScript.StartBlast();
                    }
                }
            }
        }
        
    }

    public void VSP2Input(InputAction.CallbackContext context)
    {
        if (context.performed && _pauseScript.isPaused == false && _versusScript.canJump == true)
        {
            if (_checkScript.t1Status != VersusCheckScript.T1Status.Blast)
            {
                if (_switchScript.playerChoice == PlayerSwitch.PlayerChoice.Player2)
                {
                    _switchScript.StartJump();
                }
                else
                {
                    _switchScript.StartBlast();
                }
            }
            else
            {
                if (_checkScript.t1Dist > 10f)
                {
                    if (_switchScript.playerChoice == PlayerSwitch.PlayerChoice.Player2)
                    {
                        _switchScript.StartJump();
                    }
                    else
                    {
                        _switchScript.StartBlast();
                    }
                }
            }
        }
    }

    public void DiscoP1Input(InputAction.CallbackContext context)
    {
        if (context.performed && _pauseScript.isPaused == false)
        {
            if (_checkScript.t1Status != VersusCheckScript.T1Status.Blast)
            {
                if (_switchScript.playerChoice == PlayerSwitch.PlayerChoice.Player1)
                {
                    _switchScript.StartJump();
                }
                else
                {
                    _switchScript.StartBlast();
                }
            }
            else
            {
                if (_checkScript.t1Dist > 10f)
                {
                    if (_switchScript.playerChoice == PlayerSwitch.PlayerChoice.Player1)
                    {
                        _switchScript.StartJump();
                    }
                    else
                    {
                        _switchScript.StartBlast();
                    }
                }
            }
        }
    }

    public void DiscoP2Input(InputAction.CallbackContext context)
    {
        if (context.performed && _pauseScript.isPaused == false)
        {
            if (_checkScript.t1Status != VersusCheckScript.T1Status.Blast)
            {
                if (_switchScript.playerChoice == PlayerSwitch.PlayerChoice.Player2)
                {
                    _switchScript.StartJump();
                }
                else
                {
                    _switchScript.StartBlast();
                }
            }
            else
            {
                if (_checkScript.t1Dist > 10f)
                {
                    if (_switchScript.playerChoice == PlayerSwitch.PlayerChoice.Player2)
                    {
                        _switchScript.StartJump();
                    }
                    else
                    {
                        _switchScript.StartBlast();
                    }
                }
            }
        }
    }

}
