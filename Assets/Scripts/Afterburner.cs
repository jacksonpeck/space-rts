using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Afterburner : MonoBehaviour
{
    [SerializeField] private Fighter _ship;
    [SerializeField] private Vector2 _positionMin;
    [SerializeField] private Vector2 _positionMax;

    // private void FixedUpdate()
    // {
    //     this.transform.localPosition = (Vector3)Vector2.Lerp(_positionMin, _positionMax, _ship.Throttle)
    //         + this.transform.localPosition.z * Vector3.forward;
    // }
}
