using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private GameRunner _gameRunner;
    [SerializeField] private AudioSource _landingSand;

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.gameObject.tag == "Astronaut")
        {
            _landingSand.Play();
            collider2D.GetComponent<Moveable>().IsMoveForward = false;
            Destroy(collider2D.attachedRigidbody);
            collider2D.transform.parent = GameObject.FindGameObjectWithTag("Moon").transform;
            collider2D.gameObject.tag = "AstronautLanded";

            //Handle winning stuff here
            PlayerPrefs.SetInt("MoonCoins", PlayerPrefs.GetInt("MoonCoins") + 1);

            _gameRunner.AstronautCountDown--;
            _gameRunner.AstronautCounter++;

            int value = int.Parse(_gameRunner._astronautsLeftText.text);
            _gameRunner._astronautsLeftText.text = (value - 1).ToString();


        }
    }

}
