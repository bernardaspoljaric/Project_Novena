using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AppData : MonoBehaviour
{
    public TextAsset textJson;
    public TranslatedContentsList readTranslatedContents = new TranslatedContentsList();

    // variables for UI design
    [SerializeField] private GameObject scrollLanguageContent;
    [SerializeField] private GameObject scrollTopicContent;
    [SerializeField] private GameObject languagePage;
    [SerializeField] private GameObject topicsPage;
    [SerializeField] private GameObject detailsPage;
    [SerializeField] private GameObject extraPage;

    [SerializeField] private Button languageButton;
    [SerializeField] private Button topicButton;

    [SerializeField] private TextMeshProUGUI topicNumber;
    [SerializeField] private TextMeshProUGUI topicName;

    [SerializeField] private TextMeshProUGUI topicGameNumber;
    [SerializeField] private TextMeshProUGUI topicGameName;

    // variables for slideshow
    [SerializeField] private RawImage slideshow;
    private List<string> fileName = new List<string>();
    private float t; // time one image last
    private Texture2D file;
    private List<string> facts = new List<string>();
    [SerializeField] private TextMeshProUGUI factText;

    // variables for audio
    private string audioFile;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Slider audioSlider;
    [SerializeField] private TextMeshProUGUI audioDurationText;
    private float audioDuration;
    private int audioMinutes;
    private int audioSeconds;
    private int audioMinutesPassed;
    private int audioSecondsPassed;
    private float secondsPassed = 0;
    private float jumpSecondsPassed = 0;

    // choosen language
    private int selectedLanguage;

    public void Start()
    {
        t = 5f;
        readTranslatedContents = JsonUtility.FromJson<TranslatedContentsList>(textJson.text);
        GetLanguages();

    }

    // method for displaying all available languages
    public void GetLanguages()
    {
        for (int i = 0; i < readTranslatedContents.TranslatedContents.Length; i++)
        {
            Button newButton = Instantiate(languageButton, scrollLanguageContent.transform.position, Quaternion.identity);
            newButton.transform.SetParent(scrollLanguageContent.transform);
            newButton.transform.localScale = Vector3.one;
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = readTranslatedContents.TranslatedContents[i].LanguageName;
            newButton.onClick.AddListener(delegate { SetLanguage(newButton); });
            newButton.onClick.AddListener(OpenTopics);
        }
    }

    // method for setting choosen language
    public void SetLanguage(Button clickedButton)
    {
        for (int i = 0; i < readTranslatedContents.TranslatedContents.Length; i++)
        {
            if (clickedButton.GetComponentInChildren<TextMeshProUGUI>().text == readTranslatedContents.TranslatedContents[i].LanguageName)
            {
                selectedLanguage = readTranslatedContents.TranslatedContents[i].LanguageId;
                selectedLanguage--;
                Debug.Log(selectedLanguage);
            }
         } 
    }

    // method for changing display images
    public void OpenTopics()
    {
        topicsPage.SetActive(true);
        languagePage.SetActive(false);
        GetTopics();

    }

    // method for getting selected topic's name and number
    public void GetTopics()
    {
        for (int i = 0; i < readTranslatedContents.TranslatedContents[selectedLanguage].Topics.Length; i++)
        {
            Button newButton = Instantiate(topicButton, scrollTopicContent.transform.position, Quaternion.identity);
            newButton.transform.SetParent(scrollTopicContent.transform);
            newButton.transform.localScale = Vector3.one;
            TextMeshProUGUI[] buttonTexts = newButton.GetComponentsInChildren<TextMeshProUGUI>();
            for (int j = 0; j < buttonTexts.Length; j++)
            {
                if (buttonTexts[j].name == "TopicText")
                {
                    buttonTexts[j].text = readTranslatedContents.TranslatedContents[selectedLanguage].Topics[i].Name;
                }
                else
                {
                    buttonTexts[j].text = (i+1).ToString();
                }
            }

            if (readTranslatedContents.TranslatedContents[selectedLanguage].Topics[i].Name == "Extra")
            {
                newButton.onClick.AddListener(delegate { OpenGame(newButton); });
            }
            else
            {
                newButton.onClick.AddListener(OpenDetails);
                newButton.onClick.AddListener(delegate { ShowDetails(newButton); });
            }
        }
    }

    // method for opening mini game
    public void OpenGame(Button clickedButton)
    {
        extraPage.SetActive(true);
        topicsPage.SetActive(false);

        for (int i = 0; i < readTranslatedContents.TranslatedContents[selectedLanguage].Topics.Length; i++)
        {
            if (clickedButton.GetComponentInChildren<TextMeshProUGUI>().text == readTranslatedContents.TranslatedContents[selectedLanguage].Topics[i].Name)
            {
                topicGameNumber.text = (i + 1).ToString();
                topicGameName.text = readTranslatedContents.TranslatedContents[selectedLanguage].Topics[i].Name;
            }
        }
    }

    // method for changing display images
    public void OpenDetails()
    {
        detailsPage.SetActive(true);
        topicsPage.SetActive(false);
    }

    // method for showing details for selected topic
    public void ShowDetails(Button clickedButton)
    {
        for (int i = 0; i < readTranslatedContents.TranslatedContents[selectedLanguage].Topics.Length; i++)
        {
            if (clickedButton.GetComponentInChildren<TextMeshProUGUI>().text == readTranslatedContents.TranslatedContents[selectedLanguage].Topics[i].Name)
            {
                topicNumber.text = (i+1).ToString();
                topicName.text = readTranslatedContents.TranslatedContents[selectedLanguage].Topics[i].Name;
                Slideshow(i);
            }
        }
    }

    // method for setting assets to use in slideshow and audio 
    public void Slideshow(int topicNumber)
    {
        for (int i = 0; i < readTranslatedContents.TranslatedContents[selectedLanguage].Topics[topicNumber].Media.Length; i++)
        {
            if (readTranslatedContents.TranslatedContents[selectedLanguage].Topics[topicNumber].Media[i].Name == "Gallery")
            {
                for (int j = 0; j < readTranslatedContents.TranslatedContents[selectedLanguage].Topics[topicNumber].Media[i].Photos.Length; j++)
                {
                    Debug.Log(readTranslatedContents.TranslatedContents[selectedLanguage].Topics[topicNumber].Media[i].Photos[j].Path);
                    fileName.Add(readTranslatedContents.TranslatedContents[selectedLanguage].Topics[topicNumber].Media[i].Photos[j].Path);
                    facts.Add(readTranslatedContents.TranslatedContents[selectedLanguage].Topics[topicNumber].Media[i].Photos[j].Facts);
                }
            }
            else if (readTranslatedContents.TranslatedContents[selectedLanguage].Topics[topicNumber].Media[i].Name == "Audio")
            {
                audioFile = readTranslatedContents.TranslatedContents[selectedLanguage].Topics[topicNumber].Media[i].FilePath;
            }
        }
        StartAudio();
        StartCoroutine(LoadImages(t));
    }

    //corutine for showing images at certain speed
    private IEnumerator LoadImages(float seconds)
    {
        for (int i = 0; i < fileName.Count; i++)
        {
            Debug.Log(fileName[i]);
            file = Resources.Load(fileName[i]) as Texture2D;
            slideshow.texture = file;

            factText.text = facts[i];

            if (i == fileName.Count - 1)
            {
                i = -1;
            }

            yield return new WaitForSeconds(seconds);
        }
    }

    // method for stopping and clearing fileNames for certain topic
    public void StopSlideshow()
    {
        StopCoroutine("LoadImages");
        fileName = new List<string>();
        facts = new List<string>();
    }

    // method for starting specific audio when details page is open
    private void StartAudio()
    {
        audioSource.clip = Resources.Load(audioFile) as AudioClip;
        audioSource.Play();
        audioDuration = audioSource.clip.length;
        audioMinutes = (int)audioDuration / 60;
        audioSeconds = (int)audioDuration % 60;
        audioSlider.minValue = 0;
        audioSlider.maxValue = audioSource.clip.length;
        audioSlider.value = audioSource.time;
        AudioTextManipulation();
        InvokeRepeating("AudioTrack", 0f, 1f);
    }

    // method for playing selected audio
    public void PlayAudio()
    {
        audioSource.Play();
        InvokeRepeating("AudioTrack", 0f, 1f);
    }

    // method for pausing selected audio
    public void PauseAudio()
    {
        audioSource.Pause();
        CancelInvoke("AudioTrack");
    }

    // method for stopping selected audio when leaving details page
    public void StopAudio()
    {
        audioSource.Stop();
        CancelInvoke("AudioTrack");
        secondsPassed = 0;
        audioMinutesPassed = 0;
        audioSecondsPassed = 0;
        audioSource.clip = null;
        audioDuration = 0;
        audioSource.time = 0;
    }

    // method for changing audio duration text while audio is playing
    private void AudioTextManipulation()
    {
        if (audioMinutesPassed < 10 && audioSecondsPassed < 10 && audioMinutes < 10 && audioSeconds < 10)
        {
            audioDurationText.text = "0" + audioMinutesPassed.ToString() + ":0" + audioSecondsPassed.ToString() + " / 0" + audioMinutes.ToString() + ":0" + audioSeconds.ToString();
        }
        else if (audioMinutesPassed < 10 && audioSecondsPassed >= 10 && audioMinutes < 10 && audioSeconds >= 10)
        {
            audioDurationText.text = "0" + audioMinutesPassed.ToString() + ":" + audioSecondsPassed.ToString() + " / 0" + audioMinutes.ToString() + ":" + audioSeconds.ToString();
        }
        else if (audioMinutesPassed >= 10 && audioSecondsPassed < 10 && audioMinutes >= 10 && audioSeconds < 10)
        {
            audioDurationText.text = audioMinutesPassed.ToString() + ":0" + audioSecondsPassed.ToString() + " / " + audioMinutes.ToString() + ":0" + audioSeconds.ToString();
        }
        else if (audioMinutesPassed >= 10 && audioSecondsPassed >= 10 && audioMinutes >= 10 && audioSeconds >= 10)
        {
            audioDurationText.text = audioMinutesPassed.ToString() + ":" + audioSecondsPassed.ToString() + " / " + audioMinutes.ToString() + ":" + audioSeconds.ToString();
        }
    }

    // method to invoke so we can keep track of how many seconds of audio has passed
    private void AudioTrack()
    {
        secondsPassed++;
        audioSlider.value = secondsPassed;
        audioMinutesPassed = (int)secondsPassed / 60;
        audioSecondsPassed = (int)secondsPassed % 60;
        AudioTextManipulation();
    }

    // method for cheging slider value, if there is jump in seconds audio starts from there
    public void OnValueChange()
    {
        jumpSecondsPassed = audioSlider.value;
        secondsPassed = jumpSecondsPassed;
        audioSource.time = jumpSecondsPassed;
    }

    // method for clearing topics if user decides to change language
    public void ClearTopics()
    {
        Button[] topicsToDestroy = scrollTopicContent.GetComponentsInChildren<Button>();
        for (int i = 0; i < topicsToDestroy.Length; i++)
        {
            Destroy(topicsToDestroy[i].gameObject);
        }
    }
}

[System.Serializable]
public class TranslatedContents
{
    public int LanguageId;
    public string LanguageName;
    public Topics[] Topics;
}

[System.Serializable]
public class Topics
{
    public string Name;
    public Media[] Media;
}

[System.Serializable]
public class Media
{
    public string Name;
    public Photos[] Photos;
    public string FilePath;
}

[System.Serializable]
public class Photos
{
    public string Path;
    public string Name;
    public string Facts;
}

[System.Serializable]
public class TranslatedContentsList
{
    public TranslatedContents[] TranslatedContents;
}

[System.Serializable]
public class TopicsList
{
    public Topics[] Topics;
}

[System.Serializable]
public class MediaList
{
    public Media[] Media;
}

[System.Serializable]
public class PhotosList
{
    public Photos[] Photos;
}
