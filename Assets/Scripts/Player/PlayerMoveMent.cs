using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerMoveMent : MonoBehaviour
{
    // Start is called before the first frame update
    private float moveH;
    private float moveV;
    private Rigidbody2D _rigidbody2D;
    public float Speed;
    private Vector2 moveDirection;
    private Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput();
        faceDirection();
        switchAnim();
    }

    private void FixedUpdate()
    {
        moveMent();
    }

    //移动输入
    private void moveInput()
    {
        moveH = Input.GetAxis("Horizontal");
        moveV = Input.GetAxis("Vertical");
        moveDirection = new Vector2(moveH * Speed * Time.fixedDeltaTime, moveV * Speed * Time.fixedDeltaTime);
    }
    
    //人物移动
    private void moveMent()
    {
        _rigidbody2D.velocity = moveDirection;
    }
    //人物转向
    private void faceDirection()
    {
        if(_rigidbody2D.velocity.x < 0f)
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        else if(_rigidbody2D.velocity.x > 0f)
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
    }

    //动画切换
    private void switchAnim()
    {
        if (Mathf.Abs(_rigidbody2D.velocity.x) > 0.1f)
        {
            _animator.SetBool("IsRun",true);
        }
        else
        {
            _animator.SetBool("IsRun",false);
        }
    }
}
