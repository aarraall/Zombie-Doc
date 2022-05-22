using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HumanoidView : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    protected AnimationSet _animationSet;
    public virtual void Initialize(AnimationSet animationSet)
    {
        _animationSet = animationSet;
    }
    protected void PlayAnimation(string animName)
    {
        animator.Play(animName);
    }

    protected virtual void Reset()
    {
        TryGetComponent(out animator);
    }

}
