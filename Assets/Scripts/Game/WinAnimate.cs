using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinAnimate : MonoBehaviour
{
    [SerializeField] private Vector3 _fromValue;
    [SerializeField] private Vector3 _toValue;

    [SerializeField] private float _lerpValue;

    void OnEnable()
    {
        transform.localScale = Vector3.Lerp(_fromValue, _toValue, _lerpValue);
    }

}
