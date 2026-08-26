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
        var selectedDatas = ArtifactDatas
            .OrderBy(_ => Random.value)
            .Take(ArtifactSlots.Length)
            .ToArray();

        for (int i = 0; i < ArtifactSlots.Length; i++)
        {
            bool hasData = i < selectedDatas.Length;
            ArtifactSlots[i].gameObject.SetActive(hasData);

            if (hasData)
            {
                ArtifactSlots[i].SetData(selectedDatas[i]);
            }
        }
    }

    public void UpdatePlayerMoney()
    {
        PlayerMoneyUI.text = PlayerData.Money.ToString();
    }

    private void UpdateReloadButton()
    {
        ReloadButtonText.text = $"リロード\n{CurrentReloadCost}円";
    }

    public void ReloadArtifacts()
    {
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
