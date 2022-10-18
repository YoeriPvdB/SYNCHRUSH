using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player2Script : MonoBehaviour
{
    GameObject Player, currentLvlGo, camFollow;

    [SerializeField] GameObject blueSprite, redSprite;

    PlayerSwitch _switchScript;
    PlayerMovement _moveScript;
    Obstacles _obstacleScript;
    UIScript _uiScript;

    string[] skinNames = { "Normal", "Wizard", "Rock", "Unicorn" };

    public GameObject[] skins;

    string currentSkinName;

    int currentSkin;

    float xSpeed;

    //string currentSkin;

    TrailRenderer trail;

    public AudioScript _audioScript;

    public Rigidbody2D p2RB;

    public P2Positions p2Positions;
    public P2Choice p2Choice;

    public bool p2Jump, blast;
    bool switchMode;

    public float p2Speed, p2JumpForce, p2Xpos;

    float yPos;

    [SerializeField] Slider wallSlider;
    GameObject platformPart;
    //float yVelocity;

    public enum P2Positions
    {
        Top,
        Bot
    }

    public enum P2Choice
    {
        P1, 
        P2
    }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        camFollow = GameObject.Find("Follow Object");

        _switchScript = Player.GetComponent<PlayerSwitch>();
        _moveScript = Player.GetComponent<PlayerMovement>();
        _obstacleScript = Player.GetComponent<Obstacles>();
        _uiScript = Player.GetComponent<UIScript>();

        trail = GetComponent<TrailRenderer>();

        p2RB = GetComponent<Rigidbody2D>();

        platformPart = GameObject.Find("Platform Particles 2");

        if(SceneManager.GetActiveScene().name == "VersusModeScene")
        {
            p2Speed = 1300f;
        } else
        {
            p2Speed = 0f;
        }

        wallSlider.gameObject.SetActive(false);

        p2Choice = P2Choice.P2;

        p2JumpForce = 3500f;

        _audioScript = GameObject.Find("SoundController").GetComponent<AudioScript>();

        p2Xpos = 2f;

        


        if (PlayerPrefs.HasKey("T2Skin"))
        {
            currentSkinName = PlayerPrefs.GetString("T2Skin");
            for (int i = 0; i < skins.Length; i++)
            {
                if (skinNames[i] == currentSkinName)
                {
                    skins[i].SetActive(true);

                    currentSkin = i;
                }
            }
        } else
        {
            currentSkin = 0;
        }
    }


    void SpriteFlip(bool isFlipped)
    {
        redSprite.GetComponent<SpriteRenderer>().flipY = isFlipped;
        blueSprite.GetComponent<SpriteRenderer>().flipY = isFlipped;
    }

    private void Update()
    {
        platformPart.transform.position = new Vector2(transform.position.x, platformPart.transform.position.y);

        switch (p2Choice)
        {
            case P2Choice.P1:

                blueSprite.SetActive(true);
                redSprite.SetActive(false);

                break;

            case P2Choice.P2:

                blueSprite.SetActive(false);
                redSprite.SetActive(true);

                break;
        } 
        
        switch (p2Positions)
        {
            case P2Positions.Bot:

                SpriteFlip(false);
                skins[currentSkin].transform.localScale = new Vector2(skins[currentSkin].transform.localScale.x, 1);

                break;

            case P2Positions.Top:

                if (p2Jump == false)
                {
                    //transform.position = new Vector2(transform.position.x, GameObject.Find("Top").transform.position.y - 0.7f);
                   p2RB.velocity = new Vector2(p2RB.velocity.x, GameObject.Find("Top").transform.position.y - 0.7f);

                }
                skins[currentSkin].transform.localScale = new Vector2(skins[currentSkin].transform.localScale.x, -1);
                SpriteFlip(true);

                break;
        }

        if(switchMode)
        {
            p2RB.position = new Vector2(p2RB.position.x, yPos);
        }

       

        RenderTrail();
    }

    private void FixedUpdate()
    {
        if(SceneManager.GetActiveScene().name == "VersusModeScene")
        {
            p2RB.velocity = new Vector2(p2Speed * Time.deltaTime, p2RB.velocity.y);
        }

        if (SceneManager.GetActiveScene().name == "DiscoModeScene")
        {
            p2RB.position = new Vector2(_moveScript.playerRB.position.x - p2Xpos, p2RB.position.y);
        }

        if (p2Jump)
        {
            p2RB.constraints = RigidbodyConstraints2D.FreezeRotation;
            Jump();
            
        }

        
    }

    void RenderTrail()
    {
        if (p2Jump == false)
        {
            trail.time = 0.05f;
        }
        else
        {
            trail.time = 0.2f;
        }

        if (p2Choice == P2Choice.P1)
        {
            trail.startColor = Color.green;
        }
        else
        {
            trail.startColor = Color.yellow;
        }

    }

    void ChunkSwitch()
    {
        foreach (GameObject lvlGo in Player.GetComponent<LevelBuilder>().chonks)
        {
            float xDist = Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(lvlGo.transform.position.x, 0));

            if (xDist <= 5f)
            {
                currentLvlGo = lvlGo;
                //isSwitching = true;

                if (p2Jump)
                {
                    if (p2Positions == P2Positions.Top)
                    {
                        yPos = currentLvlGo.transform.Find("Ground").transform.position.y + 0.7f;
                    }
                    else
                    {
                        yPos = currentLvlGo.transform.Find("Top").transform.position.y - 0.7f;
                    }
                }
                else
                {
                    if (p2Positions == P2Positions.Top)
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

    public void BlastCheck()
    {
        if(_switchScript.inTandem == false)
        {
            Blast();
        } else
        {
            blast = false;
            return;
        }
    }

    void Blast()
    {
        if (GetComponent<BlastScript>().canBlast && _switchScript.inTandem == false)
        {
            _audioScript.BlastAudio();
            gameObject.GetComponent<BlastScript>().StartCoroutine("Blast");
        }
    }

    public void Jump()
    {
        

        if(SceneManager.GetActiveScene().name == "VersusModeScene")
        {
            CheckXPos();
        }
        

        if (p2Positions == P2Positions.Bot)
        {
            p2RB.velocity = new Vector2(p2RB.velocity.x, (p2JumpForce) * Time.deltaTime);
        }
        else
        {
            p2RB.velocity = new Vector2(p2RB.velocity.x, (-p2JumpForce) * Time.deltaTime);
        }

    }

    void CheckXPos()
    {
        if(GameObject.Find("Level Manager").GetComponent<VersusManagerScript>().behindTeam == 0)
        {
            p2RB.position = new Vector2(_moveScript.playerRB.position.x + 2, p2RB.position.y);
        } else
        {
            p2RB.position = new Vector2(_moveScript.playerRB.position.x - 2, p2RB.position.y);
        }
    }

    public void Swap()
    {
        if (p2Choice == P2Choice.P1)
        {
            p2Choice = P2Choice.P2;
        }
        else
        {
            p2Choice = P2Choice.P1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "ModeSwitch")
        {
           
            ChunkSwitch();
            switchMode = true;
        }

        if(collision.gameObject.transform.parent != null && collision.gameObject.transform.parent.tag == "Obstacle")
        {
           // _audioScript.SmallCrashAudio();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "ModeSwitch")
        {
            
            switchMode = false;
            
            if(p2Positions == P2Positions.Top)
            {
                ShootUp();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Ground")
        {
            platformPart.transform.position = new Vector2(platformPart.transform.position.x, collision.gameObject.transform.position.y);
            var main = platformPart.GetComponent<ParticleSystem>().main;
            main.startColor = collision.gameObject.GetComponent<SpriteRenderer>().color;
            platformPart.GetComponent<ParticleSystem>().Play();
            p2Jump = false;
            p2Positions = P2Positions.Bot;
            p2Choice = P2Choice.P1;
        }

        if (collision.gameObject.tag == "Top")
        {
            platformPart.transform.position = new Vector2(platformPart.transform.position.x, collision.gameObject.transform.position.y);
            var main = platformPart.GetComponent<ParticleSystem>().main;
            main.startColor = collision.gameObject.GetComponent<SpriteRenderer>().color;
            platformPart.GetComponent<ParticleSystem>().Play();
            p2Jump = false;
            p2Positions = P2Positions.Top;
            //yVelocity = new Vector2(0, p2RB.velocity.y);
            SetYPos();
            p2Choice = P2Choice.P2;

        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Top")
        {
            if (switchMode == false && p2Jump == false)
            {
                p2RB.constraints = RigidbodyConstraints2D.FreezeRotation;
                ShootUp();
            } else
            {
                return;
            }
        }
    }

    void ShootUp()
    {
        p2RB.constraints = RigidbodyConstraints2D.FreezeRotation;
        float yVelocity = 200f * Time.deltaTime;
        p2RB.velocity = new Vector2(p2RB.velocity.x, yVelocity);
    }
    void SetYPos()
    {
        p2RB.constraints = RigidbodyConstraints2D.FreezePositionY;
        //p2RB.constraints = RigidbodyConstraints2D.FreezeRotation + RigidbodyConstraints2D.FreezePositionY;
    }

    public IEnumerator WallTimer(GameObject wall)
    {
        

        wallSlider.gameObject.SetActive(true);

        // float dist = Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(wall.transform.position.x, 0));

        float time = 100f / Player.GetComponent<PlayerMovement>().playerRB.velocity.x;

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
