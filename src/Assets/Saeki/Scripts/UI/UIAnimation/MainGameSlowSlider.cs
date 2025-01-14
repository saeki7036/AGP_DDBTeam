using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MainGameSrowSlider : MonoBehaviour
{
    [SerializeField] private Image SliderUIImage;
    [SerializeField] private RectTransform SliderValueTransform;
    [SerializeField] private Animator animator;
    // Update is called once per frame
    void FixedUpdate()
    {
        //現在のスロウ状態の残り時間を取得
        float SlowValue = TargetManeger.GetSlowValue();
        //アニメーターにパラメータを代入
        animator.SetBool("SliderDirected", SlowValue != 0f);

        //ゲージを変更する
        SetSliderValue(SlowValue);
        SetSliderColor(SlowValue);
    }

    /// <summary>
    /// 現在のゲージの色を指定
    /// </summary>
    /// <param name="Value">残りのゲージ量</param>
    private void SetSliderColor(float Value)
    {
        Color ValueColor = Color.black;
        ValueColor.r = Value;
        ValueColor.g = 1f - Value;
        SliderUIImage.color = ValueColor;
    }

    /// <summary>
    /// 現在のゲージの量を指定
    /// </summary>
    /// <param name="Value">残りのゲージ量</param>
    private void SetSliderValue(float Value)
    {
        Vector3 TransformScale = Vector3.one;
        //スケールを計算
        if (1f - Value > 0f)
        {
            TransformScale.x = 1f - Value;
        }
        else
        {
            //ゲージが無くなった時にゲージを画面外に移動する
            TransformScale.x = 0f;
            animator.SetBool("SliderDirected", false);
        }
        //スケールをを代入
        SliderValueTransform.localScale = TransformScale;
    }
}
