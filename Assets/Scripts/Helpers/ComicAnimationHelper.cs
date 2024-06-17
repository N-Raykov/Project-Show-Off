using UnityEngine;
using System;

public class ComicAnimationHelper : MonoBehaviour
{
    private Animator animator;
    private string doneParameterName = "isDone";

    public void OnLastFrame()
    {
        if (animator.GetBool(doneParameterName))
        {
            animator.speed = 0;
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            throw new Exception("There is no Animator component.");
        }
    }
}
