using System;
using UnityEngine;

namespace Weapon
{
    public class WeaponController : MonoBehaviour
    {
        [Header("状态参数")] 
        public bool IsOut;
        public bool IsCallBack;
        public bool IsOnGround;
        public bool IsInHand;

        [Header("移动速度")] public float MoveSpeed;
        [Header("旋转速度")] public float RotationSpeed;

        private Vector2 moveVelocity;
        private Vector3 outDestination;
        private PlayerMoveMent _playerMoveMent;
        private Quaternion originRotation;
        private Rigidbody2D _rigidbody2D;
        private Transform backDestination;


        private void Start()
        {
            
            //记录初始旋转值
            originRotation = transform.localRotation;
            //注册发射与召回武器事件,玩家移动事件
            _playerMoveMent = GetComponentInParent<PlayerMoveMent>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            
            _playerMoveMent.FireWeapon += handleFireWeapon;
            _playerMoveMent.CallBack += handleCallBack;
            backDestination = _playerMoveMent.WeaponPosition;

            setInHand();
        }

        private void Update()
        {
            moveCalculation();
            rotation();
        }

        private void FixedUpdate()
        {
            move();
        }
        
        private void moveCalculation()
        {
            if (IsCallBack)
            {
                if (Vector3.Distance(backDestination.position, transform.position) < 0.1f)
                {
                    setInHand();
                    transform.position = backDestination.position;
                }
                else
                {
                    moveVelocity = MoveSpeed * Time.fixedDeltaTime * (backDestination.position - transform.position).normalized;
                }
            }
            if (IsOut)
            {
                if(Vector3.Distance(outDestination, transform.position) < 0.2f)
                {
                    IsOut = false;
                    IsOnGround = true;
                    moveVelocity = Vector2.zero;
                }
                else
                {
                    moveVelocity = MoveSpeed * Time.fixedDeltaTime * (outDestination - transform.position).normalized;
                }
            }
            if(IsInHand || IsOnGround)
                moveVelocity = Vector2.zero;
        }
        private void handleFireWeapon(Vector3 _destination)
        {
            if (IsInHand && !IsOnGround && !IsOut && !IsCallBack)
            {
                if(_destination != null) 
                    outDestination = _destination;
                IsInHand = false;
                IsOut = true;
                setOutHand();
            }
        }
        
        private void handleCallBack()
        {
            if (IsOut)
            {
                IsOut = false;
                IsCallBack = true;
            }
            else if(IsOnGround)
            {
                IsOnGround = false;
                IsCallBack = true;
            }
        }

//        private void handlePlayerMove(Vector3 _destination)
//        {
//            backDestination = _destination;
//        }

        private void move()
        {
            _rigidbody2D.velocity = moveVelocity;
        }

        private void rotation()
        {
            if (IsOut)
                transform.Rotate(0,0,10 * RotationSpeed * Time.deltaTime);
            if(IsCallBack)
                transform.Rotate(0,0,-10 * RotationSpeed * Time.deltaTime);
                
        }
        //设置武器在手中，以角色为父物体，刚体失效
        private void setInHand()
        {
            IsCallBack = false;
            IsOnGround = false;
            IsCallBack = false;
            IsInHand = true;

            moveVelocity = Vector2.zero;
            transform.SetParent(_playerMoveMent.transform);
            transform.localRotation = originRotation;
            _rigidbody2D.simulated = false;
        }
        //设置武器在离开中，脱离父物体，刚体激活
        private void setOutHand()
        {
            transform.SetParent(null);
            _rigidbody2D.simulated = true;
        }
    }
}