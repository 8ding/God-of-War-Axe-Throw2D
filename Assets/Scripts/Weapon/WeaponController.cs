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
        private Vector3 backDestination;
        private PlayerMoveMent _playerMoveMent;
        private Quaternion originRotation;
        private Rigidbody2D _rigidbody2D;

        public event Action WeaponBack;  

        private void Start()
        {
            IsCallBack = false;
            IsOnGround = false;
            IsOut = false;
            IsInHand = true;
            _playerMoveMent = GetComponentInParent<PlayerMoveMent>();
            originRotation = transform.localRotation;
            //注册发射与召回武器事件
            _playerMoveMent.FireWeapon += handleFireWeapon;
            _playerMoveMent.CallBack += handleCallBack;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _playerMoveMent.PlayerMove += handlePlayerMove;
        }

        private void Update()
        {
            moveCalculation();
            rotation();
            inHandTest();
        }

        private void FixedUpdate()
        {
            move();
        }

        private void moveCalculation()
        {
            if (IsCallBack)
            {
                if (Vector3.Distance(backDestination, transform.position) < 0.1f)
                {
                    IsCallBack = false;
                    IsInHand = true;
                    moveVelocity = Vector2.zero;
                }
                else
                {
                    moveVelocity = MoveSpeed * Time.fixedDeltaTime * (backDestination - transform.position).normalized;
                }
            }
            if (IsOut)
            {
                if(Vector3.Distance(outDestination, transform.position) < 0.1f)
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
            }
        }
        
        private void handleCallBack(Vector3 _destination)
        {
            if (IsOut)
            {
                backDestination = _destination;
                IsOut = false;
                IsCallBack = true;
            }
            else if(IsOnGround)
            {
                if(_destination != null) 
                    backDestination = _destination;
                IsOnGround = false;
                IsCallBack = true;
            }
        }

        private void handlePlayerMove(Vector3 _destination)
        {
            backDestination = _destination;
        }

        private void move()
        {
            _rigidbody2D.velocity = moveVelocity;
        }

        private void inHandTest()
        {
            //如果在手里设置刚体无效跟着玩家位置移动，否则刚体有效，脱离父物体
            if (IsInHand)
            {
                transform.SetParent(_playerMoveMent.transform);
                transform.localRotation = originRotation;
                _rigidbody2D.simulated = false;
            }
            else
            {
                transform.SetParent(null);
                _rigidbody2D.simulated = true;
            }
        }

        private void rotation()
        {
            if (IsOut)
                transform.Rotate(0,0,10 * RotationSpeed * Time.deltaTime);
            if(IsCallBack)
                transform.Rotate(0,0,-10 * RotationSpeed * Time.deltaTime);
                
        }
    }
}