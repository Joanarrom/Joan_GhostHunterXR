using System;
using UnityEngine;
using UnityEngine.AI;

public class GhostMove : MonoBehaviour
{
    public NavMeshAgent agent;
    public float speed = 1;
    public Animator animator;
    
    void Update()
    {
        Vector3 destination = Camera.main.transform.position;
        agent.SetDestination(destination);
        agent.speed = speed;
    }

    public void Kill()
    {
        agent.enabled = false;
        animator.SetTrigger("Death");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
