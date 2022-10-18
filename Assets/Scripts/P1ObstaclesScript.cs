using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1ObstaclesScript : MonoBehaviour
{
    public int playerTurn, hitCount;

    PlayerMovement _moveScript;
    
    PlayerSwitch _switchScript;

    Player2Script _p2Script;

    VersusManagerScript _versusScript;

    public bool p1Slowing;

    public string winner;

    ParticleSystem particles;
    GameObject platformPart;

    private void Start()
    {
        _moveScript = GetComponent<PlayerMovement>();
        _switchScript = GetComponent<PlayerSwitch>();
        _versusScript = GameObject.Find("Level Manager").GetComponent<VersusManagerScript>();
        _p2Script = GameObject.FindGameObjectWithTag("Player 2").GetComponent<Player2Script>();
        particles = GameObject.Find("Obstacle Particles 1").GetComponent<ParticleSystem>();
        platformPart = GameObject.Find("Platform Particles 1");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Player 2")
        {
            Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider2D>());
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Shock Blast")
        {
            Physics2D.IgnoreCollision(collision, gameObject.GetComponent<Collider2D>());
        }

        if (collision.gameObject.tag == "Bounds")
        {
            platformPart.transform.position = new Vector2(platformPart.transform.position.x, collision.gameObject.transform.position.y);
            var main = platformPart.GetComponent<ParticleSystem>().main;

            if (_switchScript.playerChoice == PlayerSwitch.PlayerChoice.Player1)
            {
                main.startColor = Color.cyan;
            }
            else
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

            if (_switchScript.inTandem)
            {
                _switchScript.StartCoroutine("TandemSwitch");
            }

            hitCount++;

            StartCoroutine("SlowTime", 0.3f);
        }

        if (collision.gameObject.transform.parent != null && collision.gameObject.transform.parent.tag == "Wall")
        {
            GetParticles(collision.gameObject);
            Destroy(collision.gameObject);

            hitCount++;

            StartCoroutine("SlowTime", 0.1f);
        }

        if (collision.gameObject.transform.parent != null && collision.gameObject.transform.parent.tag == "Obstacle")
        {
            GetParticles(collision.gameObject);
            Destroy(collision.gameObject);

            GameObject.Find("SoundController").GetComponent<AudioScript>().SmallCrashAudio();

            hitCount++;

            StartCoroutine("SlowTime", 0.1f);
        }

        if(collision.gameObject.tag == "T1")
        {
            GetParticles(collision.gameObject);
            collision.gameObject.SetActive(false);
            GameObject.Find("SoundController").GetComponent<AudioScript>().SmallCrashAudio();
            GameObject.Find("Level Manager").GetComponent<VersusManagerScript>().EnableShockwave(false);
            StartCoroutine("Slow");
        }

        if(collision.gameObject.tag == "End")
        {
            if(playerTurn == 1)
            {
                winner = "TEAM 1";
            } else
            {
                winner = "TEAM 2";
            }
        }

        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bounds")
        {
             if (_moveScript.playerPositions == PlayerMovement.PlayerPositions.Bot)
                {
                    _moveScript.playerPositions = PlayerMovement.PlayerPositions.Top;
                }
                else
                {
                    _moveScript.playerPositions = PlayerMovement.PlayerPositions.Bot;
                }
            

            _moveScript.trail.enabled = false;
            _moveScript.isJumping = true;

            if (_switchScript.inTandem)
            {
                _switchScript.StartCoroutine("TandemSwitch");
            }

            hitCount++;

            StartCoroutine("SlowTime", 0.3f);
        }
    }

    public IEnumerator SlowTime(float time)
    {

        yield return new WaitForSecondsRealtime(time);

        if (hitCount < 2)
        {
            StartCoroutine("Slow");
            
        }

        hitCount = 0;
    }

    public IEnumerator Slow()
    {
        if(_versusScript.behindTeam == 1)
        {

            _moveScript.speed /= 1.3f;
            _p2Script.p2Speed *= 1.3f;

            yield return new WaitForSecondsRealtime(0.3f);

            _moveScript.playerRB.position = new Vector2(GameObject.Find("Follow Object").transform.position.x - 1, _moveScript.playerRB.position.y);
            _p2Script.p2RB.position = new Vector2(GameObject.Find("Follow Object").transform.position.x + 1, _p2Script.p2RB.position.y);

            _p2Script.p2Speed = 1300f;
            _moveScript.speed = 1300f;


            _versusScript.CheckPosNum();
            _versusScript.synchMeters[1].value = 0;

            if (GetComponent<BlastScript>().shockWave.activeSelf == true)
            {
                GetComponent<BlastScript>().shockWave.SetActive(false);
                _versusScript.wallGo.SetActive(false);
            }

        } else
        {
            _versusScript.synchMeters[0].value--;

            yield return null;
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
