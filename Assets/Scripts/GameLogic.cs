using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Points))]
public class GameLogic : MonoBehaviour
{

    public int WinningDifference = 2;
    public Font Font;

    public GameObject Player1;
    public GameObject Player2; 



    private bool _firstRound;
    private static int _round;
    private float _roundTime;
    private Points _points;

    private Vector2 _player1StartPosition;
    private Vector2 _player2StartPosition;

    private int _player1StartDirection;
    private int _player2StartDirection;

    private bool _isWaitingForEnd;


    void Start()
    {
        _player1StartPosition = Player1.transform.position;
        _player2StartPosition = Player2.transform.position;

        _player1StartDirection = Player1.GetComponent<Character>().Direction;
        _player2StartDirection = Player2.GetComponent<Character>().Direction;

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
			Application.LoadLevel(1);
            EndRound();
        }
        if (GUI.Button(new Rect(120f, 10f, 100f, 20f), "Reset Points"))
        {
            _points.Reset();
        }
        #endif

		if (GUI.Button(new Rect(230f, 10f, 100f, 20f), "Main Menu")) 
		{
			Application.LoadLevel(0);
		}


        

        _roundTime += Time.deltaTime;
        Utils.DrawRound(_round,_roundTime,Font);

        int winner = CheckWinCondition();
        if (winner == 0) return;
        Utils.DrawWinner(winner,Font);
        
        if(_isWaitingForEnd) return;
        StartCoroutine("WaitForEnd");

    }


    public void EndRound()
    {

        Player1.transform.position = _player1StartPosition;
        Player2.transform.position = _player2StartPosition;
        Player1.GetComponent<Character>().Direction = _player1StartDirection;
        Player2.GetComponent<Character>().Direction = _player2StartDirection;

        _points.Reset();
        _round++;
        _firstRound = false;
        _roundTime = 0;
        _isWaitingForEnd = false;
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


    IEnumerator WaitForEnd()
    {
        _isWaitingForEnd = true;
        Player1.GetComponent<Character>().BlockInput(true);
        Player2.GetComponent<Character>().BlockInput(true);
        yield return new WaitForSeconds(2);
        EndRound();
        Player1.GetComponent<Character>().BlockInput(false);
        Player2.GetComponent<Character>().BlockInput(false);
        
        
    }

}
