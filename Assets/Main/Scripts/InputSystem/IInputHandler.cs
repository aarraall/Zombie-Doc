using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInputHandler
{

}
public interface IInputHandler<T>
{
    T Output { get; }
    void Update();
}
