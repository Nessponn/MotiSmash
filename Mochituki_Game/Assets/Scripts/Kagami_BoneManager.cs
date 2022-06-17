using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kagami_BoneManager : SingletonMonoBehaviourFast<Kagami_BoneManager> 
{
    //マウスまたはシリアルから実数を受信し
    //その値でかがみのボーンをアニメーションさせるスクリプト


    private int rot_num = 0;//機材の実数値
    private float speed;
                       
    public GameObject arm_bone;//実数倍率1.0
    public GameObject body_upper_bone;//実数倍率0.2
    public GameObject body_lower_bone;//実数倍率0.5
    public GameObject neck_bone;//実数倍率0.7

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //追加地の値は、angleが0を基準に考えている

        if(rot_num > 230)
        {
            rot_num = 130;
        }

        if (rot_num < -240)
        {
            rot_num = -140;
        }

        //腕を回転
        arm_bone.transform.rotation = Quaternion.Euler(0, 0, arm_bone.transform.rotation.z - rot_num * speed + 180);
        //上半身を回転
        body_upper_bone.transform.rotation = Quaternion.Euler(0, 0, body_upper_bone.transform.rotation.z - rot_num * speed * 0.5f + 80);
        //下半身を回転
        body_lower_bone.transform.rotation = Quaternion.Euler(0, 0, body_lower_bone.transform.rotation.z - rot_num * speed * 0.2f + 90);
        //首を回転
        neck_bone.transform.rotation = Quaternion.Euler(0, 0, neck_bone.transform.rotation.z - rot_num * speed * 0.1f + 60);
    }

    public void rot_Set(int angle,float speed)
    {
        rot_num = angle;
        this.speed = speed;
    }
}
