using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    private static int last = -1;

    public string DialoguesFile;
    private Dialogue[] _dialogues = new Dialogue[0];

    private int _currentDialogue;

    public enum DialoguePhase
    {
        Player2,
        Player1Response,
        Player2Response
    }

    void Start()
    {
        if (_dialogues.Length == 0) // do not load if level restarts
            LoadDialogues();
        if (_currentDialogue == last)
        {
            _currentDialogue++; // choose the next one
            _currentDialogue %= _dialogues.Length;
        }
        last = _currentDialogue;

    }

    private void LoadDialogues()
    {
        var sr = new StreamReader(Application.dataPath + "/" + DialoguesFile);
        var fileContents = sr.ReadToEnd();
        sr.Close();

        var dialogues = new List<Dialogue>();
        var lines = fileContents.Split("\n"[0]);
        var i = 1;
        foreach (var line in lines)
        {
            var sentences = line.Split(';');
            if (sentences.Length < 3)
            {
                print("skipped dialogue line " + i + "because it were not 3 phrases");
                continue;
            }
            dialogues.Add(new Dialogue(sentences[0], sentences[1], sentences[2]));
            print("loaded dialogue "+i+": "+line);
            i++;
        }
        _dialogues = dialogues.ToArray();
        var rand = Random.Range(0, _dialogues.Length-1);
        _currentDialogue = rand;
    }

    public Dialogue GetCurrent()
    {
        return _dialogues[_currentDialogue];
    }

}
