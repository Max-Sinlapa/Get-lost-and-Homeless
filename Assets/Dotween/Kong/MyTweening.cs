using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class MyTweening : MonoBehaviour
{
    public int EnergyGain;
    
    [Header("Tweening UI", order = 0)] 
    [Header("Canvas", order = 1)] 
    [SerializeField] private CanvasGroup canvasBackgrond = default;
    private RectTransform rectBackground = default;

    [Header("Title")] 
    [SerializeField] private Image titleBar = default;
    [SerializeField] private TextMeshProUGUI titleText = default;

    [Header("Body")] 
    [SerializeField] private Image BodyPanel = default;
    [SerializeField] private TextMeshProUGUI EnergySUMText = default;
    [SerializeField] private Slider EnergyBar = default;
    [SerializeField] private float EnergyBarTimeFill = default;
    [SerializeField] private Image[] EnerGyIcon = default;

    [Header("Audio")] 
    [SerializeField] private AudioSource audioSourceLoop = default;
    [SerializeField] private AudioSource audioSourceSingle = default;
    [SerializeField] private AudioClip openclip = default;
    [SerializeField] private AudioClip closeClip = default;
    [SerializeField] private AudioClip EnergyBarFill = default;
    [SerializeField] private AudioClip EnergyGain_S = default;

    private Vector3 yZero = new Vector3(1, 0, 1);
    private Color alphaZero = new Color(1, 1, 1, 0);

    #region DefaultScale
        private Vector3 EnergyIcon_D_Scale = new Vector3(0.1f, 0.1f, 1);
        private Vector3 Head_D_Scale = new Vector3(1.2f, 1f, 1);
        private Vector3 Body_D_Scale = new Vector3(1.5f, 1.5f, 1);

    #endregion
    
    void Start()
    {
        EnergyBar.value = 0;
        rectBackground = canvasBackgrond.GetComponent<RectTransform>();
        TestTweeningSequence();
    }
    
    public void TestTweeningSequence()
    {
        DOTween.Sequence()
            .OnStart(OnStartSequence)
            
            .Append(rectBackground
                ///////////////////Main Sequence #1
                .DOScale(new Vector3(5,5,5), 1f).SetEase(Ease.InQuart).DOTimeScale(1,10f)
                .OnStart(() =>
                {
                    
                    
                }))
                .Join(canvasBackgrond.DOFade(1,0.25f).SetEase(Ease.InQuad))
                ///////////////////////Main Sequence #2
                .Insert(1.2f, titleBar.DOFade(1,0.5f).SetEase(Ease.InQuad))
                .Join(titleBar.rectTransform.DOScale(Head_D_Scale,0.5f).SetEase(Ease.InOutBounce))
                ///////////////////////Main Sequence #3
                .Insert(1.2f, titleText.DOFade(1,0.25f).SetEase(Ease.InCubic))
                .Join(titleText.rectTransform.DOShakeRotation(0.5f,10,5,10,false))

                .Insert(2.5f,EnergyBar.DOValue(EnergyGain,EnergyBarTimeFill,true)
                .OnStart(()=>
                {
                    if (audioSourceLoop && EnergyBarFill)
                        audioSourceLoop.Play();

                })
                .OnComplete(()=>
                {
                    audioSourceLoop.Stop();
                    print("EnergyBar OnPlay");
                    
                    if (EnergyGain >= 40)
                    {
                        audioSourceSingle.PlayDelayed(0.1f);
                        
                        Debug.Log("Energy 1");
                         DOTween.Sequence()
                             .Insert(0.2f, EnerGyIcon[0].DOFade(1, 0.4f).SetEase(Ease.InCubic))
                             .Join(EnerGyIcon[0].rectTransform.DOScale(EnergyIcon_D_Scale * 2, 0.25f).SetEase(Ease.InSine))
                             
                             .Insert(0.6f, EnerGyIcon[0].DOFade(1, 0.4f).SetEase(Ease.InCubic))
                             .Join(EnerGyIcon[0].rectTransform.DOScale(EnergyIcon_D_Scale, 0.25f).SetEase(Ease.InSine));
                    }
                    if (EnergyGain >= 65)
                    {
                        audioSourceSingle.PlayDelayed(1f);
                        
                        Debug.Log("Energy 2");
                        DOTween.Sequence()
                            .Insert(1f, EnerGyIcon[1].DOFade(1, 0.4f).SetEase(Ease.InCubic))
                            .Join(EnerGyIcon[1].rectTransform.DOScale(EnergyIcon_D_Scale * 2, 0.25f).SetEase(Ease.InSine))
                                            
                            .Insert(1.4f, EnerGyIcon[1].DOFade(1, 0.4f).SetEase(Ease.InCubic))
                            .Join(EnerGyIcon[1].rectTransform.DOScale(EnergyIcon_D_Scale, 0.25f).SetEase(Ease.InSine));
                    }
                    if (EnergyGain >= 90)
                    {
                        audioSourceSingle.PlayDelayed(1.8f);
                        
                        Debug.Log("Energy 3");
                        DOTween.Sequence()
                            .Insert(1.8f, EnerGyIcon[2].DOFade(1, 0.4f).SetEase(Ease.InCubic))
                            .Join(EnerGyIcon[2].rectTransform.DOScale(EnergyIcon_D_Scale * 2, 0.25f).SetEase(Ease.InSine))
                                            
                            .Insert(2.2f, EnerGyIcon[2].DOFade(1, 0.4f).SetEase(Ease.InCubic))
                            .Join(EnerGyIcon[2].rectTransform.DOScale(EnergyIcon_D_Scale, 0.25f).SetEase(Ease.InSine));
                    }


                }))
            /////////////////// Main Sequence #7
            
            .Append(rectBackground.DOScale(yZero,1.5f).SetEase(Ease.InQuart))
                .OnStart(() =>
                {
                    if (audioSourceSingle && closeClip)
                    {
                       // PlayAudio(closeClip);
                    }
                })
            .OnComplete(OnCompleteSequence);
    }

    public void OnStartSequence()
    {
        print("OnStartSequence : ON");
    }

    private void OnCompleteSequence()
    {
        print("OnCompleteSequence : ON");
    }

    private void PlayAudioSingle(float delay)
    {
        audioSourceSingle.PlayDelayed(delay);
    }
    
    private void PlayAudioLoop(AudioClip clip)
    {
        audioSourceLoop.loop = true;
    }
    private void Update()
    {
        EnergySUMText.text = EnergyBar.value.ToString();
    }
}
