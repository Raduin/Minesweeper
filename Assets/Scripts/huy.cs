using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    private Game game;

    private void Awake()
    {
        game = GetComponent<Game>();
    }
    public void QuestionMark()
    {
        game.questionMark = !game.questionMark;
    }
}
