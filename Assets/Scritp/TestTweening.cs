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

            //��� rect transform �ͧ  background ������ (1, 1, 1) ��������� 0.5 �Թҷ��� Set ease in �� Quart
            .Append(rectBackground.DOScale(Vector3.one, 0.5f).SetEase(Ease.InQuart)
            //���������Դ��� tween ��ҧ�鹢���դ���� onStart ���������§ openClip
            .OnStart(() =>
            {
                if (audioSource && openClip)
                {
                    PlayAudio(openClip);
                }
            }))

            //������ǡѹ��� canvas group �ͧ background ���� 1 ��������� 0.25 �Թҷ��� Set ease in �� Quart
            .Join(canvasBackground.DOFade(1, 0.25f).SetEase(Ease.InQuart))
            //��ѧ�ҡ��С�� Append ��ҹ� 0.25 �Թҷ� ���ա�������� title bar ࿤�� 1 ��������� 0.5 �Թҷ��� Set ease in �� Quart
            .Insert(0.25f, titleBar.DOFade(1, 0.5f).SetEase(Ease.InQuart))
            //������ǡѹ��� title bar scale �� (1, 1, 1) ��������� 1 �Թҷ��� Set ease in - ease out �� Bounce
            .Join(titleBar.rectTransform.DOScale(Vector3.one, 1f).SetEase(Ease.InBounce))
            //��ѧ�ҡ��С�� Append ��ҹ� 0.75 �Թҷ� ���ա�������� title text ࿤�� 1 ��������� 0.25 �Թҷ��� Set ease in �� Cubic
            .Insert(0.75f, titleText.DOFade(1, 0.25f).SetEase(Ease.InCubic))
            //������ǡѹ��� title text �ӡ�� shake rotation ���¤�� duration 1, strength 25, vibrato 5 ,randomness 25, fade out �� false
            .Join(titleText.rectTransform.DOShakeRotation(1, 25, 5, 25, false))
            //������ǡѹ��� star index 1 �ӡ�� scale �� (2, 2, 2) ��������� 0.25 �Թҷ��� Set ease in �� Sine
            .Join(star[1].rectTransform.DOScale(Vector3.one * 2, 0.25f).SetEase(Ease.InSine)
                //���������Դ��� tween ��ҧ�鹢���դ���� onStart ���������§ starClip
                .OnStart(() =>
                {
                    if (audioSource && starClip)
                    {
                        PlayAudio(starClip);
                    }
                }))



            //��ѧ�ҡ��С�� Append ��ҹ� 1.25 �Թҷ� ���ա�������� star index 0 ࿤�� 1 ��������� 0.25 �Թҷ��� Set ease in Cubic
            .Insert(1.25f, star[0].DOFade(1, 0.25f).SetEase(Ease.InCubic))
            //������ǡѹ��� star index 0 �ӡ�� scale �� (2, 2, 2) ��������� 0.25 �Թҷ��� Set ease in �� Sine
            .Join(star[0].rectTransform.DOScale(Vector3.one * 2, 0.25f).SetEase(Ease.InSine)
            //���������Դ��� tween ��ҧ�鹢���դ���� onStart ���������§ starClip
            .OnStart(() =>
            {
                if (audioSource && starClip)
                {
                    PlayAudio(starClip);
                }
            }))
            //������ǡѹ��� star index 1 �ӡ�� scale �� (1, 1, 1) ��������� 0.25 �Թҷ��� Set ease in �� Sine
            .Join(star[1].rectTransform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))



            //��ѧ�ҡ��С�� Append ��ҹ� 1.5 �Թҷ� ���ա�������� star index 2 ࿤�� 1 ��������� 0.25 �Թҷ��� Set ease in Cubic
            .Insert(1.5f, star[2].DOFade(1, 0.25f).SetEase(Ease.InCubic))
            //������ǡѹ��� star index 2 �ӡ�� scale �� (2, 2, 2) ��������� 0.25 �Թҷ��� Set ease in �� Sine
            .Join(star[2].rectTransform.DOScale(Vector3.one * 2, 0.25f).SetEase(Ease.InSine)
            //���������Դ��� tween ��ҧ�鹢���դ���� onStart ���������§ starClip
            .OnStart(() =>
            {
                if (audioSource && starClip)
                {
                    PlayAudio(starClip);
                }
            }))
            //������ǡѹ��� star index 0 �ӡ�� scale �� (1, 1, 1) ��������� 0.25 �Թҷ��� Set ease in �� Sine
            .Join(star[0].rectTransform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))
            //��ѧ�ҡ��С�� Append ��ҹ� 1.75 �Թҷ� ���ա�������� star index 2  �ӡ�� scale �� (1, 1, 1) ��������� 0.25 �¡�� Set ease in �� Sine
            .Insert(1.75f, star[2].rectTransform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))

            ////////
            ///

            .Join(butten[1].rectTransform.DOScale(Vector3.one * 2, 0.25f).SetEase(Ease.InSine)
                //���������Դ��� tween ��ҧ�鹢���դ���� onStart ���������§ starClip
                .OnStart(() =>
                {
                    if (audioSource && starClip)
                    {
                        PlayAudio(starClip);
                    }
                }))



            //��ѧ�ҡ��С�� Append ��ҹ� 1.25 �Թҷ� ���ա�������� star index 0 ࿤�� 1 ��������� 0.25 �Թҷ��� Set ease in Cubic
            .Insert(1.25f, butten[0].DOFade(1, 0.25f).SetEase(Ease.InCubic))
            //������ǡѹ��� star index 0 �ӡ�� scale �� (2, 2, 2) ��������� 0.25 �Թҷ��� Set ease in �� Sine
            .Join(butten[0].rectTransform.DOScale(Vector3.one * 2, 0.25f).SetEase(Ease.InSine)
            //���������Դ��� tween ��ҧ�鹢���դ���� onStart ���������§ starClip
            .OnStart(() =>
            {
                if (audioSource && starClip)
                {
                    PlayAudio(starClip);
                }
            }))
            //������ǡѹ��� star index 1 �ӡ�� scale �� (1, 1, 1) ��������� 0.25 �Թҷ��� Set ease in �� Sine
            .Join(butten[1].rectTransform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))



            //��ѧ�ҡ��С�� Append ��ҹ� 1.5 �Թҷ� ���ա�������� star index 2 ࿤�� 1 ��������� 0.25 �Թҷ��� Set ease in Cubic
            .Insert(1.5f, butten[2].DOFade(1, 0.25f).SetEase(Ease.InCubic))
            //������ǡѹ��� star index 2 �ӡ�� scale �� (2, 2, 2) ��������� 0.25 �Թҷ��� Set ease in �� Sine
            .Join(butten[2].rectTransform.DOScale(Vector3.one * 2, 0.25f).SetEase(Ease.InSine)
            //���������Դ��� tween ��ҧ�鹢���դ���� onStart ���������§ starClip
            .OnStart(() =>
            {
                if (audioSource && starClip)
                {
                    PlayAudio(starClip);
                }
            }))
            //������ǡѹ��� star index 0 �ӡ�� scale �� (1, 1, 1) ��������� 0.25 �Թҷ��� Set ease in �� Sine
            .Join(butten[0].rectTransform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))
            //��ѧ�ҡ��С�� Append ��ҹ� 1.75 �Թҷ� ���ա�������� star index 2  �ӡ�� scale �� (1, 1, 1) ��������� 0.25 �¡�� Set ease in �� Sine
            .Insert(1.75f, butten[2].rectTransform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InSine))

            //////


            //����� tween �ͧ Join ���� Insert �ͧ Append ��͹˹�ҹ�騺ŧ
            //�зӡ�� scale ��� rect transform �ͧ background �� (0, 0, 0) ��������� 0.25 �Թҷ��� Set ease in �� Quart
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
