using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private bool isCursorOn;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isCursorOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            CursorSwich();
        }
    }

    void CursorSwich()
    {
        if(isCursorOn==false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            isCursorOn = true;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            isCursorOn = false;
        }
    }
}
