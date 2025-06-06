using System.Collections;  
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class GhostMove : MonoBehaviour
{
    public NavMeshAgent agent;
    public float speed = 1f;
    public Animator animator;

    public GameObject GetClosestOrb()
    {
        GameObject closestOrb = null;
        float minDistance = Mathf.Infinity;

        List<GameObject> orbs = OrbSpawner.Instance.spawnedOrbs;

        foreach (var item in orbs)
        {
            float d = Vector3.Distance(item.transform.position, transform.position);

            if (d < minDistance)
            {
                minDistance = d;
                closestOrb = item;
            }
        }

        return closestOrb; 
    }

    void Update()
    {
        if (!agent.enabled)
            return;

        GameObject closest = GetClosestOrb();

        if (closest != null) 
        {
            Vector3 targetPosition = closest.transform.position;
            agent.SetDestination(targetPosition); 
            agent.speed = speed;

            if (Vector3.Distance(transform.position, closest.transform.position) < 0.2f) 
            {
                OrbSpawner.Instance.DestroyOrb(closest);
            }
        }
    }

    public void Kill()
    {
        agent.enabled = false;
        animator.SetTrigger("Die");
        Destroy(gameObject); 
       
      //  StartCoroutine(DelayedCheckWin(0.5f));
    }

  /*  private IEnumerator DelayedCheckWin(float delay)
    {
        yield return new WaitForSeconds(delay);
        GhostSpawner.Instance.CheckForWin();
        Destroy(gameObject);  
    }
*/
    public void DestroyGhost()
    {
        Destroy(gameObject);
    }
}