using TMPro;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    public GameObject ResultDialog;
    public TextMeshProUGUI RemainTimeMoneyUI;
    public TextMeshProUGUI EnemyMoneyUI;
    public GameObject PiggyBankRow;
    public TextMeshProUGUI PiggyBankUI;
    public RectTransform LineTransform;
    public RectTransform SumMoneyTransform;
    public float ExtraRewardRowHeight = 200f;
    public TextMeshProUGUI SumMoneyUI;
    public static ResultManager Instance { get; private set;}
    private Vector2 _LineBasePosition;
    private Vector2 _SumMoneyBasePosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        if (LineTransform != null)
        {
            _LineBasePosition = LineTransform.anchoredPosition;
        }

        if (SumMoneyTransform != null)
        {
            _SumMoneyBasePosition = SumMoneyTransform.anchoredPosition;
        }
    }

    public void StartResult()
    {
        int RemainTimeMoneyValue = PuzzleManager.Instance.GetRemainTime();
        int EnemyMoneyValue = BattleManager.Instance.TmpMoney;
        int PiggyBankValue = PlayerData.PiggyBankTmp;
        PlayerData.PiggyBankTmp = 0;
        int SumMoneyValue = RemainTimeMoneyValue + EnemyMoneyValue + PiggyBankValue;
        ResultDialog.SetActive(true);
        RemainTimeMoneyUI.text = RemainTimeMoneyValue.ToString();
        EnemyMoneyUI.text = EnemyMoneyValue.ToString();
        UpdatePiggyBankRow(PiggyBankValue);
        SumMoneyUI.text = SumMoneyValue.ToString();
        PlayerData.Money += SumMoneyValue;
    }

    private void UpdatePiggyBankRow(int piggyBankValue)
    {
        bool shouldShow = piggyBankValue > 0;

        if (PiggyBankRow != null)
        {
            PiggyBankRow.SetActive(shouldShow);
        }

        if (PiggyBankUI != null)
        {
            PiggyBankUI.text = piggyBankValue.ToString();
        }

        float offsetY = shouldShow ? ExtraRewardRowHeight : 0f;

        if (LineTransform != null)
        {
            LineTransform.anchoredPosition = _LineBasePosition + Vector2.down * offsetY;
        }

        if (SumMoneyTransform != null)
        {
            SumMoneyTransform.anchoredPosition = _SumMoneyBasePosition + Vector2.down * offsetY;
        }
    }
}
