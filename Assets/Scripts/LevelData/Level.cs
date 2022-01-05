using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level data", menuName ="New Level")]
public class Level : ScriptableObject
{
    //The amount of items it takes to complete the level
    public int AmountOfItems;

    //the current sprite
    public Sprite Planet;

    //Size of the planet
    public float Size;

    //sprite color
    public Color SpriteHue;

    //List of rotations that it will run through on repeat
    public List<Rotation> Rotations;

    //If the following list is empty then no obstacles, otherwise just add them as you like
    public List<Obstacle> Obstacles;
   


}
