using System.Collections;
using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;
using UnityEngine;

public class Vacuum : MonoBehaviour
{
    private List<GameObject> suckPool = new List<GameObject>();
    private HashSet<GameObject> inSuckPool = new HashSet<GameObject>();

    [SerializeField] private Transform vaccumHole;
    [SerializeField] private SphereCollider sphereCollider;

    public void SuckIn()
    {
        print("is vacuuming");
        CheckObjectsInSphere();
        foreach (GameObject rubble in suckPool)
        {
            rubble.transform.position = Vector3.MoveTowards(rubble.transform.position, vaccumHole.position, 0.05f);
        }
    }

    public void stopSuckIn()
    {
        suckPool.Clear();
        inSuckPool.Clear();
    }

    public void ClearObjectFromList(GameObject gone)
    {
        suckPool.Remove(gone);
        inSuckPool.Remove(gone);
    }

    void CheckObjectsInSphere()
    {
        Vector3 worldCenter = sphereCollider.transform.TransformPoint(sphereCollider.center);
        float radius = sphereCollider.radius * sphereCollider.transform.lossyScale.x; 

        Collider[] hits = Physics.OverlapSphere(worldCenter, radius);

        foreach (Collider hit in hits)
        {
            if (hit.gameObject.CompareTag("Ice") && !inSuckPool.Contains(hit.gameObject))
            {
                suckPool.Add(hit.gameObject);
                inSuckPool.Add(hit.gameObject);
            }
        }
    }


}
