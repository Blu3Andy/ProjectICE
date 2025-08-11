using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class blackHole : MonoBehaviour
{
    [SerializeField] private Vacuum vacuum;
    [SerializeField] private TankFiller filler;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ice")
        {
            vacuum.ClearObjectFromList(other.gameObject);
            filler.FillTank(other.gameObject);
            Destroy(other.gameObject);
        }
    }
}
