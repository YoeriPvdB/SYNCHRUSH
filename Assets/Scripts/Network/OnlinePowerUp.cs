using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlinePowerUp : MonoBehaviour
{
    public PowerUps powerUps;

    public float ogSpeed, newSpeed;

    public float zoomSpace;

    int lifeCount, currentLife;

    UIScript _uiScript;

    OnlineMovementScript _moveScript;

    ShakeyCam _camScript;

    OnlineObstacles _obstacleScript;

    public AudioScript _audioScript;

    private void Start()
    {
        currentLife = 1;
        ogSpeed = 1300f;
        newSpeed = ogSpeed * 3f;
        lifeCount = GetComponent<OnlineMovementScript>().lifeCount;

        _uiScript = GameObject.Find("UI Handler").GetComponent<UIScript>();
        _moveScript = GetComponent<OnlineMovementScript>();
        _camScript = GetComponent<ShakeyCam>();
        _obstacleScript = GetComponent<OnlineObstacles>();

        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "VersusModeScene" || UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "DiscoModeScene")
        {
            GameObject.FindGameObjectWithTag("SpeedBoost").SetActive(false);
        }
    }

    private void Update()
    {
        switch(powerUps)
        {
            case PowerUps.None:

                //currentLife = GetComponent<PlayerMovement>().lifeCount;
                break;

            case PowerUps.Speed:

                GetComponent<OnlineMovementScript>().lifeCount = 1;

                

                break;

            case PowerUps.Life:

                _uiScript.leveltime -= 10f;
                powerUps = PowerUps.None;

                break; 
        }

        

    }

    IEnumerator SpeedUp()
    {
        
        _moveScript.SlowMo();

        _camScript.isZooming = true;

        zoomSpace = 6f;

        _moveScript.speed = newSpeed;

        yield return new WaitForSecondsRealtime(_moveScript.slowdownLength);

        _moveScript.isJumping = true;
        _camScript.isZooming = true;
        zoomSpace = 10f;

        yield return new WaitForSeconds(1.5f);



        _moveScript.speed = GetComponent<PlayerMovement>().normalSpeed;
        _obstacleScript.lifeCount = currentLife;
        
        powerUps = PowerUps.None;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if(GetComponent<BlastScript>().canBlast == true)
        {
            

        }*/

        if (collision.gameObject.tag == "Life")
        {
            currentLife++;
            _obstacleScript.lifeCount = currentLife;
            _audioScript.PlayP1Audio(2);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "SpeedBoost")
        {
            _audioScript.PlayP1Audio(2);

            //GetComponent<PlayerMovement>().playerPositions = PlayerMovement.PlayerPositions.Bot;
            powerUps = PowerUps.Speed;
            Destroy(collision.gameObject);
            StartCoroutine("SpeedUp");
        }
    }


    public enum PowerUps
    {
        None,
        Speed,
        Life
    }
}
