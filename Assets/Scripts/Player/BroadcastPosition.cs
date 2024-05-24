using UnityEngine;

public class BroadcastPosition : MonoBehaviour
{
    private void FixedUpdate()
    {
        EventBus<PositionBroadcasted>.Publish(new PositionBroadcasted(transform.position));
    }
}
