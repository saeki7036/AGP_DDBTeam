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
    bool possessed;
    public bool CanPossess// 乗り移れるかどうか
    {
        get { return IsDead || possessed; }
    }
    public string ObjectTag
    {
        get { return gameObject.tag; }
    }
    float damageTimer;
    UnityEngine.Animator animator;
    void Start()
    {
        possessed = false;
        StartSetUp();
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
        animator = GetComponent<UnityEngine.Animator>();
        remainPossessTime = characterData.MaxPossessTime;
        damageTimer = 0f;
        if(tag == "Player")
        {
            possessed = true;
        }
    }

    /// <summary>
    /// ダメージを与える
    /// </summary>
    public void TakeDamage(float damage, bool launch = false)
    {
        if (damageTimer > 0f) return;// 無敵時間中はダメージをくらわない
        hp -= damage;
        if(hp <= 0f)
        {
            hp = 0f;
            if(launch && TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.AddForce(Vector3.up * 2f, ForceMode.Impulse);
            }
        }

        if(gameObject.tag == "Player")
        {
            damageTimer = characterData.ImmunityTime;
        }

        animator.SetBool("Dead", IsDead);
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
        if(!possessed)// 初回取り憑きのとき
        {
            SetHpMax();
            possessed = true;
        }

        animator.SetBool("Dead", IsDead);
    }

    void SetHpMax()
    {
        hp = characterData.MaxHp;// HPを最大に設定
    }
}
