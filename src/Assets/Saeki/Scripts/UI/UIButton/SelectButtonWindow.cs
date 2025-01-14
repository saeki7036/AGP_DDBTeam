using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;

public class SelectButtonWindow : MonoBehaviour
{
    [SerializeField] protected Button[] Buttons;
    [SerializeField] protected GameObject cursol;
    [SerializeField] protected AudioSource audioSE;
    [SerializeField] protected input_or input = input_or.Vertical;
    [SerializeField] protected int currentButtonIndex = 0;
    [SerializeField] float InputResponseValue = 0.3f;
    [SerializeField] float InputRetryValue = 0.1f;
    protected enum ButtonType
    {
        Normal,
        Up,
        Down
    }
    protected enum input_or
    {
        Vertical,
        Horizontal
    }

    protected ButtonType buttonType = ButtonType.Normal;

    /// <summary>
    /// ボタンのIndexを移動させる
    /// </summary>
    /// <param name="buttonPos">位置</param>
    public virtual void PushOnlyButtonEvent(int buttonPos)
    {
        if (buttonPos < 0 || buttonPos >= Buttons.Length) return;
        if (buttonPos == currentButtonIndex) return;

        audioSE?.PlayOneShot(audioSE.clip);

        if (currentButtonIndex == buttonPos)
        {
            ButtonInvoke();
                return;
        }

        currentButtonIndex = buttonPos;
        cursol.transform.localPosition = Buttons[currentButtonIndex].transform.localPosition;
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
        //ボタンの起動
        EnterButton();
    }

    float GetInputAxis()
    {
        return input == input_or.Vertical ? Input.GetAxisRaw("Vertical") : -Input.GetAxisRaw("Horizontal");
    }

    int GetNextSelect(float value)
    {
        //入力量を判定
        if (Math.Abs(value) < InputResponseValue) return 0;
        //入力できるかを判定
        if (buttonType != ButtonType.Normal) return 0;

        int NextButtonIndex = value < 0 ? 1 : -1;
        //範囲外チェック
        if (NextButtonIndex + currentButtonIndex >= Buttons.Length ||
           NextButtonIndex + currentButtonIndex < 0) return 0;

        return NextButtonIndex;

    }

    protected virtual void SetNextSelect(int next)
    {
        if (next == 0) return;
        //Indexを移動
        currentButtonIndex += next;
        //入力を受け付けの設定
        buttonType = next < 0 ? ButtonType.Up : ButtonType.Down;
        //カーソルの移動
        cursol.transform.localPosition = Buttons[currentButtonIndex].transform.localPosition;
        audioSE?.PlayOneShot(audioSE.clip);
    }

    /// <summary>
    /// 入力状態にを元に戻す
    /// </summary>
    /// <param name="value">入力</param>
    /// <returns>再入力ができるならNormal</returns>
    ButtonType ResetButtonType(float value)
    {
        if (Math.Abs(value) < InputRetryValue)
            if (buttonType != ButtonType.Normal)
                return ButtonType.Normal;

        return buttonType;
    }

    void EnterButton()
    {
        if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown("space"))
        {
            ButtonInvoke();
        }
    }

    protected void ButtonInvoke()
    {
        Buttons[currentButtonIndex].onClick.Invoke();
    }
}
