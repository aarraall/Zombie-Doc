using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerModel
{
    public PlayerView playerPrefab;
    public float forwardSpeed, sideSpeed, syringeThrowRate, syringeThrowSpeed,clampX;
    public int coinIncrement;
    public AnimationSet animationSet;
}
[System.Serializable]
    public struct AnimationSet
    {
        public string[] fightAnimations;
        public string idleAnimation, runAnimation, happyAnimation, sadAnimation;
    }