using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerMoveMent : MonoBehaviour
{
    // Start is called before the first frame update
    private float moveH;
    private float moveV;
    private CharacterController _characterController;
    public float Speed;
    private Vector2 moveDirection;
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        moveMent();
        faceDirection();
    }
    //人物移动
    private void moveMent()
    {
        moveH = Input.GetAxis("Horizontal");
        moveV = Input.GetAxis("Vertical");
        moveDirection = new Vector2(moveH * Speed * Time.deltaTime, moveV * Speed * Time.deltaTime);
        _characterController.Move(moveDirection);
    }

    //人物转向
    private void faceDirection()
    {
        if(_characterController.velocity.x < 0f)
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        else if(_characterController.velocity.x > 0f)
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
    }
}
