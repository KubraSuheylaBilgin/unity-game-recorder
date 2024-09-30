using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AnimationPathController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject _avatar;
    [SerializeField] private AnimationClip animationClip;

    [SerializeField] private string animationName = "Run";
    [SerializeField] private float recordingInterval = 0.1f; // Her bir kaydın zamanı
    private float timer = 0f;
    private List<KeyframeData> keyframes = new List<KeyframeData>();

    [System.Serializable]
    public class KeyframeData
    {
        public float avatarZposition;
        public float time;
        public List<TransformData> transforms;
    }

    [System.Serializable]
    public class TransformData
    {
        public string boneName;
        public Vector3 position;
        public Quaternion rotation;
    }

    void Start()
    {
        animator.Play(animationName);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= recordingInterval)
        {
            SaveCurrentFrame();

            timer = 0f;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _avatar.transform.Translate(new Vector3(0,0,-10));
        }
        
    }

    void SaveCurrentFrame()
    {
        KeyframeData frameData = new KeyframeData();
        frameData.time = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        frameData.transforms = new List<TransformData>();
        frameData.avatarZposition = _avatar.transform.position.z;

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
        }

        keyframes.Add(frameData);
    }
}
