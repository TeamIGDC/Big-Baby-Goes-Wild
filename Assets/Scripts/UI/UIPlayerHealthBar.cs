using System.Collections;
using System.Collections.Generic;
using Game.Entity;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPlayerHealthBar : MonoBehaviour
{
    [SerializeField] Slider _slider;
    private GameObject _player;
    private Player _playerScript;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("player");
        if(_player == null)
        {
            enabled = false;
        }
        _playerScript = _player.GetComponent<Player>();
        if(_playerScript == null)
        {
            enabled = false;
        }
        _slider.value = _slider.maxValue;
        _playerScript.OnHealthChange.AddListener(UpdateHealthUI);
    }

    public void UpdateHealthUI(float newValue)
    {
        StartCoroutine(SmoothLerpSliderValue(newValue));
    }

    private IEnumerator SmoothLerpSliderValue(float targetValue)
    {
        float startValue = _slider.value;
        float elapsedTime = 0f;

        while (elapsedTime < 0.1)
        {
            _slider.value = Mathf.Lerp(startValue, targetValue, elapsedTime / 0.1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _slider.value = targetValue; // Ensure the slider reaches the target value
    }

    void OnDestroy()
    {
        _playerScript.OnHealthChange.RemoveListener(UpdateHealthUI);
    }
}
