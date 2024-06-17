using UnityEngine;

public class ComicAnimationHelper : MonoBehaviour
{
    private Animator animator;
    private string doneParameterName = "isDone";

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnLastFrame()
    {
        if (animator.GetBool(doneParameterName))
        {
            animator.speed = 0;
        }
    }
}
