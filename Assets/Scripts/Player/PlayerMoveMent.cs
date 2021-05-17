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

    public event Action<Vector3> FireWeapon;
    public event Action CallBack;



    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput();
        faceDirection();
        switchAnim();
        weaponTest();
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //鼠标屏幕位置转世界坐标位置,要设定深度，这个深度z就是你所定义的屏幕距摄像机的距离
            Vector3 mouseWorlPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                transform.position.z - Camera.main.transform.position.z));

            Vector3 Destination = new Vector3(mouseWorlPosition.x, mouseWorlPosition.y, 0);
            Debug.DrawRay(transform.position, Destination - transform.position, Color.green, 1f);
            FireWeapon?.Invoke(Destination);
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            CallBack?.Invoke();
        }
    }
    
}
