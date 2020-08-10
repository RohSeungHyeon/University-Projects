using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public Sprite[] bulletSprites;
    public Sprite[] enemySprites;
    public AudioClip[] gunfireEffects;
    private UI_Control ui;
    private EnemySpawn enemySpawn;
    
    public int maxStage;
    private int stage = 1;

    public float requiredScoreRate;
    public int requiredScoreForNext;
    public int[] enemyRequiredStage;

    public int[] scorePerType;
    private int earnedScore = 0;// 점수 누적치. 혹시나 하는 것이지만 점수가 오버플로우 될 때를 대비해야할 수도 있다.
    private int currentStageScore = 0;

    public int[] shootCount;
    private float baseShootCount;
    public float bulletCountIncreasingRate;

    public int shotgunOneShotLimit;

    public float baseBulletSpeed;
    public float[] speedRatePerBulletType; // 이 정보를 Data에 넣을것인가? 아니면 이 정보를 다른곳에 둘것인가?
                                           // 그것의 판단 기준은 메모리 우선인가, 연산 우선인가로 나뉠 것이다
    public float bulletSpeedIncreasingRate;

    public float fireDelayBase;
    public float fireDelayChangingRatePerStage;
    public float[] fireDelayRatePerType;
    
    public int enableFakerStage;
    public int[] fakeChancePerType;
    private bool isFakeStage;

    public int intervalDecreasingStartStage;
    public float intervalChangingRate;

    //Special
    public float sniperWaitDelay;

    void Start()
    {
        ui = GameObject.Find("Canvas").GetComponent<UI_Control>();
        DontDestroyOnLoad(this);
        //김형준 추가:씬 옮길때 기존 값 사라지지 않도록 변경

        baseShootCount = 1.0f;
        enemySpawn = GameObject.Find("Background").GetComponent<EnemySpawn>();
    }

    public float getBulletBaseCount()
    {
        return baseShootCount;
    }

    public void PlusScore(int type)
    {
        currentStageScore += scorePerType[type];

        if (stage < maxStage)
        {
            while (currentStageScore > requiredScoreForNext && stage < maxStage)
            {
                ui.StageDisplay.text = "Stage:" + ++stage;
                if (stage >= enableFakerStage) isFakeStage = true;

                if (stage > intervalDecreasingStartStage)
                {
                    baseShootCount *= bulletCountIncreasingRate;
                    baseBulletSpeed *= bulletSpeedIncreasingRate;
                }
                else
                    enemySpawn.interval *= intervalChangingRate;

                earnedScore += requiredScoreForNext;
                currentStageScore -= requiredScoreForNext;
                requiredScoreForNext = (int)(requiredScoreForNext * requiredScoreRate);

                fireDelayBase *= fireDelayChangingRatePerStage;
                //sniperWaitDelay *= fireDelayChangingRatePerStage; // 변경의 여지가 있음
                
                
                
                for (int i = enemyRequiredStage.Length - 1; i > 0; --i)
                {
                    if (stage >= enemyRequiredStage[i])
                    {
                        enemySpawn.EnemyTypeLimit = i;
                        break;
                    }
                }
            }
            if(stage >= maxStage) ui.StageDisplay.text = "Stage:MAX";
        }
    }
    public bool getFakeEnabled()
    {
        return isFakeStage;
    }

    public int getShootCount(int type)
    {
        return (int)(baseShootCount * shootCount[type]);
    }

    public int getStage()
    {
        return stage;
    }

    public int getCurrentStageScore()
    {
        return currentStageScore;
    }

    public int getTotalScore()
    {
        return earnedScore + currentStageScore;
    }


}
