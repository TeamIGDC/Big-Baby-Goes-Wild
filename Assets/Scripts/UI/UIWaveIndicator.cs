using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UIWaveIndicator : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private float displayDuration = 2f;
    [SerializeField] private Vector3 startScale = Vector3.zero;
    [SerializeField] private Vector3 endScale = Vector3.one;
    [SerializeField] private float animationDuration = 0.5f;

    public UnityEvent OnWaveChange;

    private void Start()
    {
        if (waveText == null)
        {
            Debug.LogError("WaveText is not assigned.");
            enabled = false;
            return;
        }

        waveText.gameObject.SetActive(false);

        if (OnWaveChange == null)
        {
            OnWaveChange = new UnityEvent();
        }
        
        OnWaveChange.AddListener(AnimateWaveText);
    }

    private void AnimateWaveText()
    {
        waveText.gameObject.SetActive(true);
        waveText.transform.localScale = startScale;

        LeanTween.scale(waveText.gameObject, endScale, animationDuration).setEaseOutBack().setOnComplete(() =>
        {
            LeanTween.delayedCall(displayDuration, () =>
            {
                LeanTween.scale(waveText.gameObject, startScale, animationDuration).setEaseInBack().setOnComplete(() =>
                {
                    waveText.gameObject.SetActive(false);
                });
            });
        });
    }

    public void TriggerWaveChange(int waveNumber)
    {
        waveText.text = "Wave " + waveNumber;
        OnWaveChange.Invoke();
    }
}