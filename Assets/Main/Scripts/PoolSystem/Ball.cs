using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    private void OnEnable()
    {
        StartCoroutine(DismissCoroutine());
    }

    IEnumerator DismissCoroutine()
    {
        yield return new WaitForSeconds(.5f);

        ObjectPoolProxy.Dismiss(this);
    }
}
