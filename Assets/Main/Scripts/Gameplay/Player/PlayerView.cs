using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerView : HumanoidView
{
    [SerializeField] FieldOfView fov; public FieldOfView FOV => fov;

    public Action<Collider> OnTriggerEnterEvent;

    public override void Initialize(AnimationSet animationSet)
    {
        base.Initialize(animationSet);
        fov.Initialize();
    }
    public void Idle()
    {
        PlayAnimation(_animationSet.idleAnimation);
    }
    public void Move()
    {
        fov.Execute();
        PlayAnimation(_animationSet.runAnimation);
    }
    public void Fight()
    {
        PlayAnimation(_animationSet.fightAnimations[Random.Range(0, _animationSet.fightAnimations.Length)]);
    }
    

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(other);
    }
}
