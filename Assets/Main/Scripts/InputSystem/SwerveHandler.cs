using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveHandler : IInputHandler<Vector3>
{
    private float _lastPosX;
    private float _deltaPosX;
    public Vector3 Output => _deltaPosX * Vector3.right;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
            _lastPosX = Input.mousePosition.x;

        if (Input.GetMouseButton(0))
        {
            _deltaPosX = (Input.mousePosition.x - _lastPosX) / Screen.width;
            _lastPosX = Input.mousePosition.x;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _deltaPosX = 0;
        }
    }
}
