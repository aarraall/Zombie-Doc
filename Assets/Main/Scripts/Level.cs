using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] EnemyController[] enemies; public EnemyController[] Enemies => enemies;
}
