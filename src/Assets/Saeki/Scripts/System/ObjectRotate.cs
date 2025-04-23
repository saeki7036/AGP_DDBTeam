using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    [SerializeField] Transform obj;//回転させたいオブジェクト
    [SerializeField] float speed;

    void Start()
    {
        //TimeScaleを起動
        if (Time.timeScale == 0)
            Time.timeScale = 1;
    }
    void FixedUpdate()
    {
        //オブジェクトを回転
        obj.Rotate(Vector3.up * speed);
    }
}
