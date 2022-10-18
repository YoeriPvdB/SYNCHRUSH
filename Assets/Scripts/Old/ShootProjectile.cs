using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    Transform Player;

    public GameObject Projectile;

    public float distCheck, dist;

    bool hasSpawned;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        dist = Vector2.Distance(new Vector2(Player.position.x, 0), new Vector2(this.transform.position.x, 0));

        if (dist < distCheck && hasSpawned == false)
        {
            Debug.Log(dist);
            Instantiate(Projectile, this.transform.position, Projectile.transform.rotation);
            hasSpawned = true;
        }

        
    }

}
