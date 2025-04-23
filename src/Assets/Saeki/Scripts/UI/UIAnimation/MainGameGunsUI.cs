using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainGameGunsUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI remainBulletText;
    [SerializeField] Image mainGunImage;
    [SerializeField] Image subGunImage;

    [SerializeField] Animator animator;//銃のUIのアニメーター

    [SerializeField] PlayerMove player;//Playerの移動クラス。GunStatusのアクセスのために取得

    /// <summary>
    /// PlayerからのGunStatusにアクセス
    /// </summary>
    GunStatus playerGunStatus => player.Gun;

    // Start is called before the first frame update
    void Start()
    {
        //SerializeFieldで設定されてない時に起動
        if (player != null) return;
        //PlayerMoveを取得
        player = FindPlayerMove();
    }

    /// <summary>
    /// プレイヤーを探索してフラグ管理に必要な継承を取得
    /// </summary>
    /// <returns>PlayerMove(シーンに単一で存在)</returns>
    PlayerMove FindPlayerMove()
    {
        //Playerのオブジェクトを探索
        GameObject playerObject = GameObject.FindWithTag("Player");
        //PlayerのオブジェクトからTransformを探索
        Transform childTransform = playerObject.transform.Find("perspective");
        //childTransformのnullチェック
        if (childTransform == null) return null;
        //PlayerMoveを取得
        return childTransform.GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //残段数更新
        RemainText();
        //animatorのパラメータ設定
        animator.SetBool("MainGun", playerGunStatus.IsSubWeapon);

        //以下Debugに使用
        //animator.SetBool("ChangeHead", PauseManager.IsSlow);
        //animator.SetBool("SetHead", !PauseManager.IsSlow);
    }

    /// <summary>
    /// 頭を投げたときに発動するUI遷移
    /// </summary>
    public void AnimatorChangeHead()
    {
        //animatorのパラメータ設定
        animator.SetBool("ChangeHead", true);
        animator.SetBool("SetHead", false);
    }

    /// <summary>
    /// 頭がドッキングした時に発動するUI遷移
    /// </summary>
    public void AnimatorSetHead()
    {
        //animatorのパラメータ設定
        animator.SetBool("ChangeHead", false);
        animator.SetBool("MainGun", playerGunStatus.IsSubWeapon);
        animator.SetBool("SetHead", true);
        //ウエポンの画像変更
        GunsImage();
    }

    void GunsImage()
    {
        //サブウエポンの時画像を変更
        if (player.Gun.IsSubWeapon == false)
            mainGunImage.sprite = playerGunStatus.WeaponImage;
    }

    void RemainText() 
    {
        //残りの残段数を表示
        remainBulletText.SetText("{0}", playerGunStatus.RemainBullets);
        //残りの残段数から色を変更
        remainBulletText.color = SetUITextColor(playerGunStatus.RemainBullets);
    }

    Color SetUITextColor(int Bullets)
    {
        if (Bullets == 0)// 残弾が0のとき
        {
            return Color.red;//赤色
        }
        else // 残弾が0から回復したとき（武器切り替え時や乗り移り時）
        {
            return Color.white;//白色
        }
    }
}
