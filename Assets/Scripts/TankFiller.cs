using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankFiller : MonoBehaviour
{
    [SerializeField] private Material waterMat;

    [SerializeField] private float tankMaxVolume = 30f;
    [SerializeField] private float tankActualVolume = 0f;

    private float materialValue;

    private void Start()
    {
        waterMat = GetComponent<Renderer>().material;
    }


    public void FillTank(GameObject adder)
    {
        if (adder.CompareTag("Ice"))
        {
            tankActualVolume += 2f;
        }
    }

    private void Update()
    {
        updateFillAmount();

    }

    private void updateFillAmount()
    {
        materialValue = tankActualVolume / tankMaxVolume;
        waterMat.SetFloat("_Fill", materialValue);
    }

}
