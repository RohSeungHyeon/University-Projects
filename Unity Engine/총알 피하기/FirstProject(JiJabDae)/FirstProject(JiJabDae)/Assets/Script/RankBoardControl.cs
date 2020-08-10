using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RankBoardControl : MonoBehaviour
{

    public Text[] BoardRank;//순위 글자
    public Text[] BoardScore;//순위에 들어갈 점수
    public Text[] BoardName;//순위에 들어갈 이름
    public Text BestScore;//자신의 최고점수
    public Text NetError;//네트워크 불러오기 실패시 표시
    private string loadAddress = "http://kimchimanju.com/dodgeserver/LoadTest.php"; //랭킹 DB를 불러올 php가 있는 주소

    // Use this for initialization

    private void Awake()
    {
        StartCoroutine(ReceiveRanking()); //씬이 로딩되는 동안 랭킹 불러오기
    }
    /*
        void Start () {

            BestScore.text = "Your Best Score:"+"";//pp-이 부분에 저장되어있는 가장 큰 점수 불러오기

            for(int i = 0; i<20; i++)//pp-서버에서 1위~20위의 이름과 점수를 받아온다.
            {
                BoardName[i].text = "";
                BoardScore[i].text = ""; 

            }
        }
        */
   void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("StartMenu");
        }
    }


    private IEnumerator ReceiveRanking() //랭킹 DB를 불러올 함수
    {
        WWW webRequest = new WWW(loadAddress);
        do
        {
            yield return null;
        }
        while (!webRequest.isDone); //랭킹 DB 정보 다운로드

        if (webRequest.error != null) // 랭킹 DB 정보 다운로드 실패시 중지
        {
           // Debug.LogError("webRequest.error = " + webRequest.error);
            //pp-실패시 기존 글들을 다 지우고, 네트워크 에러라고 뜨게 한다.
            NetError.text = "NETWORK CONNECTION ERROR!";
            for (int i = 0; i < 20; i++)//pp다른 내용 전부 표백
            {
                BoardRank[i].text = "";
                BoardName[i].text = "";
                BoardScore[i].text = "";

            }

            yield break;
        }
        Debug.Log("loadAddress : " + loadAddress);
        Debug.Log("Did we call LoadTest? : " + webRequest.isDone);
        yield return webRequest;

        Debug.Log("webRequest.text : " + webRequest.text);
        string[] stringSeparators = new string[] { "\n" }; //불러들인 정보를 1차적으로 나눌 기준을 \n으로 설정
        string[] lines = webRequest.text.Split(stringSeparators, System.StringSplitOptions.RemoveEmptyEntries); //\n 기준으로 나눈 후 lines[]에 저장

        for (int i = 0; i < lines.Length; i++)
        {
            if (i > 19) { break; }
            else
            {
                string _rank_extra = "";
                string[] _parts = lines[i].Split(','); //2차적으로 ',' 기준으로 나눈 후 하나씩 _parts[]에 저장
                string _name = _parts[0]; //_parts[0]은 이름값이므로 _name에 저장
                Debug.Log("_parts[0] : " + _parts[0]);
                int _score = int.Parse(_parts[1]); //_parts[1]은 점수값이므로 _score에 저장
                Debug.Log("_parts[1] : " + _parts[1]);

                //to 노승현-랭크 관련
                //여기서 랭크 받아옴
                int _rank = int.Parse(_parts[2]); //_parts[2]는 랭킹값이므로 _rank에 저장
                

                if(_rank == 1)
                {
                    _rank_extra = "ST";
                }
                else if (_rank == 2)
                {
                    _rank_extra = "ND";
                }
                else if(_rank ==3)
                {
                    _rank_extra = "RD";
                }
                else
                {
                    _rank_extra = "TH";
                }




                BoardRank[i].text = _rank+_rank_extra;
                BoardName[i].text = _name; //랭킹보드에 있는 이름값 변경
                BoardScore[i].text = "" + _score; //랭킹보드에 있는 점수값 변경
            }
        }
    }

    public void ChangeMain()//메인으로 버튼 눌렀을경우 호출할 함수
    {
        SceneManager.LoadScene("StartMenu");
    }

}
