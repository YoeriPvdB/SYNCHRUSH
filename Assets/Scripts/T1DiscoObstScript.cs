using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class T1DiscoObstScript : MonoBehaviour
{
    public int T1Points, t1Bonus;

    public bool canBonus, isbehind;

    PlayerMovement _moveScript;
    PlayerSwitch _switchScript;
    ShakeyCam _camScript;

    [SerializeField] DiscoModeScript _discoScript;
    [SerializeField] Text pointsText;

    public Slider slider, wallSlider;

    public ParticleSystem particles;
    GameObject platformPart;

    private void Start()
    {
        T1Points = 0;
        CheckPoints();
        _moveScript = GetComponent<PlayerMovement>();
        _switchScript = GetComponent<PlayerSwitch>();
        _camScript = GetComponent<ShakeyCam>();
        t1Bonus = 0;
        isbehind = false;
        slider.gameObject.transform.position = new Vector2(transform.position.x + 0.7f, transform.position.y);
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
        if (collision.gameObject.name == "Shock Blast")
        {
            Physics2D.IgnoreCollision(collision, gameObject.GetComponent<Collider2D>());
        }

        if (collision.gameObject.tag == "Smol Block")
        {

            GetParticles(collision.gameObject);
            GameObject.Find("SoundController").GetComponent<AudioScript>().SmallCrashAudio();
            _camScript.StartCoroutine("ShakeIt", 0.2f);
            CheckBlock(collision.gameObject);
        }

        if (collision.gameObject.tag == "Bounds")
        {
            GameObject.Find("SoundController").GetComponent<AudioScript>().SmallCrashAudio();
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
        }

        if(collision.gameObject.tag == "D1")
        {
            GetParticles(collision.gameObject);
            if (T1Points > 20)
            {
                T1Points -= 30;
            } else
            {
                T1Points = 0;
            }
            
            t1Bonus = 0;
            
            _discoScript.EnableShockwave(false, 0);
            collision.gameObject.transform.position = new Vector2(0, 0);
            CheckPoints();
;            
        } 

        if(collision.gameObject.name == "ModeSwitch")
        {
            //StartCoroutine("Slow");
        }
    }

    void CheckBlock(GameObject blok)
    {
        if(blok.GetComponent<SpriteRenderer>().color == _discoScript.colours[1] || blok.GetComponent<SpriteRenderer>().color == _discoScript.colours[3])
        {
            
            T1Points += 10;

            if(canBonus)
            {

                t1Bonus++;
                slider.value++;
            }
            
        } else
        {
            if(T1Points > 0)
            {
                T1Points -= 10;
            }
            
            

            if(canBonus)
            {
                t1Bonus = 0;
                slider.value = 0;
            }
            
        }
        Destroy(blok.gameObject);
        CheckPoints();
    }

    public void CheckPoints()
    {
        pointsText.text = "TEAM 1: " + T1Points;

        if(t1Bonus >= 5 && canBonus)
        {
            _discoScript.BlastCheck(0);

            StartCoroutine("Countdown");
            canBonus = false;
            
        } else
        {
            if (slider.gameObject.activeSelf == false)
            {
                slider.gameObject.SetActive(true);
                slider.value = 0;
            }
            canBonus = true;
        }
    }

    public IEnumerator Countdown()
    {
        
        while(slider.value > 0)
        {
            slider.value -= Time.deltaTime;

            yield return null;
        }

        if(slider.value <= 0)
        {
            t1Bonus = 0;
            GameObject.Find("Level Manager").GetComponent<VersusCheckScript>().t1Status = VersusCheckScript.T1Status.Normal;
            canBonus = true;
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

        if(wallSlider.value <= 0)
        {
            wallSlider.gameObject.SetActive(false);
        }
    }

    public IEnumerator Slow()
    {
        if (!isbehind)
        {

            _moveScript.speed /= 2;
            GameObject.FindGameObjectWithTag("Player 2").GetComponent<Player2Script>().p2Xpos = 0;

            yield return new WaitForSecondsRealtime(0.3f);
            GameObject.FindGameObjectWithTag("Player 2").GetComponent<Player2Script>().p2Xpos = -1f;
            _moveScript.playerRB.position = new Vector2(GameObject.FindGameObjectWithTag("Player 2").transform.position.x - 2, _moveScript.playerRB.position.y);
            _moveScript.speed = _moveScript.normalSpeed;
            isbehind = true;

           
        }
        else
        {
            isbehind = false;
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
