using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    [SerializeField] CharacterData characterData;

    float hp;
    public float Hp
    {
        get { return hp; }
    }

    float remainPossessTime;
    public float RemainPossessTime
    {
        get { return remainPossessTime; }
    }

    public bool IsDead
    {
        get { return hp <= 0; }
    }
    public bool CanPossess// 乗り移れるかどうか
    {
        get { return IsDead && remainPossessTime > 0; }
    }
    public string ObjectTag
    {
        get { return gameObject.tag; }
    }
    float damageTimer;

    void Start()
    {
        //damageTimer = 0f;
    }

    void FixedUpdate()
    {
        if(damageTimer > 0f)
        {
            damageTimer -= Time.deltaTime;
        }
    }

    public void StartSetUp()
    {
        SetHpMax();
        remainPossessTime = characterData.MaxPossessTime;
        damageTimer = 0f;
    }

    /// <summary>
    /// ダメージを与える
    /// </summary>
    public void TakeDamage(float damage)
    {
        if (damageTimer >= 0f) return;// 無敵時間中はダメージをくらわない
        hp -= damage;
        if(hp <= 0f)
        {
            hp = 0f;
        }

        if(gameObject.tag == "Player")
        {
            damageTimer = characterData.ImmunityTime;
        }
    }

    /// <summary>
    /// 取り憑き時間を経過
    /// </summary>
    public void ElapsePossessTime()
    {
        remainPossessTime -= Time.fixedDeltaTime;
        if(remainPossessTime <= 0f)
        {
            remainPossessTime = 0f;
        }
    }

    public void OnPossess()// 取り憑き時の処理
    {
        SetHpMax();
    }

    void SetHpMax()
    {
        hp = characterData.MaxHp;// HPを最大に設定
    }
}
