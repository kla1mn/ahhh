using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UPReffects : MonoBehaviour
{
    [SerializeField] private GameObject canvasObj;

    private LensDistortion lensDistortion;
    private Vignette vignette;
    private DepthOfField depthOfField;
    private FilmGrain grain;
    private Bloom bloom;
    private ColorAdjustments colorAdjustments;

    private bool isLensAnimating;
    private bool isStoppingLensAnimation;

    private bool isVignetteOpen;
    private bool isVignetteClosed;
    private bool isVigneteCamp;

    private bool isBloomOpen;
    private bool isBloomClosed = true;

    public bool isDepthOpen;
    public bool isDepthClosed;

    public bool isColorExposuringUp;
    public bool isColorExposuringDown;

    //lens params
    private float lensIntensivity = 0f;
    private float lensAnimationSpeed = 4f;
    //vignette params
    private float vignetteStartValue = .25f;
    private float vignetteEndValue = .35f;
    private float vignetteCampValue = .55f;
    private float vignetteIntensivity;
    public float vignetteAnimationSpeed = .4f;
    //depth params
    public float depthStartValue = 1f;
    public float depthEndValue = 35f;
    public float depthIntencivity;
    public float depthAnimationSpeed = 500f;
    //bloom params
    private float bloomStartValue = .1f;
    private float bloomEndValue = .4f;
    private float bloomAnimationSpeed = .1f;
    //color adjustments
    private float exposureMax = -5f;
    private float contrastMax = 52f;
    public float exposureSpeed = 10f;
    public float contrastSpeed = 300f;
    private bool isPlaingAnimation { get => isLensAnimating | isStoppingLensAnimation | isDepthClosed 
            | isDepthOpen | isVignetteClosed | isVignetteOpen ; }


    private void Awake()
    {
        var effectsVolumeProfile = GameObject.Find("PLAYER_EFFECTS").GetComponent<Volume>()?.profile;
        if (!effectsVolumeProfile)
            Debug.LogError("[URP] Can't find Volume Profile");

        if (!effectsVolumeProfile.TryGet(out lensDistortion))
            Debug.LogError("[URP] Can't find Lens Distortion in Volume Profile");
        if (!effectsVolumeProfile.TryGet(out vignette))
            Debug.LogError("[URP] Can't find Vignette in Volume Profile");
        if (!effectsVolumeProfile.TryGet(out depthOfField))
            Debug.LogError("[URP] Can't find Depth in Volume Profile");
        if (!effectsVolumeProfile.TryGet(out grain))
            Debug.LogError("[URP] Can't find Grain in Volume Profile");
        if (!effectsVolumeProfile.TryGet(out bloom))
            Debug.LogError("[URP] Can't find Bloom in Volume Profile");
        if (!effectsVolumeProfile.TryGet(out colorAdjustments))
            Debug.LogError("[URP] Can't find Bloom in Volume Profile");
    }
    private void Update()
    {
        AnimatingLensDistortion();
        AnimatingVignette();
        AnimatingDepthOfField();
        AnimatingBloom();
        AnimatingColorAdjustments();
    }

    private void AnimatingColorAdjustments()
    {
        if (isColorExposuringUp)
        {
            if (colorAdjustments.contrast.value == contrastMax)
                isColorExposuringUp = false;

            if (colorAdjustments.contrast.value < contrastMax)
                colorAdjustments.contrast.value += 2 * Time.unscaledDeltaTime * contrastSpeed;
            else
                colorAdjustments.contrast.value = contrastMax;

        }

        if (isColorExposuringDown)
        {
            if (colorAdjustments.contrast.value == 0)
                isColorExposuringDown = false;

            if (colorAdjustments.contrast.value > 0)
                colorAdjustments.contrast.value -= Time.unscaledDeltaTime * contrastSpeed;
            else
                colorAdjustments.contrast.value = 0;

        }
    }

    private void AnimatingBloom()
    {
        if (isBloomOpen && bloom.intensity.value < bloomEndValue)
            bloom.intensity.value += bloomAnimationSpeed * Time.unscaledDeltaTime;
        if (isBloomOpen && bloom.intensity.value > bloomEndValue)
            bloom.intensity.value = bloomEndValue;

        if (isBloomClosed && bloom.intensity.value > bloomStartValue)
            bloom.intensity.value -= bloomAnimationSpeed * Time.unscaledDeltaTime;
        if (isBloomClosed && bloom.intensity.value < bloomStartValue)
            bloom.intensity.value = bloomStartValue;
    }

    private void AnimatingLensDistortion()
    {
        if (isLensAnimating)
        {
            lensIntensivity -= lensAnimationSpeed * Time.deltaTime;
            
            lensDistortion.intensity.Override(lensIntensivity);

            if (lensIntensivity < -1f)
            {
                isLensAnimating = false;
            }

            lensDistortion.intensity.Override(lensIntensivity);
        }
        if (isStoppingLensAnimation)
        {
            lensIntensivity += lensAnimationSpeed * Time.deltaTime/3f;

            if (lensIntensivity > 0f)
            {
                lensIntensivity = 0f;
                isStoppingLensAnimation = false;
            }
            lensDistortion.intensity.Override(lensIntensivity);
        }
    }

    private void AnimatingVignette()
    {
        if (isVignetteOpen)
        {
            vignette.intensity.Override(vignetteIntensivity);
            if (NeedToEndTimer(ref vignetteIntensivity, vignetteAnimationSpeed, vignetteEndValue, true))
            {
                isVignetteOpen = false;
                vignette.intensity.Override(vignetteIntensivity);
            }
        }

        if (isVignetteClosed)
        {
            vignette.intensity.Override(vignetteIntensivity);
            if (NeedToEndTimer(ref vignetteIntensivity, vignetteAnimationSpeed, vignetteStartValue, false))
            {
                isVignetteClosed = false;
                vignette.intensity.Override(vignetteIntensivity);
                vignette.active = false;
            }
        }

        if (isVigneteCamp)
        {
            vignette.intensity.Override(vignetteIntensivity);
            if (NeedToEndTimer(ref vignetteIntensivity, .3f * vignetteAnimationSpeed, vignetteCampValue, false))
            {
                isVigneteCamp = false;
                vignette.intensity.Override(vignetteIntensivity);
            }
        }
    }
    private void AnimatingDepthOfField()
    {
        if (isDepthOpen)
        {
            depthOfField.focalLength.Override(depthIntencivity);
            if (NeedToEndTimer(ref depthIntencivity, depthAnimationSpeed, depthEndValue, true))
            {
                isDepthOpen = false;
                StartDepthOfFieldAnimation(false);
            }
        }

        if (isDepthClosed)
        {
            depthOfField.focalLength.Override(depthIntencivity);
            if (NeedToEndTimer(ref depthIntencivity, depthAnimationSpeed, depthStartValue, false))
            {
                isDepthClosed = false;
                depthOfField.focalLength.Override(depthIntencivity);
                depthOfField.active = false;
            }
        }
    }

    private bool NeedToEndTimer(ref float intensivity, float speed, float endValue, bool isEnlarge)
    {
        var norma = isEnlarge ? 1 : -1;
        intensivity += norma * speed * Time.unscaledDeltaTime;

        if (isEnlarge && intensivity > endValue || !isEnlarge && intensivity < endValue)
        {
            intensivity = endValue;
            return true;
        }

        return false;
    }

    public void StartLensDistortionAnimation() => isLensAnimating = !isPlaingAnimation;
    public void StartCloselensDistortionAnimation()
    {
        lensIntensivity = -1f;
        isStoppingLensAnimation = true;

        lensDistortion.intensity.Override(lensIntensivity);
    }

    public void StartVingnetteAnimation(bool value)
    {
        if (!isPlaingAnimation)
        {
            isVignetteOpen = value;
            isVignetteClosed = !value;

            vignette.active = true;
            vignetteIntensivity = value ? vignetteStartValue : vignetteEndValue;
        }
    }

    public void StartDepthOfFieldAnimation(bool value)
    {
        if (!isPlaingAnimation)
        {
            isDepthOpen = value;
            isDepthClosed = !value;

            depthOfField.active = true;
            depthIntencivity = value ? depthStartValue : depthEndValue;
        }
    }

    public void StartDepthOfFieldAnimation()
    {
        StartDepthOfFieldAnimation(true);
    }

    public void EnableGrain()
    {
        if (!grain.active)
        {
            grain.active = true;

            isBloomOpen = true;
            isBloomClosed = false;
        }
    }
    public void DisableGrain()
    {
        if (grain.active)
        {
            grain.active = false;

            isBloomOpen = false;
            isBloomClosed = true;
        }
    }

    public void StartCampSittingAnimation()
    {
        isColorExposuringUp = true;
        canvasObj.SetActive(false);
    }

    public void StartInterfaceAnimation() => isColorExposuringUp = true;

    public void StartCampLoadAnimation()
    {
        isColorExposuringDown = true;

        if (colorAdjustments != null)
        {
            colorAdjustments.contrast.value = contrastMax;
        }
    }
}
