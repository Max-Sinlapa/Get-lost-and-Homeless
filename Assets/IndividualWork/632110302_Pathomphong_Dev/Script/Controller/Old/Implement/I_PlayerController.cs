using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Max_DEV.MoveMent
{
    public interface I_PlayerController
    {
        void Move_Forward();
        void Move_Backward();
        void Move_Left();
        void Move_Right();
        void Jump_Up();
        void Climb_Up();
    }

}
