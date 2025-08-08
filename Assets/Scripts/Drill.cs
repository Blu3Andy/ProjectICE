using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Drill : MonoBehaviour
{
    [SerializeField] private LayerMask hittable;
    [SerializeField] private Transform rayStart;

    [SerializeField] private Transform drillHead;

    [SerializeField] private float drillRange = 1f;
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(rayStart.position, transform.up * drillRange);
         
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(rayStart.position, rayStart.TransformDirection(Vector3.up), out hit, drillRange, hittable))
        {
            Debug.DrawRay(rayStart.position, rayStart.up * hit.distance, Color.red);
            
            drillHead.position = hit.point;
        }
        else
        {
            Debug.DrawRay(rayStart.position, rayStart.TransformDirection(Vector3.up) * drillRange, Color.white);
            
        }
    }
}
