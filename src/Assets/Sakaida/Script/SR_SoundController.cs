using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SR_SoundController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject SoundPrefab;

    public float AllSeVolume = 1;

    public static SR_SoundController instance;

    void Start()
    {
        if (instance = null)
        {
            Destroy(this);
        }
        else 
        { 
        instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void isPlaySE(AudioClip Clip) 
    { 
    GameObject CL_SoundPrefab = GameObject.Instantiate(SoundPrefab);
        SR_SoundPlay CL_SR_SoundPlay = CL_SoundPrefab.GetComponent<SR_SoundPlay>();
        CL_SR_SoundPlay.Clip = Clip;
        CL_SR_SoundPlay.Volume = 1*AllSeVolume;
    }
}
