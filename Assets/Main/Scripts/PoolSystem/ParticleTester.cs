using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTester : MonoBehaviour
{
    private void Awake()
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ParticlePoolProxy.GetItem("LevelupFX", Random.insideUnitSphere * 2, Vector3.zero, true);
        }
    }
}
