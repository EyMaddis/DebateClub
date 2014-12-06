using System;

[Serializable]
public class Dialogue
{
    public string Player1;
    public string Player2Response;
    public string Player1Response;

    public Dialogue(string player1, string player2Response, string player1Response)
    {
        Player1 = player1;
        Player2Response = player2Response;
        Player1Response = player1Response;
    }
}