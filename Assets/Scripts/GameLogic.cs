using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Points))]
public class GameLogic : MonoBehaviour
{

    public int WinningDifference = 2;
    public Font Font;

    private bool _firstRound;
    private int _round;
    private float _roundTime;
    private Points _points;
        

    void Start()
    {
        _round = 0;
        _firstRound = true;
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



        _roundTime += Time.deltaTime;
        Utils.DrawRound(_round,_roundTime,Font);

        int winner = CheckWinCondition();
        if (winner == 0) return;
        Utils.DrawWinner(winner,Font);

    }


    public void EndRound()
    {
        //GUI.Label(new Rect(150f, 150f, 10f, 10f), "New Round!");
        Application.LoadLevel(Application.loadedLevelName);
        _points.Reset();
        _round++;
        _firstRound = false;
        _roundTime = 0;

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
