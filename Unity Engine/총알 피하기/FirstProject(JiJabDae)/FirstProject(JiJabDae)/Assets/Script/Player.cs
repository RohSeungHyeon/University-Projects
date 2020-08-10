using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private UI_Control ui;
    public float Speed = 3f; //이동속도 (유니티 Inspector에서 제어 가능)
    public int lives;
    public Animator playeranim;
    public Image Joystick;
    public AudioClip hitEffect;
    private int currentLives;
    private Vector2 firstPos; //첫 터치의 벡터 좌표를 저장할 변수
    private Vector2 currentPos; //현재 터치중 벡터 좌표를 저장할 변수
    private Vector2 moveDir = new Vector2(0, 0); //첫 터치를 기준으로 현재 터치중인 좌표 벡터를 정규화해서 저장할 변수
    private Vector2 playerPosition;
    private AudioSource audio;



    void Start()
    {
        ui = GameObject.Find("Canvas").GetComponent<UI_Control>();
        ui.LivesDisplay.text = "Lives: " + (currentLives = lives);
        this.audio = this.gameObject.AddComponent<AudioSource>();
        this.audio.loop = false;
        this.audio.clip = hitEffect;
    }

    void Update()
    {
        Move(); //이동함수
        PushObjectBackInScreen();
       
    }
    /*이동함수*/
   /* 
    private void Move()
    {
        playeranim.SetBool("ismoving",false);//움직이지 않을경우는  서있는 애니메이션이 출력되도록

        if (Input.GetKey(KeyCode.UpArrow)) //위 화살표 클릭
        {
            transform.Translate(Vector2.up * Speed * Time.deltaTime);
            playeranim.SetBool("ismoving", true);  //움직이고 있으므로 애니메이션 파라미터를 true로
        }
        if (Input.GetKey(KeyCode.DownArrow)) // 아래 화살표 클릭
        {
            transform.Translate(Vector2.down * Speed * Time.deltaTime);
            playeranim.SetBool("ismoving", true);
        }
        if (Input.GetKey(KeyCode.RightArrow)) // 우 화살표 클릭
        {
            transform.Translate(Vector2.right * Speed * Time.deltaTime);
            playeranim.SetBool("ismoving", true);
        }
        if (Input.GetKey(KeyCode.LeftArrow)) // 좌 화살표 클릭
        {
            transform.Translate(Vector2.left * Speed * Time.deltaTime);
            playeranim.SetBool("ismoving", true);
        }
    }
    */
    //스마트폰용
    private void Move()
    {
        playeranim.SetBool("ismoving", false);//움직이지 않을경우는  서있는 애니메이션이 출력되도록
        if (Input.touchCount > 0 && !ui.isPause) //터치를 했을 경우 pp->터치를 했으며 일시정지가 아닐때만 출력되도록
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began) //터치 시작시, 터치 시작 좌표를 firstPosX, firstPosY와 currentPosX, currentPosY에 동일한 값으로 저장 (이유:같은 좌표를 계속 터치시 이동하는 것을 막기 위해서)
            {
                firstPos = Input.GetTouch(0).position;
                currentPos = new Vector2(firstPos.x, firstPos.y);
                Joystick.transform.position = new Vector2(firstPos.x, firstPos.y);
                Joystick.enabled = true;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved) //터치 이동시, 현재 좌표를 currentPos에 저장. 만약 현재 Player 위치와 터치 좌표의 거리가 0.23 미만일 시, firstPos좌표를 currentPos에 저장(중립기능)
            {
                currentPos = Input.GetTouch(0).position;
                if (Vector2.Distance(Camera.main.ScreenToWorldPoint(firstPos), Camera.main.ScreenToWorldPoint(currentPos)) < 0.345) //중립 범위 0.23에서 김형준의 요청으로 1.5배 확대해서 0.345로 수정
                {
                    currentPos = new Vector2(firstPos.x, firstPos.y);
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended) //터치 끝났을 시, 전부 0으로 초기화
            {
                //firstPos.x = 0;
                //firstPos.y = 0;
                //currentPos.x = 0;
                //currentPos.y = 0;
                Joystick.enabled = false;
            }
            moveDir = new Vector2(currentPos.x - firstPos.x, currentPos.y - firstPos.y); //시작 좌표부터 현재 좌표까지 방향이 있는 벡터
            moveDir.Normalize(); // 크기 1로 정규화
            float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg; //moveDir 각도 계산
            if (moveDir.magnitude != 0) //방향벡터의 길이가 0이 아니라면(제자리가 아니라면)
            {
                transform.rotation = Quaternion.Euler(0, 0, angle + 90); //moveDir각 + 90도 만큼 회전
            }

            transform.Translate(moveDir * Speed * Time.deltaTime, Space.World); // moveDir 방향으로 이동
            playeranim.SetBool("ismoving", true);  //움직이고 있으므로 애니메이션 파라미터를 true로
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet")) // 충돌시 대상의 태그가 "EnemyBullet"라면
        {
            Debug.Log("플레이어가 총알과 충돌했음.");
            collision.gameObject.SetActive(false); // 대상 탄 비활성화
            if (--currentLives <= 0) GameOver();
            else
            {
                Color color;
                if (currentLives < 2) color = new Color(1f, 0, 0);
                else if (currentLives < 3) color = new Color(0.6f, 0.6f, 0);
                else color = new Color(0, 0, 0);

                ui.LivesDisplay.text = "Lives: " + currentLives;
                ui.LivesDisplay.color = color;
            }
            this.audio.Play();
        }
    }

    /*화면을 못벗어나게 막음*/
    private void PushObjectBackInScreen()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position); //플레이어의 현재 위치 pos에 저장

        /*뷰포인트의 범위는 0 ~ 1이므로 0과 1사이에서 벗어나지 못하게 벗어나려고 할 때 pos.x, pos.y 다시 설정*/
        if (pos.x < 0f) { pos.x = 0f; }
        if (pos.x > 1f) { pos.x = 1f; }
        if (pos.y < 0f) { pos.y = 0f; }
        if (pos.y > 1f) { pos.y = 1f; }

        transform.position = Camera.main.ViewportToWorldPoint(pos); // 설정한 pos로 플레이어 위치 수정
    }

    // 게임오버시 동작을 이곳에 정의한다.
    private void GameOver()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
    }

    //일시정지 함수

}
