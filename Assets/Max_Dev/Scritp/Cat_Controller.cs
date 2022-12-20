using System;
using System.Collections;
using System.Collections.Generic;
using Max_DEV.MoveMent;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Max_DEV.MoveMent
{
    [RequireComponent(typeof(Rigidbody))]
    public class Cat_Controller : PlayerController
    {
        private bool Can_Climb;
        void Start()
        {
            Can_Climb = false;
        }

        // Update is called once per frame
        void FixedUpdate()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            ObjectType_Identities OtherType = other.GetComponent<ObjectType_Identities>();
            if (OtherType != null)
            {
                switch (OtherType.Type)
                {
                    case ObjectType.ClimbArea:
                        ResetJump();
                        Can_Climb = true;
                        print("can Climb");
                        break;
                    case ObjectType.Floor:
                        ResetJump();
                        break;
                }
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            ObjectType_Identities OtherType = other.GetComponent<ObjectType_Identities>();
            if (OtherType != null)
            {
                switch (OtherType.Type)
                {
                    case ObjectType.ClimbArea:
                        Can_Climb = false;
                        print("No Climb");
                        break;
                }
            }
        }

        #region MoveMent
        public override void Move_Forward()
        {
            Rigidbody cat_rigid = GetComponent<Rigidbody>();
            cat_rigid.AddForce(0, 0, AcceleRation);
        }

        public override void Move_Backward()
        {
            Rigidbody cat_rigid = GetComponent<Rigidbody>();
            cat_rigid.AddForce(0, 0, -AcceleRation);
        }

        public override void Move_Left()
        {
            Rigidbody cat_rigid = GetComponent<Rigidbody>();
            cat_rigid.AddForce(-AcceleRation, 0, 0);
        }

        public override void Move_Right()
        {
            Rigidbody cat_rigid = GetComponent<Rigidbody>();
            cat_rigid.AddForce(AcceleRation, 0, 0);
        }

        public override void Jump_Up()
        {
            Rigidbody cat_rigid = GetComponent<Rigidbody>();
            print("jump");
            cat_rigid.AddForce(0, JumpPower, 0, ForceMode.Impulse);
        }

        public override void Climb_Up()
        {
            if (Can_Climb == true)
            {
                Rigidbody cat_rigid = GetComponent<Rigidbody>();
                print("Climb ing");
                cat_rigid.AddForce(0, AcceleRation * 2f, 0);
                ResetJump();
            }
            else
            {
                return;
            }
        }
        #endregion
    }
}
