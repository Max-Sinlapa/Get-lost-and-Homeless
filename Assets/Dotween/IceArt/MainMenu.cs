using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Tweening UI", order = 0)]
   

    [Header("Title")]
    [SerializeField] private TextMeshProUGUI titleText = default;

    [Header("Button")]
    [SerializeField] private Button[]  buttons= default;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource = default;
    [SerializeField] private AudioClip buttonClip = default;

    // Start is called before the first frame update
    void Start()
    {
       
        TestTweeningSequence();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestTweeningSequence()
    {
        DOTween.Sequence()
            .OnStart(OnStartSequence)
            //main
            .Insert(0.75f, titleText.DOFade(1, 0.25f).SetEase(Ease.InCubic))
            .Join(titleText.rectTransform.DOShakeRotation(1, 25, 5, 25, false))
            .Join(buttons[0].transform.DOScale(Vector3.one * 1, 0.25f).SetEase(Ease.InSine)
                 .OnStart(() =>
                 {
                     if (audioSource && buttonClip)
                     {
                         PlayAudio(buttonClip);
                     }
                 }))
            .Insert(1.25f, buttons[1].transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))
            .Join(buttons[1].transform.DOScale(Vector3.one * 1, 0.25f).SetEase(Ease.InSine)
                 .OnStart(() =>
                 {
                     if (audioSource && buttonClip)
                     {
                         PlayAudio(buttonClip);
                     }
                 }))
            .Insert(1.75f, buttons[2].transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))
            .Join(buttons[2].transform.DOScale(Vector3.one * 1, 0.25f).SetEase(Ease.InSine)
                 .OnStart(() =>
                 {
                     if (audioSource && buttonClip)
                     {
                         PlayAudio(buttonClip);
                     }
                 }))

            .OnComplete(OnCompleteSequence);
    }

    private void OnStartSequence()
    {
        titleText.alpha = 0;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].transform.localScale = Vector3.zero;
        }
    }

    private void OnCompleteSequence()
    {

    }

        private void PlayAudio(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
