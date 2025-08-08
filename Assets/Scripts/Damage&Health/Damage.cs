using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float dmg = 10f;
   
    private void OnTriggerStay(Collider other) //rember the Opfer needs an Rigidbody
    {
        if(other.TryGetComponent(out Health hp)&&Helpers.IsInLayerMask(other.gameObject, enemyLayer))
        {
            hp.TakeDamage(dmg);
        }
    }

     public void SetDmg(float dmg)
    {
        this.dmg = dmg;
    }
}
