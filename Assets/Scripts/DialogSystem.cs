using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Collections;

public class DialogSystem : MonoBehaviour
{
    public String DialoguesFile;
    public Dialogue[] Dialogues;

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
        foreach (String line in lines)
        {
            string[] sentences = line.Split(';');
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
    }

    void OnGUI()
    {
        
    }
}
