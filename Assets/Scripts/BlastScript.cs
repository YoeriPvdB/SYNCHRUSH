using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastScript : MonoBehaviour
{
    public GameObject shockWave;

    CircleCollider2D shockCol;

    public float blastTime, blastRadius, blastOpacity;

    PlayerSwitch _switchScript;

    AudioScript _audioScript;

    public Material blueBlastMat, redBlastMat;


    public bool canBlast;

    GameObject[] blastObstacles;

    

    private void Start()
    {
        shockCol = shockWave.GetComponent<CircleCollider2D>();
        blastRadius = 1.2f;

        blastOpacity = 255f;

        _switchScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSwitch>();

        canBlast = true;

        shockCol.enabled = false;

        blastObstacles = GameObject.FindGameObjectsWithTag("BlastObstacle");

        BlastStatus();

        _audioScript = GameObject.Find("SoundController").GetComponent<AudioScript>();
    }

    private void Update()
    {
        //shockWave.GetComponent<SpriteRenderer>().color = Color.red;

        

        shockWave.transform.localScale = new Vector2(blastRadius, blastRadius);

        if(_switchScript.playerChoice == PlayerSwitch.PlayerChoice.Player1)
        {
            shockWave.GetComponent<SpriteRenderer>().color = new Color(1f, 0, 0, blastOpacity);
            shockWave.GetComponent<SpriteRenderer>().material = redBlastMat;

            
        } else
        {
            shockWave.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1f, blastOpacity);
            shockWave.GetComponent<SpriteRenderer>().material = blueBlastMat;

            
        }

        shockWave.transform.position = this.transform.position;
        
    }

    

    public void BlastStatus()
    {

        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "BlastModeScene" || UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "NormalModeScene"
            || UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "EndlessModeScene" || UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 11)
        {
            if ((_switchScript.inTandem))
            {
                shockWave.SetActive(false);
            }
            else
            {
                shockWave.SetActive(true);
            }
        }
            // || (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "VersusModeScene" 
            //|| UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "DiscoModeScene")
    }

    public IEnumerator Blast()
    {
        shockCol.enabled = true;
        
       
        while(blastRadius <= 12)
        {
            blastRadius += (0.7f * 180f) * Time.deltaTime;
            canBlast = false;
            if (blastRadius >= 4)
            {
                blastOpacity -= 5f;
            }

           

            yield return null;
        }

        if (blastRadius > 12)
        {
            shockCol.enabled = false;
            blastRadius = 0.6f;
            StartCoroutine("Recharge");
            
            blastOpacity = 255f;
        }

        
    }

    IEnumerator Recharge()
    {
       

        while (blastRadius <= 1.2f)
        {

            blastRadius += (0.01f * 180f) * Time.deltaTime;

            yield return null;
        }

        if(blastRadius > 1.2f)
        {
            _audioScript.PlayP1Audio(3);
            canBlast = true;
        }
    }

    

    
}
