using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Fire : MonoBehaviour
{
    public GameObject EnemyBullet; //복제할 적의 탄
    //public Transform BulletLocation; //탄이 생성될 위치
    private float FireDelay; //탄과 탄을 발사할 때 시간 간격
    public float FirePreparingTime;

    private Data data;
    private SpriteRenderer imgRenderer;
    internal int remainingFire; // 남은 발사 횟수

    public int MaxBulletCount; //메모리 배열에 저장할 탄 개수
    private GameObject[] BulletArray; //사용할 탄 배열
    private GameObject target;
    private Vector2 direction; // 발사 위치
    private Transform firePosition;
    //private AudioSource sfxPlayer;

    private bool isFiring;
    private bool isLaserActive;
    
    internal int unitType; // 현재 스크립트를 적용시킬 적(gameObject)의 종류로서, EnemySpawn에서 활성화할 때마다 부여받는다.

    private Enemy_Bullet_Move bulletScript;
    private LineRenderer line; // 모두 잠재적으로 저격소총을 들 수 있기 때문

    private const float LaserWidth = 0.01f;

    //private AudioSource audio;

    // 게임이 종료될 때 자동으로 호출되는 함수
    private void OnApplicationQuit()
    {
        StopAllCoroutines();
        for (int i = 0; i < MaxBulletCount; ++i)
            Destroy(BulletArray[i]);
    }

    //생성 시 호출되는 함수
    void Awake() // 이 인스턴스가 생성될때 호출된다.
    {
        data = GameObject.Find("DataContainer").GetComponent<Data>();
        unitType = 0;
        BulletArray = new GameObject[MaxBulletCount]; // 배열 초기화(모든 값이 null로 설정)
        for (int i = 0; i < MaxBulletCount; ++i) // 배열에 채워넣되
        {
            BulletArray[i] = GameObject.Instantiate(EnemyBullet) as GameObject;//EnemyBullet으로 들어온 게임 오브젝트를 똑같이 넣으면서
            BulletArray[i].SetActive(false);// 비활성화 시킨다.
        }
        target = GameObject.Find("Player"); //target에 "Player"라는 이름이 붙어있는 오브젝트를 찾고 저장
        isFiring = true; // 이는 비활성화를 한번 거치는 알고리즘상으로 처음 개체가 코루틴을 두번 실행하지 못하게 하기 위한 설정이다

        isLaserActive = false;
        //라인렌더러 초기화 부분
        line = gameObject.AddComponent<LineRenderer>();
        line.startWidth = line.endWidth = LaserWidth;
        line.startColor = line.endColor = Color.red;

        imgRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        firePosition = transform.GetChild(0).GetChild(0).GetComponent<Transform>();
        //sfxPlayer = this.gameObject.AddComponent<AudioSource>();
        //sfxPlayer.loop = false;
        //sfxPlayer.playOnAwake = false;
    }
    
    void OnEnable()
    {
        imgRenderer.sprite = data.enemySprites[unitType];
        if (!isFiring)
        {
            if (data.getFakeEnabled())
                if (Random.Range(0, 100) < data.fakeChancePerType[unitType])
                {
                    StartCoroutine(FakeRoutine());
                    return;
                }
            Invoke("FirePreparing", FirePreparingTime); // FirePreparingTime으로 지정된 시간동안 대기하다 FirePreparing을 작동시킴
            isFiring = true;
        }
        else
        {
            isFiring = false;
        }
    }
    void OnDisable()
    {
        StopAllCoroutines(); // 비활성화시 이 스크립트에서 발생한 모든 코루틴을 종료
        isFiring = false;
    }

    /// unitType
    /// 0 권총
    /// 1 라이플
    /// 2 샷건
    /// 3 머신건
    /// 4 저격소총
    private void FirePreparing()
    {
        //Debug.Log("발사 준비 요청을 확인");
        if (gameObject.activeSelf)
        {
            FireDelay = data.fireDelayBase * data.fireDelayRatePerType[unitType];
            remainingFire = data.getShootCount(unitType); // Data에 들어있는 값으로서, 발사 제한 횟수를 나타낸다.

            //sfxPlayer.clip = data.gunfireEffects[unitType];
            //sfxPlayer.Stop();

            switch (unitType)
            {
                case 2: StartCoroutine(ShotgunFire()); break;
                //case 3: StartCoroutine(MachinegunFire()); break; // Spray'n Pray 방식을 사용하지 않음에 따라서 주석처리.
                case 4: StartCoroutine(SniperFire()); break;

                default: StartCoroutine(NormalFire()); break;
            }
        }
    }

    IEnumerator FakeRoutine()
    {
        yield return new WaitForSeconds(FirePreparingTime);
        gameObject.SetActive(false);
    }

    IEnumerator NormalFire()
    {
        bool fired;
        while (remainingFire-- > 0)
        {
            if (target != null) //목표물이 건재할 경우
            {
                fired = false;
                for (int i = 0; i < MaxBulletCount; i++)
                { // 탄 배열에서 활성화되지 않은 미사일을 찾아서 발사
                    if (BulletArray[i].activeSelf == false)
                    { // 탄 배열[i]가 비어있을 시

                        direction = target.transform.position - firePosition.position; // 탄의 초기위치로부터 플레이어의 위치까지의 방향 계산
                        Shoot(i);
                        fired = true;
                        break; // 발사 후 for문 탈출
                    }
                }
                if (!fired) remainingFire++;
            }
            yield return new WaitForSeconds(FireDelay); //  탄이 재배치 되든 안되든 한번의 루틴이 끝나면 FireDelay만큼 휴식시킨다
        }
        gameObject.SetActive(false);
    }

    IEnumerator ShotgunFire()
    {
        int i, requireCount;

        Quaternion rotation = Quaternion.Euler(0, 0, 10f);
        Quaternion oppositeRotation = Quaternion.Euler(0, 0, -10f);


        while (remainingFire > 0)
        {
            if (target != null) //목표물이 건재할 경우
            {
                requireCount = data.shotgunOneShotLimit + 1;
                for (i = 0; i < MaxBulletCount && requireCount > 0; ++i)
                {
                    if (BulletArray[i].activeSelf == false)
                        --requireCount;
                }

                if (requireCount <= 0)
                {
                    direction = target.transform.position - firePosition.position;
                    //우선 플레이어 방향으로 한발 발사
                    for (i = 0; i < MaxBulletCount; i++)
                    { // 탄 배열에서 활성화되지 않은 미사일을 찾아서 발사
                        if (BulletArray[i].activeSelf == false)
                        { // 탄 배열[i]가 비어있을 시
                            Shoot(i);
                            break; // 발사 후 for문 탈출
                        }
                    }

                    requireCount = data.shotgunOneShotLimit >> 1; // 절반으로 토막내기
                    while (requireCount > 0)
                        for (i = 0; i < MaxBulletCount; i++)
                        { // 탄 배열에서 활성화되지 않은 미사일을 찾아서 발사
                            if(requireCount>0)
                                if (BulletArray[i].activeSelf == false)
                                { // 탄 배열[i]가 비어있을 시

                                    direction = oppositeRotation * direction;
                                    Shoot(i);
                                    requireCount--;
                                }
                        }
                    
                    direction = target.transform.position - firePosition.position;
                    requireCount = data.shotgunOneShotLimit >> 1;
                    while (requireCount > 0)
                        for (i = 0; i < MaxBulletCount; i++)
                        { // 탄 배열에서 활성화되지 않은 미사일을 찾아서 발사
                            if (requireCount > 0)
                                if (BulletArray[i].activeSelf == false)
                                { // 탄 배열[i]가 비어있을 시

                                    direction = rotation * direction;

                                    Shoot(i);
                                    requireCount--;
                                }
                        }
                    remainingFire--; // 발사에 성공했기 때문에 값을 내림
                }
            }
            yield return new WaitForSeconds(FireDelay); //  탄이 재배치 되든 안되든 한번의 루틴이 끝나면 FireDelay만큼 휴식시킨다
        }
        gameObject.SetActive(false);
    }


    IEnumerator MachinegunFirePrev()// 사용되지 않는 함수이다. Spray'n Pray 방식의 발사루틴
    {
        direction = target.transform.position - firePosition.position; // 사수로부터 플레이어의 위치까지의 방향 계산
        
        int fireCount = 5;
        int flagCount = 2;
        bool directionFlag = true;

        Quaternion rotation = Quaternion.Euler(0, 0, 10f);
        Quaternion oppositeRotation = Quaternion.Euler(0, 0, -10f);

        while (remainingFire-- > 0)
        {
            if (target != null) //목표물이 건재할 경우
            {
                for (int i = 0; i < MaxBulletCount; i++)
                { // 탄 배열에서 활성화되지 않은 미사일을 찾아서 발사
                    if (BulletArray[i].activeSelf == false)
                    { // 탄 배열[i]가 비어있을 시

                        Shoot(i);

                        if (directionFlag)
                        {
                            direction = rotation * direction;
                            if(--flagCount <= 0)
                            {
                                flagCount = fireCount;
                                directionFlag = false;
                            }
                        }
                        else
                        {
                            direction = oppositeRotation * direction;
                            if (--flagCount <= 0)
                            {
                                flagCount = fireCount;
                                directionFlag = true;
                            }
                        }
                        break; // 발사 후 for문 탈출
                    }
                }
            }
            yield return new WaitForSeconds(FireDelay); //  탄이 재배치 되든 안되든 한번의 루틴이 끝나면 FireDelay만큼 휴식시킨다
        }
        gameObject.SetActive(false);
    }

    
    IEnumerator SniperFire() // 무조건 한발만 발사하는 것으로 가정하고
    {
        bool isFire = false;
        while (true)
        {
            if (target != null) //목표물이 건재할 경우
            {
                for (int i = 0; i < MaxBulletCount; i++)
                { // 탄 배열에서 활성화되지 않은 미사일을 찾아서 발사
                    if (BulletArray[i].activeSelf == false)
                    { // 탄 배열[i]가 비어있을 시
                        direction = target.transform.position - firePosition.position; // 탄의 초기위치로부터 플레이어의 위치까지의 방향 계산

                        line.positionCount = 2;
                        isLaserActive = true;
                        yield return new WaitForSeconds(data.sniperWaitDelay); //  궤적 그리고 난 뒤에 발사전 휴식
                        isLaserActive = false;
                        line.positionCount = 0;

                        Shoot(i);

                        isFire = true;
                        break; // 발사 후 for문 탈출
                    }

                }
            }
            if (!isFire) yield return new WaitForSeconds(FireDelay); // 발사하지 못했다면 1사이클당 1 휴식
            else break;
        }
        yield return new WaitForSeconds(FireDelay / 3); // 잔류시간
        gameObject.SetActive(false);
    }

    void Shoot(int bulletIndex)
    {
        BulletArray[bulletIndex].transform.position = firePosition.position; // 해당 탄의 위치를 탄 발사 지점으로 변경
        bulletScript = BulletArray[bulletIndex].GetComponent<Enemy_Bullet_Move>();
        bulletScript.bulletType = unitType;
        //sfxPlayer.Play(0);
        BulletArray[bulletIndex].SetActive(true);
        bulletScript.Setting(direction); //Enemy_Bullet_Move 클래스에 있는 _direction 변수에 방향 저장
    }
    
    // 업데이트 함수에는 현재 레이저 포인터 관련 기능이 들어있다.
    // 적이 플레이어를 쳐다보게 하는 것도 여기에 들어간다

    private void Update() 
    {
        Ray2D ray = new Ray2D(transform.position, direction);
        RaycastHit2D hit = new RaycastHit2D();
        Vector2 tmpDirection = direction;


        if (isLaserActive)
        {
            line.SetPosition(0, ray.origin);

            if (hit = Physics2D.Raycast(ray.origin, direction, Mathf.Infinity))
                tmpDirection = hit.point;
            else
                tmpDirection = direction * 80;
            line.SetPosition(1, tmpDirection);
        }
        else
        {
            float angle = Mathf.Atan2(target.transform.position.y - transform.position.y,
                                        target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            
            transform.rotation = Quaternion.Euler(0, 0, angle + 90);
        }
    }
}

