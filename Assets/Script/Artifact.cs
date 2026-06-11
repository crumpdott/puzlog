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
            ShopManger.Instance.CheckModal.Open("購入確認", new[]{ArtifactData.Name,$"価格:{ArtifactData.Cost}",ArtifactData.detail},"購入","キャンセル",Purchase);
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
            gameObject.SetActive(false);
        } else
        {
            ShopManger.Instance.CheckModal.Open("所持金不足!",new[]{"買えないよ!"},"確認",null);
        }
    }
}
