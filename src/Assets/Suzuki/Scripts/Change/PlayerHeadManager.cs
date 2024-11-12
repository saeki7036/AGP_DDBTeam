using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadManager : MonoBehaviour
{
    [SerializeField] GameObject head;
    
    public void OnHeadThrow()
    {
        head.SetActive(false);
    }

    public void OnHeadLand()
    {
        head.SetActive(true);
    }
}
