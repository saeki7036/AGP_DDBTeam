using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButtonBetween : SelectButtonWindow
{
    [SerializeField] RectTransform LeftIcon;//カーソルの左の位置
    [SerializeField] RectTransform RightIcon;//カーソルの右の位置
    [SerializeField] float IconAdjustmentValue = 20f;//ボタンとカーソルまでの距離

    /// <summary>
    /// ボタンのIndexを移動させる(外部からの入力用)
    /// </summary>
    /// <param name="buttonPos">位置</param>
    public override void PushOnlyButtonEvent(int buttonPos)
    {
        //選択したbuttonPosと現在のボタンの位置が同じならreturn
        if (buttonPos == currentButtonIndex) return;
        //外部入力から範囲外ならreturn
        if (buttonPos < 0 || buttonPos >= Buttons.Length) return;
        //SE再生
        audioSE?.PlayOneShot(audioSE.clip);
        //選択したbuttonPosと現在のボタンの位置が同じなら
        if (currentButtonIndex == buttonPos)
        {
            //Buttonのイベントを実行
            ButtonInvoke();
        }
        else
        {
            //Indexを選択したbuttonPosに移動
            currentButtonIndex = buttonPos;
            //カーソルを選択したbuttonPosに移動
            cursol.transform.localPosition = Buttons[currentButtonIndex].transform.localPosition;
            //カーソルアイコンの処理
            SetIconBetween();
        }  
    }

    protected override void SetNextSelect(int next)
    {
        //入力が0ならreturn
        if (next == 0) return;
        //Indexを移動
        currentButtonIndex += next;
        //入力を受け付けの設定
        buttonType = next < 0 ? ButtonType.Up : ButtonType.Down;
        //カーソルの移動
        cursol.transform.localPosition = Buttons[currentButtonIndex].transform.localPosition;
        //SE再生
        audioSE?.PlayOneShot(audioSE.clip);
        //カーソルアイコンの処理
        SetIconBetween();       
    }

    /// <summary>
    /// Iconの両端の位置を設定
    /// </summary>
    void SetIconBetween()
    {
        //ボタンのRectTransformを取得
        RectTransform button = Buttons[currentButtonIndex].GetComponent<RectTransform>();
        //ボタンのカーソルの位置を計算(buttonの横幅の半分に距離パラメータ[IconAdjustmentValue]を加算)
        Vector3 iconPos = Vector3.right * (button.rect.width/2) + Vector3.right * IconAdjustmentValue;
        //ボタンのカーソルに代入
        RightIcon.localPosition = iconPos;
        //ボタンのカーソルにマイナスを付けて右と逆の位置に代入
        LeftIcon.localPosition = -iconPos;
    }
}
