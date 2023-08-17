using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public class RsvpTab : EditorWindow
{
    // RSVP быстрое визуальное представление https://translated.turbopages.org/proxy_u/en-ru.ru.15c8fdc5-64de32db-c9e31514-74722d776562/https/en.wikipedia.org/wiki/Rapid_serial_visual_presentation

    private float wordDelay = 0.2f;

    private string _wordToShow = "";

    private int _currentNumberElement, _currentNumberWord;

    private bool _infiniteRepeat, _randomOrder, _dynamicSpeed, _needChangeListElement;
    private float _speedForOneCharacter = 0.06f;
    private List<string> _rsvpText = new List<string>();
    private List<String> _textList = new();
    private float _lastUpdateTime = 0f;






    [MenuItem("Window/RsvpTab")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(RsvpTab));
    }

    private bool showSettings = false;
    private GUIStyle foldoutStyle;

    private void OnGUI()
    {
        // Initialize the foldout style with the desired font size
        if (foldoutStyle == null)
        {
            foldoutStyle = new GUIStyle(EditorStyles.foldout);
            foldoutStyle.fontSize = 16; // Adjust the font size as needed
        }
        showSettings = EditorGUILayout.Foldout(showSettings, "НАСТРОЙКИ", foldoutStyle);

        if (showSettings)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);

            if (GUILayout.Button("Добавить файл"))
            {
                LoadTextFromFile();
            }

            GUILayout.Label("Нулевая строка:");
            GUILayout.Label(_rsvpText[0]);

            GUILayout.Space(10);
            _dynamicSpeed = EditorGUILayout.Toggle("Динамическая скорость", _dynamicSpeed);
            _speedForOneCharacter = EditorGUILayout.FloatField(_speedForOneCharacter);
            GUILayout.Space(10);

            GUILayout.Label("Задержка перед следующим словом:");
            wordDelay = EditorGUILayout.Slider(wordDelay, 0.01f, 2f);


            _infiniteRepeat = EditorGUILayout.Toggle("Бесконечное повторение", _infiniteRepeat);
            _randomOrder = EditorGUILayout.Toggle("Случайный выбор строки", _randomOrder);


            GUILayout.EndVertical();
        }

        GUILayout.BeginHorizontal();
        GUILayout.TextField("Элемент: " + _currentNumberElement.ToString());
        GUILayout.TextField("Слово: " + _currentNumberWord.ToString());

        if (GUILayout.Button("Start RSVP"))
        {
            StartRSVP();
        }
        if (GUILayout.Button("Stop RSVP"))
        {
            Stop();
        }

        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();


        // блок отображаемого слова, размер шрифта

        GUIStyle labelStyle = new GUIStyle(EditorStyles.largeLabel);
        labelStyle.fontSize = 30;
        labelStyle.normal.textColor = Color.white;

        // цвет 
        if (_currentNumberElement % 2 == 0)
        {
            labelStyle.normal.textColor = Color.red;
        }
        else
        {
            labelStyle.normal.textColor = Color.white;
        }

        GUILayout.Label(_wordToShow, labelStyle, GUILayout.ExpandHeight(true));

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
    }

    private void LoadTextFromFile()
    {
        string filePath = EditorUtility.OpenFilePanel("Select Text File", "", "txt");

        if (!string.IsNullOrEmpty(filePath))
        {
            _rsvpText = new List<string>(System.IO.File.ReadAllLines(filePath));
        }
    }

    private void StartRSVP()
    {
        _currentNumberElement = 1;
        _currentNumberWord = 0;
        _needChangeListElement = true;
        EditorApplication.update += ReadString;
    }

    private void Stop()
    {
        _needChangeListElement = false;
        EditorApplication.update -= ReadString;
    }




    private void ReadString()
    {
        float currentTime = (float)EditorApplication.timeSinceStartup;
        float timeElapsed = currentTime - _lastUpdateTime;


        if (timeElapsed >= wordDelay)
        {
            _lastUpdateTime = currentTime;

            if (_needChangeListElement)
            {
                _textList = new List<string>(_rsvpText[_currentNumberElement].Split(' '));

                if (_randomOrder)
                {
                    _currentNumberElement = UnityEngine.Random.Range(1, _rsvpText.Count);

                    _textList = new List<string>(_rsvpText[_currentNumberElement].Split(' '));


                }
                _needChangeListElement = false;
            }


            if (_currentNumberWord < _textList.Count)
            {
                _wordToShow = _textList[_currentNumberWord];

                Repaint();
                _currentNumberWord++;
                if (_dynamicSpeed && _currentNumberWord < _textList.Count)
                {
                    ChangeSpeedLengthWord(_textList[_currentNumberWord].Length);
                }

            }
            else
            {
                ChangeString();
                _currentNumberWord = 0;
            }

            if (_currentNumberElement >= _rsvpText.Count)
            {
                if (_infiniteRepeat)
                {
                    _currentNumberElement = 0;
                }
                else
                {
                    EditorApplication.update -= ReadString;
                }
            }


        }
    }


    private void ChangeString()
    {
        _needChangeListElement = true;
        _currentNumberElement++;
        _currentNumberWord = 0;
        if (_dynamicSpeed)
        {
            wordDelay = 0.2f;
        }


    }

    private void ChangeSpeedLengthWord(int wordLength)
    {
        wordDelay = wordLength * _speedForOneCharacter;
    }

    
}
