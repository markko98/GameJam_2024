using UnityEngine;

public static class AnimatorExtensions
{
    public static bool IsAnimationPlaying(this Animator animator, string stateName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    } 
}