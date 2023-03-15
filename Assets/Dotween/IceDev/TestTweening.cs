using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IceDEV
{
    public class TestTweening : MonoBehaviour
{
    [Header("Tweening UI", order = 0)]
    [Header("Canvas", order = 0)]
    [SerializeField] private CanvasGroup canvasBackground = default;
    private RectTransform rectBackground = default;

    [Header("Title")]
    [SerializeField] private Image titleBar = default;
    [SerializeField] private TextMeshProUGUI titleText = default;

    [Header("Star")]
    [SerializeField] private Image[] star = default;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource = default;
    [SerializeField] private AudioClip openClip = default;
    [SerializeField] private AudioClip closeclip = default;
    [SerializeField] private AudioClip starClip = default;

    private Vector3 yZero = new Vector3(1, 0, 1);
    private Color alphaZero = new Color(1, 1, 1, 0);

    void Start()
    {
        rectBackground = canvasBackground.GetComponent<RectTransform>();
        TestTweeningSequence();
    }

    public void TestTweeningSequence()
    {
        DOTween.Sequence()
            .OnStart(OnStartSequence)

            //Main Sequence
            .Append(rectBackground.DOScale(Vector3.one, 0.5f).SetEase(Ease.InQuart)
            .OnStart(() =>
            {
                if (audioSource && openClip)
                {
                    PlayAudio(openClip);
                }
            }))
            .Join(canvasBackground.DOFade(1, 0.25f).SetEase(Ease.InQuart))

            .Insert(0.25f, titleBar.DOFade(1, 0.5f).SetEase(Ease.InQuart))
            .Join(titleBar.rectTransform.DOScale(Vector3.one, 1f).SetEase(Ease.InOutBounce))

            .Insert(0.75f, titleText.DOFade(1, 0.25f).SetEase(Ease.InCubic))
            .Join(titleText.rectTransform.DOShakeRotation(1, 25, 5, 25, false))
            .Join(star[1].rectTransform.DOScale(Vector3.one * 2,0.25f).SetEase(Ease.InSine)

            .OnStart(() =>
            {
                if (audioSource && starClip)
                {
                    PlayAudio(starClip);
                }
            }))
            
            .Insert(1.25f, star[0].DOFade(1,0.25f).SetEase(Ease.InCubic))
            .Join(star[0].rectTransform.DOScale(Vector3.one *2,0.25f).SetEase(Ease.InSine)
            .OnStart(() =>
            {
                if (audioSource && starClip)
                {
                    PlayAudio(starClip);
                }
            }))
            .Join(star[1].rectTransform.DOScale(Vector3.one,0.25f).SetEase(Ease.InSine))

            .Insert(1.5f, star[2].DOFade(1, 0.25f).SetEase(Ease.InCubic))
            .Join(star[2].rectTransform.DOScale(Vector3.one * 2, 0.25f).SetEase(Ease.InSine)
            .OnStart(() =>
            {
                if (audioSource && starClip)
                {
                    PlayAudio(starClip);
                }
            }))
            .Join(star[0].rectTransform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))

            .Insert(1.75f, star[2].rectTransform.DOScale(Vector3.one,0.25f).SetEase(Ease.InSine))

            .Append(rectBackground.DOScale(yZero,0.25f).SetEase(Ease.InQuart)
            .OnStart(() =>
            {
                if (audioSource && closeclip)
                {
                    PlayAudio(closeclip);
                }
            }))

            .OnComplete(OnCompleteSequence);

        
    }

    private void OnStartSequence()
    {
        canvasBackground.alpha = 0;
        rectBackground.localScale = yZero;

        titleText.alpha = 0;

        titleBar.rectTransform.localScale = Vector3.zero;
        titleBar.color = alphaZero;

        for (int i = 0; i < star.Length; i++)
        {
            star[i].rectTransform.localScale = Vector3.zero;
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

}