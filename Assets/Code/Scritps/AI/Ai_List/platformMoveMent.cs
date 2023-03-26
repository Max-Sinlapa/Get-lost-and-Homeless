using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using Tree = BehaviorTree.Tree;
using UnityEngine.AI;

public class platformMoveMent : Tree
{
    public NavMeshAgent EnemyAgent;
    public float waitTime = 1f;
    public float StartSpeed;
    public float PetrolSpeed = 2f;
    public UnityEngine.Transform[] waypoint;

    public float _CheckDistant;

    void Start()
    {
        StartSpeed = PetrolSpeed;
        Debug.Log("petroll StartSpeed" + StartSpeed);

    }

    protected override Node SetupTree()
    {
        Debug.Log("petroll");

        Node root = new Selector(new List<Node>
        {
            new TaskPatrol(transform, waypoint,_CheckDistant, PetrolSpeed, waitTime, EnemyAgent),
        });
        return root;
    }
}
