using System.Collections;
using System.Collections.Generic;
//씬 전환을 위해 씬매니저 사용
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using UnityEngine;

public class StartMenuControl : MonoBehaviour {



    public GameObject InstructionImage;//인스트럭션 누르면 나올 설명 창
    public GameObject CreditImage;
    public GameObject StartCanvas;
    public GameObject[] btnMenu;
    private bool isInstOn = false;
    private bool isCreditOn = false;

    //private GameObject IS;

    void Start()
    {
        InstructionImage.SetActive(false);   
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (!isInstOn)
            {
                Application.Quit();
            }
            else
            {
                CloseInstruction();
            }
        }
        
    }

    public void OpenInstruction()//이미지와 버튼을 포함한 오브젝트를 생성
    {
        InstructionImage.SetActive(true);
        for (int i = 0; i<5; i++)
        {
            btnMenu[i].SetActive(false);
        }
        isInstOn = true;
        
        
    }

    public void CloseInstruction()
    {
        InstructionImage.SetActive(false);
        for (int i = 0; i < 5; i++)
        {
            btnMenu[i].SetActive(true);
        }
        isInstOn = false;
    }

    public void OpenCredit()//이미지와 버튼을 포함한 오브젝트를 생성
    {
        CreditImage.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            btnMenu[i].SetActive(false);
        }
        isCreditOn = true;


    }

    public void CloseCredit()
    {
        CreditImage.SetActive(false);
        for (int i = 0; i < 5; i++)
        {
            btnMenu[i].SetActive(true);
        }
        isCreditOn = false;
    }

    public void ChangeSceneGame()
    {
        SceneManager.LoadScene("InGameMain");
    }
    public void ChangeSceneRank()
    {
        SceneManager.LoadScene("RankBoard");
    }


    public void ExitGame()
    {
        #if UNITY_EDITOR//유니티 에디터에서는 게임이 정상 종료되지 않으므로 따로 처리

            UnityEditor.EditorApplication.isPlaying = false;

        #else
        Application.Quit();

        #endif
    }

}
