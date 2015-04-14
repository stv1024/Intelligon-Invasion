using Assets.Scripts.Data.Proto;
using Edu.CSV;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 关卡按钮
/// </summary>
public class StageButton : MonoBehaviour
{
    public Sprite UnlockRound, LockRound;
    public NaviUI NaviUI;
    public int StageID;
    public CsvConfig StageData;
    public UserStageData UserStageData;

    public GameObject[] StarArcs;

    /// <summary>
    /// userStageData为null表示未解锁
    /// </summary>
    /// <param name="line"></param>
    /// <param name="userStageData"></param>
    public void SetAndRefresh(CsvConfig line, UserStageData userStageData)
    {
        StageID = line["ID"];
        StageData = line;
        UserStageData = userStageData;
        var sprRound = GetComponent<Image>();
        if (null == UserStageData) //未解锁
        {
            sprRound.sprite = LockRound;
            for (var i = 0; i < 3; i++)
            {
                StarArcs[i].SetActive(false);
            }
        }
        else
        {
            sprRound.sprite = UnlockRound;
            if (UserStageData.Id != StageID) Debug.LogError("UserStageData.Id != StageID");
            for (var i = 0; i < 3; i++)
            {
                StarArcs[i].SetActive(i < UserStageData.Stars);
            }
        }
    }
    public void OnClick()
    {
        NaviUI.ToggleStageInfoPanel(this);
    }
}