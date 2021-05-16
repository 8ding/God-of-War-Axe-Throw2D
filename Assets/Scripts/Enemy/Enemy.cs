using System;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Transform wayDestination1, wayDestination2;
        [SerializeField] private float Speed;

        private Rigidbody2D _rigidbody2D;
        private Vector3 position1, position2;
        private Vector2 moveVelocity;
        private bool isToDestination2;
        
        private void Start()
        {
            isToDestination2 = true;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            position1 = wayDestination1.position;
            position2 = wayDestination2.position;
        }

        private void Update()
        {
            moveCalculation();
        }

        private void FixedUpdate()
        {
            move();
        }

        private void moveCalculation()
        {
            if (isToDestination2)
            {
                if (Vector3.Distance(transform.position, position2) < 0.1f)
                    isToDestination2 = false;
            }
            else
            {
                if (Vector3.Distance(transform.position, position1) < 0.1f)
                    isToDestination2 = true;
            }

            if (isToDestination2)
            {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                moveVelocity = Time.fixedDeltaTime * Speed * (position2 - transform.position).normalized;
            }
            else
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                moveVelocity = Time.fixedDeltaTime * Speed * (position1 - transform.position).normalized;
            }
        }

        private void move()
        {
            _rigidbody2D.velocity = moveVelocity;
        }
    }
}