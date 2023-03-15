using UnityEngine;
using TMPro;

public class MenuScore : MonoBehaviour
{
    [SerializeField] private Game game;

    [SerializeField] private TMP_Text beginnerName;
    [SerializeField] private TMP_Text beginnerTime;
    [SerializeField] private TMP_Text intermediateName;
    [SerializeField] private TMP_Text intermediateTime;
    [SerializeField] private TMP_Text expertName;
    [SerializeField] private TMP_Text expertTime;
    [SerializeField] private GameObject scoreMenu;

    private void OnEnable()
    {
        ShowData();
    }

    public void ClearScore()
    {
        PlayerPrefs.DeleteAll();
        game.LoadScore();
        ShowData();
    }

    private void ShowData()
    {
        beginnerName.text = game.beginnerName;
        beginnerTime.text = game.beginnerTime.ToString();
        intermediateName.text = game.intermediateName;
        intermediateTime.text = game.intermediateTime.ToString();
        expertName.text = game.expertName;
        expertTime.text = game.expertTime.ToString();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) scoreMenu.SetActive(false);
    }
}
