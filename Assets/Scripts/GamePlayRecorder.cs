using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GamePlayRecorder : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Animator animatorAvatar2;

    [SerializeField] private GameObject avatarKyle;
    [SerializeField] private GameObject avatar2;
    [SerializeField] private PlayerController avatar2MoveScript;

    [SerializeField] private TextMeshProUGUI saveText;
    [SerializeField] private KeyFrameCollection keyFrameCollection = new KeyFrameCollection();
    [SerializeField] private Material kylePointMaterial;
  //  public Material avatar2PointMaterial;
    private bool _kylePointRed = false;
    private bool _avatar2PointRed = false;





    public bool isGamePlayRecording = false;

    [System.Serializable]
    public class KeyFrameCollection
    {
        public List<KeyframeData> keyframes;
    }

    [System.Serializable]
    public class KeyframeData
    {
        public Vector3 avatar2Position;
        public Vector3 avatarPosition;
        public Quaternion avatarRotation;
        public Quaternion avatar2Rotation;
        public bool isKylePointRed;
        public bool isAvatar2PointRed;
        public List<TransformData> transforms;
        public List<TransformData> TransformsAvatar2;
    }  


    [System.Serializable]
    public class TransformData
    {
        public string boneName;
        public Vector3 position;
        public Quaternion rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        avatar2MoveScript.enabled = false;
        InvokeRepeating("ChangeColor", 0f, 0.5f); 
        Application.targetFrameRate = 24;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isGamePlayRecording)
        {

            SaveGameStartRecording();
        }
        else if (Input.GetKeyDown(KeyCode.R) && isGamePlayRecording)
        {
            SaveGameRecord();
        }
        
        if (isGamePlayRecording)
        {
            SaveKeyFrameData();
        }
    }

    public void SaveGameStartRecording()
    {
        isGamePlayRecording = true;
        avatar2MoveScript.enabled = true;
        saveText.text = "Recording..";
    }
    public void SaveGameRecord()
    {
        SaveGamePlayRecord();
        isGamePlayRecording = false;
        saveText.text = "Recording Stop..";
    }
    public void ReplayGameRecord()
    {
        SceneManager.LoadScene("ReplayScene");
        PlayerPrefs.SetInt("EnteringMenu",0);

    }
    void SaveKeyFrameData()
    {
        KeyframeData frameData = new KeyframeData();
        frameData.transforms = new List<TransformData>();
        frameData.TransformsAvatar2 = new List<TransformData>();
        frameData.avatarPosition = avatarKyle.transform.position;
        frameData.avatar2Position = avatar2.transform.position;
        frameData.avatarRotation = avatarKyle.transform.rotation;
        frameData.avatar2Rotation = avatar2.transform.rotation;
        frameData.isKylePointRed = _kylePointRed;
        frameData.isAvatar2PointRed = _avatar2PointRed;

        foreach (HumanBodyBones bone in System.Enum.GetValues(typeof(HumanBodyBones)))
        {
            if (bone == HumanBodyBones.LastBone) continue;

            Transform boneTransform = animator.GetBoneTransform(bone);
            if (boneTransform != null)
            {
                TransformData transformData = new TransformData();
                transformData.boneName = bone.ToString();
                transformData.position = boneTransform.localPosition;
                transformData.rotation = boneTransform.localRotation;
                frameData.transforms.Add(transformData);
            }
            
            Transform boneTransform2 = animatorAvatar2.GetBoneTransform(bone);
            if (boneTransform2 != null)
            {
                TransformData transformDataAvatar2 = new TransformData();
                transformDataAvatar2.boneName = bone.ToString();
                transformDataAvatar2.position = boneTransform2.localPosition;
                transformDataAvatar2.rotation = boneTransform2.localRotation;
                frameData.TransformsAvatar2.Add(transformDataAvatar2);
            }
        }

        keyFrameCollection.keyframes.Add(frameData);
    }

    void SaveGamePlayRecord()
    {
        string localDate = DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
        var json = JsonUtility.ToJson(keyFrameCollection);
        string fullPathName = "Records" + "/" + "Record_" + localDate + ".json";
        System.IO.File.WriteAllText(fullPathName, json);
        PlayerPrefs.SetString("RecordSceneMain",fullPathName);

    }
    void ChangeColor()
    {
        var result = Random.Range(0, 2);
        if (result == 1)
        {
            _kylePointRed = !_kylePointRed; // bool değişkeninin değerini değiştir (true ise false, false ise true yap)
            _avatar2PointRed = !_avatar2PointRed;
        }
        
        if (_kylePointRed || _avatar2PointRed)
        {
            kylePointMaterial.color = Color.red;
          //  avatar2PointMaterial.color = Color.red;
        }
        else
        {
            kylePointMaterial.color = Color.green;
           // avatar2PointMaterial.color = Color.green;
        }
    }
}