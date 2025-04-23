using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameSrowSlider : MonoBehaviour
{
    [SerializeField] private Image SliderUIImage;
    [SerializeField] private RectTransform SliderValueTransform;//SliderのTransform
    [SerializeField] private Animator animator;//SliderUIのanimator
    // Update is called once per frame
    void FixedUpdate()
    {
        //現在のスロウ状態の残り時間を取得
        float SlowValue = TargetManeger.GetSlowValue();
        //アニメーターにパラメータを代入
        animator.SetBool("SliderDirected", SlowValue != 0f);
        //ゲージを変更する
        SetSliderValue(SlowValue);
        //ゲージの色を変更する
        SetSliderColor(SlowValue);
    }

    /// <summary>
    /// 現在のゲージの色を指定
    /// </summary>
    /// <param name="Value">残りのゲージ量から遷移(赤色～緑色)</param>
    private void SetSliderColor(float Value)
    {
        //RGBを0から計算する
        Color ValueColor = Color.black;
        //Rを計算する
        ValueColor.r = Value;
        //Gを1fから反転させ計算する
        ValueColor.g = 1f - Value;
        //UIImageを代入する
        SliderUIImage.color = ValueColor;
    }

    /// <summary>
    /// 現在のゲージの量を指定
    /// </summary>
    /// <param name="Value">残りのゲージ量</param>
    private void SetSliderValue(float Value)
    {
        //スケールを1で初期化してから計算
        Vector3 TransformScale = Vector3.one;
        //xのみ計算する

        //ゲージの残量を判定
        if (1f > Value)
        {
            //スケールのxを計算
            TransformScale.x = 1f - Value;
        }
        else//上限(1f == Value)の時に以下の処理に移動
        {
            //上限の時に(ゲージが無くなった時)にゲージを画面外に移動する
            //xに0を代入
            TransformScale.x = 0f;
            //アニメーターにパラメータを設定
            animator.SetBool("SliderDirected", false);
        }
        //スケールをを代入
        SliderValueTransform.localScale = TransformScale;
    }
}
