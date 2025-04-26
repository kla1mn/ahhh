using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class BurnItDown : MonoBehaviour
{
    private Bloom bloom;
    private LensDistortion lensDistortion;
    private Vignette vignette;
    private ChromaticAberration colorAdjustments;

    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject screenOfObj;

    private bool isBurnAnimation;

    private void Awake()
    {
        var effectsVolumeProfile = GameObject.Find("BURN").GetComponent<Volume>()?.profile;

        effectsVolumeProfile?.TryGet(out lensDistortion);
        effectsVolumeProfile?.TryGet(out vignette);
        effectsVolumeProfile.TryGet(out colorAdjustments);
        effectsVolumeProfile.TryGet(out bloom);
    }

    public void StartBurnAnimation()
    {
        isBurnAnimation = true;

        fire.SetActive(true);
    }

    private void Update()
    {
        if (!isBurnAnimation)
            return;

        bloom.intensity.value = Mathf.Min(11, bloom.intensity.value += 3 * Time.deltaTime);
        lensDistortion.intensity.value = Mathf.Max(-.35f, lensDistortion.intensity.value += -.1f * Time.deltaTime);
        vignette.smoothness.value = Mathf.Min(.8f, vignette.smoothness.value += .5f * Time.deltaTime);
        colorAdjustments.intensity.value = Mathf.Min(1f, colorAdjustments.intensity.value += Time.deltaTime);

        if (bloom.intensity.value == 11)
        {
            isBurnAnimation = false;

        }

        Invoke("EndBurn", 5f);
        Invoke("LoadDemonsDialogue", 7f);
    }

    private void EndBurn() => screenOfObj.SetActive(true);
    private void LoadDemonsDialogue() => SceneLoader.LoadScene(13);
}
