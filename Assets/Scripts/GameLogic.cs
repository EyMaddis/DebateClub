using System;
using System.Resources;
using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour
{

    public int RoundWinningDifference = 1;
    public int WinningDifference = 2;
    public Font Font;

    public GameObject Player1;
    public GameObject Player2;

    public int MaxPoints;
    public Animator Intro;

    [Header("GUI")] 
    
    public Color TextOutlineColor;
    public Color RoundColorStart;
    public Color RoundColorEnd;

    public Color[] PlayerWinColors = {Color.blue, Color.red};

    public Sprite Medal;

    private static readonly int[] PlayerPoints = { 0, 0 };
    private static readonly int[] PlayerRoundsWon = {0, 0};

    private static int _round;
    private float _roundTime;
    
    private Vector2 _player1StartPosition;
    private Vector2 _player2StartPosition;

    private int _player1StartDirection;
    private int _player2StartDirection;

    private bool _isWaitingForEnd;

    private bool _introStopped = false;
    private GUIStyle _roundStyle;


    void Start()
    {
        _player1StartPosition = Player1.transform.position;
        _player2StartPosition = Player2.transform.position;

        _player1StartDirection = Player1.GetComponent<Character>().Direction;
        _player2StartDirection = Player2.GetComponent<Character>().Direction;

        _roundStyle = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            font = Font,
            fontSize = 50,
            richText = true,
            normal = { textColor =  RoundColorStart}
        };

        _round = 1;
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
            ResetPoints(true);
        }
        #endif

		if (GUI.Button(new Rect(230f, 10f, 100f, 20f), "Main Menu")) 
		{
			Application.LoadLevel(0);
		}

        Utils.DrawPoints(PlayerPoints, Font);
        
        if(_introStopped)
            _roundTime += Time.deltaTime;

        DrawWinner();
        DrawRound();
    }


    public void EndRound(bool endGame = false)
    {

        Player1.transform.position = _player1StartPosition;
        Player2.transform.position = _player2StartPosition;
        Player1.transform.localScale = new Vector3(_player1StartDirection,1,1);
        Player2.transform.localScale = new Vector3(_player2StartDirection,1,1);
       
        Player1.GetComponent<Character>().Direction = _player1StartDirection;
        Player2.GetComponent<Character>().Direction = _player2StartDirection;

        ResetPoints(endGame);
        _round++;
        _roundTime = 0;
        _isWaitingForEnd = false;
    }


    /// <summary>
    ///  Check if a player has won
    /// </summary>
    /// <returns>0 if nobody wins, 1 if player 1 wins, 2 for player 2</returns>
    public int CheckRoundWinCondition()
    {
        if (PlayerPoints[0] >= MaxPoints)
            return 1;
        return PlayerPoints[1] >= MaxPoints ? 2 : 0;
    }

    public int CheckWinCondition()
    {
        var diff = PlayerRoundsWon[0] - PlayerRoundsWon[1];
        if (diff <= -RoundWinningDifference) return 2;
        return diff >= RoundWinningDifference ? 1 : 0;
    }


    private IEnumerator WaitForEnd(bool endRoundOnly)
    {
        _isWaitingForEnd = true;
        var players = new []{Player1, Player2};
        
        foreach (var player in players)
        {
            player.GetComponent<Character>().BlockInput(true);
        }

        yield return new WaitForSeconds(2);
        EndRound(endRoundOnly);

        foreach (var player in players)
        {
            player.GetComponent<Character>().BlockInput(false);
        }
    }

    public void AddPoints(int playerID, int points)
    {
        Debug.Log("player" + playerID + " got a point");
        PlayerPoints[playerID + -1] += points;
    }

    public void ResetPoints(bool endGame)
    {
        PlayerRoundsWon[0] = 0;
        PlayerRoundsWon[1] = 0;
        if (!endGame) return;
        PlayerPoints[0] = 0;
        PlayerPoints[1] = 0;
    }


    private void DrawRound()
    {
        var box = new Rect(Screen.width / 2f - 100f, Screen.height / 2f - 50f, 200f, 100f);
        if (_introStopped)
        {
            var lerpTime = Math.Max(0, _roundTime-1);
           
            box = new Rect(
                Mathf.Lerp(box.xMin, Screen.width - box.width - 15, lerpTime),
                Mathf.Lerp(box.yMin, 0, lerpTime),
                box.width, box.height);
            
            _roundStyle.fontSize = (int) Mathf.Lerp(50f, 20f, lerpTime);
            _roundStyle.normal.textColor = Color.Lerp(RoundColorStart, RoundColorEnd, lerpTime);

            GUI.Label(box, "Round " + _round, _roundStyle);
            Utils.DrawOutline(box, "Round " + _round, _roundStyle, TextOutlineColor, (int)Mathf.Lerp(3, 2, lerpTime));
        }
    }

    public void DeclareIntroEnd()
    {
        _introStopped = true;
    }

    private void DrawWinner()
    {



        var winner = CheckWinCondition();
        var roundWinner = CheckRoundWinCondition();
        var end = winner != 0 || roundWinner != 0;

        if (winner == 0 && roundWinner == 0) return; // no need to do draw the winner/end the round.
        var endRoundOnly = roundWinner != 0 && winner == 0;

        if (end && !_isWaitingForEnd) 
            StartCoroutine("WaitForEnd", endRoundOnly);
     
        var player = roundWinner;

        Debug.Log("Round: "+roundWinner+" game:" + winner);

        if (!endRoundOnly) player = winner;
        var centeredStyle = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            font = Font,
            fontSize = 50,
            normal = { textColor = PlayerWinColors[player-1] }
        };

        var text = "Player " + player + " got a point";
        if (!endRoundOnly) text = "Player " + player + " won!"; 

        var size = centeredStyle.CalcSize(new GUIContent(text));

        var box = new Rect(Screen.width / 2f - size.x / 2f, Screen.height / 2f - size.y / 2f, size.x, size.y);
        Utils.DrawOutline(box, text, centeredStyle, TextOutlineColor, 2);
    }
}
