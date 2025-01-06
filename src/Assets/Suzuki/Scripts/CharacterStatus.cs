using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    [SerializeField] CharacterData characterData;
    [Header("プレイヤーが乗り移った際に使用するデータ"), SerializeField] CharacterData playerData;

    float hp;
    float remainPossessTime;
    float damageTimer;
    Animator animator;
    bool possessed;
    bool deadFirstTime;
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
        get { return IsDead || possessed; }
    }
    public string ObjectTag
    {
        get { return gameObject.tag; }
    }
    public Animator CharacterAnimator
    {
        get { return animator; }
    }
    void Start()
    {
        possessed = false;
        deadFirstTime = false;
        StartSetUp();
    }

    virtual protected void Update()
    {
        SearchAnimator();
        if(damageTimer > 0f)
        {
            damageTimer -= Time.deltaTime;
        }
    }

    public void StartSetUp()
    {
        SetHpMax();
        TryGetComponent<Animator>(out animator);
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
                rb.AddForce(Vector3.up * 18f, ForceMode.Impulse);
            }
        }

        if(gameObject.tag == "Player")
        {
            damageTimer = characterData.ImmunityTime;
        }

        if (tag == "Player" && !IsDead) return;// 乗り移ったあとにドラム缶の爆発に当たると立ち上がってしまうため
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
            characterData = playerData;
            SetHpMax();
            possessed = true;
        }

        animator.SetBool("Dead", IsDead);
    }
    public void OnPossessToOther()
    {
        animator.SetBool("Dead", true);
    }
    void SetHpMax()
    {
        hp = characterData.MaxHp;// HPを最大に設定
    }

    void SearchAnimator()
    {
        if (animator != null && animator.gameObject != null &&
            animator.gameObject == gameObject) return;// きちんと設定されている（変更がない）なら再取得しない

        TryGetComponent<Animator>(out animator);
    }
}
