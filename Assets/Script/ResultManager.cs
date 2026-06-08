using TMPro;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    public GameObject ResultDialog;
    public TextMeshProUGUI RemainTimeMoneyUI;
    public TextMeshProUGUI EnemyMoneyUI;
    public TextMeshProUGUI SumMoneyUI;
    public static ResultManager Instance { get; private set;}
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
    }

    public void StartResult()
    {
        int RemainTimeMoneyValue = PuzzleManager.Instance.GetRemainTime();
        int EnemyMoneyValue = BattleManager.Instance.TmpMoney;
        int SumMoneyValue = RemainTimeMoneyValue + EnemyMoneyValue;
        ResultDialog.SetActive(true);
        RemainTimeMoneyUI.text = RemainTimeMoneyValue.ToString();
        EnemyMoneyUI.text = EnemyMoneyValue.ToString();
        SumMoneyUI.text = SumMoneyValue.ToString();
        PlayerData.Money += SumMoneyValue;
    }
}
