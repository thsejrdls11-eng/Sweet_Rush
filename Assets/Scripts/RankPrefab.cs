using TMPro;
using UnityEngine;

public class RankPrefab : MonoBehaviour
{
    public TextMeshProUGUI rank, id, time;
    public GameObject playerMark;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void InitPrefab(bool _isPlayer, string _id, int _rank, float _t)
    {
        if (_isPlayer) playerMark.SetActive(true);
        else playerMark.SetActive(false);
        id.text = _id;
        rank.text = _rank.ToString();
        if(_t > 0)
            time.text = $"{_t / 60:00}:{_t % 60:00.00}";
        else
        {
            time.text = "RETIRE";
        }
    }
}
