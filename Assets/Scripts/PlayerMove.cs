﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;

    public PanelManager manager;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CapsuleCollider2D capsuleCollider;
    Vector3 dirVec;
    GameObject scanObject;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        //Jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }

        //Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //Direction Sprite
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        //Move Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);

        //Scan Object
        if (Input.GetButtonDown("Fire1") && scanObject != null)
        {
            manager.Action(scanObject);
        }
    }

    void FixedUpdate()
    {
        //Move Speed
        float h = manager.isAction ? 0 :Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        //Max Speed
        if (rigid.velocity.x > maxSpeed) //Right Max Speed
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
            dirVec = Vector3.right;
        }
        else if (rigid.velocity.x < maxSpeed * (-1)) //Left Max Speed 
        {
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
            dirVec = Vector3.left;
        }
        //Landing Platform
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 0, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                    anim.SetBool("isJumping", false);
            }
        }

        //Scan Ray
        Debug.DrawRay(rigid.position, dirVec * 1, new Color(0, 1, 0));
        RaycastHit2D NPCrayHit = Physics2D.Raycast(rigid.position, dirVec, 1, LayerMask.GetMask("NPC"));

        if (NPCrayHit.collider != null)
        {
            scanObject = NPCrayHit.collider.gameObject;
        }
            
        else
            scanObject = null;
    }
}
