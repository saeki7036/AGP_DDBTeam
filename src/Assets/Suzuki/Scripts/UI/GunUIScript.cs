using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunUIScript : MonoBehaviour
{
    [Tooltip("残弾数のテキスト"),SerializeField] TMP_Text remainBulletText;
    [SerializeField] PlayerMove player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        remainBulletText.SetText("Remain:{0}", player.Gun.RemainBullets);
        if(player.Gun.RemainBullets == 0)
        {
            remainBulletText.color = Color.red;
        }
        else if(remainBulletText.color == Color.red)
        {
            remainBulletText.color = Color.white;
        }
    }
}
