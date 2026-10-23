using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI PlayerMoneyUI;
    [FormerlySerializedAs("ArtifactPrefabs")]
    public ArtifactShopItem[] ArtifactSlots;
    public ArtifactData[] ArtifactDatas;
    public static ShopManager Instance { get; private set;}
    public CheckModal CheckModal;
    public Button ReloadButton;
    public TextMeshProUGUI ReloadButtonText;

    public int ReloadCount => PlayerData.ReloadCountInCurrentRound;
    public int CurrentReloadCost => PlayerData.Round * 2 + PlayerData.ReloadCountInCurrentRound;

    void Start()
    {
        Instance = this;
        for (int i = 0; i < ArtifactSlots.Length; i++) ArtifactSlots[i].SlotIndex = i;
        ReloadButton.onClick.AddListener(ReloadArtifacts);
        UpdatePlayerMoney();
        UpdateReloadButton();
        ShowArtifact();
    }

    private void OnDestroy()
    {
        ReloadButton.onClick.RemoveListener(ReloadArtifacts);
    }

    private void ShowArtifact()
    {
        for (int i = 0; i < ArtifactSlots.Length; i++)
            PlayerData.ShopOffers[i] = PlayerData.KeptSlots[i] ? PlayerData.KeptOffers[i] : null;

        var available = ArtifactDatas
            .Where(data => data != null && data.NameId != ArtifactNameId.None && data.Cost >= 0)
            .GroupBy(data => data.NameId)
            .Select(group => group.First())
            .Where(data => !PlayerData.ShopOffers.Contains(data))
            .OrderBy(_ => Random.value)
            .ToList();

        for (int i = 0; i < ArtifactSlots.Length; i++)
        {
            if (!PlayerData.KeptSlots[i] && available.Count > 0)
            {
                PlayerData.ShopOffers[i] = available[0];
                available.RemoveAt(0);
            }
            bool hasData = PlayerData.ShopOffers[i] != null;
            ArtifactSlots[i].gameObject.SetActive(hasData);

            if (hasData)
            {
                ArtifactSlots[i].SetData(PlayerData.ShopOffers[i]);
            }
        }
        UpdateReloadButton();
    }

    public void ToggleKeep(int slot)
    {
        if (slot < 0 || slot >= ArtifactSlots.Length || PlayerData.ShopOffers[slot] == null) return;
        PlayerData.KeptSlots[slot] = !PlayerData.KeptSlots[slot];
        PlayerData.KeptOffers[slot] = PlayerData.KeptSlots[slot] ? PlayerData.ShopOffers[slot] : null;
        ArtifactSlots[slot].SetData(PlayerData.ShopOffers[slot]);
        UpdateReloadButton();
    }

    public void PurchaseSlot(int slot)
    {
        PlayerData.ShopOffers[slot] = null;
        PlayerData.KeptOffers[slot] = null;
        PlayerData.KeptSlots[slot] = false;
        UpdateReloadButton();
    }

    public void UpdatePlayerMoney()
    {
        PlayerMoneyUI.text = PlayerData.Money.ToString();
    }

    private void UpdateReloadButton()
    {
        ReloadButtonText.text = $"リロード\n{CurrentReloadCost}円";
        ReloadButton.interactable = PlayerData.KeptSlots.Take(ArtifactSlots.Length).Any(kept => !kept);
    }

    public void ReloadArtifacts()
    {
        if (PlayerData.KeptSlots.Take(ArtifactSlots.Length).All(kept => kept)) return;
        int cost = CurrentReloadCost;

        if (PlayerData.Money < cost)
        {
            CheckModal.Open("所持金不足!", new[] {"リロードできないよ!"}, "確認", null);
            return;
        }

        PlayerData.Money -= cost;
        PlayerData.ReloadCountInCurrentRound++;
        UpdatePlayerMoney();
        UpdateReloadButton();
        ShowArtifact();
    }
}
