﻿using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using System;

public class Player : PlayerControl
{
    public delegate void PlayerDeathDel();
    public static event PlayerDeathDel PlayerDeathEvent;
    private bool canUpSpeed = true;
    //Animator anim;

    float h, v;
    void Update()
    {
        if (Input.GetKey(KeyCode.R) || Input.GetMouseButton(0))
        {
            anim.SetInteger("upspeed", 1);
            if (canUpSpeed)
            {
                StartCoroutine(upSpeed());
            }
        }
        if (Input.GetKeyUp(KeyCode.R) || Input.GetMouseButtonUp(0))
        {
            anim.SetInteger("upspeed", 0);
        }
    }
    private void FixedUpdate()
    {
#if UNITY_EDITOR
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        if (h == 0 && v == 0)
        {
            anim.SetInteger("condition", 0);
        }
        else
        {
            anim.SetInteger("condition", 1);
        }
        Vector3 dir = new Vector3(h, 0, v);
        nav.velocity = dir.normalized * nav.speed;

#elif UNITY_ANDROID
        if (Input.GetMouseButtonUp(0))
        {
            h = 0;
            v = 0;
        }
        if (h == 0 && v == 0)
        {
            h = Input.GetTouch(0).deltaPosition.x;
            v = Input.GetTouch(0).deltaPosition.y;
        }
        Vector3 _dir = new Vector3(h, 0, v);
        nav.velocity = _dir.normalized * nav.speed;
#endif
    }
    private void OnDisable()
    {
        PlayerDeathEvent?.Invoke();
    }

    IEnumerator upSpeed()
    {
        canUpSpeed = false;
        var speedDefault = nav.speed;
        nav.speed = 8;
        yield return new WaitForSeconds(3f);
        while (nav.speed > speedDefault)
        {
            nav.speed -= 1;
        }
        canUpSpeed = true;

    }

}
