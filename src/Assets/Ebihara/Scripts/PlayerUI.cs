using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    Change change;
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
        if (oldHp != change.CharacterStatusHp)
        {
            //ƒ_ƒ[ƒW‚ğó‚¯‚½‚É”í’e‚Ìˆ—
            Debug.Log("HP:" + change.CharacterStatusHp);

            //damageEffect.DamageEffect(change.CharacterStatusHp);
        }
        damageEffect.DamageEffect(change.CharacterStatusHp);

        oldHp = change.CharacterStatusHp;
    }
}
