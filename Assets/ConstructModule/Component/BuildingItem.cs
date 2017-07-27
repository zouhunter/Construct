using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class BuildingItem : MonoBehaviour
{
    public BuildState state;
    private NavMeshAgent agent;
    private UnityEngine.AI.NavMeshObstacle obstacle;
    private void OnEnable()
    {
        SetBuildState(BuildState.Normal);
    }
    public void SetBuildState(BuildState state)
    {
        this.state = state;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();

        if (state == BuildState.Normal)
        {
            if (agent != null && agent.enabled) agent.enabled = false;

            if (obstacle == null)
            {
                obstacle = gameObject.AddComponent<UnityEngine.AI.NavMeshObstacle>();
            }
            else
            {
                obstacle.enabled = true;
            }
        }
       else if(state == BuildState.Inbuild)
        {
            if (obstacle != null && obstacle.enabled) obstacle.enabled = false;

            if (agent == null)
            {
                agent = gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
            }
            else
            {
                agent.enabled = true;
            }
        }
    }
}

