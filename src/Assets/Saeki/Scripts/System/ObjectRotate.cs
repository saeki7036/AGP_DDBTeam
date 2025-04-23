using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    [SerializeField] Transform obj;//��]���������I�u�W�F�N�g
    [SerializeField] float speed;

    void Start()
    {
        //TimeScale���N��
        if (Time.timeScale == 0)
            Time.timeScale = 1;
    }
    void FixedUpdate()
    {
        //�I�u�W�F�N�g����]
        obj.Rotate(Vector3.up * speed);
    }
}
