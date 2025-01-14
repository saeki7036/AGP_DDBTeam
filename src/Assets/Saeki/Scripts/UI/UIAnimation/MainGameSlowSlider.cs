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
        //���݂̃X���E��Ԃ̎c�莞�Ԃ��擾
        float SlowValue = TargetManeger.GetSlowValue();
        //�A�j���[�^�[�Ƀp�����[�^����
        animator.SetBool("SliderDirected", SlowValue != 0f);

        //�Q�[�W��ύX����
        SetSliderValue(SlowValue);
        SetSliderColor(SlowValue);
    }

    /// <summary>
    /// ���݂̃Q�[�W�̐F���w��
    /// </summary>
    /// <param name="Value">�c��̃Q�[�W��</param>
    private void SetSliderColor(float Value)
    {
        Color ValueColor = Color.black;
        ValueColor.r = Value;
        ValueColor.g = 1f - Value;
        SliderUIImage.color = ValueColor;
    }

    /// <summary>
    /// ���݂̃Q�[�W�̗ʂ��w��
    /// </summary>
    /// <param name="Value">�c��̃Q�[�W��</param>
    private void SetSliderValue(float Value)
    {
        Vector3 TransformScale = Vector3.one;
        //�X�P�[�����v�Z
        if (1f - Value > 0f)
        {
            TransformScale.x = 1f - Value;
        }
        else
        {
            //�Q�[�W�������Ȃ������ɃQ�[�W����ʊO�Ɉړ�����
            TransformScale.x = 0f;
            animator.SetBool("SliderDirected", false);
        }
        //�X�P�[��������
        SliderValueTransform.localScale = TransformScale;
    }
}
