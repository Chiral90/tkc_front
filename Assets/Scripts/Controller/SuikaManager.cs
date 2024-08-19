using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SuikaManager : MonoBehaviour
{
    public double score;
    public SuikaController lastDongle;
    public int maxLevel = 8;
    public GameObject donglePrefab; //���� ������
    public Transform dongleGroup;   //������ ������ ��ġ
    TextMeshProUGUI _score;
    private void Awake()
    {
        Application.targetFrameRate = 60; //프레임을 60으로 고정
        _score = gameObject.transform.parent.Find("Canvas").Find("Score").GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        NextDongle();
    }

    SuikaController GetDongle()
    {
        //���� ������ �����ؼ� ������, �� �� �θ�� ���� �׷����� ����
        GameObject instant = Instantiate(donglePrefab, dongleGroup);
        SuikaController instantDongle = instant.GetComponent<SuikaController>();
        return instantDongle;
    }

    void NextDongle()
    {
        //생성된 동글을 가져와 new Dongle로 지정
        SuikaController newDongle = GetDongle();
        lastDongle = newDongle;
        lastDongle.manager = this; //게임매니저를 넘겨준다.
        lastDongle.SetLevel(Random.Range(0, maxLevel - 3)); //레벨 0 ~ maxLevel-1에서 랜덤하게 생성되도록 구현
        lastDongle.gameObject.SetActive(true); //레벨 설정 후 활성화
        StartCoroutine(WaitNext()); //대기후 NextDongle을 실행하는 코루틴 시작
    }

    IEnumerator WaitNext()
    {
        while (lastDongle != null)
        {
            yield return null; //�� �������� ����Ѵ�.
        }

        yield return new WaitForSeconds(2.5f); //2.5�ʸ� ����Ѵ�

        NextDongle();
    }

    public void TouchDown()
    {
        if (lastDongle == null) //lastDongle�� ������ �������� ����
            return;

        lastDongle.Drag();
    }

    public void TouchUp()
    {
        if (lastDongle == null) //lastDongle�� ������ �������� ����
            return;
        lastDongle.Drop();
        lastDongle = null; //����ϸ鼭 �����뵵�� �����ص� ������ null�� ����.
    }

    public void SumScore(int level)
    {
        score += level;
        UpdateScore();
    }

    void UpdateScore()
    {
        _score.text = score.ToString();
    }
}
