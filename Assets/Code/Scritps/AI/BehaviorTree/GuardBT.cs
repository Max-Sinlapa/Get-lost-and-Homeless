using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.Events;
using Tree = BehaviorTree.Tree;
using UnityEngine.AI;

namespace Max_DEV.Ai
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class GuardBT : Tree
    {
        public NavMeshAgent EnemyAgent;
        public float waitTime = 1f;
        public float PetrolSpeed = 2f;
        public UnityEngine.Transform[] waypoint;
    
        public float FOV_Range = 6f;
        public float SpeedToTarget = 1f;
        public float AttackRange = 2f;
    
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
                    new TaskGoToTarget(transform,SpeedToTarget, EnemyAgent),
                }),
                new TaskPatrol(transform, waypoint, PetrolSpeed, waitTime, EnemyAgent),
            });
            return root;
        }
    }
}


