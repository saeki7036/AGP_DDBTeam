using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    [SerializeField] CharacterData characterData;
    float hp;
    float remainPossessTime;

    // プロパティ
    public float Hp
    {
        get { return hp; }
    }
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
    // Start is called before the first frame update
    public void StartSetUp()
    {
        SetHpMax();
        remainPossessTime = characterData.MaxPossessTime;
    }

    /// <summary>
    /// ダメージを与える
    /// </summary>
    public void TakeDamage(float damage)
    {
        hp -= damage;
        if(hp <= 0f)
        {
            hp = 0f;
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
