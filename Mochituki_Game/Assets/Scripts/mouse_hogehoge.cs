using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class mouse_hogehoge : MonoBehaviour
{
    //マウスのみでのテストプレイ


    //オブジェクト
    public GameObject arm;

    //テキスト、UI
    public Text Score_Tx;
    public Text Debug_SmashIcon;
    public Text Debug_angleTx;


    //変数
    private int Score = 0;
    public float speed = 0.8f;
    public int ReSmash_angle;//再び叩くために必要な角度
    public int SmashOk_angle;//叩いた判定にするために必要な角度
    public bool Smashable;//叩判定になるかどうか

    void Start()
    {
        
    }

    

    void Update()
    {
        
        Vector3 data = Input.mousePosition;

        //文字列を送信(Arduinoに対して)
        //serialHandler.Write("hogehoge");
        //叩いた判定になったら
        //再度叩ける状態で、叩いた場合(設置した感圧版から反応があった時)
        if (Input.GetKeyDown(KeyCode.Space) && Smashable)
        {
            GameMaster.Instance.StartingText_Animation(1);

            Debug_SmashIcon.text = "NO";

            //餅をついた判定を書く
            if(!GameMaster.Instance.gameFlag)
            GameMaster.Instance.GameStart();

            //スコアを増やし、反映する
            Score++;
            Score_Tx.text = "" + Score;

            Smashable = false;//ハンマーでの連打による点数稼ぎは行えないようにする

            GameMaster.Instance.Smash_ScoreAdd();
            SmashEffect.Instance.Smash();

            //////
            ///
            ///振り上げた状態で叩いた判定になるときがあるバグ修正はフラグ管理の最適化を行ってから実装する
            ///
            //////


            return;
        }

        //数字なら、腕の角度を変える
        int angle = (int)data.y;
        Debug_angleTx.text = "" + angle;

        /* arm.transform.rotation = Quaternion.Euler(0, 0, -angle * speed + 180);
        */
        if (angle > ReSmash_angle && !Smashable)
         {
             Debug_SmashIcon.text = "OK";

             Smashable = true;
            GameMaster.Instance.ReSmash_Assign();
         }

        Kagami_BoneManager.Instance.rot_Set(angle,speed);
    }

}
