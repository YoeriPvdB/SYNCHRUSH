using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    List<GameObject> backgroundSprites = new List<GameObject>();

    Vector2[] directions = { Vector2.up, Vector2.right };

    public float moveTime;

    public int dir;

    private void Start()
    {
        UpDown();
        GetMovement();
    }

    private void Update()
    {
        moveTime -= Time.deltaTime;

        transform.Translate(directions[dir] * 1f * Time.deltaTime);

        if(moveTime <= 0)
        {
            GetMovement();
        }
    }

    void UpDown()
    {
        float dirFlip = Random.Range(0, 5);

        float yDir = Random.Range(5, 10);

        if (dirFlip > 2.5f)
        {
            directions[0] *= -1;
            transform.position = new Vector2(transform.position.x, yDir);
        } else
        {
            transform.position = new Vector2(transform.position.x, -yDir);
        }
    }

    void GetMovement()
    {
        float dirCheck = Random.Range(0, 5);
        float dirFlip = Random.Range(0, 5);
        

        if (dirCheck > 3f)
        {
            moveTime = Random.Range(0.2f, 0.5f);
            dir = 1;
        } else
        {
            moveTime = Random.Range(0.5f, 1f);
            dir = 0;
        }

        if(dirFlip > 2.5f)
        {
            directions[1] *= -1;
        }
        
    }
}
