using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacles : MonoBehaviour
{
    Rigidbody2D playerRb;

    UIScript _uiScript;

    PowerUp _powerUp;

    ShakeyCam _shakeScript;

    PlayerMovement _moveScript;
    PlayerSwitch _switchScript;
    Player2Script _p2Script;

    public int addedTime, playerNum;

    public AudioScript _audioScript;

    public ParticleSystem particles;

    GameObject platformPart;

    public int lifeCount = 1;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        _uiScript = GameObject.Find("UI Handler").GetComponent<UIScript>();
        _powerUp = GetComponent<PowerUp>();
        _shakeScript = GetComponent<ShakeyCam>();
        _moveScript = GetComponent<PlayerMovement>();
        _switchScript = GetComponent<PlayerSwitch>();
        //_p2Script = GameObject.Find("Player 2").GetComponent<Player2Script>();
        particles = GameObject.Find("Obstacle Particles").GetComponent<ParticleSystem>();

        platformPart = GameObject.Find("Platform Particles");
        playerNum = 1;
    }

    private void Update()
    {
        if(lifeCount <= 0)
        {
            GetComponent<MenuScript>().GetEndScore();
            GetComponent<MenuScript>().StartCoroutine("End");

            lifeCount = 1;
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Player 2")
        {
            Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider2D>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.transform.parent != null && collision.transform.parent.gameObject.tag == "Obstacle")
        {

            // _audioScript.SmallCrashAudio();
            _audioScript.PlayP1Audio(1);
            

            if (_powerUp.powerUps != PowerUp.PowerUps.Speed)
            {
                if(SceneManager.GetActiveScene().name == "EndlessModeScene")
                {
                    EndlessGameOver();
                } else
                {
                    addedTime = 5;
                    _uiScript.StartCoroutine("AddTimeText", addedTime);
                }
                
            }

            GetParticles(collision.gameObject);
            _shakeScript.StartCoroutine("ShakeIt", .3f);

            Destroy(collision.gameObject);

            //StartCoroutine("Slow");
        }

        if(collision.transform.parent != null && collision.transform.parent.gameObject.tag == "Wall")
        {
            //GetComponent<AudioScript>().StartCoroutine("CrashAudio");

            

            _audioScript.LargeCrashAudio();

            if (_powerUp.powerUps != PowerUp.PowerUps.Speed)
            {
                if (SceneManager.GetActiveScene().name == "EndlessModeScene")
                {
                    EndlessGameOver();
                }
                else
                {
                    addedTime = 10;
                    _uiScript.StartCoroutine("AddTimeText", addedTime);
                }
            }

            GetParticles(collision.gameObject);
            _shakeScript.StartCoroutine("ShakeIt", .3f);

            Destroy(collision.gameObject);

            //StartCoroutine("Slow");
        }

        if(collision.gameObject.tag == "Bounds")
        {
            platformPart.transform.position = new Vector2(platformPart.transform.position.x, collision.gameObject.transform.position.y);
            var main = platformPart.GetComponent<ParticleSystem>().main;

            if(_switchScript.playerChoice == PlayerSwitch.PlayerChoice.Player1)
            {
                main.startColor = Color.cyan;
            } else
            {
                main.startColor = Color.red;
            }
            
            platformPart.GetComponent<ParticleSystem>().Play();

            if (_moveScript.isJumping)
                {
                    if (_moveScript.playerPositions == PlayerMovement.PlayerPositions.Bot)
                    {
                        _moveScript.playerPositions = PlayerMovement.PlayerPositions.Top;
                    }
                    else
                    {
                        _moveScript.playerPositions = PlayerMovement.PlayerPositions.Bot;
                    }
                }

                _moveScript.trail.enabled = false;
                _moveScript.isJumping = true;
                _shakeScript.StartCoroutine("ShakeIt", .4f);

            if (SceneManager.GetActiveScene().name == "EndlessModeScene")
            {
                EndlessGameOver();
            }
            else
            {
                addedTime = 10;
                _uiScript.StartCoroutine("AddTimeText", addedTime);
            }

            if (_switchScript.inTandem)
                {
                    _switchScript.StartCoroutine("TandemSwitch");
                }

                
        }

        
    }

    void EndlessGameOver()
    {
        if (SceneManager.GetActiveScene().name == "EndlessModeScene" || SceneManager.GetActiveScene().buildIndex == 11)
        {
            lifeCount --;
            return;
        }
        else
        {
            return;
        }
    }

    public void GetParticles(GameObject collision)
    {
        var main = particles.main;
        main.startColor = collision.gameObject.GetComponent<SpriteRenderer>().color;
        particles.gameObject.transform.position = collision.gameObject.transform.position;
        particles.Play();
    }


}
