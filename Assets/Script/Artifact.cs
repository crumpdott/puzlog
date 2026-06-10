using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Artifact : MonoBehaviour
{
    public Image IconImg;
    public TextMeshProUGUI CostUI;
    public ArtifactData ArtifactData;
    public Button ArtifactButton;
    void Start()
    {
        ArtifactButton.onClick.AddListener(() =>
        {
            ShopManger.Instance.CheckModal.Open("購入確認", "このアーティファクトを買いますか？","購入","キャンセル",Purchase);
        });
    }

    public void SetData(ArtifactData data)
    {
        ArtifactData = data;
        IconImg.sprite = data.Icon;
        CostUI.text = data.Cost.ToString();

    }

    public void Purchase()
    {
        if(PlayerData.Money >= ArtifactData.Cost)
        {
            PlayerData.Money -= ArtifactData.Cost;
            ShopManger.Instance.UpdatePlayerMoney();
        }
    }
}
