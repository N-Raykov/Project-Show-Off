using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleGathering : MonoBehaviour
{
    [SerializeField] private RectTransform candyCollectionPoint;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float collectionFlySpeed = 10;
    [SerializeField] private float minimalDistance = 20;

    private Camera mainCamera;
    private Rigidbody rb;
    private Collider collider;
    private bool collectionInProgress;
    private Vector3 candyCollectionCenter;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            throw new System.Exception("There is no Rigidbody component.");
        }

        collider = GetComponent<Collider>();
        if (collider == null)
        {
            throw new System.Exception("There is no Collider component.");
        }

        mainCamera = Camera.main;
        candyCollectionCenter = candyCollectionPoint.transform.TransformPoint(candyCollectionPoint.rect.center);
    }

    private void FixedUpdate()
    {
        if (collectionInProgress)
        {
            //TODO Lerp scale

            Vector3 screenSpacePos = mainCamera.WorldToScreenPoint(transform.position);
            float distance = (screenSpacePos - candyCollectionCenter).magnitude;

            if (distance <= 20)
            {
                CollectionFinished();
            }
        }

    }

    private void CollectionStarted()
    {
        collider.enabled = false;
        collectionInProgress = true;
        rb.useGravity = false;
        rb.velocity = (candyCollectionCenter - mainCamera.WorldToScreenPoint(transform.position)).normalized * collectionFlySpeed;
        transform.parent = mainCamera.transform;
    }

    private void CollectionFinished()
    {
        //Publish collectible gathered -> candyCount++ && pop bigger effect
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == playerTag)
        {
            CollectionStarted();
        }
    }
}
