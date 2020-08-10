using System.Collections;
using System.Collections.Generic;
//stringbuilder 사용을 위해 text 사용
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UI_Control : MonoBehaviour
{
    
    public Text ScoreDisplay;
    public Text StageDisplay;
    public Text LivesDisplay;

    private StringBuilder sb = new StringBuilder();

    int time_minute = 0;
    int time_second = 0;
    int time_mili = 0;
    //int time_mili_10 = 0;

    public bool isPause = false;//일시정지 여부

    public GameObject pausePanel;//일시정지 UI창


    float timer = 0;//프레임 체크용 변수

    // Update is called once per frame
    void Update()
    {
        checkTime_mili();//매 프레임마다

        timer += 60*Time.deltaTime;//수정:deltatime사용
        if (timer > 60)//1초가 지날때마다
        {
            checkTime();
            timer = 0;
        }

        sb.Length = 0;//sb 초기화
        sb.Append("Time ");
        sb.Append(time_minute / 10);
        sb.Append(time_minute % 10);
        sb.Append(":");
        sb.Append(time_second / 10);
        sb.Append(time_second % 10);
        sb.Append(":");
        sb.Append(time_mili / 1000);
        sb.Append(time_mili % 1000 / 100);
        ScoreDisplay.text = sb.ToString();
        //"Time " + time_minute / 10 + time_minute % 10 + ":" + time_second / 10 + time_second % 10 + ":" + time_mili/1000 + time_mili%1000/100;//텍스트 갱신
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();//일시정지&일시정지 해제 함수
        }

    }

    public void checkTime()
    {

        //시간초 올라갈때마다 자리수 변경한다

        if (++time_second > 59)
        {
            ++time_minute;
            time_second = 0;
        }

            time_mili = 0;//밀리타임이 올라가는 속도와 초가 올라가는속도를 같게 하기위해 1초마다 밀리세컨드를 리셋한다.

    }

    //매 프레임마다 0.n초단위 올라감, 60프레임이므로 100/60의 근사값인 1.66을 썼음
    public void checkTime_mili()
    {if(!isPause)
        {
            if (time_mili > 9800)//time_mili%1000/100을 세자리로 만들지 않게 10000 전에 타임 밀리를 리셋한다
            {
                time_mili = 0;
            }
            time_mili += 166;
        }
    }

    private void Pause()
    {
        if (isPause)
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
            isPause = false;
        }
        else
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            isPause = true;
        }
    }
    public void ChangeMain()//메인으로 버튼 눌렀을경우 호출할 함수
    {
        Destroy(GameObject.Find("DataContainer"));//사용 끝났으니 새 게임을 위해 데이터 콘테이너 제거
        Time.timeScale = 1;
        isPause = false;
        SceneManager.LoadScene("StartMenu");
    }

    public void ReturnGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        isPause = false;
    }

}