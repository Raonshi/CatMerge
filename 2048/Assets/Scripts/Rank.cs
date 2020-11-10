
/*
 * 랭킹 비교
 * 1. 개인랭킹은 1~15등까지 표시된다.
 * 2. 일간, 주간, 종합 랭킹은 개인 랭킹의 최고점이 등록된다.
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rank : MonoBehaviour
{
    public GameObject soloRankPanel;
    public GameObject dailyRankPanel;
    public GameObject weekRankPanel;
    public GameObject totalRankPanel;
    public GameObject currentRankPanel;

    public GameObject goToMain;

    public static Rank instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentRankPanel = soloRankPanel;
        soloRankPanel.SetActive(true);
        dailyRankPanel.SetActive(false);
        weekRankPanel.SetActive(false);
        totalRankPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //개인 랭킹 갱신
    public void SoloRank()
    {

    }

    #region 버튼

    public void OnClickSolo()
    {
        if(currentRankPanel != null)
        {
            currentRankPanel.SetActive(false);
        }
        currentRankPanel = soloRankPanel;
        currentRankPanel.SetActive(true);
    }


    public void OnClickDaily()
    {
        if (currentRankPanel != null)
        {
            currentRankPanel.SetActive(false);
        }
        currentRankPanel = dailyRankPanel;
        currentRankPanel.SetActive(true);
    }


    public void OnClickWeek()
    {
        if (currentRankPanel != null)
        {
            currentRankPanel.SetActive(false);
        }
        currentRankPanel = weekRankPanel;
        currentRankPanel.SetActive(true);
    }


    public void OnClickTotal()
    {
        if (currentRankPanel != null)
        {
            currentRankPanel.SetActive(false);
        }
        currentRankPanel = totalRankPanel;
        currentRankPanel.SetActive(true);
    }

    public void OnClickClose()
    {
        goToMain.SetActive(true);
    }
    #endregion
}
