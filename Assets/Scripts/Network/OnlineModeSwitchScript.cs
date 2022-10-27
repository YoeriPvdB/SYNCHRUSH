using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class OnlineModeSwitchScript : MonoBehaviour
{
    public ModeStates modeStates;


    public enum ModeStates
    {
        Normal,
        Zoomed,
        Flip,
        Split
    }

    ShakeyCam _camScript;
    OnlineMovementScript _moveScript;
    Player2Script _p2Script;
    OnlinePlayerSwitch _switchScript;
    LevelBuilder _buildScript;

    public int currentFlipCount, reqFlipCount, currentLevelChonk;

    public bool isSwitching;

    float yPos, switchTime = 0.1f;

    GameObject Player1, Player2, currentLvlGo;

    List<GameObject> chunks = new List<GameObject>(); 

    private void Start()
    {
        //Player2 = GameObject.Find("Player 2");

        //Player2.SetActive(false);

        modeStates = ModeStates.Normal;
        _camScript = gameObject.GetComponent<ShakeyCam>();
        _moveScript = gameObject.GetComponent<OnlineMovementScript>();
        _switchScript = GameObject.FindGameObjectWithTag("Player").GetComponent<OnlinePlayerSwitch>();
        _buildScript = gameObject.GetComponent<LevelBuilder>();
        chunks.AddRange(GameObject.FindGameObjectsWithTag("Chonk"));
        //_p2Script = GameObject.FindGameObjectWithTag("Player 2").GetComponent<Player2Script>();

        NewReq();
        //CheckMode();

        StartCoroutine("waitToCheck");

        transform.position = new Vector2(transform.position.x, _buildScript.chonks[currentLevelChonk].transform.Find("Ground").position.y + 0.7f);
    }

    IEnumerator waitToCheck()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        CheckMode();
    }

    private void Update()
    {
        switch (modeStates)
        {
            case ModeStates.Normal:

                break;

            case ModeStates.Zoomed:

                break;

            case ModeStates.Flip:

                
                break;

            case ModeStates.Split:

                break;
        }

        if(isSwitching)
        {
            NewPos();
        }

        if (_camScript.vCam.m_Lens.OrthographicSize >= 10f && _camScript.vCam.m_Lens.Dutch >= 0)
        {
            _camScript.vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.39f;
        } else
        {
            if(SceneManager.GetActiveScene().name == "VersusModeScene" || SceneManager.GetActiveScene().name == "DiscoModeScene")
            {
                _camScript.vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.25f;
            } else
            {
                _camScript.vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.20f;
            }
           
        }

    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject != null)
        {
            if (collision.gameObject.name == "ModeSwitch")
            {
                currentLevelChonk++;
                FlipIt();

                Position();

                

                if(currentLevelChonk >= 2 && SceneManager.GetActiveScene().name == "EndlessModeScene")
                {
                    _buildScript.Infinte();
                    currentLevelChonk = 1;
                }

                if (currentLevelChonk < chunks.Count)
                {
                    //CheckMode();
                    //_buildScript.BackgroundSet(currentLevelChonk);
                }
            }

            if (collision.gameObject.tag == "ZoomModeTrigger" && _camScript.vCam.m_Lens.OrthographicSize >= 10f)
            {

                //StartCoroutine(NewPos(1f));
                _camScript.isZooming = true;
                //_camScript.vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.25f;
                modeStates = ModeStates.Zoomed;
            }

            if(collision.gameObject.name == "ModeSwitch" && collision.gameObject.tag != "ZoomModeTrigger" && _camScript.vCam.m_Lens.OrthographicSize < 10f)
            {
                _camScript.isZooming = true;
               // _camScript.vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.39f;
                //StartCoroutine(NewPos(3.5f));
            }

            if(collision.gameObject.tag == "SplitModeTrigger")
            {
                _switchScript.isSplit = true;
                modeStates = ModeStates.Split;
                SplitIt();
            }

            if(collision.gameObject.tag == "NormalModeTrigger")
            {
                //StartCoroutine(NewPos(3.5f));

                if (modeStates == ModeStates.Split)
                {
                    Normie();
                    
                } else
                {
                    modeStates = ModeStates.Normal;
                }
            }

        }
    }

    void Position()
    {
        foreach(GameObject lvlGo in chunks)
        {
            float xDist = Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(lvlGo.transform.position.x, 0));

            if(xDist <= 5f)
            {
                currentLvlGo = lvlGo;
                isSwitching = true;
                
                if(_moveScript.isJumping)
                {
                    if (_moveScript.playerPositions == OnlineMovementScript.PlayerPositions.Top)
                    {
                        yPos = currentLvlGo.transform.Find("Ground").transform.position.y + 0.7f;
                    }
                    else
                    {
                        yPos = currentLvlGo.transform.Find("Top").transform.position.y - 0.7f;
                    }
                } else
                {
                    if (_moveScript.playerPositions == OnlineMovementScript.PlayerPositions.Top)
                    {
                        yPos = currentLvlGo.transform.Find("Top").transform.position.y - 0.7f;

                    }
                    else
                    {
                        yPos = currentLvlGo.transform.Find("Ground").transform.position.y + 0.7f;
                    }
                }
                
            }
        }

        
    }

    void NewPos()
    {
        //_moveScript.playerRB.velocity = new Vector2(_moveScript.playerRB.velocity.x, 0);
      // _moveScript.isJumping = false;

        switchTime -= Time.deltaTime;

        _moveScript.playerRB.position = new Vector2(_moveScript.playerRB.position.x, yPos);


        /*if(SceneManager.GetActiveScene().name == "VersusModeScene")
        {
            _p2Script.p2RB.position = new Vector2(_p2Script.p2RB.position.x, yPos);
        }*/
        

        if(switchTime < 0)
        {
            isSwitching = false;
            switchTime = 0.1f;
        }

    }

    void FlipIt()
    {

        currentFlipCount++;

        if (currentFlipCount == reqFlipCount)
        {
            modeStates = ModeStates.Flip;
            //_camScript.vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.25f;
            _camScript.isTurning = true;
        }

        if (currentFlipCount > reqFlipCount)
        {
            _camScript.isTurning = true;

            
            
            currentFlipCount = 0;
            modeStates = ModeStates.Normal;

            //gameObject.transform.Find("Red Player Sprite").GetComponent<SpriteRenderer>().flipY = false;
            //gameObject.transform.Find("Blue Player Sprite").GetComponent<SpriteRenderer>().flipY = false;
             
            NewReq();
        }

        
    }

    void NewReq()
    {
        reqFlipCount = Random.Range(1, 3);

        return;
    }

    void SplitIt()
    {
        Player2.SetActive(true);

        Player2.transform.position = new Vector2(this.transform.position.x, 10.65f);

        gameObject.transform.position = new Vector2(this.transform.position.x, -1.3f);

        _switchScript.playerChoice = OnlinePlayerSwitch.PlayerChoice.Player1;
    }

    void Normie()
    {
        
        Player2.SetActive(false);
        _switchScript.isSplit = false;

        modeStates = ModeStates.Normal;

        _switchScript.inTandem = true;
        GetComponent<BlastScript>().canBlast = true;

    }

    public void CheckMode()
    {

        if(_buildScript.chonks[currentLevelChonk].transform.Find("Walls") != null)
        {
            _switchScript.inTandem = false;
            
            _switchScript.SwitchPlayer();
            GetComponent<BlastScript>().BlastStatus();

            foreach (GameObject wall in GameObject.FindGameObjectsWithTag("BlastObstacle"))
            {
                if(_switchScript.playerChoice == OnlinePlayerSwitch.PlayerChoice.Player1)
                {
                    wall.GetComponent<SpriteRenderer>().color = new Color(1f, 0, 0f, 1f);
                } else
                {
                    wall.GetComponent<SpriteRenderer>().color = new Color(0f, 0, 1f, 1f);
                }
            }

        } else
        {
            _switchScript.inTandem = true;
            GetComponent<BlastScript>().BlastStatus();
        }

        print("in Tandem: " + _switchScript.inTandem);

        print("can Blast: " + GetComponent<BlastScript>().canBlast);
        //GetComponent<BlastScript>().BlastStatus();
    }

}
