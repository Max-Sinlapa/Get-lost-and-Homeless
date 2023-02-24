using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestTweening : MonoBehaviour
{
    [Header("Tweening UI", order = 0)]
    [Header("Canvas", order = 1)]
    [SerializeField] private CanvasGroup canvasBackground = default;
    private RectTransform rectBackground = default;

    [Header("Title")]
    [SerializeField] private Image titleBar = default;
    [SerializeField] private TextMeshProUGUI titleText = default;

    [Header("Star")]
    [SerializeField] private Image[] star = default;
    [SerializeField] private Image[] butten = default;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource = default;
    [SerializeField] private AudioClip openClip = default;
    [SerializeField] private AudioClip closeClip = default;
    [SerializeField] private AudioClip starClip = default;

    private Vector3 yZero = new Vector3(1, 0, 1);
    private Color alphaZero = new Color(1, 1, 1, 0);

    // Start is called before the first frame update
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

            //ให้ rect transform ของ  background ขยายเป็น (1, 1, 1) ในระยะเวลา 0.5 วินาทีโดย Set ease in เป็น Quart
            .Append(rectBackground.DOScale(Vector3.one, 0.5f).SetEase(Ease.InQuart)
            //และเมื่อเกิดการ tween ข้างต้นขึ้นมีคำสั่ง onStart ให้เล่นเสียง openClip
            .OnStart(() =>
            {
                if (audioSource && openClip)
                {
                    PlayAudio(openClip);
                }
            }))

            //ขณะเดียวกันให้ canvas group ของ background เฟลเป็น 1 ในระยะเวลา 0.25 วินาทีโดย Set ease in เป็น Quart
            .Join(canvasBackground.DOFade(1, 0.25f).SetEase(Ease.InQuart))
            //หลังจากประกาศ Append ผ่านไป 0.25 วินาที จะมีการสั่งให้ title bar เฟคเป็น 1 ในระยะเวลา 0.5 วินาทีโดย Set ease in เป็น Quart
            .Insert(0.25f, titleBar.DOFade(1, 0.5f).SetEase(Ease.InQuart))
            //ขณะเดียวกันให้ title bar scale เป็น (1, 1, 1) ในระยะเวลา 1 วินาทีโดย Set ease in - ease out เป็น Bounce
            .Join(titleBar.rectTransform.DOScale(Vector3.one, 1f).SetEase(Ease.InBounce))
            //หลังจากประกาศ Append ผ่านไป 0.75 วินาที จะมีการสั่งให้ title text เฟคเป็น 1 ในระยะเวลา 0.25 วินาทีโดย Set ease in เป็น Cubic
            .Insert(0.75f, titleText.DOFade(1, 0.25f).SetEase(Ease.InCubic))
            //ขณะเดียวกันให้ title text ทำการ shake rotation ด้วยค่า duration 1, strength 25, vibrato 5 ,randomness 25, fade out เป็น false
            .Join(titleText.rectTransform.DOShakeRotation(1, 25, 5, 25, false))
            //ขณะเดียวกันให้ star index 1 ทำการ scale เป็น (2, 2, 2) ในระยะเวลา 0.25 วินาทีโดย Set ease in เป็น Sine
            .Join(star[1].rectTransform.DOScale(Vector3.one * 2, 0.25f).SetEase(Ease.InSine)
                //และเมื่อเกิดการ tween ข้างต้นขึ้นมีคำสั่ง onStart ให้เล่นเสียง starClip
                .OnStart(() =>
                {
                    if (audioSource && starClip)
                    {
                        PlayAudio(starClip);
                    }
                }))



            //หลังจากประกาศ Append ผ่านไป 1.25 วินาที จะมีการสั่งให้ star index 0 เฟคเป็น 1 ในระยะเวลา 0.25 วินาทีโดย Set ease in Cubic
            .Insert(1.25f, star[0].DOFade(1, 0.25f).SetEase(Ease.InCubic))
            //ขณะเดียวกันให้ star index 0 ทำการ scale เป็น (2, 2, 2) ในระยะเวลา 0.25 วินาทีโดย Set ease in เป็น Sine
            .Join(star[0].rectTransform.DOScale(Vector3.one * 2, 0.25f).SetEase(Ease.InSine)
            //และเมื่อเกิดการ tween ข้างต้นขึ้นมีคำสั่ง onStart ให้เล่นเสียง starClip
            .OnStart(() =>
            {
                if (audioSource && starClip)
                {
                    PlayAudio(starClip);
                }
            }))
            //ขณะเดียวกันให้ star index 1 ทำการ scale เป็น (1, 1, 1) ในระยะเวลา 0.25 วินาทีโดย Set ease in เป็น Sine
            .Join(star[1].rectTransform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))



            //หลังจากประกาศ Append ผ่านไป 1.5 วินาที จะมีการสั่งให้ star index 2 เฟคเป็น 1 ในระยะเวลา 0.25 วินาทีโดย Set ease in Cubic
            .Insert(1.5f, star[2].DOFade(1, 0.25f).SetEase(Ease.InCubic))
            //ขณะเดียวกันให้ star index 2 ทำการ scale เป็น (2, 2, 2) ในระยะเวลา 0.25 วินาทีโดย Set ease in เป็น Sine
            .Join(star[2].rectTransform.DOScale(Vector3.one * 2, 0.25f).SetEase(Ease.InSine)
            //และเมื่อเกิดการ tween ข้างต้นขึ้นมีคำสั่ง onStart ให้เล่นเสียง starClip
            .OnStart(() =>
            {
                if (audioSource && starClip)
                {
                    PlayAudio(starClip);
                }
            }))
            //ขณะเดียวกันให้ star index 0 ทำการ scale เป็น (1, 1, 1) ในระยะเวลา 0.25 วินาทีโดย Set ease in เป็น Sine
            .Join(star[0].rectTransform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))
            //หลังจากประกาศ Append ผ่านไป 1.75 วินาที จะมีการสั่งให่ star index 2  ทำการ scale เป็น (1, 1, 1) ในระยะเวลา 0.25 โดยการ Set ease in เป็น Sine
            .Insert(1.75f, star[2].rectTransform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))

            ////////
            ///

            .Join(butten[1].rectTransform.DOScale(Vector3.one * 2, 0.25f).SetEase(Ease.InSine)
                //และเมื่อเกิดการ tween ข้างต้นขึ้นมีคำสั่ง onStart ให้เล่นเสียง starClip
                .OnStart(() =>
                {
                    if (audioSource && starClip)
                    {
                        PlayAudio(starClip);
                    }
                }))



            //หลังจากประกาศ Append ผ่านไป 1.25 วินาที จะมีการสั่งให้ star index 0 เฟคเป็น 1 ในระยะเวลา 0.25 วินาทีโดย Set ease in Cubic
            .Insert(1.25f, butten[0].DOFade(1, 0.25f).SetEase(Ease.InCubic))
            //ขณะเดียวกันให้ star index 0 ทำการ scale เป็น (2, 2, 2) ในระยะเวลา 0.25 วินาทีโดย Set ease in เป็น Sine
            .Join(butten[0].rectTransform.DOScale(Vector3.one * 2, 0.25f).SetEase(Ease.InSine)
            //และเมื่อเกิดการ tween ข้างต้นขึ้นมีคำสั่ง onStart ให้เล่นเสียง starClip
            .OnStart(() =>
            {
                if (audioSource && starClip)
                {
                    PlayAudio(starClip);
                }
            }))
            //ขณะเดียวกันให้ star index 1 ทำการ scale เป็น (1, 1, 1) ในระยะเวลา 0.25 วินาทีโดย Set ease in เป็น Sine
            .Join(butten[1].rectTransform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))



            //หลังจากประกาศ Append ผ่านไป 1.5 วินาที จะมีการสั่งให้ star index 2 เฟคเป็น 1 ในระยะเวลา 0.25 วินาทีโดย Set ease in Cubic
            .Insert(1.5f, butten[2].DOFade(1, 0.25f).SetEase(Ease.InCubic))
            //ขณะเดียวกันให้ star index 2 ทำการ scale เป็น (2, 2, 2) ในระยะเวลา 0.25 วินาทีโดย Set ease in เป็น Sine
            .Join(butten[2].rectTransform.DOScale(Vector3.one * 2, 0.25f).SetEase(Ease.InSine)
            //และเมื่อเกิดการ tween ข้างต้นขึ้นมีคำสั่ง onStart ให้เล่นเสียง starClip
            .OnStart(() =>
            {
                if (audioSource && starClip)
                {
                    PlayAudio(starClip);
                }
            }))
            //ขณะเดียวกันให้ star index 0 ทำการ scale เป็น (1, 1, 1) ในระยะเวลา 0.25 วินาทีโดย Set ease in เป็น Sine
            .Join(butten[0].rectTransform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))
            //หลังจากประกาศ Append ผ่านไป 1.75 วินาที จะมีการสั่งให่ star index 2  ทำการ scale เป็น (1, 1, 1) ในระยะเวลา 0.25 โดยการ Set ease in เป็น Sine
            .Insert(1.75f, butten[2].rectTransform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))

            //////


            //เมื่อ tween ของ Join หรือ Insert ของ Append ก่อนหน้านี้จบลง
            //จะทำการ scale ตัว rect transform ของ background เป็น (0, 0, 0) ในระยะเวลา 0.25 วินาทีโดย Set ease in เป็น Quart
            .Append(rectBackground.DOScale(yZero, 0.25f).SetEase(Ease.InQuart)
                .OnStart(() =>
                {
                    if (audioSource && closeClip)
                    {
                        PlayAudio(closeClip);
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

        for (int j = 0; j < star.Length; j++)
        {
            butten[j].rectTransform.localScale = Vector3.zero;
        }
    }
    private void OnCompleteSequence()
    {

    }
    private void PlayAudio(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
