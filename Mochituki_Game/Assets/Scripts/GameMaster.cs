using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameMaster : SingletonMonoBehaviourFast<GameMaster>
{
    private float t;
    private int time = 15;
    public GameObject NormalCount;
    public GameObject Count;
    private Sequence text;
    [Space]
    public CanvasGroup ResultDisplay;
    private Sequence ResultJob;

    [Space]
    [HideInInspector]public bool gameFlag;
    [HideInInspector] public bool resultFlag;//リザルト表示中はこんな感じ

    [Space]
    //テキスト、UI(実戦使用)
    public Text SmashStart_Text;
    private Sequence tx1;

    [Space]
    public Text MoreSmash_Text;
    private Sequence tx2;

    [Space]
    private int Count_00;
    private int Count_10;
    private int Result_00;
    private int Result_10;

    public Text Count_00Tx;
    public Text Count_10Tx;
    private Sequence CountJob00;
    private Sequence CountJob10;
    public GameObject Count_00_obj;
    public GameObject Count_10_obj;
    //アニメーションの移動先
    public GameObject To_Count_00_obj;
    public GameObject To_Count_10_obj;
    public GameObject To_Count_00_obj_Start;
    public GameObject To_Count_10_obj_Start;

    [Space]
    public Text[] Resulttx;

    [Space]

    public Animator Motti_Anim;

    public Image Flash;

    public GameObject motii1;
    public GameObject motii2;


    // Start is called before the first frame update
    void Start()
    {
        tx1 = DOTween.Sequence();//DOTweenアニメーション使用のために必ず記述

        //UIのアニメーション
        StartingText_Animation(0);

        //DOTweenアニメーションセットアップ
        text = DOTween.Sequence();
        ResultJob = DOTween.Sequence();
        CountJob00 = DOTween.Sequence();
        CountJob10 = DOTween.Sequence();

        text = text.Append(Count.transform.DOScale(new Vector3(3, 3, 3), 0.5f))
            .Join(Count.transform.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, 710), 0.3f, RotateMode.WorldAxisAdd))
            .Append(Count.transform.DOScale(new Vector3(6, 6, 6), 0.5f))
            .Join(Count.GetComponent<Text>().DOFade(0, 0.3f))
            .SetAutoKill(false);

        ResultJob = ResultJob.Append(ResultDisplay.GetComponent<RectTransform>().DOMoveY(0, 2))
            .SetDelay(1)
            .SetEase(Ease.InBounce, 4)
            .SetAutoKill(false);

        CountJob00 = CountJob00.Append(Count_00Tx.transform.GetComponent<RectTransform>().DOAnchorPosY(50f, 0))
                   .Append(Count_00Tx.transform.GetComponent<RectTransform>().DOAnchorPosY(0f, 0.2f))
                   .SetAutoKill(false);

        CountJob10 = CountJob10.Append(Count_10Tx.transform.GetComponent<RectTransform>().DOAnchorPosY(50f, 0))
                   .Append(Count_10Tx.transform.GetComponent<RectTransform>().DOAnchorPosY(0f, 0.2f))
                   .SetAutoKill(false);

        ResultJob.Pause();
        // Display.displays[0] は主要デフォルトディスプレイで、常に ON。
        // 追加ディスプレイが可能かを確認し、それぞれをアクティベートします。
        if (Display.displays.Length > 1)
            Display.displays[1].Activate();
        if (Display.displays.Length > 2)
            Display.displays[2].Activate();


    }

    //振り上げてたたくことを促す
    //0…true
    //1…false
    public void Warning_Smash(int num)
    {
        if(num == 0)
        {

        }
    }

    public void Waiting()
    {
        //おもちをたたいてスタート！の奴を再表示
        SmashStart_Text.gameObject.SetActive(true);
        ResultJob.Join(ResultDisplay.GetComponent<RectTransform>().DOAnchorPosY(600, 1f))
            .SetEase(Ease.InBounce, 4)
            .SetAutoKill(false);
    }

    public void ReSmash_Assign()
    {
        if(gameFlag) Motti_Anim.SetTrigger("Smash");


        
    }

    //待機状態でおもちがつかれた時はここを参照する
    public void GameStart()
    {
        if (resultFlag) return;
        SmashStart_Text.gameObject.SetActive(false);
        NormalCount.gameObject.SetActive(true);

        

        if (gameFlag) return;//ゲーム開始中はここから先は走らせない
        ResultJob.Join(ResultDisplay.GetComponent<RectTransform>().DOAnchorPosY(600, 0.5f))
            .SetEase(Ease.InBounce, 4)
            .SetAutoKill(false);

        //カウントリセット
        Count_00 = 0;
        Count_10 = 0;
        Count_10Tx.text = "" + Count_10;

        Count_00Tx.text = "" + Count_00;


        time = 15;

        ResultJob.Join(Count_00_obj.GetComponent<RectTransform>().DOAnchorPos(To_Count_00_obj_Start.GetComponent<RectTransform>().anchoredPosition, 0f))
                    .Join(Count_10_obj.GetComponent<RectTransform>().DOAnchorPos(To_Count_10_obj_Start.GetComponent<RectTransform>().anchoredPosition, 0f))
                    .Join(Count_00_obj.GetComponent<RectTransform>().DORotateQuaternion(To_Count_00_obj_Start.GetComponent<RectTransform>().rotation, 0f))
                    .Join(Count_10_obj.GetComponent<RectTransform>().DORotateQuaternion(To_Count_10_obj_Start.GetComponent<RectTransform>().rotation, 0f));

        motii1.SetActive(false);
        motii2.SetActive(true);

        Resulttx[2].gameObject.SetActive(false);
        Resulttx[3].gameObject.SetActive(false);
        Resulttx[4].gameObject.SetActive(false);

        gameFlag = true;//ゲーム開始
    }

    public void GameOver()
    {
        gameFlag = false;//ゲーム終了
        resultFlag = true;//リザルト開始

        MoreSmash_Text.gameObject.SetActive(true);

        Count.GetComponent<Text>().fontSize = 40;
        DOVirtual.DelayedCall(1f, () =>
        {
            Count.GetComponent<Text>().fontSize = 80;
            Count.gameObject.SetActive(false);
        });

        Result();
    }

    //GameOver処理から１秒後辺りに実行
    public void Result()
    {
        Debug.Log("走った");
        ResultJob.Restart();

        DOVirtual.DelayedCall(2f,() =>
        {
            Resulttx[0].gameObject.SetActive(true);

            ResultJob.Join(Count_00_obj.GetComponent<RectTransform>().DOAnchorPos(To_Count_00_obj.GetComponent<RectTransform>().anchoredPosition, 2f))
                    .Join(Count_10_obj.GetComponent<RectTransform>().DOAnchorPos(To_Count_10_obj.GetComponent<RectTransform>().anchoredPosition, 2f))
                    .Join(Count_00_obj.GetComponent<RectTransform>().DORotateQuaternion(To_Count_00_obj.GetComponent<RectTransform>().rotation, 2f))
                    .Join(Count_10_obj.GetComponent<RectTransform>().DORotateQuaternion(To_Count_10_obj.GetComponent<RectTransform>().rotation, 2f))
                    
                    ;


            motii1.SetActive(true);
            motii2.SetActive(false);
        });
        DOVirtual.DelayedCall(4f, () =>
        {
            //Resulttx[1].gameObject.SetActive(true);

            Flash.gameObject.SetActive(true);
            ResultJob.Join(Flash.DOFade(1, 0f));
            ResultJob.Join(Flash.DOFade(0, 1f));
            Resulttx[2].gameObject.SetActive(true);
            Resulttx[3].gameObject.SetActive(true);

            if(Count_10 >= 1)
            {
                Resulttx[2].GetComponent<Text>().fontSize = 120;
                Resulttx[2].GetComponent<Text>().text = "GREAT!"; 
            }
            else if (Count_00 >= 7)
            {
                Resulttx[2].GetComponent<Text>().fontSize = 100;
                Resulttx[2].GetComponent<Text>().text = "GoodJob!";
            }
            else if (Count_00 <= 3)
            {
                Resulttx[2].GetComponent<Text>().fontSize = 60;
                Resulttx[2].GetComponent<Text>().text = "MORE SMASH!";
            }

            

            Count_10Tx.text = "" + Count_10;

            Count_00Tx.text = "" + Count_00;
            resultFlag = false;
        });
        DOVirtual.DelayedCall(5f, () =>
        {
            Flash.gameObject.SetActive(false);

            Resulttx[4].gameObject.SetActive(true);
            //カウントダウン開始
            Resulttx[4].GetComponent<Text>().text = "5";
        });

        DOVirtual.DelayedCall(6f, () =>
        {
           
            Resulttx[4].GetComponent<Text>().text = "4";
        });

        DOVirtual.DelayedCall(7f, () =>
        {

            Resulttx[4].GetComponent<Text>().text = "3";
        });

        DOVirtual.DelayedCall(8f, () =>
        {

            Resulttx[4].GetComponent<Text>().text = "2";
        });

        DOVirtual.DelayedCall(9f, () =>
        {

            Resulttx[4].GetComponent<Text>().text = "1";
        });

        DOVirtual.DelayedCall(10f, () =>
        {

            Resulttx[4].GetComponent<Text>().text = "0";
        });

        DOVirtual.DelayedCall(11f, () =>
        {

            Waiting();
        });
    }

    // Update is called once per frame
    void Update()
    {
        //ゲームが始まっていない（餅が叩かれていない）場合はそもそも走らせない
        if (resultFlag)
        {
            Result_00++;
            Result_10++;
            if (Result_00 == 10) Result_00 = 0;
            if (Result_10 == 10) Result_10 = 1;

            Count_10Tx.text = "" + Result_10;
       
            Count_00Tx.text = "" + Result_00;

            SmashStart_Text.gameObject.SetActive(false);
            MoreSmash_Text.gameObject.SetActive(false);
            return;
        }

        if (!gameFlag) return;

        if (time == 0) return;//タイマーがゼロになったら行う処理 

        t += Time.deltaTime;


        //ここからカウントダウン処理
        if (time > 10)
        {
            NormalCount_Down();
        }
        else
        {
            Count_Down();
        }

    }
    void NormalCount_Down()
    {
        if (t >= 1f)
        {
            //Count.SetActive(false);
            t = 0;
            time--;
            NormalCount.GetComponent<Text>().text = "" + time;
        }
    }
    void Count_Down()
    {
        if (time == 10f)
        {
            text.Play();
            Count.SetActive(true);

            NormalCount.SetActive(false);
        }
        if (t >= 1f)
        {
            t = 0;
            time--;
            Count.GetComponent<Text>().text = "" + time;
            if (time <= 10)
            {
                Count.SetActive(true);

                NormalCount.SetActive(false);
                text.Restart();
            }
            //ゲーム終了時の処理
            if(time == 0)
            {
                Count.GetComponent<Text>().text = "Time Up";
                GameOver();
            }
        }


        if (Input.GetKeyDown(KeyCode.K))
        {
            //再生中に再生命令が来た場合、リスタート
            text.Restart();
        }
    }

    public void Smash_ScoreAdd()
    {
        if (!gameFlag) return;

        SmashStart_Text.gameObject.SetActive(false);
        MoreSmash_Text.gameObject.SetActive(false);

        Count_00++;
        if(Count_00 == 10)
        {
            Count_00 = 0;
            Count_10++;

            CountJob10.Restart();
            Count_10Tx.text = "" + Count_10;
        }

        CountJob00.Restart();
        Count_00Tx.text = "" + Count_00;
    }

    //待機アニメーション...0
    //フェードアウトアニメーション...１
    public void StartingText_Animation(int num)
    {
        if (num == 0)
        {
            tx1 = tx1.Append(SmashStart_Text.DOFade(0.7f, 1))//フェードアウト
                .Join(SmashStart_Text.GetComponent<RectTransform>().DOScaleX(0.7f, 3f))
                .Join(SmashStart_Text.GetComponent<RectTransform>().DOScaleY(0.7f, 3f))
                .Append(SmashStart_Text.DOFade(1, 1))//フェードイン
                .Join(SmashStart_Text.GetComponent<RectTransform>().DOScaleX(0.8f, 6f))
                .Join(SmashStart_Text.GetComponent<RectTransform>().DOScaleY(0.8f, 6f))
                 .SetLoops(-1)//ループ
                .SetAutoKill(false);

        }
        else if (num == 1)
        {
            tx1 = tx1.Join(SmashStart_Text.GetComponent<RectTransform>().DOScaleZ(0.3f, 0.5f))
                .Join(SmashStart_Text.DOFade(0f, 0.4f))//フェードアウト
                .SetAutoKill(false);

            DOVirtual.DelayedCall(0.4f, () => SmashStart_Text.gameObject.SetActive(false));
        }

        tx1.Play();
    }

    public void MoreText_Animation(int num)
    {
        if (num == 0)
        {
            MoreSmash_Text.gameObject.SetActive(true);
            tx2 = tx2.Append(MoreSmash_Text.DOFade(0.7f, 1))//フェードアウト
                .Join(MoreSmash_Text.GetComponent<RectTransform>().DOScaleX(0.7f, 3f))
                .Join(MoreSmash_Text.GetComponent<RectTransform>().DOScaleY(0.7f, 3f))
                .Append(MoreSmash_Text.DOFade(1, 1))//フェードイン
                .Join(MoreSmash_Text.GetComponent<RectTransform>().DOScaleX(0.8f, 6f))
                .Join(MoreSmash_Text.GetComponent<RectTransform>().DOScaleY(0.8f, 6f))
                .SetLoops(-1)//ループ
                .SetAutoKill(false);

        }
        else if (num == 1)
        {
            MoreSmash_Text.gameObject.SetActive(false);
        }

        tx2.Play();
    }
}
