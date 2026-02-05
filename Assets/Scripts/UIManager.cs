using System.Collections;
using UnityEngine;
using Game;
using TMPro;
using UnityEditor.ShaderGraph.Internal;

public class UIManager : MonoBehaviour
{

    public GameObject[] countdowns;

    public TextMeshProUGUI rankingText;
    public TextMeshProUGUI lapText;
    public TextMeshProUGUI timeText;
    public GameObject resultPopup;
    public GameObject rankPrefap;
    public Transform rankParent;


    private void Awake()
    {
        foreach(var a in countdowns)
        {
            a.gameObject.SetActive(false);
        }
        resultPopup.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateLap();
        UpdateRank();
        UpdateTime();

        StartCoroutine(StartCountDown());

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartCountDown()
    {
        int count = -1;
        
        while(true)
        {
            if(count >=0) countdowns[count].SetActive(false);
            count++;
            countdowns[count].SetActive(true);
            if (count == 3) break;
            yield return new WaitForSeconds(1);
        }

        MainManager.instance.StartRace();
        yield return new WaitForSeconds(1);
        countdowns[count].SetActive(false);
    }

    public void UpdateLap()
    {
        lapText.text = $"{MainManager.instance.player.labCount+1} / {MainManager.instance.maxLapCount}";
    }

    public void UpdateRank()
    {
        rankingText.text = $"{MainManager.instance.rank} / {MainManager.instance.allCar.Count}";
    }

    public void UpdateTime()
    {
        float time = MainManager.instance.timer;
        float s = time % 60;
        float m = Mathf.FloorToInt( time / 60);
        timeText.text = $"{m:00}.{s:00.00}";
    }

    public void ResultOpen()
    {
        resultPopup.SetActive(true);
        for(int i = 0; i < MainManager.instance.goalList.Count; i++)
        {
            CheckManager c = MainManager.instance.goalList[i];
            RankPrefab r = Instantiate(rankPrefap, rankParent).GetComponent<RankPrefab>();
            r.InitPrefab(c == MainManager.instance.player,
                c.name, i+1, c.time);
        }
    }

    
}
