using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private RectTransform candyCollectionPoint;
    [SerializeField] private int value = 1;
    [SerializeField] private float collectionFlySpeed = 10;
    [SerializeField] private float minimalDistanceToDestination = 20;

    private Camera mainCamera;
    private Rigidbody rb;
    private SpriteRenderer sr;
    private new Collider collider;
    private Vector3 candyCollectionCenter;
    private string playerTag = "Player";
    private bool collectionInProgress;

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

        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            throw new System.Exception("There is no SpriteRenderer component.");
        }

        mainCamera = Camera.main;
        candyCollectionCenter = candyCollectionPoint.transform.TransformPoint(candyCollectionPoint.rect.center);
    }

    private void FixedUpdate()
    {
        if(name == "CandyPrefabWhite (7)")
            Debug.Log(rb.velocity);

        if (collectionInProgress)
        {
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
        sr.sortingOrder = 1;
        collider.enabled = false;
        collectionInProgress = true;
        rb.useGravity = false;
        rb.velocity = (candyCollectionCenter - mainCamera.WorldToScreenPoint(transform.position)).normalized * collectionFlySpeed;

        Debug.Log(name);
        Debug.Log("candyCollectionCenter: "+candyCollectionCenter);
        Debug.Log("candy: "+ mainCamera.WorldToScreenPoint(transform.position));
        Debug.Log(candyCollectionCenter - mainCamera.WorldToScreenPoint(transform.position));
        Debug.Log((candyCollectionCenter - mainCamera.WorldToScreenPoint(transform.position)).normalized);
        Debug.Log((candyCollectionCenter - mainCamera.WorldToScreenPoint(transform.position)).normalized * collectionFlySpeed);
        Debug.Log("velocity: "+rb.velocity);

        transform.parent = mainCamera.transform;

        Debug.Log("velocity: " + rb.velocity);

    }

    private void CollectionFinished()
    {
        EventBus.TriggerEvent<int>(EventBusEnum.EventName.CollectibleGathered, value);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == playerTag)
        {
            CollectionStarted();
        }
    }
}
