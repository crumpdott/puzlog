using TMPro;
using UnityEngine;

public class ShopManger : MonoBehaviour
{
    public TextMeshProUGUI PlayerMoneyUI;
    public static ShopManger Instance { get; private set;}
    void Start()
    {
        Instance = this;
        UpdatePlayerMoney();
    }

    public void UpdatePlayerMoney()
    {
        PlayerMoneyUI.text = PlayerData.Money.ToString();
    }
}
