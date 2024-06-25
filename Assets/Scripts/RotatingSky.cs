using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSky : MonoBehaviour
{

    [SerializeField]
    private Material skyMaterial;

    [SerializeField]
    private float startRotation;

    [SerializeField]
    private float rotationSpeed = 1.0f;


    // Update is called once per frame
    void Update()
    {
        startRotation += rotationSpeed * Time.deltaTime;
        skyMaterial.SetFloat("_Rotation", startRotation);

        if (startRotation >= 360) {
            startRotation = 0;
        }
    }
}
