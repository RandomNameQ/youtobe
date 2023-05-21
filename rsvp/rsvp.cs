using System.Net.Mime;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;

public class RSVP : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textMesh;

    [SerializeField]
    private float _timeToNextWord = 0.15f;

    [SerializeField]
    private bool _infinityWork;

    [SerializeField]
    private bool _firstType;

    private string _text =
        "вот так работает rsvp мы очень быстро предоставляе информацию юзеру это называется быстрое последовательное визуальное предъявление и кстати а ты кто и почему так быстро читаешь слова? неужели ты тот самый альберт КОНО ДИО ДА енштейн, что может использовать ЗЕ ВОРЛД и мгновенно останавливать время? МММ классно плюс интересно, что еще придумаешь тоесть расскажешь? ладно давай не выебывайся, ставь лайк, хуярь не смешную шутку в коменты и оформляй мне подпиську. Окей?";
    private float _timer;
    private bool _needNextLoop;
    private string _textTemp;
    private float _timeTemp;

    [SerializeField]
    private bool _ignoreSlowSpeed;

    [SerializeField]
    private bool _timerOn = true;

    [SerializeField]
    private float _wordPerMinute = 50;

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        _timeToNextWord = 60 / _wordPerMinute;
        _timeTemp = _timeToNextWord;
        _textTemp = _text;
        StartTimer();
    }

    private void StartTimer()
    {
        _timer = _timeToNextWord;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timerOn && _timer <= 0)
        {
            NextWords();
            Settings();
            _timer = _timeToNextWord;
        }
    }

    private void NextWords()
    {
        int spaceIndex = _text.IndexOf(' ');
        string firstWord;

        // first spaceIndex == 3 in "вот так работает"

        if (spaceIndex != -1)
        {
            firstWord = _text.Substring(0, spaceIndex);
            _text = _text.Substring(spaceIndex + 1);
            _needNextLoop = false;
        }
        else
        {
            _text = "";
            _needNextLoop = true;
            return;
        }

        _timeToNextWord = _timeTemp;
        if (!_ignoreSlowSpeed)
        {
            SlowSpeedForSmallLetter(firstWord);
            SlowLastWords();
        }

        UpdateUiText(firstWord);
    }

    private void UpdateUiText(string firstWord)
    {
        if (_firstType)
        {
            int centralIndex = firstWord.Length / 2;
            string formattedText =
                $"<color=white>{firstWord.Substring(0, centralIndex)}</color>"
                + $"<color=red>{firstWord[centralIndex]}</color>"
                + $"<color=white>{firstWord.Substring(centralIndex + 1)}</color>"
                + $" <color=white>{_text}</color>";
            _textMesh.text = formattedText;
        }
        else
        {
            _textMesh.text = firstWord;
        }
    }

    private void Settings()
    {
        if (_needNextLoop && _infinityWork)
        {
            _text = _textTemp;
        }
        else if (_needNextLoop)
        {
            string lastWord = _textTemp.Substring(_textTemp.LastIndexOf(' ') + 1);
            _textMesh.text = lastWord;
            _timerOn = false;
            Debug.Log(lastWord);
        }
    }

    private void SlowSpeedForSmallLetter(string firstWord)
    {
        if (firstWord.Length <= 3)
        {
            _timeToNextWord *= 2;
        }
        if (firstWord.Length > 7)
        {
            _timeToNextWord *= 1.5f;
        }
    }

    private void SlowLastWords()
    {
        if (_text.Length < 20)
        {
            _timeToNextWord = _timeTemp;
            _timeToNextWord *= 3;
        }
    }
}
