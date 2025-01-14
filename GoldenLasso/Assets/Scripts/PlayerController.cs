﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    float resetSpeed;
    public float dashSpeed;
    public float dashTime;




    PhotonView view;
    Animator anim;
    Health healthScript;



    LineRenderer rend;

    public float minX, maxX, minY, maxY;

    public Text nameDisplay;



    private void Start()
    {
        resetSpeed = speed;
        view = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
        healthScript = FindObjectOfType<Health>();
        rend = FindObjectOfType<LineRenderer>();

        if (view.IsMine)
        {
            nameDisplay.text = PhotonNetwork.NickName;
        }
        else
        {
            nameDisplay.text = view.Owner.NickName;
        }
    }

    private void Update()
    {
        if (view.IsMine)
        {
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector2 moveAmount = moveInput.normalized * speed * Time.deltaTime;
            transform.position += (Vector3)moveAmount;

            Wrap();

            if (Input.GetKeyDown(KeyCode.Space) && moveInput != Vector2.zero)
            {

                StartCoroutine(Dash());
            }

            if (moveInput == Vector2.zero)
            {
                anim.SetBool("isRunning", false);
            }
            else
            {
                anim.SetBool("isRunning", true);
            }

            rend.SetPosition(0, transform.position);
        }
        else
        {
            rend.SetPosition(1, transform.position);
        }
    }

    IEnumerator Dash()
    {
        speed = dashSpeed;
        yield return new WaitForSeconds(dashTime);
        speed = resetSpeed;
    }

    private void Wrap()
    {
        if (transform.position.x < minX)
        {
            transform.position = new Vector2(maxX, transform.position.y);
        }

        if (transform.position.x > maxX)
        {
            transform.position = new Vector2(minX, transform.position.y);
        }

        if (transform.position.y < minY)
        {
            transform.position = new Vector2(maxY, transform.position.x);
        }

        if (transform.position.y > maxY)
        {
            transform.position = new Vector2(minY, transform.position.x);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (view.IsMine)
        {
            if (other.tag == "Enemy")
            {
                healthScript.TakeDamage();
            }
        }
    }
}
