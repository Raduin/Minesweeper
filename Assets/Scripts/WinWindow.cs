using UnityEngine;
using TMPro;

public class WinWindow : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private TMP_Text winMessage;
    [SerializeField] private Mouse mouseScript;
    [SerializeField] private GameObject winWindow;
    [SerializeField] private GameObject winWindowShadow;
    public TMP_InputField playerName;

    private void OnEnable()
    {
        if (game.beginnerOn) winMessage.text = "You have the fastest time for beginner level. Please enter your name.";
        if (game.intermediateOn) winMessage.text = "You have the fastest time for intermediate level. Please enter your name.";
        if (game.expertOn) winMessage.text = "You have the fastest time for expert level. Please enter your name.";
        playerName.text = "Anonymous";
        playerName.Select();
    }


    public void ButtonOkPressed()
    {
        if (game.beginnerOn) game.beginnerName = playerName.text;
        if (game.intermediateOn) game.intermediateName = playerName.text;
        if (game.expertOn) game.expertName = playerName.text;
        SaveScore();
        mouseScript.enabled = true;
        game.enabled = true;
        winWindow.SetActive(false);
        winWindowShadow.SetActive(false);
    }

    private void SaveScore()
    {
        PlayerPrefs.SetString("BeginnerName", game.beginnerName);
        PlayerPrefs.SetInt("BeginnerTime", game.beginnerTime);
        PlayerPrefs.SetString("IntermediateName", game.intermediateName);
        PlayerPrefs.SetInt("IntermediateTime", game.intermediateTime);
        PlayerPrefs.SetString("ExpertName", game.expertName);
        PlayerPrefs.SetInt("ExpertTime", game.expertTime);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) ButtonOkPressed();
    }
}
