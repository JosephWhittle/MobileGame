using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    public bool IsMoveForward;

    void Start()
    {
        IsMoveForward = true;
    }

    void FixedUpdate()
    {
        //Eventually make this so it uses gravity/soem sort of gravity like equation for the speed it move towards
        if (IsMoveForward)
        {
            if(_rigidbody2D != null) _rigidbody2D.velocity = Vector2.up * _speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.tag == "Astronaut")
        {
            FindObjectOfType<InGameUI>().ShowUI();

        }
    }

}
