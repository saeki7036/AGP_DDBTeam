using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppeaaranceChangeScript : MonoBehaviour
{
    // 敵のデータでマテリアルを設定するかも、ゆくゆくはアニメーションを取ってくる
    [SerializeField] Material possessableMaterial;
    [SerializeField] Material deadMaterial;
    
    public void ChangeMaterialToPossessable(GameObject gameObject)
    {
        gameObject.GetComponent<MeshRenderer>().material = possessableMaterial;
    }
    public void ChangeMaterialToDead(GameObject gameObject)
    {
        gameObject.GetComponent<MeshRenderer>().material = deadMaterial;
    }
}
