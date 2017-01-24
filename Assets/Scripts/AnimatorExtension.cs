using UnityEngine;

public static class AnimatorExtension
{

	public static bool IsPlaying(this Animator animator)
	{
		return animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
	}
}
