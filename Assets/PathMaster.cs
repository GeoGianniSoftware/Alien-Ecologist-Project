using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathMaster : MonoBehaviour
{
    
}
public class PathTicket
{
    public NavMeshAgent agent;
    public Vector3 destination;

    public PathTicket(NavMeshAgent a, Vector3 d) {
        agent = a;
        destination = d;
    }
}
