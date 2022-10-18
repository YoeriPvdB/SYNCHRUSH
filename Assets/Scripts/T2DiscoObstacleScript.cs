using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class T2DiscoObstacleScript : MonoBehaviour
{
    public int T2Points, T2Bonus;

    Player2Script _p2Script;
    PlayerSwitch _switchScript;
    ShakeyCam _camScript;

    [SerializeField] DiscoModeScript _discoScript;
    [SerializeField] Text pointsText;
    public bool canBonus;
    public Slider slider, wallSlider;
    public ParticleSystem particles;
    GameObject platformPart;

    private void Start()
    {
        T2Points = 0;
        CheckPoints();
        _camScript = GameObject.FindGameObjectWithTag("Player").GetComponent<ShakeyCam>();
        _p2Script = GetComponent<Player2Script>();
        slider.gameObject.transform.position = new Vector2(transform.position.x + 0.7f, transform.position.y);
        particles = GameObject.Find("Obstacle Particles 2").GetComponent<ParticleSystem>();
        platformPart = GameObject.Find("Platform Particles 2");
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
        }

        if (collision.gameObject.tag == "D2")
        {
            GameObject.Find("SoundController").GetComponent<AudioScript>().SmallCrashAudio();
            if (T2Points > 20)
            {
                T2Points -= 30;
            } else
            {
                T2Points = 0;
            }
            
            T2Bonus = 0;

            GetParticles(collision.gameObject);
            _discoScript.EnableShockwave(false, 1);
            collision.gameObject.transform.position = new Vector2(0, 0);
            CheckPoints();
        }

        if (collision.gameObject.name == "ModeSwitch")
        {
            StartCoroutine("Slow");
        }
    }

    void CheckBlock(GameObject blok)
    {
        if (blok.GetComponent<SpriteRenderer>().color == _discoScript.colours[0] || blok.GetComponent<SpriteRenderer>().color == _discoScript.colours[2])
        {
            
            T2Points += 10;
            if(canBonus)
            {
                T2Bonus++;
                slider.value++;
            }
            
        }
        else
        {
            if(T2Points > 0)
            {
                T2Points -= 10;
            }

            
            if(canBonus)
            {
                T2Bonus = 0;
                slider.value = 0;
            }
        }

        Destroy(blok.gameObject);
        CheckPoints();
    }

    public void CheckPoints()
    {
        pointsText.text = "TEAM 2: " + T2Points;

        if (T2Bonus >= 5 && canBonus)
        {
            _discoScript.BlastCheck(1);
            
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
        

        while (slider.value > 0)
        {
            slider.value -= Time.deltaTime;

            yield return null;
        }

        if (slider.value <= 0)
        {
            T2Bonus = 0;
            
            print("normie 2");
            GameObject.Find("Level Manager").GetComponent<VersusCheckScript>().t2Status = VersusCheckScript.T2Status.Normal;

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

        if (wallSlider.value <= 0)
        {
            wallSlider.gameObject.SetActive(false);
        }
    }

    public IEnumerator Slow()
    {
        /*if (GameObject.FindGameObjectWithTag("Player").GetComponent<T1DiscoObstScript>().isbehind)
        {

           
        }
        else
        {
            yield return null;
        }*/

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<T1DiscoObstScript>().isbehind == true)
        {
            for (float i = -2f; i < 2f; i += 0.1f)
            {
                
                _p2Script.p2Xpos = i;

                yield return null;
            }

            /*GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().speed *= 1.3f;
            _p2Script.p2Speed /= 1.3f;
            _p2Script.p2Xpos = 0;

            yield return new WaitForSecondsRealtime(0.3f);

            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().playerRB.position = new Vector2(GameObject.Find("Follow Object").transform.position.x + 1f,
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().playerRB.position.y);
            //_p2Script.p2RB.position = new Vector2(GameObject.Find("Follow Object").transform.position.x - 1f, _p2Script.p2RB.position.y);

            _p2Script.p2Xpos = -1f;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().speed = 1150f;*/

            GameObject.FindGameObjectWithTag("Player").GetComponent<T1DiscoObstScript>().isbehind = false;
            

        } else
        {
            for (float i = 2f; i > -2.5f; i -= 0.1f)
            {
               
                _p2Script.p2Xpos = i;

                yield return null;
            }

            /*GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().speed *= 1.3f;
            _p2Script.p2Xpos = 0;
            // _p2Script.p2Speed /= 1.3f;

            yield return new WaitForSecondsRealtime(0.3f);

            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().playerRB.position = new Vector2(GameObject.Find("Follow Object").transform.position.x + 1f,
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().playerRB.position.y);
            //_p2Script.p2RB.position = new Vector2(GameObject.Find("Follow Object").transform.position.x - 1f, _p2Script.p2RB.position.y);

            _p2Script.p2Xpos = 1f;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().speed = 1150f;*/

            GameObject.FindGameObjectWithTag("Player").GetComponent<T1DiscoObstScript>().isbehind = true;
            
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
