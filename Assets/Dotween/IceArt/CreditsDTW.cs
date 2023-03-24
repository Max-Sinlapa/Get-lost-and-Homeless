using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreditsDTW : MonoBehaviour
{
   [Header("Tweening UI", order = 0)]

    [Header("Title")]
    [SerializeField] private TextMeshProUGUI titleText = default;

    [Header("Title")]
    [SerializeField] private Image Paper = default;

    [Header("Team")]
    [SerializeField] private TextMeshProUGUI[]  Member= default;

    [Header("Button")]
    [SerializeField] private Button  button= default;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource = default;
    [SerializeField] private AudioClip openClip = default;
    [SerializeField] private AudioClip closeclip = default;
    [SerializeField] private AudioClip buttonClip = default;

    private Vector3 yZero = new Vector3(1, 0, 1);
    private Color alphaZero = new Color(1, 1, 1, 0);

    void Start()
    {
        TestTweeningSequence();
    }

    public void TestTweeningSequence()
    {
        DOTween.Sequence()
            .OnStart(OnStartSequence)

            .Insert(0.75f, titleText.DOFade(1, 0.25f).SetEase(Ease.InCubic))
            .Join(titleText.rectTransform.DOShakeRotation(1, 25, 5, 25, false))
            //-------------------------------------------------------------

            //Member 1
            .Insert(0.75f, Member[0].transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))
            .Join(Member[0].transform.DOScale(Vector3.one * 1, 0.25f).SetEase(Ease.InSine))

            //Member 2
            .Insert(1.25f, Member[1].transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))
            .Join(Member[1].transform.DOScale(Vector3.one * 1, 0.25f).SetEase(Ease.InSine))

            //Member 3
            .Insert(1.75f, Member[2].transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))
            .Join(Member[2].transform.DOScale(Vector3.one * 1, 0.25f).SetEase(Ease.InSine))

            //Member 4
            .Insert(2.25f, Member[3].transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))
            .Join(Member[3].transform.DOScale(Vector3.one * 1, 0.25f).SetEase(Ease.InSine))

            //Member 5
            .Insert(2.75f, Member[4].transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))
            .Join(Member[4].transform.DOScale(Vector3.one * 1, 0.25f).SetEase(Ease.InSine))
               
    
            //---------------------------------------------------------------
            //Paper
            .Insert(0.25f, Paper.DOFade(1, 0.5f).SetEase(Ease.InQuart))
            .Join(Paper.rectTransform.DOScale(Vector3.one, 1f).SetEase(Ease.InOutBounce))

            //---------------------------------------------------------------
            //Button

           .Insert(3.00f, button.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))
            .Join(button.transform.DOScale(Vector3.one * 1, 0.25f).SetEase(Ease.InSine)
                 .OnStart(() =>
                 {
                     if (audioSource && buttonClip)
                     {
                         PlayAudio(buttonClip);
                     }
                 }))
            .Join(titleText.rectTransform.DOShakeRotation(1, 25, 5, 25, false))

            .OnComplete(OnCompleteSequence);

        
    }

    private void OnStartSequence()
    {
        
        Paper.rectTransform.localScale = Vector3.zero;
        Paper.color = alphaZero;

        for (int i = 0; i < Member.Length; i++)
        {
            Member[i].transform.localScale = Vector3.zero;
        }
        
        button.transform.localScale = Vector3.zero;
       
    }

    private void OnCompleteSequence()
    {

    }

    private void PlayAudio(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
