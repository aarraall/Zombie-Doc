using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieView : HumanoidView
{
    [SerializeField] SkinnedMeshRenderer meshRenderer;

    public Action<Collider> OnTriggerEnterEvent;
    public override void Initialize(AnimationSet animationSet)
    {
        base.Initialize(animationSet);
    }
    public void OnIdle()
    {
        PlayAnimation(_animationSet.idleAnimation);
    }

    public void OnFollow()
    {
        PlayAnimation(_animationSet.runAnimation);
    }

    public void OnFight()
    {
        TriggerAnimation(_animationSet.fightAnimations[Random.Range(0, _animationSet.fightAnimations.Length)]);
    }

    public void OnChase()
    {
        TriggerAnimation(_animationSet.runAnimation);
    }

    public void SetMaterial(Material material)
    {
        meshRenderer.sharedMaterial = material;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(other);
    }
    private void TriggerAnimation(string condition)
    {
        animator.SetTrigger(condition);
    }

    protected override void Reset()
    {
        base.Reset();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }



}
