using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTransition : MonoBehaviour
{
    [Header("Спрайт круга")] public Sprite circleSprite;

    [Header("Настройки анимации")] public float fadeDuration = 2f;

    [SerializeField] public EnemyHealth enemyHealth;

    private bool isFading;

    void Update()
    {
        if (isFading)
            return;

        if (enemyHealth.IsDead || enemyHealth == null)
            StartCoroutine(FadeAndNext());
    }

    private IEnumerator FadeAndNext()
    {
        isFading = true;


        var canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            var go = new GameObject("EndCanvas");
            canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            go.AddComponent<CanvasScaler>();
            go.AddComponent<GraphicRaycaster>();
        }

        // 2) Создаём Image-круг в центре
        var circleGO = new GameObject("EndCircle");
        circleGO.transform.SetParent(canvas.transform, false);
        var image = circleGO.AddComponent<Image>();
        image.sprite = circleSprite;
        image.color = Color.white;
        image.preserveAspect = true;

        var rt = image.rectTransform;
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(100f, 100f);

        var screenDiag = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height);
        var targetScale = screenDiag / rt.sizeDelta.x;

        circleGO.transform.localScale = Vector3.zero;

        var elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            var s = Mathf.Lerp(0f, targetScale, elapsed / fadeDuration);
            circleGO.transform.localScale = Vector3.one * s;
            yield return null;
        }

        circleGO.transform.localScale = Vector3.one * targetScale;

        ToNextLevel();
    }

    private static void ToNextLevel()
    {
        var nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        var sceneCount = SceneManager.sceneCountInBuildSettings;
        if (nextLevel >= sceneCount)
            Debug.Log("Is already last level. Go to Menu");
        SceneManager.LoadScene(nextLevel % sceneCount);
    }
}