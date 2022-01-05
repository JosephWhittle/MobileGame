using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Obstacle Data", menuName = "Obstacle")]
public class Obstacle : ScriptableObject
{
    //Obstacle sprite
    public Sprite Sprite;

    //Overall sprite size
    public float OverallScale;

    //Size of the Sprite gameobject
    public Vector3 SpriteSize;

    //Size of the collider if the default one isnt correct
    public Vector3 ColliderScale;

    //The Rotation of the obstacle, think of this like a clock face, it will spawn on the surface
    public float Rotation;

    [Header("*Ensure only either collectible OR Dangerious is selected")]
    //If its collectable and not damagable, then just 
    public bool IsCollectable;

    //Tick below if its dangerious
    public bool IsDangerious;
}
