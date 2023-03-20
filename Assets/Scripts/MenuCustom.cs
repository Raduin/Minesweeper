using UnityEngine;
using TMPro;

public class MenuCustom : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private MainMenu mainMenu;

    public TMP_InputField widthField;
    public TMP_InputField heightField;
    public TMP_InputField minesField;
    [SerializeField] private GameObject customMenu;

    // Min size of the field is 8x8. Max size of the field is 99x99.
    // Min mines count is 1. Max mines count is width*height. Mines default is 10.

    private void Start()
    {
        if (widthField.text == "" || widthField.text == null) widthField.text = "8";
        if (heightField.text == "" || heightField.text == null) heightField.text = "8";
        if (minesField.text == "" || minesField.text == null) minesField.text = "10";

        game.width = System.Int32.Parse(widthField.text);
        game.height = System.Int32.Parse(heightField.text);
        game.mineCount = System.Int32.Parse(minesField.text);

    }

    public void ValueWidthCheck()
    {
        if (widthField.text == "-" || widthField.text == "0") widthField.text = "";
    }

    public void ValueHeightCheck()
    {
        if (heightField.text == "-" || heightField.text == "0") heightField.text = "";
    }

    public void ValueMinesCheck()
    {
        if (minesField.text == "-" || minesField.text == "0") minesField.text = "";
    }

    public void EndWidthField()
    {
        if (widthField.text == "" || widthField.text == null) widthField.text = "8";
        if (System.Int32.Parse(widthField.text) < 8) widthField.text = "8";
        game.width = System.Int32.Parse(widthField.text);
    }

    public void EndHeightField()
    {
        if (heightField.text == "" || heightField.text == null) heightField.text = "8";
        if (System.Int32.Parse(heightField.text) < 8) heightField.text = "8";
        game.height = System.Int32.Parse(heightField.text);
    }

    public void EndMinesField()
    {
        if (minesField.text == "" || minesField.text == null) minesField.text = "10";
        //if (System.Int32.Parse(minesField.text) > (999)) minesField.text = "" + 999;
        if (System.Int32.Parse(minesField.text) > game.width * game.height) minesField.text = "" + game.width * game.height;
        if (System.Int32.Parse(minesField.text) < 1) minesField.text = "1";
        game.mineCount = System.Int32.Parse(minesField.text);
    }

    public void StartCustomField()
    {
        game.width = System.Int32.Parse(widthField.text);
        game.height = System.Int32.Parse(heightField.text);
        if (System.Int32.Parse(minesField.text) > game.width * game.height) minesField.text = "" + game.width * game.height;
        game.mineCount = System.Int32.Parse(minesField.text);
        mainMenu.Flags();
        game.NewGame();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) customMenu.SetActive(false);
    }
}
