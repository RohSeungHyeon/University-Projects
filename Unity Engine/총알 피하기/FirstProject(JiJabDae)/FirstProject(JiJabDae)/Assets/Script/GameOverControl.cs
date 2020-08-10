using System.Collections;
using System.Collections.Generic;

//씬 전환을 위해 씬매니저 사용
using UnityEngine.SceneManagement;

using UnityEngine.UI;
using UnityEngine;


public class GameOverControl : MonoBehaviour {

    private Data data;
    public Text FinalScoreDisplay;
    public Text HighestDisplay;
    public Button GoToRanking;
    public GameObject Button_Ranking;

    // Use this for initialization
    void Awake ()
    {
        int finalscore = 0;

        HighestDisplay.text = "";
        Button_Ranking.SetActive(false);
        
       

        data = GameObject.Find("DataContainer").GetComponent<Data>();//인게임에서 생성된 데이터 컨테이너가 남아있다.

        finalscore = data.getTotalScore();

        if(finalscore>0)//PP 김형준:서버에서 받아올 자신의 최종 점수보다 높을경우로 재설정해야함
        {
            HighestDisplay.text = "Update Your Score!";
            Button_Ranking.SetActive(true);
        }

        FinalScoreDisplay.text = "Score:" + finalscore;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("StartMenu");
        }
    }


    public void ChangeMain()//메인으로 버튼 눌렀을경우 호출할 함수
    {
        Destroy(GameObject.Find("DataContainer"));//사용 끝났으니 새 게임을 위해 데이터 콘테이너 제거
        SceneManager.LoadScene("StartMenu");
    }

    public void ChangeRetry()//재도전 눌렀을경우 호출할 함수
    {
        Destroy(GameObject.Find("DataContainer"));//사용 끝났으니 새 게임을 위해 데이터 콘테이너 제거
        SceneManager.LoadScene("InGameMain");
    }
    public void ChangeRanking()//랭킹페이지에서는 아직 데이터 컨테이너를 사용하니, 지우지 않는다.
    {
        SceneManager.LoadScene("UpdateRanking");
    }



}
