using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class VersusCheckScript : MonoBehaviour
{

    [SerializeField] InputActionReference[] playerActions;

    public T1Status t1Status;
    public T2Status t2Status;

    int playerCheck = 0;

    bool t1Pressed, t2Pressed;

    public GameObject wall;

    public float t1Dist, t2Dist;

    GameObject Player, Player2;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Player2 = GameObject.FindGameObjectWithTag("Player 2");

        foreach(InputActionReference action in playerActions)
        {
            action.action.Enable();
        }
    }

    public enum T1Status
    {
        Normal,
        Check, 
        Blast
    }

    public enum T2Status
    {
        Normal,
        Check,
        Blast
    }

    private void Update()
    {
        switch(t1Status)
        {
            case T1Status.Normal:

                break;

            case T1Status.Check:

                

                if(Player.GetComponent<PlayerSwitch>().playerChoice == PlayerSwitch.PlayerChoice.Player1)
                {
                    if(playerActions[1].action.triggered)
                    {
                        Prep(true, 0);
                        Player.GetComponent<T1DiscoObstScript>().StopCoroutine("Countdown");
                        Player.GetComponent<T1DiscoObstScript>().slider.gameObject.SetActive(false);
                        t1Status = T1Status.Blast;
                    }
                } else
                {
                    if(playerActions[0].action.triggered)
                    {
                        Player.GetComponent<T1DiscoObstScript>().StopCoroutine("Countdown");
                        Player.GetComponent<T1DiscoObstScript>().slider.gameObject.SetActive(false);
                        Prep(true, 0);
                        t1Status = T1Status.Blast;
                    }
                }



                break;

            case T1Status.Blast:

                if (Player.GetComponent<BlastScript>().shockWave.activeSelf == false)
                {
                    t1Status = T1Status.Normal;
                }

                if(wall != null)
                {
                    t1Dist = Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(wall.transform.position.x, 0));

                    if (t1Dist <= 10f)
                    {
                        Player.GetComponent<PlayerMovement>().isJumping = false;
                        if (t1Pressed == false)
                        {
                            if (playerActions[0].action.triggered)
                            {
                                
                                StartCoroutine("T1TwoPress", 1);
                                t1Pressed = true;
                            }

                            if (playerActions[1].action.triggered)
                            {
                                //t1Status = T1Status.Normal;
                                StartCoroutine("T1TwoPress", 0);
                                t1Pressed = true;
                            }

                        }
                    }

                }

                break;
        }

        switch(t2Status)
        {
            case T2Status.Normal:

                break;

            case T2Status.Check:

                if (Player2.GetComponent<Player2Script>().p2Choice == Player2Script.P2Choice.P1)
                {
                    if (playerActions[3].action.triggered)
                    {
                        Prep(true, 1);
                        Player2.GetComponent<T2DiscoObstacleScript>().StopCoroutine("Countdown");
                        Player2.GetComponent<T2DiscoObstacleScript>().slider.gameObject.SetActive(false);
                        t2Status = T2Status.Blast;
                    }
                }
                else
                {
                    if (playerActions[2].action.triggered)
                    {
                        Prep(true, 1);
                        Player2.GetComponent<T2DiscoObstacleScript>().StopCoroutine("Countdown");
                        Player2.GetComponent<T2DiscoObstacleScript>().slider.gameObject.SetActive(false);
                        t2Status = T2Status.Blast;
                    }
                }

                break;

            case T2Status.Blast:

                if(Player2.GetComponent<BlastScript>().shockWave.activeSelf == false)
                {
                    print("normie");
                    t2Status = T2Status.Normal;
                }

                if(wall != null)
                {
                    t2Dist = Vector2.Distance(new Vector2(Player2.transform.position.x, 0), new Vector2(wall.transform.position.x, 0));

                    if (t2Dist <= 10f)
                    {
                        Player2.GetComponent<Player2Script>().p2Jump = false;

                        if (t2Pressed == false)
                        {
                            if (playerActions[2].action.triggered)
                            {
                                StartCoroutine("T2TwoPress", 3);
                                t2Pressed = true;
                                //t2Status = T2Status.Normal;
                            }

                            if (playerActions[3].action.triggered)
                            {
                                StartCoroutine("T2TwoPress", 2);
                                t2Pressed = true;
                                //t2Status = T2Status.Normal;
                            }
                        }
                    }
                }
                

                

                break;
        }

       
    }

    void Prep(bool status, int team)
    {
        GetComponent<DiscoModeScript>().BlastPrep(team);
        GetComponent<DiscoModeScript>().EnableShockwave(status, team);
    }

    public IEnumerator T1TwoPress(int playerNum)
    {
        bool pressed = false;
        float timer = 5f;
        int team = 0;

        while (timer >= 0)
        {
            timer -= Time.fixedDeltaTime;


            if (playerActions[playerNum].action.triggered)
            {

                BlastStart(0);
                t1Pressed = false;

                timer = -0.05f;
            }



            yield return null;
        }

        

        if (timer <= 0)
        {
            t1Pressed = false;
        }
    }

    IEnumerator T2TwoPress(int playerNum)
    {
        bool pressed = false;
        float timer = 5f;
        int team = 1;

        while (timer >= 0)
        {
            timer -= Time.fixedDeltaTime;

            if (playerActions[playerNum].action.triggered)
            {
                
                BlastStart(1);
                t2Pressed = false;
                timer = -0.05f;

            }
            
            yield return null;
        }

        if (timer <= 0f)
        {
            t2Pressed = false;
        }
    }

    void BlastStart(int team)
    {
        if(SceneManager.GetActiveScene().name == "VersusModeScene")
        {
            GetComponent<VersusManagerScript>().StartShockwave(team);
        } else
        {
            GetComponent<DiscoModeScript>().StartShockwave(team);
        }
    }

    
}
