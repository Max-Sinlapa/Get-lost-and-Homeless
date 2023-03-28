using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompletedDTW : MonoBehaviour
{

    [Header("Tweening UI", order = 0)]
    [Header("Canvas", order = 0)]
    [SerializeField] private CanvasGroup canvasBackground = default;
    private RectTransform rectBackground = default;

    [Header("Title")]
    [SerializeField] private Image titleBar = default;
    [SerializeField] private TextMeshProUGUI titleText = default;

    [Header("Body")]
    [SerializeField] private Image Paper = default;
    [SerializeField] private Image Stamp = default;

    [Header("Star")]
    [SerializeField] public Image[] star = default;

     [Header("Button")]
    [SerializeField] private Button[]  buttons= default;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource = default;
    [SerializeField] private AudioClip openClip = default;
    [SerializeField] private AudioClip closeclip = default;
    [SerializeField] private AudioClip starClip = default;
    [SerializeField] private AudioClip buttonClip = default;

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


            //----------------------------------------------------------------

            //Button
            .Insert(1.25f, buttons[0].transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))
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
            .Insert(1.25f, buttons[2].transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))
            .Join(buttons[2].transform.DOScale(Vector3.one * 1, 0.25f).SetEase(Ease.InSine)
                 .OnStart(() =>
                 {
                     if (audioSource && buttonClip)
                     {
                         PlayAudio(buttonClip);
                     }
                 }))
            .Join(titleText.rectTransform.DOShakeRotation(1, 25, 5, 25, false))

            //-------------------------------------------------------------
            // STAR
            .Insert(1.75f, star[0].transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))
            .Join(star[0].transform.DOScale(Vector3.one * 1, 0.25f).SetEase(Ease.InSine)
                 .OnStart(() =>
                 {
                     if (audioSource && buttonClip)
                     {
                         PlayAudio(buttonClip);
                     }
                 }))
            .Insert(2.25f, star[1].transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))
            .Join(star[1].transform.DOScale(Vector3.one * 1, 0.25f).SetEase(Ease.InSine)
                 .OnStart(() =>
                 {
                     if (audioSource && buttonClip)
                     {
                         PlayAudio(buttonClip);
                     }
                 }))
            .Insert(2.75f, star[2].transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))
            .Join(star[2].transform.DOScale(Vector3.one * 1, 0.25f).SetEase(Ease.InSine)
                 .OnStart(() =>
                 {
                     if (audioSource && buttonClip)
                     {
                         PlayAudio(buttonClip);
                     }
                 }))
            //--------------------------------------------------------------
            .Insert(3.00f, Stamp.DOFade(1, 0.5f).SetEase(Ease.InQuart))
            .Join(Stamp.rectTransform.DOScale(Vector3.one*2, 1f).SetEase(Ease.InOutBounce))

            .Join(titleText.rectTransform.DOShakeRotation(1, 25, 5, 25, false))
            //--------------------------------------------------------------
            .Insert(0.25f, titleBar.DOFade(1, 0.5f).SetEase(Ease.InQuart))
            .Join(titleBar.rectTransform.DOScale(Vector3.one, 1f).SetEase(Ease.InOutBounce))
            //---------------------------------------------------------------
            .Insert(0.25f, Paper.DOFade(1, 0.5f).SetEase(Ease.InQuart))
            .Join(Paper.rectTransform.DOScale(Vector3.one, 1f).SetEase(Ease.InOutBounce))
            
            




            .OnComplete(OnCompleteSequence);

        
    }

    private void OnStartSequence()
    {
        canvasBackground.alpha = 0;
        rectBackground.localScale = yZero;

        Paper.rectTransform.localScale = Vector3.zero;
        Paper.color = alphaZero;

        Stamp.rectTransform.localScale = Vector3.zero;
        Stamp.color = alphaZero;

        titleBar.rectTransform.localScale = Vector3.zero;
        titleBar.color = alphaZero;
        
        for (int i = 0; i < star.Length; i++)
        {
            star[i].rectTransform.localScale = Vector3.zero;
        }
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
