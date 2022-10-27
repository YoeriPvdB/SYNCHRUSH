using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMODUnity;


public class OnlinePlayerSwitch : MonoBehaviour
{
    public PlayerChoice playerChoice;

    bool canJump, canSlow;
    public bool inTandem, isSplit;
    string[] skinNames = { "Normal", "Wizard", "Rock", "Unicorn"};

    public GameObject[] skins;
    //public float switchTime;

    OnlineMovementScript moveScript;

    OnlineTandemHandler tandemScript;

    int joystickCount, modeSelect = 0, modeCount = 0, playerNum;

    public int currentSkin;

    public string p1JumpButton, p2JumpButton;

    public UIScript uiScript;

    GameObject[] turnSwitches;

    RythmScript _rhythmScript;

    SpriteRenderer redSprite, blueSprite;

    Color red, blue, pink, purple;

    public int checkPointCount = 1, nextCount = 0, playerCheck;

    public float counter = 0, previousTime;

    string currentSkinName;

    public List<float> Times = new List<float>();

    //public float[] Times;

    public float[] reqCount = {13f, 19f, 13f, 10f};

    public AudioScript _audioScript;

    [SerializeField] Animation[] jumpAnims;

    [SerializeField] Slider wallSlider;


    private void Start()
    {
        currentSkinName = PlayerPrefs.GetString("skin");
       

        if (PlayerPrefs.HasKey("skin"))
        {
            print("has");
            for(int i =0; i < skins.Length; i ++)
            {
                if(skinNames[i] == currentSkinName)
                {
                    skins[i].SetActive(true);

                    currentSkin = i;
                }
            }
        } 

        moveScript = gameObject.GetComponent<OnlineMovementScript>();
       

        redSprite = GameObject.Find("Red Player Sprite").GetComponent<SpriteRenderer>();
        blueSprite = GameObject.Find("Blue Player Sprite").GetComponent<SpriteRenderer>();

        turnSwitches = GameObject.FindGameObjectsWithTag("TurnSwitch");

        if(SceneManager.GetActiveScene().name == "VersusModeScene" || SceneManager.GetActiveScene().name == "DiscoModeScene")
        {
            wallSlider.gameObject.SetActive(false);
        }

        canJump = true;

        playerChoice = PlayerChoice.Player1;

        pink = new Color(245, 175, 234, 150);
        red = new Color(231, 38, 38, 150);
        blue = new Color(28, 264, 233, 150);

        tandemScript = gameObject.GetComponent<OnlineTandemHandler>();

        inTandem = true;

    }

    private void Update()
    {
        switch (playerChoice)
        {
            case PlayerChoice.Player1:

                blueSprite.enabled = true;
                redSprite.enabled = false;
                
                foreach (GameObject turnSwitch in turnSwitches)
                {
                    turnSwitch.GetComponent<SpriteRenderer>().color = Color.red;
                }

                playerNum = 1;

                
                
                break;

            case PlayerChoice.Player2:

                blueSprite.enabled = false;
                redSprite.enabled = true;
                
                foreach (GameObject turnSwitch in turnSwitches)
                {
                    turnSwitch.GetComponent<SpriteRenderer>().color = Color.blue;
                }

                playerNum = 0;

                break;
        }

        if(playerCheck == 1)
        {
            StartCoroutine("TwoPress");
        }

        counter = uiScript.leveltime - previousTime;

        if(isSplit)
        {
            inTandem = false;
            GetComponent<BlastScript>().canBlast = false;

            //Debug.Log("banana");
        } 
    }

    public void StartBlast()
    {
        if (GetComponent<BlastScript>().canBlast && inTandem == false)
        {
           _audioScript.BlastAudio();
            GetComponent<BlastScript>().StartCoroutine("Blast");
           
        }
    }

    public void StartJump()
    {
        if(canJump)
        {
            moveScript.isJumping = true;

            _audioScript.JumpAudio();

            

            if (inTandem)
            {
                StartCoroutine("TandemSwitch");
            }
        }
    }

    public IEnumerator TandemSwitch()
    {
        canJump = false;
        float switchTime = 0.01f;

        yield return new WaitForSecondsRealtime(switchTime);
        
        canJump = true;

        SwitchPlayer();
    }

    public void SwitchPlayer()
    {
        

        if (playerChoice == PlayerChoice.Player1)
        {
            playerChoice = PlayerChoice.Player2;

        }
        else
        {

            playerChoice = PlayerChoice.Player1;
        }

        

    }

    public enum PlayerChoice
    {
        Player1,
        Player2
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Top")
        {
            jumpAnims[playerNum].Play();
        }
    }

    public IEnumerator WallTimer(GameObject wall)
    {
        wallSlider.gameObject.SetActive(true);

        // float dist = Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(wall.transform.position.x, 0));

        float time = 100f / GetComponent<Rigidbody2D>().velocity.x;

        wallSlider.maxValue = time;
        wallSlider.value = time;



        while (wallSlider.value > 0)
        {
            wallSlider.value -= Time.deltaTime;

            yield return null;
        }

        if (wallSlider.value <= 0)
        {
            wallSlider.gameObject.SetActive(false);
        }
    }

}
