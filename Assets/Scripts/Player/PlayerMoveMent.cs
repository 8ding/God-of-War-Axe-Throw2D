using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using Weapon;

public class PlayerMoveMent : MonoBehaviour
{
    // Start is called before the first frame update
    private float moveH;
    private float moveV;
    private Rigidbody2D _rigidbody;
    public float Speed;
    private Vector2 moveDirection;
    private Animator _animator;
    public Transform WeaponPosition;
    public Transform Destination;

    private bool weaponOnhand;
    public event Action<Vector3> FireWeapon;
    public event Action<Vector3> CallBack;
    public event Action<Vector3> PlayerMove; 

    private WeaponController weapon;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        weaponOnhand = true;
        weapon = GetComponentInChildren<WeaponController>();
        weapon.WeaponBack += handleWeaponBack;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput();
        faceDirection();
        switchAnim();
        weaponTest();
        if(!weaponOnhand)
            PlayerMove?.Invoke(WeaponPosition.position);
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
        _rigidbody.velocity = moveDirection;
    }
    //人物转向
    private void faceDirection()
    {
        if(_rigidbody.velocity.x < 0f)
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        else if(_rigidbody.velocity.x > 0f)
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
    }

    //动画切换
    private void switchAnim()
    {
        if (Mathf.Abs(_rigidbody.velocity.x) > 0.1f)
        {
            _animator.SetBool("IsRun",true);
        }
        else
        {
            _animator.SetBool("IsRun",false);
        }
    }

    private void weaponTest()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireWeapon?.Invoke(Destination.position);
            weaponOnhand = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            CallBack?.Invoke(WeaponPosition.position);
        }
    }

    private void handleWeaponBack()
    {
        weaponOnhand = true;
    }
}
