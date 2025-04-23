using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class SelectButtonMovement : SelectButtonWindow
{
    [SerializeField] RectTransform[] ButtonTransforms;//ボタンのRectTransform
    [SerializeField] float movementValue = 200f;//ボタンの量

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
            //ボタンの位置を移動させる
            ButtonMovememts(buttonPos - currentButtonIndex);
            //Indexを選択したbuttonPosに移動
            currentButtonIndex = buttonPos;
            //カーソルを選択したbuttonPosに移動
            cursol.transform.localPosition = Buttons[currentButtonIndex].transform.localPosition;    
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
        //ボタンの位置を移動させる
        ButtonMovememts(next);
        //カーソルの移動
        cursol.transform.localPosition = Buttons[currentButtonIndex].transform.localPosition;
        //SE再生
        audioSE?.PlayOneShot(audioSE.clip);
    }

    /// <summary>
    /// ボタンの位置を移動させる
    /// </summary>
    /// <param name="count">移動量</param>
    void ButtonMovememts(int count)
    {
        //ボタンの移動量を計算
        Vector3 movement = AddPositions(count);
        for(int i = 0; i < ButtonTransforms.Length; i++)
        {
            //ボタンの位置を移動
            ButtonTransforms[i].localPosition += movement;
        }
    }

    /// <summary>
    /// 一度に移動する移動量を取得
    /// </summary>
    /// <param name="moveValue">移動量</param>
    /// <returns>一度に移動する移動量</returns>
    Vector3 AddPositions(int moveValue)
    {
        //ボタンの移動量を計算
        Vector3 add = Vector3.one * movementValue * moveValue;
        //z軸を0に
        add.z = 0f;
        //入力方向の設定に応じて一方向を0にする
        if (input == input_or.Vertical)
        {
            add.x = 0f;//縦方向に移動
        }
        else
        {
            add.y = 0f;//横方向に移動
        }
        //最後にマイナスをかける
        add *= -1;
        //計算した値を返す
        return add;
    }
}
