using UnityEngine;
using UnityEngine.UI;

public class Translator : MonoBehaviour
{
    private Text text;

    [SerializeField] [TextArea] private string EnText;
    [SerializeField] [TextArea] private string RuText;

    private void Start()
    {
        text = GetComponent<Text>();

        switch (Game.language)
        {
            case Language.ru:
                text.text = RuText;
                break;
            case Language.en:
                text.text = EnText;
                break;
            default:
                goto case Language.en;
        }
    }

    static public string GetTranslate(string ru, string en)
    {
        switch (Game.language)
        {
            case Language.ru:
                return ru;
            case Language.en:
                return en;
            default:
                goto case Language.en;
        }
    }
}