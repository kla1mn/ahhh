using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroSpeackStarter : MonoBehaviour
{
    [SerializeField] private bool NeedToInput;
    [SerializeField] private bool canComplete;

    [SerializeField] private GameObject hintObj;
    [SerializeField] private GameObject alertObj;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private MicroSpeacer speacker;

    [Header("speeches")]
    [SerializeField] private MicroSpeach[] speaches;

    private bool behindSpeaker;
    private bool needToDialogue = true;
    private bool isTalking;
    private bool canTalk = true;

    private int currentSpeechInd;

    public bool wasDialogue;

    private InputManager inputManager;
    private bool isDialogueInpit;

    private void Awake()
    {
        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        inputManager.OnUsePerfomed += (snd, args) => isDialogueInpit = !isDialogueInpit;
    }

    private void Start()
    {
        hintObj.SetActive(false);
        alertObj.SetActive(needToDialogue);
    }

    private void Update()
    {
        InputToStartDialogue();
    }

    private void StartSpeech()
    {
        behindSpeaker = false;
        isTalking = true;
        canTalk = canComplete;
        needToDialogue = canComplete;

        NextSpeach();   
        
        alertObj.SetActive(false);
        hintObj.SetActive(false);
    }

    public void NextSpeach()
    {
        var speach = speaches[currentSpeechInd];

        speacker.StartSpeach(speach);
        currentSpeechInd++;

        if (currentSpeechInd < speaches.Length)
            Invoke("NextSpeach", speach.SpeachTime + 1f);
        else
            EndDialogue();
    }

    private void EndDialogue()
    {
        isTalking = false;
        currentSpeechInd = 0;

        wasDialogue = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTalking && canTalk && layerMask == (layerMask | (1 << collision.gameObject.layer)))
        {
            behindSpeaker = true;
            hintObj.SetActive(NeedToInput);
            alertObj.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isTalking && layerMask == (layerMask | (1 << collision.gameObject.layer)))
        {
            behindSpeaker = false;
            hintObj.SetActive(false);
            alertObj.SetActive(needToDialogue);
        }
    }

    private void InputToStartDialogue()
    {
        if (!isTalking && behindSpeaker && (!NeedToInput || isDialogueInpit))
        {
            StartSpeech();
            isDialogueInpit = false;
        }
    }

}
