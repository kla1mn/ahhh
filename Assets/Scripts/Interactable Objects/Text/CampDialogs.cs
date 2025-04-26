using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


public class CampDialogs : MonoBehaviour
{
    private bool isDialog;
    private bool isSilence;

    private int oldSpeacerId;
    private int oldDialogueId;

    private int currentSpeachInd;
    private int currentDialogueSpeachCount;

    private readonly Random rnd = new();

    private MicroSpeacer currentSpeacker;
    private CampDialogue currentDialogue;

    [SerializeField] private float timeOfSilence;

    [SerializeField] private MicroSpeacer[] speackers;
    [SerializeField] private CampDialogue[] dialogs;

    

    void Update()
    {
        TryStartDialogue();
    }

    private void TryStartDialogue()
    {
        if (!isDialog && !isSilence)
        {
            isSilence = true;
            Invoke("StartDialogue", timeOfSilence);
        }
    }

    private void StartDialogue()
    {
        MakeRandomIndexes();

        currentSpeacker = speackers[oldSpeacerId];
        currentDialogue = dialogs[oldDialogueId];
        currentDialogueSpeachCount = currentDialogue.Speaches.Length;

        isDialog = true;
        isSilence = false;

        NextSpeach();
    }

    private void MakeRandomIndexes()
    {
        var speackerId = oldSpeacerId;
        var dialogueId = oldDialogueId;

        while (speackerId == oldSpeacerId && dialogueId == oldDialogueId)
        {
            speackerId = rnd.Next(0, speackers.Length);
            dialogueId = rnd.Next(0, dialogs.Length);
        }

        oldDialogueId = dialogueId;
        oldSpeacerId = speackerId;
    }

    public void NextSpeach()
    {
        var speach = currentDialogue.Speaches[currentSpeachInd];

        currentSpeacker.StartSpeach(speach);
        currentSpeachInd++;

        if (currentSpeachInd < currentDialogueSpeachCount)
            Invoke("NextSpeach", speach.SpeachTime + 1f);
        else
            EndDialog();
    }

    private void EndDialog()
    {
        isDialog = false;
        currentSpeacker = null;
        currentDialogue = null;
        currentSpeachInd = 0;
        currentDialogueSpeachCount = 0;
    }
}

[System.Serializable]
public class CampDialogue
{
    public MicroSpeach[] Speaches;
}
