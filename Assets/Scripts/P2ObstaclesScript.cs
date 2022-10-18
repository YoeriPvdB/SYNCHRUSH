using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2ObstaclesScript : MonoBehaviour
{
    Player2Script _p2Script;
    P1ObstaclesScript _p1Obstacle;
    VersusManagerScript _versusScript;

    ParticleSystem particles;
    GameObject platformPart;
    int playerTurn;

    public bool p2Slowing;

    private void Start()
    {
        _p2Script = GetComponent<Player2Script>();
        _p1Obstacle = GameObject.FindGameObjectWithTag("Player").GetComponent<P1ObstaclesScript>();
        _versusScript = GameObject.Find("Level Manager").GetComponent<VersusManagerScript>();
        particles = GameObject.Find("Obstacle Particles 2").GetComponent<ParticleSystem>();
        platformPart = GameObject.Find("Platform Particles 2");
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
            return;
        }

        if(collision.gameObject.tag == "Bounds")
        {
            GameObject.Find("SoundController").GetComponent<AudioScript>().SmallCrashAudio();
            platformPart.transform.position = new Vector2(platformPart.transform.position.x, collision.gameObject.transform.position.y);
            var main = platformPart.GetComponent<ParticleSystem>().main;

            if (_p2Script.p2Choice == Player2Script.P2Choice.P1)
            {
                main.startColor = Color.green;
            }
            else
            {
                main.startColor = Color.yellow;
            }

            platformPart.GetComponent<ParticleSystem>().Play();

            if (_p2Script.p2Jump)
            {
                if (_p2Script.p2Positions == Player2Script.P2Positions.Bot)
                {
                    _p2Script.p2Positions = Player2Script.P2Positions.Top;
                }
                else
                {
                    _p2Script.p2Positions = Player2Script.P2Positions.Bot;
                }
            }

           // _moveScript.trail.enabled = false;
            _p2Script.p2Jump = true;

            _p2Script.Swap();

            _p1Obstacle.hitCount++;
            StartCoroutine("SlowTime", 0.3f);

        }

        if(collision.gameObject.transform.parent != null && collision.gameObject.transform.parent.tag == "Wall")
        {
            GetParticles(collision.gameObject);
            Destroy(collision.gameObject);

            StartCoroutine("Slow");
        }

        if (collision.gameObject.transform.parent != null && collision.gameObject.transform.parent.tag == "Obstacle")
        {
            GetParticles(collision.gameObject);
            Destroy(collision.gameObject);
            GameObject.Find("SoundController").GetComponent<AudioScript>().SmallCrashAudio();
            _p1Obstacle.hitCount++;
            StartCoroutine("SlowTime", 0.05f);
        }

        if(collision.gameObject.tag == "T2")
        {
            GameObject.Find("SoundController").GetComponent<AudioScript>().SmallCrashAudio();
            GetParticles(collision.gameObject);
            collision.gameObject.SetActive(false);
            GameObject.Find("Level Manager").GetComponent<VersusManagerScript>().EnableShockwave(false);
            StartCoroutine("Slow");
        }

        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bounds")
        {
                       
                if (_p2Script.p2Positions == Player2Script.P2Positions.Bot)
                {
                    _p2Script.p2Positions = Player2Script.P2Positions.Top;
                }
                else
                {
                    _p2Script.p2Positions = Player2Script.P2Positions.Bot;
                }
            

            // _moveScript.trail.enabled = false;
            _p2Script.p2Jump = true;

            _p2Script.Swap();

            _p1Obstacle.hitCount++;
            StartCoroutine("SlowTime", 0.3f);

        }
    }

    IEnumerator SlowTime(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        if(_p1Obstacle.hitCount < 2)
        {
            StartCoroutine("Slow");
        }

        _p1Obstacle.hitCount = 0;
    }

    public IEnumerator Slow()
    {
        if(_versusScript.behindTeam == 0)
        {
            /*_p2Script.p2Speed /= 2;

            yield return new WaitForSecondsRealtime(0.3f);

            _p2Script.p2RB.position = new Vector2(GameObject.FindGameObjectWithTag("Player").transform.position.x - 2, _p2Script.p2RB.position.y);
            _p2Script.p2Speed = 1300f;*/

            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().speed *= 1.3f;
            _p2Script.p2Speed /= 1.3f;

            yield return new WaitForSecondsRealtime(0.3f);

            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().playerRB.position = new Vector2(GameObject.Find("Follow Object").transform.position.x + 1f, 
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().playerRB.position.y);
            _p2Script.p2RB.position = new Vector2(GameObject.Find("Follow Object").transform.position.x - 1f, _p2Script.p2RB.position.y);

            _p2Script.p2Speed = 1300f;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().speed = 1300f;

            _versusScript.CheckPosNum();
            _versusScript.synchMeters[0].value = 0;

            if (GetComponent<BlastScript>().shockWave.activeSelf == true)
            {
                GetComponent<BlastScript>().shockWave.SetActive(false);
                _versusScript.wallGo.SetActive(false);
            }

        } else
        {
            _versusScript.synchMeters[1].value--;

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
