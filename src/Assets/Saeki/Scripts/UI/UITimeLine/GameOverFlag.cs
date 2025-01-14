using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameOverFlag : MonoBehaviour
{
    [SerializeField] private Change change;
    [SerializeField] private PlayableDirector playableDirector;
    private bool clearCheck;
    public bool IsClearFlag => clearCheck;

    float PlayerHP => change.CharacterStatusHp;
    // Start is called before the first frame update
    void Start()
    {
        clearCheck = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //PlayerのHPが全損しているか判定
        if (!clearCheck && !change.Changing && PlayerHP <= 0)
        {
            Time.timeScale = 0f;
            PlayTimeline();
        }
    }
 
    void PlayTimeline()
    {
        //TimeLine起動
        playableDirector.Play();
        clearCheck = true;
    }
}
