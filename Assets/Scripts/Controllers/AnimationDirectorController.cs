using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDirectorController : MonoBehaviour
{
    public void Moving(Animator animator, float moveValue)
    {
        animator.SetFloat("Moving", moveValue);
    }
}
