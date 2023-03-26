using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using Tree = BehaviorTree.Tree;

namespace Max_DEV.Ai
{
    public class WaitToAttack : Tree
    {
        public float AttackRange = 2f;
        public float FOV_Range = 6f;

        
        public AttackController _AttackController;
    
        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new CheckEnemyInAttackRange(transform, AttackRange),
                    new TaskAttack(transform, _AttackController),
                }),
                new Sequence(new List<Node>
                {
                    new CheckEnemyInFOVRang(transform, FOV_Range),
                }),

            });
            return root;
        }
    }

}
