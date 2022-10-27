using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using FMODUnity;

public class OnlineMovementScript : MonoBehaviour
{
    public Rigidbody2D playerRB;

    public float normalSpeed, speed, maxSpeed, maxJumpVelocity, jumpForce, maxJumpDist, slowdownLength, slowdownFactor, p1XPos;

    public bool isGrounded, isFalling = false, hasChecked = false, isJumping = false, isPaused = false, isDashing = false;

    public int jumpCount, lifeCount;

    OnlinePlayerSwitch switchScript;

    public Vector2 currentVelocity, previousVelocity, startYPos;

    public string jumpButton;

    public TrailRenderer trail;

    public PlayerPositions playerPositions;

    public AudioScript _audioScript;

    Player2Script _p2Script;

    bool switchMode;

    private Transform currentTop;

    GameObject platformPart;

    PhotonView myPV;

    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        startYPos = playerRB.position;
        trail = GetComponent<TrailRenderer>();
        switchScript = GetComponent<OnlinePlayerSwitch>();
        myPV = GetComponent<PhotonView>();
        

        playerPositions = PlayerPositions.Bot;

       // speed = 0;

        p1XPos = 0;

        if(SceneManager.GetActiveScene().name == "VersusModeScene" || SceneManager.GetActiveScene().name == "DiscoModeScene")
        {
           _p2Script = GameObject.FindGameObjectWithTag("Player 2").GetComponent<Player2Script>();
            platformPart = GameObject.Find("Platform Particles 1");
           
        } else
        {
            platformPart = GameObject.Find("Platform Particles").gameObject;
        }

        if(SceneManager.GetActiveScene().name == "DiscoModeScene")
        {
            normalSpeed = 1150f;
        } else
        {
            normalSpeed = 1300f;
        }

        if(SceneManager.GetActiveScene().name == "EndlessModeScene")
        {
            StartCoroutine("IncreaseSpeed", 33f);
        }

        if(SceneManager.GetActiveScene().name == "TandemModeScene" || SceneManager.GetActiveScene().buildIndex == 11)
        {
            GameObject.Find("Shock Blast").SetActive(false);
        }

