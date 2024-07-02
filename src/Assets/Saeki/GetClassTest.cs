using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetClassTest : MonoBehaviour
{
    [SerializeField]
    public Enemy_BeseTest test;
    // Start is called before the first frame update
    void Start()
    {
        test = new Stay_test();
        FindAnyObjectByType(typeof(Stay_test));
        test.SetUp();
        Debug.Log(test.A);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
