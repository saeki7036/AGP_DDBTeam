using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadManager : MonoBehaviour
{
    [SerializeField] GameObject head;
    
    public void OnHeadThrow()// animator‚©‚çŒÄ‚Ño‚³‚ê‚é
    {
        head.SetActive(false);
        TargetManeger.StartHeadChange();
    }

    public void OnHeadLand()
    {
        head.SetActive(true);
    }
}
