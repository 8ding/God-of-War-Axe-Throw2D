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
        private Transform backDestination;


        private void Start()
        {
            
            //记录初始旋转值
            originRotation = transform.localRotation;
            //注册发射与召回武器事件,玩家移动事件
            _playerMoveMent = GetComponentInParent<PlayerMoveMent>();

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
        
        
        private void moveCalculation()
        {
            if (IsCallBack)
            {
                transform.position = Vector3.MoveTowards(transform.position, backDestination.position, MoveSpeed * Time.deltaTime);
                if (Vector3.Distance(backDestination.position, transform.position) < 0.1f)
                {
                    setInHand();
                    transform.position = backDestination.position;
                }

            }
            if (IsOut)
            {
                transform.position = Vector3.MoveTowards(transform.position, outDestination, MoveSpeed * Time.deltaTime);
                if(Vector3.Distance(outDestination, transform.position) < 0.1f)
                {
                    IsOut = false;
                    IsOnGround = true;
                }
            }
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


        private void rotation()
        {
            if (IsOut)
                transform.Rotate(0,0,10 * RotationSpeed * Time.deltaTime);
            if(IsCallBack)
                transform.Rotate(0,0,-10 * RotationSpeed * Time.deltaTime);
                
        }
        //设置武器在手中，以角色为父物体,设置为初始旋转度
        private void setInHand()
        {
            IsCallBack = false;
            IsOnGround = false;
            IsCallBack = false;
            IsInHand = true;
            
            transform.SetParent(_playerMoveMent.transform);
            transform.localRotation = originRotation;
        }
        //设置武器在离开中，脱离父物体
        private void setOutHand()
        {
            transform.SetParent(null);
        }
    }
}