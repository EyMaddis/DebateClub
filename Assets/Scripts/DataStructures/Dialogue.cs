using System;

[Serializable]
public class Dialogue
{
    public string Player2;
    public string Player1Response;
    public string Player2Response;

    public Dialogue(string player2, string player1Response, string player2Response)
    {
        Player2 = player2;
        Player1Response = player1Response;
        Player2Response = player2Response;
    }
}