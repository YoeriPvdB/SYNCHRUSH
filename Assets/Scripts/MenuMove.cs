using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMove : MonoBehaviour
{
    [SerializeField] GameObject[] Bloks;

    [SerializeField] GameObject[] Levels;

    public GameObject[] Sprites;

    [SerializeField] Transform[] levelEnd;

    [SerializeField] int nextBlock, nextLvl, currentLvl;

    float dist, jumpForce, camSpace;

    Vector2 startPos;

    GameObject Level;

    Rigidbody2D pRb, levelRb;

    TrailRenderer trail;

    Camera cam;

    GameObject Skins;

    private void Start()
    {
        nextBlock = 0;
        currentLvl = 0;
        nextLvl = 1;
        jumpForce = 3800f;
        Level = GameObject.Find("BG Level");
        pRb = GetComponent<Rigidbody2D>();
        //levelRb = Level.GetComponent<Rigidbody2D>();
        startPos = this.transform.position;
        cam = Camera.main;
        trail = GetComponent<TrailRenderer>();

        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Menu Scene")
        {
            camSpace = 0;
        } else
        {
            camSpace = 5f;
            Skins = GameObject.Find("Game Skins");
        }
    }

    private void Update()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Menu Scene")
        {
            Skins.transform.position = transform.position;
        }

        //pRb.velocity = new Vector2(1000f * Time.deltaTime, pRb.velocity.y);

        

        

        if(nextBlock > 7)
        {
            nextBlock = 0;
        } 

        if(currentLvl > 1)
        {
            currentLvl = 0;
        }

        if(nextLvl > 1)
        {
            nextLvl = 0;
        }
    }

    private void FixedUpdate()
    {
        pRb.velocity = new Vector2(1000f * Time.deltaTime, pRb.velocity.y);

        dist = Vector2.Distance(new Vector2(pRb.position.x, 0), new Vector2(Bloks[nextBlock].transform.position.x, 0));

        if (dist <= 3f)
        {
            trail.time = 0.2f;
            pRb.velocity = new Vector2(pRb.velocity.x, (jumpForce) * Time.deltaTime);
            jumpForce *= -1;
            nextBlock++;

        }
    }

    private void LateUpdate()
    {

        cam.gameObject.transform.position = new Vector3(this.transform.position.x - camSpace, 0, -10);
        /*Levels[0].transform.Translate(Vector2.left * 15 * Time.deltaTime, 0);
        Levels[1].transform.Translate(Vector2.left * 15 * Time.deltaTime, 0);*/



    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "ModeSwitch")
        {
            // nextBlock = 0;
            /*Levels[currentLvl].transform.position = levelEnd[nextLvl].transform.position;
            currentLvl++;
            nextLvl++;*/
            

            if(nextLvl == 0)
            {
                transform.position = new Vector2(Levels[nextLvl].transform.position.x - 1f, transform.position.y);
                trail.time = 0;
                nextBlock = 0;
            }

            nextLvl++;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Top")
        {
            trail.time = 0.05f;
            trail.startColor = Color.red;
            Sprites[0].SetActive(false);
            Sprites[1].SetActive(true);

            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Menu Scene")
            {
                Skins.transform.localScale = new Vector2(Skins.transform.localScale.x, -1);
            }

        }

        if(collision.gameObject.tag == "Ground")
        {
            trail.time = 0.05f;
            trail.startColor = Color.cyan;
            Sprites[1].SetActive(false);
            Sprites[0].SetActive(true);


            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Menu Scene")
            {
                Skins.transform.localScale = new Vector2(Skins.transform.localScale.x, 1);
            }

        }
    }


}
