using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MainGameSrowSlider : MonoBehaviour
{
    [SerializeField] private Image SliderUIImage;
    [SerializeField] private RectTransform SliderValueTransform;

    // Update is called once per frame
    void FixedUpdate()
    {
        float SlowValue = TargetManeger.GetSlowValue();
        Debug.Log(SlowValue);
        SetSliderValue(SlowValue);
        SetSliderColor(SlowValue);
    }

    private void SetSliderColor(float Value)
    {
        Color ValueColor = Color.black;
        ValueColor.r = Value;
        ValueColor.g = 1f - Value;
        SliderUIImage.color = ValueColor;
    }

    private void SetSliderValue(float Value)
    {
        Vector3 TransformScale = Vector3.one;
        TransformScale.x = 1f - Value > 0f ? 1f - Value : 0f;
        SliderValueTransform.localScale = TransformScale;
    }
}
