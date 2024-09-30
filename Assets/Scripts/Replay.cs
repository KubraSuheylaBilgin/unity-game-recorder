using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Replay : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Animator animatorAvatar2;

    [SerializeField] private GameObject scriptHolder;
    [SerializeField] private GameObject avatar2sScriptHolder;
    private bool _isKyleRed;
   // private bool _isAvatarRed;
   [SerializeField] private Material replayKyleMaterial;
    //public Material replayAvatar2Material;

    private GamePlayRecorder.KeyFrameCollection _keyFrameCollection;
    private int recordLength;
    private bool _isRecordPlaying=true;
    

    private int currentFrame = 0;

    void Start()
    {
        Application.targetFrameRate = 24;
        string json="";
        if (PlayerPrefs.GetInt("EnteringMenu")==1)
        {
            string dropdownOptionText = PlayerPrefs.GetString("RecordName");
            json = System.IO.File.ReadAllText("Records/" + dropdownOptionText);
        }

        if (PlayerPrefs.GetInt("EnteringMenu")==0)
        {
            string jsonMainScene =  PlayerPrefs.GetString("RecordSceneMain");

            json = System.IO.File.ReadAllText( jsonMainScene);
        }
        
        _keyFrameCollection = JsonUtility.FromJson<GamePlayRecorder.KeyFrameCollection>(json);
        recordLength = _keyFrameCollection.keyframes.Count;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void JumpForward()
    {
        currentFrame += 2 * 24;
        if (currentFrame >= recordLength - 1)
        {
            currentFrame = recordLength - 1;
        }
    }
    
    public void JumpBackForward()
    {
        currentFrame -= 2 * 24;
        if (currentFrame <= 0)
        {
            currentFrame =0;
        }
    }

    public void PlayRecord()
    {
        _isRecordPlaying = true;
    } 
    public void PauseRecord()
    {
        _isRecordPlaying = false;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    

    private void LateUpdate()
    {
        if (currentFrame < recordLength-1)
        {
            ReplayGamePlayRecord(currentFrame);
        }
        else if (currentFrame==recordLength-1)
        {
            _isRecordPlaying = false;
        }


        if (_isRecordPlaying)
        {
            currentFrame++;
        }

    }

    void ReplayGamePlayRecord(int frameNumber)
    {
        var keyframeData = _keyFrameCollection.keyframes[frameNumber];
        scriptHolder.transform.position = keyframeData.avatarPosition;
        scriptHolder.transform.rotation = keyframeData.avatarRotation;
        avatar2sScriptHolder.transform.position = keyframeData.avatar2Position;
        avatar2sScriptHolder.transform.rotation = keyframeData.avatar2Rotation;
        _isKyleRed = keyframeData.isKylePointRed;
        
        RePlayChangeColor(_isKyleRed);
        
        var transformDatas = keyframeData.transforms.ToDictionary(transform => transform.boneName, transform => transform);
        var transformAvatar2Datas = keyframeData.TransformsAvatar2.ToDictionary(transform => transform.boneName, transform => transform);


        foreach (HumanBodyBones bone in System.Enum.GetValues(typeof(HumanBodyBones)))
        {
            if (!transformDatas.ContainsKey(bone.ToString())) continue;
            Transform boneTransform = animator.GetBoneTransform(bone);
            boneTransform.localPosition = transformDatas[bone.ToString()].position;
            boneTransform.localRotation = transformDatas[bone.ToString()].rotation;
        }

        foreach (HumanBodyBones bone in System.Enum.GetValues(typeof(HumanBodyBones)))
        {
            if (!transformDatas.ContainsKey(bone.ToString())) continue;
            Transform boneTransformAvatar2 = animatorAvatar2.GetBoneTransform(bone);
            boneTransformAvatar2.localPosition = transformAvatar2Datas[bone.ToString()].position;
            boneTransformAvatar2.localRotation = transformAvatar2Datas[bone.ToString()].rotation;
        }
    }
    void RePlayChangeColor(bool _isRed)
    {
        if (_isRed)
        {
            replayKyleMaterial.color = Color.red;
  //          replayAvatar2Material.color =Color.red;
        }
        else
        {
            replayKyleMaterial.color = Color.green;
    //        replayAvatar2Material.color =Color.green;

        }
    }
}