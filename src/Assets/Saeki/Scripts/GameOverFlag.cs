using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameOverFlag : MonoBehaviour
{
    [SerializeField] private Change chenge;
    [SerializeField] private PlayableDirector playableDirector;
    private bool clearCheck;
    public bool IsClearFlag => clearCheck;

    float PlayerHP => chenge.CharacterStatusHp;
    // Start is called before the first frame update
    void Start()
    {
        clearCheck = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!clearCheck && PlayerHP <= 0)
        {
            Time.timeScale = 0f;
            PlayTimeline();
        }
    }
 
    void PlayTimeline()
    {
        playableDirector.Play();
        clearCheck = true;
    }
}
