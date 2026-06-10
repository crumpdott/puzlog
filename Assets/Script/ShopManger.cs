using System.Linq;
using TMPro;
using UnityEngine;

public class ShopManger : MonoBehaviour
{
    public TextMeshProUGUI PlayerMoneyUI;
    public Transform[] ArtifactPositions;
    public Artifact[] ArtifactPrefabs;
    public static ShopManger Instance { get; private set;}
    public CheckModal CheckModal;
    void Start()
    {
        Instance = this;
        UpdatePlayerMoney();
        ShowArtifact();
    }

    private void ShowArtifact()
    {
        var artifacts = ArtifactPrefabs.OrderBy(_ => Random.value).Take(3).ToArray();

        for(int i = 0; i < artifacts.Length; i++)
        {
            Instantiate(artifacts[i],ArtifactPositions[i]);
        }
    }

    public void UpdatePlayerMoney()
    {
        PlayerMoneyUI.text = PlayerData.Money.ToString();
    }
}
