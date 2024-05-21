using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastPosition : MonoBehaviour
{
    private void FixedUpdate()
    {
        EventBus<PositionBroadcasted>.Publish(new PositionBroadcasted(transform.position));
    }
}
