using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    [SerializeField] Transform obj;
    [SerializeField] float speed;
   
    void FixedUpdate()
    {
        obj.Rotate(Vector3.up * speed);
    }
}
