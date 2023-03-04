using UnityEngine;

public class MenuSettings : MonoBehaviour
{
     [SerializeField] private Game game;

    public void QuestionMark()
    {
        game.questionMark = !game.questionMark;
    }
}
