using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPlayerAction : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody rb;
    protected bool isGrounded;
    protected float distToBottomOfSprite;
    protected float spriteRadius;

    private int _points = 10;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        if (rb == null)
        {
            throw new System.Exception("There is no Rigidbody component.");
        }

        anim = GetComponentInChildren<Animator>();
        if (anim == null)
        {
            throw new System.Exception("There is no Animator component.");
        }

        distToBottomOfSprite = GetComponent<CapsuleCollider>().bounds.extents.y;
        spriteRadius = GetComponent<Collider>().bounds.extents.x;
    }

    protected bool IsGrounded()
    {
        float anglebetween = 360f / _points;

        for (int i = 0; i <= _points; i++)
        {
            float angle = i * anglebetween * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            for (float y = 0; y <= spriteRadius; y++)
            {
                Vector3 position = direction * y;
                Debug.DrawRay(transform.position, direction * y, Color.red);
                Debug.DrawRay(position + transform.position, -Vector3.up * (distToBottomOfSprite + 0.1f), Color.red);
                if (Physics.Raycast(position + transform.position, -Vector3.up, distToBottomOfSprite + 1f, ~0, QueryTriggerInteraction.Ignore))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
