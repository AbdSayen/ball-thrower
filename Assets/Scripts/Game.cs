using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class Game : MonoBehaviour
{
    [SerializeField] private Language _language;
    static public Language language;

    private void Awake()
    {
        if (_language == Language.auto)
        {
            switch (YandexGame.lang)
            {
                case "ru":
                    language = Language.ru;
                    break;
                case "en":
                    language = Language.en;
                    break;
                default: 
                    goto case "en";
            }
        }
        else language = _language;
    }

    public static void PushMessage(MonoBehaviour monoBehaviour, string message, float duration)
    {
        monoBehaviour.StartCoroutine(PushMessageCoroutine(message, duration));
    }

    private static IEnumerator PushMessageCoroutine(string message, float duration)
    {
        Text text = GameObject.FindGameObjectWithTag("Message").GetComponent<Text>();
        CanvasGroup canvasGroup = text.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = text.gameObject.AddComponent<CanvasGroup>();
        }

        text.text = message;
        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(duration);

        float fadeOutTime = 0.25f;
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime / fadeOutTime;
            yield return null;
        }

        text.text = string.Empty;
    }
}

public enum Language 
{
    auto, en, ru
}