using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmGameManager : MonoBehaviour
{
    public AudioSource theMusic; //오디오 소스

    public bool startPlaying; //게임이 시작되었는지...

    public BeatScroller theBS; // BeatScroller 받아오기

    public static RhythmGameManager instance; //이 자체로 인스턴스화

    public int currentScore; //현재 점수
    public int scorePerNote = 100; //(한 노트 당) 추가 될 점수
    //public int scorePerGoodNote = 125; //(한 노트 당) 추가 될 점수(Good)
    //public int scorePerPerfectNote = 150; //(한 노트 당) 추가 될 점수(Perfect)

    //public int currentMultiplier; //현재 추가 점수
    //public int multiplierTracker; //연속해서 (정해진 개수의) 노트를 맞췄을 경우... 다음 레벨로(더 높은 점수) 이동할 때 (그 시점을 추적할 때 사용 -> 몇 개 기준?
    //public int[] multiplierThresholds; //다음 레벨로(더 높은 점수) 이동할 수 있게 Thresholds를 호출 -> 몇 개 기준인지



    public Text scoreText; //화면에 표시 될 점수 텍스트
    //public Text multiText; //화면에 표시 될 배속 속도 텍스트

    public float totalNotes; //적중된 총 노트들?
    public float normalHits;
    //public float goodHits;
    //public float perfectHits;
    public float missedHits;

    public GameObject rgstButton; //리듬게임(기우제) 버튼
    public GameObject gamePanel;
    public GameObject resultsScreen; //점수 요소 가져다쓰려고 참조 쓰기 /점수 팝업으로 점수 보여주기
    //public Text percentHitText, normalsText, goodsText, perfectsText, missesText, rankText, finalScoreText; //점수판에 나타날 각 점수들
    public Text normalsText, missesText, rankText, finalScoreText; //점수판에 나타날 각 점수들


    void Start()
    {
        //음악 재생과 바 떨어뜨리기를 시작할 때 게임매니저를 컨트롤 
        instance = this;

        scoreText.text = "Score: 0"; //처음 시작할 때 점수 = 0으로 
        //currentMultiplier = 1; //처음 시작할 때 획득 점수 = x 1

        totalNotes = FindObjectsOfType<NoteObject>().Length;
        //Arrow컴퍼넌트에 이미 NoteObject 스크립트가 있음 -> 여기서 Length 가져오기
        //Length가 노트가 얼마나 찍혔는지 알려줄 것
    }


    void Update()
    {
        //startPlaying = false일때 시작(디폴트임)
        if(!startPlaying) 
        {
            if(Input.anyKeyDown) //아무 버튼을 누르게 되면 
            {
                startPlaying = true; //게임이 시작이 되고
                theBS.hasStarted = true; //BeatScroller또한 시작

                //버튼을 눌렀을 때 음악이 플레잉 되길 원함
                theMusic .Play();
            }
        }
        else
            {
                if(!theMusic.isPlaying && !resultsScreen.activeInHierarchy && gamePanel.activeInHierarchy) //음악이 플레이 되지 않고, 점수판이 활성화 되지 않았다면...
                {
                    resultsScreen.SetActive(true); //점수판 활성화

                    normalsText.text = "" + normalHits; //위 아래 같은 방식임?
                    //goodsText.text = goodHits.ToString();
                   // perfectsText.text = perfectHits.ToString();
                    missesText.text = "" + missedHits;

                    float totalHit = normalHits; //총 점수 받아오기
                    //float percentHit = (totalHit / totalNotes) * 100f; //퍼센트로 (획득한 총 점수 / 만점?) 나타내기
                
                    //percentHitText.text = percentHit.ToString("F1") + "%"; //한 자릿 수 소수점까지만 보여주기

                    string rankVal = "F"; //default rank value

                    if(totalHit > 400)
                    {
                        rankVal = "water";
                        if(totalHit < 500)
                        {
                            rankVal = "Thirsty";

                            
                        }
                    }

                    rankText.text = rankVal; //점수판에 나타날  최종 랭크

                    finalScoreText.text = currentScore.ToString(); //최종 점수
                }
            }
    }

    public void NoteHit()
    {
        Debug.Log("Hit On Time");
//multiplierThresholds element값이 4라면(인스펙터)
//multiplier값이 1인데 0으로 만들고 싶음 -> 1을 빼주면 됨...
        // if (currentMultiplier - 1 < multiplierThresholds.Length) //currentMultiplier 가 4라면, 아래 실행 x
        // {
        //     multiplierTracker++;

        //     //currentMultiplier - 1 -> 배열에서 -1을 함 -> 포지션을 0으로 만들기
        //     if(multiplierThresholds[currentMultiplier - 1] <= multiplierTracker)
        //     {
        //         multiplierTracker = 0;
        //         currentMultiplier++;
        //     }
        // }

        // 2:12 영상에서도! 주석 처리// multiText.text = "Multiplier: x" + currentMultiplier;

        //2:12 영상에서쯤! 주석 처리//currentScore += scorePerNote; //노트 맞추기를 성공했을 경우 점수 추가
        //currentScore += scorePerNote * currentScore; //배속으로 돌렸을 때 그 점수에 추가 점수 단위를 곱해 / 더 높은 점수를 준다
        scoreText.text = "Score: " + currentScore; //(점수가 추가된) 현재 점수 표시
    }

    public void NormalHit()
    {
        currentScore += scorePerNote;
        NoteHit();

        normalHits++; //총 스코어인 NormalHits에 1 합산
    }

    public void NoteMissed()
    {
        Debug.Log("Missed Note");

        // currentMultiplier = 1; //노트를 놓쳤을 경우 점수 획득 값(단위)을 되돌리기
        // multiplierTracker = 0;

        // multiText.text = "Multiplier:x" + currentMultiplier;

        missedHits++; //총 스코어인 missedHits에 1 합산
    }
}
