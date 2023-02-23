using System;
using System.Collections.Generic;
using BehaviorDesigner;
using UnityEngine;

public class MovementTest : MonoBehaviour
{
    [SerializeField]
    private BehaviorTree[] behaviorTrees;
    [SerializeField]
    private int behaviorIndex;

    private void Awake()
    {
        behaviorTrees = GetComponents<BehaviorTree>();
    }

    private void Update()
    {
        behaviorTrees[behaviorIndex].Tick();
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<-"))
        {
            if (--behaviorIndex < 0)
            {
                behaviorIndex = behaviorTrees.Length - 1;
            }

            behaviorTrees[behaviorIndex].Restart();
        }

        GUILayout.Box(behaviorTrees[behaviorIndex].Source.behaviorName, GUILayout.MinWidth(200f));
        if (GUILayout.Button("->"))
        {
            if (++behaviorIndex >= behaviorTrees.Length)
            {
                behaviorIndex = 0;
            }

            behaviorTrees[behaviorIndex].Restart();
        }

        GUILayout.EndHorizontal();
    }
}