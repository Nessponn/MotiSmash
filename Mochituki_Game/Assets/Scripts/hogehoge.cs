using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class hogehoge : MonoBehaviour
{
    //シリアル通信受付クラス
    public SerialHandler serialHandler;

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
    private bool WaitSmash;

    void Start()
    {
        //信号を受信したときに、そのメッセージの処理を行う
        serialHandler.OnDataReceived += OnDataReceived;

        
        Debug.Log("displays connected: " + Display.displays.Length);
        // Display.displays[0] は主要デフォルトディスプレイで、常に ON。
        // 追加ディスプレイが可能かを確認し、それぞれをアクティベートします。
        if (Display.displays.Length > 1)
            Display.displays[1].Activate();
        if (Display.displays.Length > 2)
            Display.displays[2].Activate();
        
    }

    void Update()
    {
        //文字列を送信(Arduinoに対して)
        //serialHandler.Write("hogehoge");
        if (!Smashable)
        {
            DOVirtual.DelayedCall(0.3f,() =>
            {

                WaitSmash = true;
            });

        }
        else
        {
            WaitSmash = false;
        }

    }

    //受信した信号(message)に対する処理
    void OnDataReceived(string message)
    {
        var data = message.Split(new string[] { "\n" }, System.StringSplitOptions.None);
       
        try
        {
            //叩いた判定になったら
            //再度叩ける状態で、叩いた場合
            if (data[0] == "x" && Smashable)
            {
                Debug_SmashIcon.text = "NO";

                //餅をついた判定を書く
                if (!GameMaster.Instance.gameFlag)
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
            }else if (WaitSmash)
            {
                GameMaster.Instance.MoreText_Animation(0);
            }

            //数字なら、腕の角度を変える
            int angle = int.Parse(data[0]);
            Debug_angleTx.text = ""+angle;

            /*arm.transform.rotation = Quaternion.Euler(0, 0, -angle * speed + 180);

            if(angle > ReSmash_angle)
            {
                Debug_SmashIcon.text = "OK";
                
                Smashable = true;
            }*/
            //実数値をkagamiへ持っていく
            if (angle > ReSmash_angle && !Smashable)
            {
                Debug_SmashIcon.text = "OK";

                Smashable = true;
                GameMaster.Instance.ReSmash_Assign();
                
            }

            Kagami_BoneManager.Instance.rot_Set(angle,speed);

        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }
}
