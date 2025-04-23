using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;

public class SelectButtonWindow : MonoBehaviour
{
    [SerializeField] protected Button[] Buttons;//操作するボタン
    [SerializeField] protected GameObject cursol;//カーソルのオブジェクト
    [SerializeField] protected AudioSource audioSE;//効果音
    [SerializeField] protected input_or input = input_or.Vertical;//インプットの入力方向
    [SerializeField] protected int currentButtonIndex = 0;//最初のIndexの位置

    [SerializeField] float InputResponseValue = 0.3f;//入力のパラメータ
    [SerializeField] float InputRetryValue = 0.1f;//再入力のパラメータ

    //インプットの入力状態
    protected enum ButtonType
    {
        Normal,
        Up,
        Down
    }
    //インプットの入力方向
    protected enum input_or
    {
        Vertical,
        Horizontal
    }

    protected ButtonType buttonType = ButtonType.Normal;

    /// <summary>
    /// ボタンのIndexを移動させる(外部からの入力用)
    /// </summary>
    /// <param name="buttonPos">位置</param>
    public virtual void PushOnlyButtonEvent(int buttonPos)
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
        }
    }

    // Update is called once per frame
    void Update()
    {
        //入力を取得
        float inputValue = GetInputAxis(); Debug.Log(inputValue);
        //入力をintに変換
        int NextSelect = GetNextSelect(inputValue);
        //Indexをに設定
        SetNextSelect(NextSelect);
        //再入力の判定を設定
        buttonType = ResetButtonType(inputValue);
        //ボタンの起動の判定
        EnterButton();
    }

    float GetInputAxis()
    {
        //input_orで横で判定を取るか縦で判定を取るか変更
        return input == input_or.Vertical ? Input.GetAxisRaw("Vertical") : -Input.GetAxisRaw("Horizontal");
    }

    int GetNextSelect(float value)
    {
        //入力量を判定。一定以下ならreturn
        if (Math.Abs(value) < InputResponseValue) return 0;
        //入力できるかを判定。入力がされたままならreturn
        if (buttonType != ButtonType.Normal) return 0;
        //入力量から1or-1に正規化
        int NextButtonIndex = value < 0 ? 1 : -1;
        //範囲外チェック(Buttonの数～0の間の外に出たらreturn)
        if (NextButtonIndex + currentButtonIndex >= Buttons.Length ||
           NextButtonIndex + currentButtonIndex < 0) return 0;
        //入力量から入力値を返す
        return NextButtonIndex;

    }

    protected virtual void SetNextSelect(int next)
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
    }

    /// <summary>
    /// 入力状態にを元に戻す
    /// </summary>
    /// <param name="value">入力</param>
    /// <returns>再入力ができるならNormal</returns>
    ButtonType ResetButtonType(float value)
    {
        //入力量から再入力できるまでの判定
        if (Math.Abs(value) < InputRetryValue)
            if (buttonType != ButtonType.Normal)
                return ButtonType.Normal;//未入力状態に変更
        //元の入力を返す
        return buttonType;
    }

    void EnterButton()
    {
        //ゲームパッドのボタン下もしくはスペースキー
        if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown("space"))
        {
            //Buttonのイベントを実行
            ButtonInvoke();
        }
    }

    protected void ButtonInvoke()
    {
        //Buttonから登録してあるイベントを実行
        Buttons[currentButtonIndex].onClick.Invoke();
    }
}
