using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class StartFlag : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] string animText;

    private bool startCheck;

    // Start is called before the first frame update
    void Start()
    {
        //timeScaleを0にしてFixedUpdeteを止める
        Time.timeScale = 0;
        //初期化
        startCheck = false;
    }
   
    // Update is called once per frame
    void Update()
    {
        //左クリックと四方向の移動を取得
        bool MouseLeft = Input.GetMouseButton(0);
        float inputVertical = Input.GetAxisRaw("Vertical");
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
       
        //左クリックと四方向の入力を判定
        if (!startCheck && (inputVertical != 0 || inputHorizontal != 0 || MouseLeft))
        {
            //animationを起動
            AnimationOnePlay();
            //timeScaleを1にしてFixedUpdeteを動かす
            Time.timeScale = 1;

            //TimeLine使用のためお役御免
            //StartCounting();
        }
    }

    void AnimationOnePlay()
    {
        //アニメーション起動
        anim.Play(animText, 0, 1f);
    }

    //TimeLine使用のためお役御免

    /*
    /// <summary>
    /// 一定時間後に非表示に設定
    /// </summary>
    async Task StartCounting()
    {
        if (startCheck)
        {
            Debug.LogWarning("既にカウント中です！");
            return;
        }

        startCheck = true;
       
        float startTime = Time.realtimeSinceStartup;

        // 非同期で待機
        while (Time.realtimeSinceStartup - startTime < 0.2f)
        {
            await Task.Yield(); // 非同期で次のフレームまで待機
        }

        this.gameObject.SetActive(false);
    }*/
}
