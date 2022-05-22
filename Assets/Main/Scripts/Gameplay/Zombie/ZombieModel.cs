using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZombieModel
{
    public float followSpeed, followDistance;
    public float nextTargetFindRate, fightingDistance, chaseSpeed;
    public AnimationSet animationSet;
    public Material dummyMat, zombieMat;
}
