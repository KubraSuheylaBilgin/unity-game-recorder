using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RecordListManager : MonoBehaviour
{
    [SerializeField] private Dropdown dropdown;

    // Start is called before the first frame update
    void Start()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo("Records/");
        FileInfo[] Files = directoryInfo.GetFiles("*.json");

        foreach (var fileInfo in Files)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData();
            optionData.text = fileInfo.Name;
            dropdown.options.Add(optionData);
        }

        dropdown.onValueChanged.AddListener(delegate
        {
            SelectRecord();
        });
    }
    void Update()
    {

    }

    void SelectRecord()
    {
        PlayerPrefs.SetString("RecordName", dropdown.options[dropdown.value].text);
        PlayerPrefs.SetInt("EnteringMenu",1);
    }

    public void StartRecord()
    {
        SceneManager.LoadScene("ReplayScene");

    }
    
    public void PlayGame()
    {
        SceneManager.LoadScene("RecordScene");
        PlayerPrefs.SetInt("EnteringMenu",0);

    }

}