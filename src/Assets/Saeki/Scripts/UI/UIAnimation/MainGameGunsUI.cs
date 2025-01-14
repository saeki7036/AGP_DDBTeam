using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainGameGunsUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI remainBulletText;
    [SerializeField] Image mainGunImage, subGunImage;
    [SerializeField] Animator animator;
    [SerializeField] PlayerMove player;
    bool IsSubGun;
    // Start is called before the first frame update
    void Start()
    {
        IsSubGun = false;
        //SerializeFieldで設定されてない時に起動
        if (player != null) return;

        player = FindPlayerMove();
    }
    /// <summary>
    /// プレイヤーを探索してフラグ管理に必要な継承を取得
    /// </summary>
    /// <returns>PlayerMove(シーンに単一で存在)</returns>
    PlayerMove FindPlayerMove()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        Transform childTransform = playerObject.transform.Find("perspective");
        if (childTransform == null) return null;
        return childTransform.GetComponent<PlayerMove>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        RemainText();
        animator.SetBool("MainGun", player.Gun.IsSubWeapon);
        //animator.SetBool("ChangeHead", PauseManager.IsSlow);
        //animator.SetBool("SetHead", !PauseManager.IsSlow);
    }

    /// <summary>
    /// 頭を投げたときに発動するUI遷移
    /// </summary>
    public void AnimatorChangeHead()
    {
        animator.SetBool("ChangeHead", true);
        animator.SetBool("SetHead", false);
    }

    /// <summary>
    /// 頭がドッキングした時に発動するUI遷移
    /// </summary>
    public void AnimatorSetHead()
    {
        animator.SetBool("ChangeHead", false);
        animator.SetBool("MainGun", player.Gun.IsSubWeapon);
        animator.SetBool("SetHead", true);

        GunsImage();
    }
    void GunsImage()
    {
        //サブウエポンの時画像を変更
        if (player.Gun.IsSubWeapon == false)
            mainGunImage.sprite = player.Gun.WeaponImage;
    }

    void RemainText() 
    {
        //残りの残段数を表示
        remainBulletText.SetText("{0}", player.Gun.RemainBullets);
        remainBulletText.color = SetUITextColor(player.Gun.RemainBullets);
    }

    Color SetUITextColor(int Bullets)
    {
        if (Bullets == 0)// 残弾が0のとき
        {
            return Color.red;
        }
        else // 残弾が0から回復したとき（武器切り替え時や乗り移り時）
        {
            return Color.white;
        }
    }
}
