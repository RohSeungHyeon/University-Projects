using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class UpdateRankingControl : MonoBehaviour {

    //private int rankingScore = 0;
    private int rankingScore = 666;
    private Data data;
    private string saveAddress = "http://kimchimanju.com/dodgeserver/InsertTest.php?name="; //랭킹을 저장할 php가 있는 주소
    private string loadMyAddress = "http://kimchimanju.com/dodgeserver/MyRankLoadTest.php?MyScore="; //랭킹을 저장할 php가 있는 주소


    public Text[] names;
    public GameObject[] names_up;
    public GameObject[] names_down;
    public Text nameEnd;
    public Text finalScore;
    public Text yourRanking;
    public Text successMessage;
    private char namechar0 = 'A';//현재 입력중인 문자
    private char namechar1 = 'A';
    private char namechar2 = 'A';
    private bool isEnd = false;//현재 입력 완료를 눌러 end키가 떠있음
    private bool isReallyEnd = false;//end 이후 끝까지 다 완료됐을때 true
    public GameObject buttonRetry;
    public GameObject buttonGoMain;
    public GameObject buttonConfirm;



    // Use this for initialization
    void Start () {
             data = GameObject.Find("DataContainer").GetComponent<Data>();//인게임에서 생성된 데이터 컨테이너가 남아있다.

                rankingScore = data.getTotalScore();
                Destroy(GameObject.Find("DataContainer"));//사용 끝났으니 새 게임을 위해 데이터 콘테이너 제거

        names[0].text = "A";
        nameEnd.text = "";
        buttonGoMain.SetActive(false);
        buttonRetry.SetActive(false);
        
         
        finalScore.text = "Final score:" + rankingScore;
        StartCoroutine(ReceiveMyRanking());
        

    }
	
	// Update is called once per frame
	void Update () {
        
                

            
            if (Input.GetKey(KeyCode.Escape))
            {
                if (isEnd && !isReallyEnd)
                {
                    deleteEndText();
                }
                else if(isReallyEnd){
                ChangeMain1();

            }
                else
                {
                    //취소하고 메인메뉴로 돌아가겠냐고 뜨게하기
                }

            }
        


    }

    public void OnUpClick0()
    {


            if (namechar0 < 'Z')//Z까지 넘어갔으면 다시 A로 복귀
            {
                namechar0++;
            }
            else
            {
                namechar0 = 'A';
               
            }
            names[0].text = "" + namechar0;
        

    }
    public void OnDownClick0()
    {


        if (namechar0 > 'A')//Z까지 넘어갔으면 다시 A로 복귀
        {
            namechar0--;
            
        }
        else
        {
            namechar0 = 'Z';
        }
        names[0].text = "" + namechar0;
    }
    public void OnUpClick1()
    {


        if (namechar1 < 'Z')//Z까지 넘어갔으면 다시 A로 복귀
        {
            namechar1++;
        }
        else
        {
            namechar1 = 'A';

        }
        names[1].text = "" + namechar1;


    }
    public void OnDownClick1()
    {


        if (namechar1 > 'A')//Z까지 넘어갔으면 다시 A로 복귀
        {
            namechar1--;

        }
        else
        {
            namechar1 = 'Z';
        }
        names[1].text = "" + namechar1;
    }
    public void OnUpClick2()
    {


        if (namechar2 < 'Z')//Z까지 넘어갔으면 다시 A로 복귀
        {
            namechar2++;
        }
        else
        {
            namechar2 = 'A';

        }
        names[2].text = "" + namechar2;


    }
    public void OnDownClick2()
    {


        if (namechar2 > 'A')//Z까지 넘어갔으면 다시 A로 복귀
        {
            namechar2--;

        }
        else
        {
            namechar2 = 'Z';
        }
        names[2].text = "" + namechar2;
    }

    public void OnConfirm()
    {
        if (isEnd)
        {
            //pp-이 부분에서 서버에 입력된 문자열을 전송해야한다
            StartCoroutine(SubmitHighScore());
            //???(names[0].text + names[1].text + names[2].text);

            //pp-서버 업데이트가 정상적으로 됐을경우
            /* if ()
                 successMessage.text = "UPDATE COMPLETE!";
             else
                 successMessage.text = "오류가 발생했습니다";
                 */
            
            buttonGoMain.SetActive(true);
            buttonRetry.SetActive(true);
            buttonConfirm.SetActive(false);
            isReallyEnd = true;
        }
        else
        {
            isEnd = true;
            for(int i = 0; i<3; i++)
            {
                names_up[i].SetActive(false);
                names_down[i].SetActive(false);
            }
            nameEnd.text = "END";
        }

    }





    void deleteEndText()
    {
        isEnd = false;
        for (int i = 0; i < 3; i++)
        {
            names_up[i].SetActive(true);
            names_down[i].SetActive(true);
        }
        nameEnd.text = "";

    }
    public void ChangeMain1()//메인으로 버튼 눌렀을경우 호출할 함수
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void ChangeRetry1()//재도전 눌렀을경우 호출할 함수
    {
        SceneManager.LoadScene("InGameMain");
    }

    private IEnumerator ReceiveMyRanking() //내 최고 점수와 랭킹 DB에 있는 점수들 비교 후, 등수 가져오기
    {
        WWW webRequest = new WWW(loadMyAddress + rankingScore);
        do
        {
            yield return null;
        }
        while (!webRequest.isDone); //내 최고 점수 업로드 및 등수 다운로드

        if (webRequest.error != null) //실패시 중지
        {
           // Debug.LogError("webRequest.error = " + webRequest.error);
            yield break;
        }
        Debug.Log("loadAddress : " + loadMyAddress);
        Debug.Log("Did we call MyRankLoadTest? : " + webRequest.isDone);
        yield return webRequest;

        Debug.Log("webRequest.text : " + webRequest.text);

        int MyRank = int.Parse(webRequest.text); //내 등수 저장

        yourRanking.text = "Your Rank: " + MyRank; //등수 출력
    }

    private IEnumerator SubmitHighScore() //랭킹 정보를 업로드할 함수
    {
        string _score = "" + rankingScore; //_score에 점수 저장
        Debug.Log("Score : " + _score);
        string _name = names[0].text + names[1].text + names[2].text; //_name에 유저가 지정한 이름 저장
        Debug.Log("Name : " + _name);
        string _hash_origin = _name + "_" + _score + "_hash"; //_치팅을 방지하기 위한 보안 Hash_Origin
        string _hash = Md5Sum(_hash_origin); //Hash_Origin을 hash화
        WWW webRequest = new WWW(saveAddress + _name + "&score=" + _score + "&hash=" + _hash);
        do
        {
            yield return null;
        }
        while (!webRequest.isDone); //랭킹 정보 업로드

        if (webRequest.error != null) //정보 업로드 실패시 중지
        {
            //Debug.LogError("webRequest.error = " + webRequest.error);
            successMessage.text = "NETWORK ERROR!";
            yield break;
        }
        Debug.Log("webRequest.txt : " + webRequest.text);
        Debug.Log("Did we call insertTest? : " + webRequest.isDone);
        successMessage.text = "UPDATE COMPLETE!";

        yield return webRequest;
    }

    public string Md5Sum(string strToEncrypt) //Md5 해쉬 알고리즘
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }

}
