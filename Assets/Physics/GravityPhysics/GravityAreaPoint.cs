using System.Collections.Generic;
using UnityEngine;

public class GravityAreaPoint : GravityArea
{
    [SerializeField] private Transform _center;

    
    public override Vector3 GetGravityDirection(GravityBody _gravityBody)
    {
        return (_center.position - _gravityBody.transform.position).normalized;
    }
}
