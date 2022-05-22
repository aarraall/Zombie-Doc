using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : HumanoidView
{
    public void OnChase()
    {
        TriggerAnimation(_animationSet.runAnimation);
    }
    public void OnFight()
    {
        TriggerAnimation(_animationSet.fightAnimations[Random.Range(0, _animationSet.fightAnimations.Length)]);
    }

    private void TriggerAnimation(string condition)
    {
        animator.SetTrigger(condition);
    }
}
