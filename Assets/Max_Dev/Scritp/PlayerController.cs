using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace  Max_DEV.MoveMent
{
    public abstract class PlayerController : MonoBehaviour, I_PlayerController
    {
        [FormerlySerializedAs("cat_ForwardKey")]
        [Header("Keys Config")]
        [SerializeField] protected Key ForwardKey = Key.W;
        [SerializeField] protected Key BackwardKey = Key.S;
        [SerializeField] protected Key TurnLeftKey = Key.A;
        [SerializeField] protected Key TurnRightKey = Key.D;
        
        [SerializeField] protected Key JumpKey = Key.Space;

        [Header("Stat Config")] 
        [SerializeField] protected float AcceleRation = 0.1f;
        [SerializeField] protected float MaxSpeed = 10.0f;
        [SerializeField] protected float JumpPower = 5.0f;
        [SerializeField] protected int JumpLimit = 1;
        
        
        private int JumpCount = 0;
        

        private void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            Keyboard keyboard = Keyboard.current;
            
            if (keyboard[ForwardKey].isPressed)
                Move_Forward();
            if (keyboard[BackwardKey].isPressed)
                Move_Backward();
            if (keyboard[TurnLeftKey].isPressed)
                Move_Left();
            if (keyboard[TurnRightKey].isPressed)
                Move_Right();

            if (keyboard[JumpKey].wasPressedThisFrame && JumpCount < JumpLimit)
            {
                Jump_Up(); 
                JumpCount++;
            }
            else if (keyboard[JumpKey].isPressed)
            {
                Climb_Up(); 
            }

        }

        public void ResetJump()
        {
            JumpCount = 0;
        }

        public abstract void Move_Forward();
        public abstract void Move_Backward();
        public abstract void Move_Left();
        public abstract void Move_Right();
        public abstract void Jump_Up();
        public abstract void Climb_Up();
    }
}