        /*if (PhotonNetwork.IsMasterClient)
        {
            myPV.RPC("Follow", RpcTarget.Others, playerRB.velocity);
        }*/
    }

    

    private void Update()
    {
        RenderTrail();

        platformPart.transform.position = new Vector2(transform.position.x, platformPart.transform.position.y);

        Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;

        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

        if (Time.timeScale >= 0.99f)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }

        

        /*if(Input.GetKeyDown(KeyCode.R))
        {
            //Scene currentScene = SceneManager.GetActiveScene();
            string scene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(scene);
        }*/

        switch(playerPositions)
        {
            case PlayerPositions.Bot:

                if(switchScript.inTandem)
                {
                    switchScript.playerChoice = OnlinePlayerSwitch.PlayerChoice.Player1;
                }

                SpriteFlip(false);

               break;

            case PlayerPositions.Top:

                if (switchScript.inTandem)
                {
                    switchScript.playerChoice = OnlinePlayerSwitch.PlayerChoice.Player2;
                }

                SpriteFlip(true);

                break;
        }

        if(speed >= 2000f)
        {
            playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        
    }

    public enum PlayerPositions
    {
        Top,
        Bot
    }

    private void FixedUpdate()
    {

        //playerRB.velocity = new Vector2((speed * Time.timeScale) * Time.deltaTime, (playerRB.velocity.y * Time.timeScale));

        playerRB.velocity = new Vector2(speed * Time.deltaTime, playerRB.velocity.y);

        
        if (isJumping)
        {
            
            //
            playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            maxJumpDist = 0.4f;
            isGrounded = false;
            Jump();
            
        }

        
    }

    public void Jump()
    {
        
        
        if (SceneManager.GetActiveScene().name == "VersusModeScene")
        {
            CheckXPos();

        }

        if (playerPositions == PlayerPositions.Bot)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, (jumpForce) * Time.deltaTime);
            //myPV.RPC("Follow", RpcTarget.Others, playerRB.velocity);
        } else
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, (-jumpForce) * Time.deltaTime);
           
        }

        jumpCount++;
        isDashing = false;

        
    }

    void CheckXPos()
    {
        if (GameObject.Find("Level Manager").GetComponent<VersusManagerScript>().behindTeam == 1)
            {
                playerRB.position = new Vector2(_p2Script.p2RB.position.x + 2, playerRB.position.y);
            }
            else
            {
                playerRB.position = new Vector2(_p2Script.p2RB.position.x - 2, playerRB.position.y);
            }
    }

    /*public void MovePause()
    {
        if (GameObject.Find("UI Handler").GetComponent<UIScript>().timeText.enabled == true)
        {
            if(isPaused == false)
            {
                speed = 1;
                isPaused = true; 
            } else
            {
                speed = 1300f;
                isPaused = false;
            }
        }
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Ground")
        {
            platformPart.transform.position = new Vector2(platformPart.transform.position.x, collision.gameObject.transform.position.y);
            var main = platformPart.GetComponent<ParticleSystem>().main;
            main.startColor = collision.gameObject.GetComponent<SpriteRenderer>().color;
            platformPart.GetComponent<ParticleSystem>().Play();
            ResetValues();
            playerPositions = PlayerPositions.Bot;
            GetComponent<ShakeyCam>().StartCoroutine("ShakeIt", .2f);
            switchScript.skins[switchScript.currentSkin].transform.localScale = new Vector2(switchScript.skins[switchScript.currentSkin].transform.localScale.x, 1);

            if (speed > 2000f)
            {
                _audioScript.JumpAudio();
                //isJumping = true;
                //Jump();
            }

            if(trail.enabled == false)
            {
                trail.enabled = true;
            }

           // myPV.RPC("Follow", RpcTarget.Others, playerRB.velocity);

        }

        if (collision.gameObject.tag == "Top")
        {
            platformPart.transform.position = new Vector2(platformPart.transform.position.x, collision.gameObject.transform.position.y);
            platformPart.GetComponent<ParticleSystem>().Play();
            playerRB.constraints = RigidbodyConstraints2D.FreezePositionY;
            switchScript.skins[switchScript.currentSkin].transform.localScale = new Vector2(switchScript.skins[switchScript.currentSkin].transform.localScale.x, -1);

            currentTop = collision.gameObject.transform;
            ResetValues();
            //isJumping = false;
            playerPositions = PlayerPositions.Top;
            GetComponent<ShakeyCam>().StartCoroutine("ShakeIt", .2f);

            if (speed >= 2000f)
            {
                _audioScript.JumpAudio();
                //isJumping = true;
                //Jump();
                
            }


            if (trail.enabled == false)
            {
                trail.enabled = true;
            }

            //myPV.RPC("Follow", RpcTarget.Others, playerRB.velocity);

        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Top")
        {
            

            if (isJumping == false && switchMode == false)
            {
                ShootUP();

            }
        }
    }

    void SpriteFlip(bool isFlipped)
    {
        gameObject.transform.Find("Red Player Sprite").GetComponent<SpriteRenderer>().flipY = isFlipped;
        gameObject.transform.Find("Blue Player Sprite").GetComponent<SpriteRenderer>().flipY = isFlipped;
    }

    void ShootUP()
    {
        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;

        float yVelocity = 200f * Time.deltaTime;
        playerRB.velocity = new Vector2(playerRB.velocity.x, yVelocity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "ModeSwitch")
        {
            switchMode = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.name == "ModeSwitch")
        {
            switchMode = false;

            if(playerPositions == PlayerPositions.Top)
            {
                ShootUP();
            }
            

            //playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void RenderTrail()
    {
        if (isGrounded && speed != GetComponent<OnlinePowerUp>().newSpeed)
        {
            trail.time = 0.05f;
        }
        else
        {
            trail.time = 0.2f;
        }

        if(switchScript.playerChoice == OnlinePlayerSwitch.PlayerChoice.Player1)
        {
            trail.startColor = Color.blue;
        } else
        {
            trail.startColor = Color.red;
        }
        
    }

    public void ResetValues()
    {
        
        playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
        isJumping = false;
        isDashing = false;
        jumpCount = 0;
        maxJumpDist = 0.4f;
        maxJumpVelocity = 500f;
        //playerRB.gravityScale = 1f;
        jumpForce = 3500f;
        isGrounded = true;
        isFalling = false;
        //myPV.RPC("Follow", RpcTarget.Others, false);
        //
        //platformPart.GetComponent<SpriteRenderer>().enabled = false;

    }

    public void SlowMo()
    {
        
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

    }

    IEnumerator Fall()
    {
        if(isGrounded == false)
        {
            playerRB.velocity -= new Vector2(0, (80f)) * Time.deltaTime;
        }

        yield return new WaitForSeconds(0.2f);

        if (playerRB.velocity.y > 0.5f && isGrounded == false)
        {
            StartCoroutine("Fall");
        }
    }

    IEnumerator IncreaseSpeed (float time)
    {
        yield return new WaitForSecondsRealtime(time);

        normalSpeed += 50;

        speed = normalSpeed;

        StartCoroutine("IncreaseSpeed", 30f);
    }

    

}
