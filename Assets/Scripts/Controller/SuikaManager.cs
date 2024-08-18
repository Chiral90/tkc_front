using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuikaManager : MonoBehaviour
{
    public SuikaController lastDongle;
    public GameObject donglePrefab; //���� ������
    public Transform dongleGroup;   //������ ������ ��ġ
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
        //������ ������ ������ new Dongle�� ����
        SuikaController newDongle = GetDongle();
        lastDongle = newDongle;
        lastDongle.level = Random.Range(0, 8); //���� 0, 1, 2 ���� �����ϰ� �����ǵ��� ����
        Debug.Log(lastDongle.level);
        lastDongle.gameObject.SetActive(true); //���� ���� �� Ȱ��ȭ
        StartCoroutine(WaitNext()); //����� NextDongle�� �����ϴ� �ڷ�ƾ ����
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
}
