using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI question;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject answerButton;
    [SerializeField] private List<string> colorList;
    [SerializeField] private List<string> wordList;

    private string hexColor;
    private string hexColorName;
    private int score;

    private void Start()
    {
        score = 0;
        scoreText.text = score.ToString();
        InvokeRepeating("ShowQuestion", 0f, 1f);
    }

    private void ShowQuestion()
    {
        question.text = wordList[Random.Range(0, wordList.Count)];

        hexColor = colorList[Random.Range(0, colorList.Count)];

        Color newCol;
        if (ColorUtility.TryParseHtmlString(hexColor, out newCol))
        {
            question.color = newCol;
        }

        SetColorName();
    }

    private void SetColorName()
    {
        switch (hexColor)
        {
            case "#FF0000":
                hexColorName = "Red";
                break;
            case "#0000FF":
                hexColorName = "Blue";
                break;
            case "#008000":
                hexColorName = "Green";
                break;
            case "#FFFF00":
                hexColorName = "Yellow";
                break;
            case "#800080":
                hexColorName = "Purple";
                break;
            case "#FFA500":
                hexColorName = "Orange";
                break;
            default:
                Debug.Log("Not a color.");
                break;
        }
    }

    public void CheckAnswer()
    {
        if (question.text == hexColorName)
        {
            CancelInvoke("ShowQuestion");
            score++;
            scoreText.text = score.ToString();
            InvokeRepeating("ShowQuestion", 0f, 1f);
        }
        else
        {
            CancelInvoke("ShowQuestion");
            finalScoreText.text = score.ToString();
            gameOverPanel.SetActive(true);
            answerButton.SetActive(false);
            question.gameObject.SetActive(false);
        }
    }

}
