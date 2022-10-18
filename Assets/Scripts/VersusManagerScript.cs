using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class VersusManagerScript : MonoBehaviour
{
    [SerializeField] InputActionReference[] teamActions;

    int actionCheck1, actionCheck2;

    public int behindPoints, behindTeam;

    [SerializeField] GameObject[] StartIcons;
    public GameObject[] Players;

    public Text pointsText;

    GameObject wall1, wall2;

    public bool ready, canJump;

    public Slider[] synchMeters;

    Transform firstTop, firstBot;

    [SerializeField] Animation wallAnim1, wallAnim2;

    public GameObject wallGo;

    string[] wallTags = { "T2", "T1" };

    
    void Start()
    {
        behindPoints = 3;

        wall1 = GameObject.FindGameObjectWithTag("T1");
        wall2 = GameObject.FindGameObjectWithTag("T2");

        Players[1].GetComponent<Player2Script>().p2Positions = Player2Script.P2Positions.Top;

        //Players[0].GetComponent<PlayerMovement>().ResetValues();
        


        for (int i = 0; i < StartIcons.Length; i++)
        {
            StartIcons[i].SetActive(false);
        }

        
        StartCoroutine("VersusStart");
        actionCheck1 = Random.Range(0, 2);
        actionCheck2 = Random.Range(2, 4);

        print("1: " + actionCheck1 + "2: " + actionCheck2);

        teamActions[1].action.Enable();
        teamActions[0].action.Enable();
        teamActions[2].action.Enable();
        teamActions[3].action.Enable();
        //teamActions[4].action.Enable();

        //Players[1].transform.position = new Vector2(Players[1].transform.position.x, GameObject.FindGameObjectWithTag("Top").transform.position.y - 0.7f);
        Players[1].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;

        for(int i = 0; i < synchMeters.Length; i++)
        {
            synchMeters[i].transform.position = new Vector2(Players[i].transform.position.x + 0.75f, Players[i].transform.position.y);
            synchMeters[i].gameObject.SetActive(false);
        }

        firstTop = Players[0].GetComponent<LevelBuilder>().chonks[0].transform.Find("Top").transform;
        firstBot = Players[0].GetComponent<LevelBuilder>().chonks[0].transform.Find("Ground").transform;

        GameObject.FindGameObjectWithTag("Player 2").GetComponent<Player2Script>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().speed = 0;

        Players[0].transform.position = new Vector2(Players[0].transform.position.x, firstBot.position.y + 0.6f);
        Players[1].transform.position = new Vector2(Players[1].transform.position.x, firstTop.position.y - 0.7f);

        

        canJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(ready)
        {
            if (teamActions[actionCheck2].action.triggered)
            {
                StartCoroutine("CheckWinner", 1);
                behindTeam = 0;
            }

            if (teamActions[actionCheck1].action.triggered)
            {
                StartCoroutine("CheckWinner", 0);
                behindTeam = 1;
            }
        }

        if(canJump == false)
        {
            GameObject.FindGameObjectWithTag("Player 2").GetComponent<Player2Script>().p2Jump = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().isJumping = false;
        }

               
       // pointsText.text = "CATCH UP: " + behindPoints;

    }

    IEnumerator CheckWinner(int team)
    {
        ready = false;

        StartIcons[actionCheck1].SetActive(false);
        StartIcons[actionCheck2].SetActive(false);

        StartCoroutine("MoveWinner", team);

        GameObject.FindGameObjectWithTag("Player").GetComponent<P1ObstaclesScript>().playerTurn = team + 1;

        
        //GameObject.FindGameObjectWithTag("Player 2").GetComponent<Player2Script>().p2Speed = 1300f;

        yield return new WaitForSecondsRealtime(1f);

       
        Players[1].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        GameObject.FindGameObjectWithTag("Player 2").GetComponent<Player2Script>().enabled = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = true;
        //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSwitch>().enabled = true;

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().speed = 1300f;
        //GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().AddForce(Vector2.down * 500 * Time.deltaTime);


        // StartCoroutine("AddPoints");
        synchMeters[behindTeam].gameObject.SetActive(true);
        StartCoroutine("MeterTimer");

        GameObject.Find("SoundController").GetComponent<AudioScript>().StartCoroutine("PlayMusic"); 

        GameObject.Find("Follow Object").GetComponent<VSCamScript>().canMove = true;
        canJump = true;
    }

    IEnumerator MoveWinner(int team)
    {
        for(float i = 2; i < 5; i+= 0.1f)
        {
            Players[team].transform.position = new Vector2(i, Players[team].transform.position.y);
            yield return null;
        }

    }

    IEnumerator MeterTimer()
    {
        yield return new WaitForSecondsRealtime(1f);

        

        if(synchMeters[behindTeam].value < 5)
        {
            synchMeters[behindTeam].value++;
            StartCoroutine("MeterTimer");
        } else
        {
            synchMeters[behindTeam].value = 0;
            //CreateWall();
            BlastPrep();
            EnableShockwave(true);
        }
    }

    void  CreateWall()
    {
        Object wall = Resources.Load("Prefabs/Mega Block 1", typeof (GameObject));

        Vector2 wallPos = new Vector2(Players[behindTeam].transform.position.x + 100f, 0);
        wallGo =  (GameObject)Instantiate(wall, wallPos, transform.rotation);

        wallGo.transform.tag = wallTags[behindTeam];

       // BlastPrep(wallGo);

        StartCoroutine("MeterTimer");
    }

    void BlastPrep()
    {
        GameObject wall;

        if(behindTeam == 0)
        {
            GetComponent<VersusCheckScript>().t2Status = VersusCheckScript.T2Status.Blast;
            wall = wall2;
            wall.transform.position = new Vector2(Players[1].transform.position.x + 100f, 0);
            Players[1].GetComponent<Player2Script>().StartCoroutine("WallTimer", wall);

        } else
        {
            GetComponent<VersusCheckScript>().t1Status = VersusCheckScript.T1Status.Blast;
            wall = wall1;
            wall.transform.position = new Vector2(Players[0].transform.position.x + 100f, 0);
            Players[0].GetComponent<PlayerSwitch>().StartCoroutine("WallTimer", wall);
        }

        wall.SetActive(true);

        GetComponent<VersusCheckScript>().wall = wall;
        CheckNearbyObst(wall);

        StartCoroutine("MeterTimer");
    }

    void CheckNearbyObst(GameObject wall)
    {
        Collider2D[] blokCols = Physics2D.OverlapCircleAll(wall.transform.position, 5f);

        foreach (Collider2D blok in blokCols)
        {
            if (blok.gameObject.tag == "Smol Block")
            {
                blok.gameObject.SetActive(false);
            }
        }
    }

    public void EnableShockwave(bool status)
    {
        for (int i = 0; i < Players.Length; i++)
        {
            if (i != behindTeam || status == false)
            {
                Players[i].GetComponent<BlastScript>().shockWave.SetActive(status);
            }

            
        }

        
    }

    public void StartShockwave(int team)
    {
        if (team != behindTeam && Players[team].GetComponent<BlastScript>().shockWave.activeSelf)
        {
            Players[team].GetComponent<BlastScript>().StartCoroutine("Blast");
            GameObject.Find("SoundController").GetComponent<AudioScript>().BlastAudio();
        }
    }

    public void CheckPosNum()
    {
        behindTeam++;

        if(behindTeam > 1)
        {
            behindTeam = 0;
        }

        for(int i = 0; i < synchMeters.Length; i++)
        {
            if(i == behindTeam)
            {
                synchMeters[i].gameObject.SetActive(true);
            } else
            {
                synchMeters[i].gameObject.SetActive(false);
            }
        }
    }

    IEnumerator VersusStart()
    {
       yield return new WaitForSecondsRealtime(3f);

        ready = true;

        StartIcons[actionCheck1].SetActive(true);
        StartIcons[actionCheck2].SetActive(true);
        

    }


}
