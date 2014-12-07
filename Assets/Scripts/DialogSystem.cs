using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    public string DialoguesFile;
    public Dialogue[] Dialogues;

    private Dialogue _currentDialogue;

    public enum DialoguePhase
    {
        Player2,
        Player1Response,
        Player2Response
    }

    void Start()
    {
        LoadDialogues();
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
        Dialogues = dialogues.ToArray();
        var rand = Random.Range(0, Dialogues.Length-1);
        _currentDialogue = Dialogues[rand];
    }

    public Dialogue GetCurrent()
    {
        return _currentDialogue;
    }

}
