using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D shootRb;

    public float speed;

    GameObject Player;

    bool trackPlayer;

    Vector2 pStartYPosition;

    private void Start()
    {
        shootRb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        pStartYPosition = Player.GetComponent<PlayerMovement>().startYPos;
    }

    private void FixedUpdate()
    {
        shootRb.velocity = new Vector2(-speed * Time.deltaTime, shootRb.velocity.y);

        float dist = Vector2.Distance(new Vector2(0, Player.GetComponent<Rigidbody2D>().position.y), new Vector2(0, pStartYPosition.y));
    }
}
