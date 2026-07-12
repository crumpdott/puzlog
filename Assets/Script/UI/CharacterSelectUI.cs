using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Button generalPersonButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private GameObject characterDetailPanel;
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private TMP_Text characterDescriptionText;

    private void Start()
    {
        generalPersonButton.onClick.AddListener(ShowGeneralPerson);
        confirmButton.onClick.AddListener(OpenDifficultySelect);
        confirmButton.interactable = false;
        characterDetailPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        generalPersonButton.onClick.RemoveListener(ShowGeneralPerson);
        confirmButton.onClick.RemoveListener(OpenDifficultySelect);
    }

    private void ShowGeneralPerson()
    {
        characterDetailPanel.SetActive(true);
        characterNameText.text = "一般人";
        characterDescriptionText.text = "特になし";
        confirmButton.interactable = true;
    }

    private void OpenDifficultySelect()
    {
        SceneManager.LoadScene("DifficultySelect");
    }
}
