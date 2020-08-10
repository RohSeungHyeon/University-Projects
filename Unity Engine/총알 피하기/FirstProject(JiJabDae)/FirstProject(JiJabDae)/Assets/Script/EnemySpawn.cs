using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawn : MonoBehaviour
{

    public float interval;
    public int maxEnemy;
    public GameObject enemy;
    private GameObject[] enemyArray;

    public int[] weightPerType;

    private float bgSizeX;
    private float bgSizeY;
    private int enemyTypeNum;
    public int EnemyTypeLimit
    {
        get { return enemyTypeNum;  }
        set { enemyTypeNum = value; }
    }
    
    // Use this for initialization
    void OnApplicationQuit()
    {
        StopCoroutine(CreateRoutine());
        for (int i = 0; i < maxEnemy; ++i)
            Destroy(enemyArray[i]);
    }

    void Start()
    {
        enemyArray = new GameObject[maxEnemy];
        for (int i = 0; i < maxEnemy; ++i)
        {
            enemyArray[i] = GameObject.Instantiate(enemy) as GameObject; // enemy로 지정된 오브젝트를 인스턴스화 하여 배열에 집어넣는다. as 키워드는 안전식
            enemyArray[i].SetActive(false);
        }

        bgSizeX = gameObject.GetComponent<SpriteRenderer>().bounds.size.x / 2 - 0.02f;
        bgSizeY = gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2 - 0.02f;

        enemyTypeNum = 0;
        for (int i = 1; i < weightPerType.Length; ++i)
            weightPerType[i] += weightPerType[i - 1]; // 적의 가중치를 재 계산하여 후의 계산에 사용할 범위를 만든다.

        StartCoroutine(CreateRoutine());
    }

    IEnumerator CreateRoutine()
    {
        int typeTmp,i,j;
        while (true)
        {
            for (i = 0; i < maxEnemy; ++i)
            {
                if (!enemyArray[i].activeSelf)
                {
                    typeTmp = Random.Range(0, weightPerType[enemyTypeNum]); // 적 타입 랜더마이즈
                    for (j=0; j<enemyTypeNum; ++j)
                        if (typeTmp < weightPerType[j]) break;
                    enemyArray[i].GetComponent<Enemy_Fire>().unitType = j;

                    enemyArray[i].SetActive(true);
                    switch (Random.Range(1, 5))
                    {
                        case 1:
                            enemyArray[i].transform.position = new Vector2(Random.Range(-bgSizeX, bgSizeX), bgSizeY);
                            break;
                        case 2:
                            enemyArray[i].transform.position = new Vector2(Random.Range(-bgSizeX, bgSizeX), -bgSizeY);
                            break;
                        case 3:
                            enemyArray[i].transform.position = new Vector2(bgSizeX, Random.Range(-bgSizeY, bgSizeY));
                            break;
                        case 4:
                            enemyArray[i].transform.position = new Vector2(-bgSizeX, Random.Range(-bgSizeY, bgSizeY));
                            break;
                    }
                    break; // 대상 찾고 생성 완료 후에는 for 탈출
                }
            }
            yield return new WaitForSeconds(interval);// 실행이 끝난 후 휴식.
        }
    }
}