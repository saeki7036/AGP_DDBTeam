using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameSrowSlider : MonoBehaviour
{
    [SerializeField] private Image SliderUIImage;
    [SerializeField] private RectTransform SliderValueTransform;//Slider��Transform
    [SerializeField] private Animator animator;//SliderUI��animator
    // Update is called once per frame
    void FixedUpdate()
    {
        //���݂̃X���E��Ԃ̎c�莞�Ԃ��擾
        float SlowValue = TargetManeger.GetSlowValue();
        //�A�j���[�^�[�Ƀp�����[�^����
        animator.SetBool("SliderDirected", SlowValue != 0f);
        //�Q�[�W��ύX����
        SetSliderValue(SlowValue);
        //�Q�[�W�̐F��ύX����
        SetSliderColor(SlowValue);
    }

    /// <summary>
    /// ���݂̃Q�[�W�̐F���w��
    /// </summary>
    /// <param name="Value">�c��̃Q�[�W�ʂ���J��(�ԐF�`�ΐF)</param>
    private void SetSliderColor(float Value)
    {
        //RGB��0����v�Z����
        Color ValueColor = Color.black;
        //R���v�Z����
        ValueColor.r = Value;
        //G��1f���甽�]�����v�Z����
        ValueColor.g = 1f - Value;
        //UIImage��������
        SliderUIImage.color = ValueColor;
    }

    /// <summary>
    /// ���݂̃Q�[�W�̗ʂ��w��
    /// </summary>
    /// <param name="Value">�c��̃Q�[�W��</param>
    private void SetSliderValue(float Value)
    {
        //�X�P�[����1�ŏ��������Ă���v�Z
        Vector3 TransformScale = Vector3.one;
        //x�̂݌v�Z����

        //�Q�[�W�̎c�ʂ𔻒�
        if (1f > Value)
        {
            //�X�P�[����x���v�Z
            TransformScale.x = 1f - Value;
        }
        else//���(1f == Value)�̎��Ɉȉ��̏����Ɉړ�
        {
            //����̎���(�Q�[�W�������Ȃ�����)�ɃQ�[�W����ʊO�Ɉړ�����
            //x��0����
            TransformScale.x = 0f;
            //�A�j���[�^�[�Ƀp�����[�^��ݒ�
            animator.SetBool("SliderDirected", false);
        }
        //�X�P�[��������
        SliderValueTransform.localScale = TransformScale;
    }
}
