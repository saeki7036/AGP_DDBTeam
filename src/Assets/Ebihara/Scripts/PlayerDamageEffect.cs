using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageEffect : MonoBehaviour
{
    [SerializeField] Image damageImage;
    [SerializeField] int damage;
    int maxDamage;
    float[] damagesAlpha;
    Color color;
    Change change;

    // Start is called before the first frame update
    void Start()
    {
        change= GameObject.FindObjectOfType<Change>();
        color = damageImage.GetComponent<Image>().color;
        color.a = 0f;
        damageImage.GetComponent<Image>().color = color;
        damage = 0;
        maxDamage = 3;
        damagesAlpha =new float[maxDamage];
        for (int i = 0; i < maxDamage; i++) { damagesAlpha[i] = 0.2f * (i + 1); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageEffect()
    {
        if (change.Changing == false)
        {
            Debug.Log("Hit:" + damage);
            if (damage < maxDamage) { color.a = damagesAlpha[damage]; }
            else { color.a = damagesAlpha[maxDamage - 1]; }
            damage++;
            damageImage.GetComponent<Image>().color = color;
        }
    }
    public void Reset()
    {
        damage= 0;
        color.a = 0f;
        damageImage.GetComponent<Image>().color = color;
    }
}
