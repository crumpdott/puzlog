using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI PlayerMoneyUI;
    [FormerlySerializedAs("ArtifactPrefabs")]
    public ArtifactShopItem[] ArtifactSlots;
    public ArtifactData[] ArtifactDatas;
    public static ShopManager Instance { get; private set;}
    public CheckModal CheckModal;
    void Start()
    {
        Instance = this;
        UpdatePlayerMoney();
        ShowArtifact();
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
}
