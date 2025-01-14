using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBaseClass : MonoBehaviour
{
    //[SerializeField] private GameObject Player;
    [SerializeField] private float DestroyIntarval = 10f;
    [SerializeField] private float BulletPower = 60;
    [SerializeField] AudioClip PlayerDamageClip;
    private float DestroyTime = 0;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField] private BulletData bulletData;
    [SerializeField] private Material playerBulletMaterial;
    [SerializeField] private Material enemyBulletMaterial;
    [SerializeField] private GameObject EffectObject;
    [Header("ヒット時のエフェクト"), SerializeField] private ParticleSystem particle;
    [Header("弾が衝突するレイヤー"), SerializeField] private LayerMask hitLayerMask;
    [Header("弾が消滅するレイヤー"), SerializeField] private LayerMask lapseLayerMask;

    SR_SoundController sound => SR_SoundController.instance;

    Vector3 Forward;
    // Start is called before the first frame update
    void Start()
    {      
        if (this.tag == "PlayerBullet")
        {
            Forward = transform.forward;
            GetComponent<MeshRenderer>().material = playerBulletMaterial;
        }
        else
        {
            Forward = TargetManeger.getPlayerObj().transform.position - transform.position + Vector3.up * 0.5f;
            GetComponent<MeshRenderer>().material = enemyBulletMaterial;
            //敵だけエフェクト起動
            EffectObject.SetActive(true);
        }
        Forward = transform.forward;
        Forward.Normalize();
        //弾丸を回転
        Quaternion look = Quaternion.LookRotation(Forward);
        transform.rotation = look * Quaternion.Euler(90, 0, 0);

        Forward *= BulletPower;
        //rb.AddForce(Forward, ForceMode.Impulse);
        //Debug.Log(Forward * BulletPower +""+""+ rb.velocity);
    }

    // Update is called once per frame
    void Update()
    {
        //Timescaleが0でも動くようにUpdateを使用
        DestroyTime += Time.deltaTime;
        //時間経過で消滅
        if (DestroyTime > DestroyIntarval)
        {
            DestroyTime = 0f;
            Destroy(this.gameObject);
        }
        // プレイヤーの弾はスロー中でも飛び方を変えないためTime.unscaledDeltaTimeを使用
        float deltaTime = tag == "PlayerBullet" ? Time.unscaledDeltaTime : Time.deltaTime;
        // 当たり判定の確認(transform.positionで動かしているため)                                                                   
        CheckHit(deltaTime);
        // オブジェクトの移動
        transform.position += Forward * deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        //レイヤーの数値を取得
        int otherLayer = other.gameObject.layer;
        // 衝突したとき
        if (CompareLayer(hitLayerMask, otherLayer))
        {
            //消滅判定
            if (CompareLayer(lapseLayerMask, otherLayer))
                Destroy(this.gameObject);
            // キャラクターに当たったとき
            if (other.TryGetComponent<CharacterStatus>(out CharacterStatus character))
            {
                // 弾のtagと衝突した相手のtagが違うとき
                //（プレイヤーの弾が敵に、敵の弾がプレイヤーに当たったとき）
                if (HitTagCheck(other.tag))
                {
                    if (character.ObjectTag == "Player")
                    {
                        //プレイヤーにダメージ
                        sound.PlaySEOnce(PlayerDamageClip);
                        character.TakeDamage(1f);
                    }
                    else
                    {
                        //敵ににダメージ
                        character.TakeDamage(bulletData.AttackPower, false);
                    }
                    //エフェクト生成
                    Instantiate(particle, transform.position, Quaternion.identity);
                    //オブジェクト破棄
                    Destroy(this.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// 衝突したLayerがLayerMaskに含まれているか確認
    /// </summary>
    /// <param name="layerMask">レイヤーマスク</param>
    /// <param name="layer">レイヤーの番号</param>
    /// <returns>含まれている場合trueを返す</returns>
    private bool CompareLayer(LayerMask layerMask, int layer)
    {
        return ((1 << layer) & layerMask) != 0;
    }

    /// <summary>
    /// Tagの違いによっての衝突を確認
    /// </summary>
    /// <param name="otherTag">タグ名前</param>
    /// <returns>プレイヤーの弾が敵に、敵の弾がプレイヤーの時true</returns>
    private bool HitTagCheck(string otherTag)
    {
        if (this.tag == "PlayerBullet")
            return otherTag != "Player";
        else
            return otherTag != "Enemy";
    }

    /// <summary>
    /// Rayを使った当たり判定の取得
    /// </summary>
    /// <param name="deltaTime">フレーム単位時間</param>
    private void CheckHit(float deltaTime)
    {
        //Rayを飛ばす
        Ray moveCheckRay = new Ray(transform.position/* - Forward.normalized * 0.5f*/, Forward);
        //Hitした情報を格納
        RaycastHit[] hits = Physics.SphereCastAll(moveCheckRay.origin, 0.3f, moveCheckRay.direction, moveCheckRay.direction.magnitude * deltaTime/* + Forward.normalized.magnitude * 0.5f*/, hitLayerMask);
        List<RaycastHit> hitCharacterList = new List<RaycastHit>();

        //キャラクターのHit判定
        foreach (RaycastHit raycastHit in hits)
        {
            //Characterに指定されたTagであるか
            if (CompareLayer(LayerMask.GetMask("Enemy", "Player", "Destructive"), raycastHit.transform.gameObject.layer))
            {
                hitCharacterList.Add(raycastHit);
            }
        }

        //Characterの判定をOntriggerに飛ばす
        foreach (RaycastHit hitCharacter in hitCharacterList)
        {
            OnTriggerEnter(hitCharacter.collider);
        }

        //全体の判定をOntriggerに飛ばす
        foreach (RaycastHit hit in hits)
        {
            OnTriggerEnter(hit.collider);
        }
    }
}
