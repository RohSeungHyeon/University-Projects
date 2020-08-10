using UnityEngine;

public class Enemy_Bullet_Move : MonoBehaviour
{

    private float MoveSpeed; // 탄 속도
    private Vector2 _direction; //탄의 초기위치에서 플레이어에게 날아갈 방향을 저장할 변수

    private Data data;//상수들을 저장해놓는 스크립트 로드용
    private SpriteRenderer imgRenderer;
    internal int bulletType = 0; // Enemy_Fire에서 값을 부여받는다.

    //임시변수
    private Vector3 pos;

    private AudioSource audio;

    void Awake()
    {
        imgRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        data = GameObject.Find("DataContainer").GetComponent<Data>();

        this.audio = this.gameObject.AddComponent<AudioSource>();
        this.audio.loop = false;
        this.audio.playOnAwake = false;
    }

    void OnEnable()
    {
        this.audio.clip = data.gunfireEffects[bulletType];
        this.audio.Play();

        imgRenderer.sprite = data.bulletSprites[bulletType];
    }

    void Update()
    {
        //이동 함수가 들어있다.
        Vector2 position = transform.position; // position에 탄의 초기위치 저장
        position += _direction * MoveSpeed * Time.deltaTime;//position에 매 프레임 목적지의 방향 만큼 날아갈 때 마다 변경되는 위치값 저장
        transform.position = position; // 탄의 위치를 position의 위치로 설정
        MakeBulletInvisible(); // 화면 밖으로 벗어나면 탄 없어짐
    }

    //화면 밖으로 벗어나면 없어지게 하는 함수
    void MakeBulletInvisible()
    {
        pos = Camera.main.WorldToViewportPoint(transform.position); //탄의 현재 위치 pos에 저장

        /*뷰포인트의 범위는 0 ~ 1이므로 0과 1사이에서 벗어나면 없앰*/
        //김형준 수정:없앨때 지금까지 사라진 총알 갯수 플러스 함
        //부수기재 수정:더는 총알 갯수를 올려주지 않고 총알의 형식을 넘겨서 점수증가 처리
        if (pos.x < 0f || pos.x > 1f || pos.y < 0f || pos.y > 1f)
        {
            gameObject.SetActive(false);
            data.PlusScore(bulletType);// 총알이 삭제되면 점수 증가
        }
    }

    //탄의 초기위치에서 목적지를 향해 방향(회전도)과 이동방향을 설정하고 탄속을 결정하는 함수
    public void Setting(Vector2 direction)
    {
        Vector2 position = transform.position;

        MoveSpeed = data.baseBulletSpeed * data.speedRatePerBulletType[bulletType];
        _direction = direction.normalized;
        var angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg + 270;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        transform.position = position;
    }
}
