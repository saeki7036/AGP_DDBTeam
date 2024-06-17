using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SR_TankMove : MonoBehaviour
{
    // Start is called before the first frame update

    public bool Move = false;
    float Speed = 0;
    float SpeedPP = 0.005f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q)) 
        { 
        Move = true;
        }
        if (Move) 
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z +Speed*Time.deltaTime);
            if (Speed < 0.5)
            {
                Speed -= SpeedPP;
            }

        }
    }
}
