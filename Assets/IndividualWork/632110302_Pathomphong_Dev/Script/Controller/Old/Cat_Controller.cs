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
            Vector3 CatTranform = this.transform.position;
            CatTranform.z += MaxSpeed * Time.deltaTime;
            this.transform.position = CatTranform;
        }

        public override void Move_Backward()
        {
            Vector3 CatTranform = this.transform.position;
            CatTranform.z -= MaxSpeed * Time.deltaTime;
            this.transform.position = CatTranform;
        }

        public override void Move_Left()
        {
            Vector3 CatTranform = this.transform.position;
            CatTranform.x -= MaxSpeed * Time.deltaTime;
            this.transform.position = CatTranform;
        }

        public override void Move_Right()
        {
            Vector3 CatTranform = this.transform.position;
            CatTranform.x += MaxSpeed * Time.deltaTime;
            this.transform.position = CatTranform;
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
                print("Climbing");
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
