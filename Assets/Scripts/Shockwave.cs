using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    [SerializeField] int points;
    [SerializeField] float maxRadius;
    [SerializeField] float speed;
    [SerializeField] float startWidth;
    
    private LineRenderer linerender;

    private void Awake()
    {
        linerender = GetComponent<LineRenderer>(); 
        linerender.positionCount = points + 1;
        StartCoroutine(Blast());
    }

    private IEnumerator Blast() 
    {
        float currentRadius = 0f; 
        while(currentRadius < maxRadius)
        {
            currentRadius += Time.deltaTime * speed; 
            Draw(currentRadius);
            Contact(currentRadius);
            yield return null;
        }
        Destroy(this.gameObject);
    }

    private void Draw(float currentRadius)
    {
        float anglebetween = 360f / points;

        for(int i=0; i <= points; i++)
        {
            float angle = i * anglebetween * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            Vector3 position = direction * currentRadius; 

            linerender.SetPosition(i, position); 
        }

        linerender.widthMultiplier = Mathf.Lerp(0f, startWidth, 1f - currentRadius/maxRadius);
    }

    private void Contact(float currentRadius) 
    {
        Collider[] hittingObjects = Physics.OverlapSphere(transform.position, currentRadius);

        for (int i = 0; i < hittingObjects.Length; i++)
        {
            Activateable activateableObject = hittingObjects[i].GetComponent<Activateable>();

            if (!activateableObject)
            {
                continue;
            }

            activateableObject.Activate();
        }
    }
}
