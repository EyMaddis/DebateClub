using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Points))]
public class GameLogic : MonoBehaviour
{

    private Points _points;

    void Start()
    {
        _points = GetComponent<Points>();
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
    }

    public void EndRound()
    {
        //GUI.Label(new Rect(150f, 150f, 10f, 10f), "New Round!");
        Application.LoadLevel(Application.loadedLevelName);
        _points.Reset();
    }
}
