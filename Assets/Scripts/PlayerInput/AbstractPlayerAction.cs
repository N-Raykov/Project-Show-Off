using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPlayerAction : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody rb;
    protected bool isGrounded;
    protected float distToBottomOfSprite;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        if (rb == null)
        {
            throw new System.Exception("There is no Rigidbody component.");
        }

        distToBottomOfSprite = GetComponent<BoxCollider>().bounds.extents.y;
    }

    protected bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToBottomOfSprite + 1f, ~0, QueryTriggerInteraction.Ignore);
    }
}
