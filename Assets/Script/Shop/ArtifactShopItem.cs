using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactShopItem : MonoBehaviour
{
    public Image IconImg;
    public TextMeshProUGUI CostUI;
    public ArtifactData ArtifactData;
    public Button ArtifactButton;
    public Button KeepButton;
    public TextMeshProUGUI KeepButtonText;
    public int SlotIndex;

    void Start()
    {
        ArtifactButton.onClick.AddListener(() =>
        {
            ShopManager.Instance.CheckModal.Open("購入確認", new[]{ArtifactData.Name,$"価格:{ArtifactData.Cost}",ArtifactData.detail},"購入","キャンセル",Purchase);
        });
        if (KeepButton != null) KeepButton.onClick.AddListener(() => ShopManager.Instance.ToggleKeep(SlotIndex));
    }

    public void SetData(ArtifactData data)
    {
        ArtifactData = data;
        IconImg.sprite = data.Icon;
        CostUI.text = data.Cost.ToString();
        if (KeepButtonText != null) KeepButtonText.text = PlayerData.KeptSlots[SlotIndex] ? "解除" : "キープ";
    }

    public void Purchase()
    {
        if(PlayerData.Money >= ArtifactData.Cost)
        {
            PlayerData.Money -= ArtifactData.Cost;
            ShopManager.Instance.UpdatePlayerMoney();
            PlayerData.Artifacts.Add(ArtifactData.NameId);
            ShopManager.Instance.PurchaseSlot(SlotIndex);
            gameObject.SetActive(false);
        } else
        {
            ShopManager.Instance.CheckModal.Open("所持金不足!",new[]{"買えないよ!"},"確認",null);
        }
    }
}
