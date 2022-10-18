using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockColScript : MonoBehaviour
{
    T1DiscoObstScript t1Obst;
    T2DiscoObstacleScript t2Obst;

    AudioScript _audioScript;

    Obstacles _obstacleScript;

    private void Start()
    {
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "DiscoModeScene")
        {
            t1Obst = GameObject.FindGameObjectWithTag("Player").GetComponent<T1DiscoObstScript>();
            t2Obst = GameObject.FindGameObjectWithTag("Player 2").GetComponent<T2DiscoObstacleScript>();
        }

        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "DiscoModeScene" || UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "VersusModeScene")
        {
            _obstacleScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Obstacles>();
        }

        _audioScript = GameObject.Find("SoundController").GetComponent<AudioScript>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Player 2")
        {
            Physics2D.IgnoreCollision(collision, gameObject.GetComponent<Collider2D>());
        }

        if (collision.gameObject.tag == "BlastObstacle")
        {
            _audioScript.LargeCrashAudio();
            //_obstacleScript.particles.emission.burstCount = 
            _obstacleScript.GetParticles(collision.gameObject);
            Destroy(collision.gameObject);
            GameObject.FindGameObjectWithTag("Player").GetComponent<ShakeyCam>().StartCoroutine("ShakeIt", 0.5f);
        }

        if(collision.gameObject.tag == "T1" || collision.gameObject.tag == "T2")
        {
            _audioScript.LargeCrashAudio();
            collision.gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<ShakeyCam>().StartCoroutine("ShakeIt", 0.5f);
            //GameObject.Find("Level Manager").GetComponent<VersusManagerScript>().EnableShockwave(false);

            StartCoroutine("WaitToDisableRush");
        }

        if(collision.gameObject.tag == "D1")
        {
            t1Obst.GetParticles(collision.gameObject);
            _audioScript.LargeCrashAudio();
            t1Obst.T1Points += 30;

            if (t2Obst.T2Points > 20)
            {
                t2Obst.T2Points -= 30;
            }
            else
            {
                t2Obst.T2Points = 0;
            }

            t1Obst.t1Bonus = 0;
            t1Obst.CheckPoints();
            t2Obst.CheckPoints();
            
            collision.gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<ShakeyCam>().StartCoroutine("ShakeIt", 0.5f);





            StartCoroutine("WaitToDisableDisco", 0);

            //hello darkness my old friend
        }

        if(collision.gameObject.tag == "D2")
        {
            t2Obst.GetParticles(collision.gameObject);
            _audioScript.LargeCrashAudio();

            if(t1Obst.T1Points > 20)
            {
                t1Obst.T1Points -= 30;
            } else
            {
                t1Obst.T1Points = 0;
            }
            
            t2Obst.T2Points += 30;
            t2Obst.T2Bonus = 0;
            t1Obst.CheckPoints();
            t2Obst.CheckPoints();
            collision.gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<ShakeyCam>().StartCoroutine("ShakeIt", 0.5f);

            StartCoroutine("WaitToDisableDisco", 1);

           
        }

        
    }

    

    IEnumerator WaitToDisableRush()
    {
        yield return new WaitForSecondsRealtime(0.2f);

        GameObject.Find("Level Manager").GetComponent<VersusManagerScript>().EnableShockwave(false);
    }

    IEnumerator WaitToDisableDisco(int num)
    {
        yield return new WaitForSecondsRealtime(0.2f);

        GameObject.Find("Level Manager").GetComponent<DiscoModeScript>().EnableShockwave(false, num);
    }
}
