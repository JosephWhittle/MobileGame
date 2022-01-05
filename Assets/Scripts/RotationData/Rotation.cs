using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Rotation ", menuName = "New Rotation")]
public class Rotation : ScriptableObject
{
    [Header("Linear movement")]
    public int MotorSpeed;
    public int MaximumMotorSpeed;
    public bool RunInfinity;

    [Header("*Add duration ")]
    public int Duration;

    [Header("Ensure you tick the bool below")]
    [Header("Add a target speed below, goes from MotorSpeed to TargetSpeed")]
    public bool AddTargetSpeed;
    public int TargetSpeed;
    public float LerpScalar;


}
