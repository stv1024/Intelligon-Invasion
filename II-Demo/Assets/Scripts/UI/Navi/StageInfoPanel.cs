using Assets.Scripts.Data.Proto;
using Edu.CSV;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 关卡信息面板
/// </summary>
public class StageInfoPanel : MonoBehaviour
{
    public Vector2 Offset = new Vector2(-21.1484f, -21.1484f);
    public Text LblDescription;

    public int StageID;

    void Start()
    {
        //var img = transform.Find("Sprite").GetComponent<Image>();
        //img.Raycast()
    }

    public void SetAndRefresh(int stageID, CsvConfig line, UserStageData userStageData)
    {
        StageID = stageID;
        LblDescription.text = line["Description"];
    }

    public void OnStartStageClick()
    {
        MainController.Instance.EnterStage(StageID);
    }

    
}