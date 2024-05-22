using UnityEngine;
using DG.Tweening;

public class Collectible : MonoBehaviour
{
    [SerializeField] private RectTransform candyCollectionRectTransform;
    [SerializeField] private int value = 1;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float minimalTargetDist = 0.1f;

    private Camera mainCamera;
    private Rigidbody rb;
    private SpriteRenderer sr;
    private Collider col;
    private Vector3 targetPosition;
    private string playerTag = "Player";
    private bool collectionInProgress;
    private float standardCameraDistance = 10f; //Standard distance between player and camera

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            throw new System.Exception("There is no Rigidbody component.");
        }

        col = GetComponent<Collider>();
        if (col == null)
        {
            throw new System.Exception("There is no Collider component.");
        }

        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            throw new System.Exception("There is no SpriteRenderer component.");
        }

        mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        if (collectionInProgress)
        {
            float currentCameraZDist = mainCamera.transform.position.z - transform.position.z;

            targetPosition = mainCamera.ScreenToWorldPoint(candyCollectionRectTransform.transform.position
                + new Vector3(0, 0, currentCameraZDist));

            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(targetPosition.x, targetPosition.y, mainCamera.transform.position.z - standardCameraDistance),
                speed * Time.fixedDeltaTime);

            float targetDist = (new Vector2(transform.position.x, transform.position.y)
                            - new Vector2(targetPosition.x, targetPosition.y)).magnitude;

            if (targetDist <= minimalTargetDist)
            {
                CollectionFinished();
            }
        }
    }

    private void CollectionStarted()
    {
        sr.sortingOrder = 1;
        col.enabled = false;
        collectionInProgress = true;
        transform.parent = mainCamera.transform;
        EventBus<SoundEffectPlayed>.Publish(new SoundEffectPlayed(SoundEffectType.Collectible));
    }

    private void CollectionFinished()
    {
        EventBus<CollectibleGathered>.Publish(new CollectibleGathered(value));
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
