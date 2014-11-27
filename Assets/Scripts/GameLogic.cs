using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Points))]
public class GameLogic : MonoBehaviour
{

    public int WinningDifference = 2;
    private Points _points;
    public Font Font;

    void Start()
    {
        _points = GetComponent<Points>();
        GUI.color = Color.white;
    }

    void OnGUI()
    {
        #if UNITY_EDITOR   
        if (GUI.Button(new Rect(10f, 10f, 100f, 20f), "Restart Level")) 
        {
            EndRound();
        }
        if (GUI.Button(new Rect(120f, 10f, 100f, 20f), "Reset Points"))
        {
            _points.Reset();
        }
        #endif

        int winner = CheckWinCondition();
        if (winner == 0) return;


        var centeredStyle = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            font = Font,
            fontSize = 50,
            normal = {textColor = winner == 1 ? Color.blue : Color.red}
        };

        GUI.Label(new Rect(0f, 0f, Screen.width, Screen.height), "Player " + winner + " won!", centeredStyle);
    }


    public void EndRound()
    {
        //GUI.Label(new Rect(150f, 150f, 10f, 10f), "New Round!");
        Application.LoadLevel(Application.loadedLevelName);
        _points.Reset();
    }


    /// <summary>
    ///  Check if a player has won
    /// </summary>
    /// <returns>0 if nobody wins, 1 if player 1 wins, 2 for player 2</returns>
    public int CheckWinCondition()
    {
        var diff = _points.GetPoints(1) - _points.GetPoints(2);
        if (diff <= -WinningDifference) return 2;
        return diff >= WinningDifference ? 1 : 0;
    }


    void DrawWinner()
    {

    }
}
