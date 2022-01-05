using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollisionHandler : MonoBehaviour
{
    public bool DoesDamage;
    public bool IsCollectible;
    public Obstacle ThisObstacle;


    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        DoesDamage = ThisObstacle.IsDangerious;
        IsCollectible = ThisObstacle.IsCollectable;

        if (collider2D.tag == "Astronaut")
        {
            if (DoesDamage)
            {
                FindObjectOfType<InGameUI>().ShowUI();
                FindObjectOfType<GameRunner>()._crack.Play();
            }

            if (IsCollectible)
            {
                PlayerPrefs.SetInt("MoonCoins", PlayerPrefs.GetInt("MoonCoins") + 5);
                //animate as well
                Destroy(transform.parent.gameObject);
            }

        }
    }
}

