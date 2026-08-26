using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextButton : MonoBehaviour
{
    public String NextScene;
    public bool AdvanceRound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (AdvanceRound)
            {
                PlayerData.Round++;
                PlayerData.ReloadCountInCurrentRound = 0;
            }

            SceneManager.LoadScene(NextScene);
        });
    }
}
