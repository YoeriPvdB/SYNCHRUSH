using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmScript : MonoBehaviour
{
    Rigidbody2D rhythmRb;

    PlayerMovement _moveScript;

    PowerUp _powerUpScript;

    float leveltime;

    GameObject Player, Obstacles;

    public GameObject rhythmStuff;

    public GameObject currentBlock;

    public bool barActive = false;

    public  bool canActivate = false;

    public int hitCount;

    private void Start()
    {
        
        Player = GameObject.FindGameObjectWithTag("Player");
        //rhythmStuff = GameObject.FindGameObjectWithTag("RhythmObstacle");
        Obstacles = GameObject.FindGameObjectWithTag("Obstacle");

        _powerUpScript = Player.GetComponent<PowerUp>();

        hitCount = 0;

        foreach(Transform Child in Obstacles.transform)
        {
            //Instantiate(Child, new Vector2(Child.transform.position.x - 20f, -3.5f), Child.transform.rotation, rhythmStuff.transform);

            if(Child.gameObject.GetComponent<BoxCollider2D>().size.y <= 1f)
            {
                Child.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(3.5f, 2f);
            }
            
        }

        /*foreach(Transform child in rhythmStuff.transform)
        {
            child.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            child.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(2f, 2f);
            child.gameObject.tag = rhythmStuff.tag;
            child.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            child.gameObject.GetComponent<SpriteRenderer>().color = new Color (1f,1f,1f,0.5f);
        }*/

    }

    private void Update()
    {
        this.transform.position = new Vector2(Player.transform.position.x + 8f, this.transform.position.y);

        if(hitCount >= 5)
        {
            GameObject.Find("UI Handler").GetComponent<UIScript>().leveltime -= 10f;
            /*_powerUpScript.powerUps = PowerUp.PowerUps.Speed;
            _powerUpScript.StartCoroutine("SpeedUp");*/
            hitCount = 0;
        }
    }

    void BeatTime()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.transform.parent != null && collision.gameObject.transform.parent.tag == "Obstacle")
        {
            currentBlock = collision.gameObject;
            canActivate = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.transform.parent != null && collision.gameObject.transform.parent.tag == "Obstacle")
        {
            //currentBlock = null;
            canActivate = false;
            
            if((collision.GetComponent<SpriteRenderer>().color != Color.blue) && (collision.GetComponent<SpriteRenderer>().color != Color.red))
            {
                hitCount = 0;
            }

            if(collision.gameObject.GetComponent<BoxCollider2D>().size.x >= 3f)
            {
                collision.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1f, 1f);
            }
            
        }
    }
}
