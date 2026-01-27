using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectManager : MonoBehaviour
{

    public List<GameObject> carList = new List<GameObject>();
    public int selectIndex = 0;
    
    private void Awake()
    {
        for(int i = 0; i < carList.Count; i++)
        {
            if (i == 0) carList[i].SetActive(true);
            else carList[i].SetActive(false);
        }
        //첫번째 차량만 활성화
    }

    public void  OnSelectCarButton(int index) // index번째의 차량만 활성화
    {
        selectIndex = index;
        for (int i = 0; i < carList.Count; i++)
        {
            if (i == index) carList[i].SetActive(true);
            else carList[i].SetActive(false);
        }
    }

    public void NextCarSelect(int index) //  -1 또는 1
    {
        if(index == -1)
        {
            selectIndex--;
            if (selectIndex < 0) selectIndex = carList.Count - 1;
            OnSelectCarButton(selectIndex);
        }
        else if (index == 1)
        {
            selectIndex++;
            if (selectIndex >= carList.Count) selectIndex = 0;
            OnSelectCarButton(selectIndex);
        }
    }
    public void StartBtn()
    {
        PlayerData.selectIndex = selectIndex;
        SceneManager.LoadScene(1);
        //씬이동
    }
}
