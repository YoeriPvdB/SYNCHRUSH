using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScript : MonoBehaviour
{
    public float scrollSpeed;

    Vector2 offset;

    MeshRenderer brenderer;

    SpriteRenderer srend;

    Shader shad;
    Material mat;

    Transform Player;

    private void Start()
    {
        brenderer = GetComponent<MeshRenderer>();
        //shad = GetComponent<Shader>();

        //srend = GetComponent<SpriteRenderer>();

        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        offset = new Vector2(scrollSpeed * Time.deltaTime, 0);
        
        brenderer.material.mainTextureOffset += offset;
        
        gameObject.transform.position = new Vector2(Player.position.x, 0);
    }
}
