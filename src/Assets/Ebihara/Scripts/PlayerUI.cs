using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    Change change;
    [SerializeField] TMP_Text text;
    [SerializeField] PlayerDamageEffect damageEffect;
    float oldHp;

    // Start is called before the first frame update
    void Start()
    {
        change = GameObject.FindObjectOfType<Change>();
        oldHp = 0;
    }

    // Update is called once per frame
    void Update()
    {
        text.SetText("HP:{0}", change.CharacterStatusHp);
        Debug.Log("old:"+oldHp+"/hp:"+change.CharacterStatusHp);
        if (oldHp > change.CharacterStatusHp)
        {
            //ƒ_ƒ[ƒW‚ğó‚¯‚½‚É”í’e‚Ìˆ—
            Debug.Log("HP:" + change.CharacterStatusHp);

            damageEffect.DamageEffect();
        }

        oldHp= change.CharacterStatusHp;
    }
}
