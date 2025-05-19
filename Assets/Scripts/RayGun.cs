using Oculus.Interaction.DebugTree;
using UnityEngine;

public class RayGun : MonoBehaviour
{
    public OVRInput.RawButton shootingButton;
    public LineRenderer linePrefab;
    public Transform shootingPoint;
    public float maxLineDistance = 5;
    public float lineShowTimer = 0.05f;
    public LayerMask layerMask;
    public GameObject rayImpactPrefab;

    void Update()
    {
        if (OVRInput.GetDown(shootingButton))
        {
            //Debug.Log("Shoot");
            Ray ray = new Ray(shootingPoint.position, shootingPoint.forward);
            bool hasHit = Physics.Raycast(ray, out RaycastHit hit, maxLineDistance, layerMask);
            Vector3 endPoint = Vector3.zero;
            
            if (hasHit)
            {
                //stop the ray
                endPoint = hit.point;
                GhostMove ghost = hit.transform.gameObject.GetComponentInParent<GhostMove>();

                if (ghost != null)
                {
                    // Kill the ghost
                    hit.collider.enabled = false;
                    ghost.Kill();
                }
                else
                {
                    Quaternion rayImpactRotation = Quaternion.LookRotation(-hit.normal);
                    GameObject rayImpact = Instantiate(rayImpactPrefab, hit.point, rayImpactRotation);
                    Destroy(rayImpact, 1f);
                }
            }
            else
            {
                endPoint = shootingPoint.position + shootingPoint.forward * maxLineDistance;
            }
            
            LineRenderer line = Instantiate(linePrefab);
            line.positionCount = 2;
            line.SetPosition(0, shootingPoint.position);

            line.SetPosition(1, endPoint);
            Destroy(line.gameObject, lineShowTimer);
        }
    }
}
