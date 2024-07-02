using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BeseTest
{
    public int A = 2;
    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }
    public virtual void SetUp()
    {
        A = 3;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
