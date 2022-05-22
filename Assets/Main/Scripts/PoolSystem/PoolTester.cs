using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTester : MonoBehaviour
{
    [SerializeField] Ball prefab;

    GenericPool<Ball> ballPool;

    GenericPool<string> stringPool; 

    private void Awake()
    {
        ballPool = new GenericPool<Ball>(
            () => Instantiate(prefab),
            (item) => item.gameObject.SetActive(false),
            (item) => item.gameObject.SetActive(true)); 

        stringPool = new GenericPool<string>(() => "GoodBoy!");
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Ball newBall = ballPool.Get();
        //    newBall.transform.position = Random.insideUnitSphere * 2;
        //}


        if (Input.GetKeyDown(KeyCode.Space))
        {
            ObjectPoolProxy.GetItem("Ball", Random.insideUnitSphere * 2).GetComponent<Ball>();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            string newString = stringPool.Pull();
            print(newString);
        }
    }
}
