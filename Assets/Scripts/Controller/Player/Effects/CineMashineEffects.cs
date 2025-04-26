using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CineMashineEffects : MonoBehaviour
{
    public float DefaultFOV = 58f;
    public float SlideFOV = 63f;
    public float InventoryFOV = 16f;
    public float DialogueFOV = 50f;
    public float DeathOrCampFOV = 35f;
    public float SimpleDialogueFOV = 42f;

    private bool isAttackShaking;
    private bool isDamageShaking;
    private bool isOpeningInventory;
    private bool isClosingInventory;
   
    private float desiredFOV;
    private float speed = 10f;
    private float confiderResizingSpeed = 12f;
    private float fastSpeed = 100f;
    private float normalSpeed = 10f;
    private float dialogueSpeed = 35f;

    [SerializeField] private float shakeDuration;
    private float attackShakeMagnitude = 0.05f;
    private float damageShakeMagnitude = 1;
    private float dampingSpeed = 0.5f;

    private float defaultConfiderMaxSize = 5f;
    private float largeConfiderMaxSize = 1f;

    Vector3 initialPosition;

    private CinemachineVirtualCamera virtualCamera;

    [Header("Confider")]
    [SerializeField] private CinemachineConfiner2D confider;

    [SerializeField] private Transform cameraPoint;

    private void Awake()
    {
        desiredFOV = DefaultFOV;
    }

    public float DesiredFOV
    {
        get => desiredFOV;
        set 
        {
            if (value == DefaultFOV)
                desiredFOV = value;

            if (value == SlideFOV)
                desiredFOV = value;

            if (value == InventoryFOV)
            {
                desiredFOV = value;
                FastMode();
            }

            if (value == DialogueFOV)
                desiredFOV = value;

            if (value == DeathOrCampFOV) 
                desiredFOV = value;

            if (value == SimpleDialogueFOV)
                desiredFOV = value;
        }
    }

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        SettingFOV();

        Shaking();

        //SettingConfiderSize();
    }
    
    private void SettingConfiderSize()
    {
        if (isOpeningInventory)
            ConfiderResizing(true);
        if (isClosingInventory)
            ConfiderResizing(false);
    }

    private void ConfiderResizing(bool isIncreasing)
    {
        var confiderWindowSize = confider.m_MaxWindowSize;
        var necessarySize = isIncreasing ? largeConfiderMaxSize : defaultConfiderMaxSize;

        confiderWindowSize += Time.unscaledDeltaTime * confiderResizingSpeed * (isIncreasing ? -1 : 1);
        confider.m_MaxWindowSize = confiderWindowSize;

        if (Math.Abs(confiderWindowSize - necessarySize) < .1f)
        {
            if (isIncreasing)
            {
                isOpeningInventory = false;
                Time.timeScale = 0;
            }
            else 
                isClosingInventory = false;

            confider.m_MaxWindowSize = necessarySize;


             
        }
            
    }
    
    

    private void Shaking()
    {
        if (isAttackShaking && shakeDuration > 0f)
        {
            Shake(attackShakeMagnitude);
        }
        else if (isAttackShaking)
        {
            StopShake();
            isAttackShaking = false;
        }

        if (isAttackShaking && shakeDuration > 0f)
            Shake(damageShakeMagnitude);
        else if (isAttackShaking)
        {
            StopShake();
            isDamageShaking = false;
        }
    }

    private void Shake(System.Single magnitude)
    {
        transform.localPosition = initialPosition + UnityEngine.Random.insideUnitSphere*attackShakeMagnitude;

        shakeDuration -= Time.unscaledDeltaTime * dampingSpeed;
    }
    private void StopShake()
    {
        shakeDuration = 0f;
        transform.localPosition = initialPosition;

        virtualCamera.Follow = cameraPoint;
    }



    private void SettingFOV()
    {
        if (virtualCamera.m_Lens.FieldOfView < desiredFOV)
            PlusFOV();
        if (virtualCamera.m_Lens.FieldOfView > desiredFOV)
            MinusFOV();
    }

    private void MinusFOV()
    {
        virtualCamera.m_Lens.FieldOfView -= Time.unscaledDeltaTime * speed;

        if (virtualCamera.m_Lens.FieldOfView < desiredFOV)
        {
            virtualCamera.m_Lens.FieldOfView = desiredFOV;
            if (desiredFOV == DefaultFOV)
                NormalMode();
        }
    }

    private void PlusFOV()
    {
        virtualCamera.m_Lens.FieldOfView += Time.unscaledDeltaTime * speed;

        if (virtualCamera.m_Lens.FieldOfView > desiredFOV) 
        { 
            virtualCamera.m_Lens.FieldOfView = desiredFOV;
            if (desiredFOV == DefaultFOV)
                NormalMode();
        }
    }

    public void FastMode() => speed = fastSpeed;
    public void DialogueMod() => speed = dialogueSpeed;
    public void NormalMode() => speed = normalSpeed;

    public void StartAttackShake()
    {
        if (!isDamageShaking)
        {
            isAttackShaking = true;
            shakeDuration = 0.1f;

            virtualCamera.Follow = null;

            initialPosition = transform.localPosition;
        }   
    }
    public void StartDamageShake()
    {
        StopShake();
        isAttackShaking = false;

        isDamageShaking = true;
        shakeDuration = 0.45f;

        virtualCamera.Follow = null;

        initialPosition = transform.localPosition;
    }

    public void OpenInventory()
    {
        DesiredFOV = InventoryFOV;
        isOpeningInventory = true;
    }

    public void CloseInventory()
    {
        DesiredFOV = DefaultFOV;
        isClosingInventory = true;
        Time.timeScale = 1.0f;
    }
}
