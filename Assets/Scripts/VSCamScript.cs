using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VSCamScript : MonoBehaviour
{
    Rigidbody2D camRb;

    public float camSpeed;

    PauseScript _pauseScript;

    public bool canMove;

    PlayerMovement _moveScript;

    private void Start()
    {
        camRb = GetComponent<Rigidbody2D>();
        _pauseScript = GameObject.Find("Menu Canvas").GetComponent<PauseScript>();
        _moveScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        if(SceneManager.GetActiveScene().name == "DiscoModeScene")
        {
            camSpeed = 1150f;
        } else
        {
            camSpeed = 1300f;
        }
        //camSpeed = _moveScript.normalSpeed;

        canMove = false;
    }

    private void Update()
    {
        
        
    }

    private void FixedUpdate()
    {
        if(canMove)
        {
            if (SceneManager.GetActiveScene().name == "DiscoModeScene")
            {
                camRb.velocity = new Vector2(_moveScript.playerRB.velocity.x, 0);
            }
            else
            {
                camRb.velocity = new Vector2(camSpeed * Time.deltaTime, 0);
            }
            
        } else
        {
            camRb.velocity = new Vector2(0, 0);
            camRb.position = new Vector2(_moveScript.gameObject.transform.position.x + 1, camRb.position.y);
        }
    }
}
