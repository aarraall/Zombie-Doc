using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syringe : MonoBehaviour
{
    private Transform _senderTransform; public Transform SenderTransform => _senderTransform;

    float _throwCounter = 0;

    private void OnEnable()
    {
        _throwCounter = 0;
    }
    public void Throw(Transform senderTransform, Transform targetTransform, float speed)
    {
        _senderTransform = senderTransform;

        transform.position = senderTransform.position;
        Vector3 direction = (targetTransform.position - senderTransform.position).normalized;
        transform.forward = direction;

        StartCoroutine(ThrowCoroutine());

        IEnumerator ThrowCoroutine()
        {
            while (gameObject.activeInHierarchy)
            {
                transform.position += Time.deltaTime * speed * direction;
                yield return null;

                _throwCounter += Time.deltaTime;

                if (_throwCounter > 3)
                {
                    ObjectPoolProxy.Dismiss(this);
                    yield break;
                }
            }
        }
    }
}
