using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : EventUser
{
    private Dictionary<string, Slider> sliderDict;
    private SpriteRenderer overlayRenderer;
    private TextMeshProUGUI scoreText;
    public UIManager(SpriteRenderer _overlayRenderer, TextMeshProUGUI _scoreText)
    {
        sliderDict = new()
        {
            ["HealthSlider"] = GameObject.Find("HealthSlider").GetComponent<Slider>(),
            ["ThirstSlider"] = GameObject.Find("ThirstSlider").GetComponent<Slider>(),
            ["HungerSlider"] = GameObject.Find("HungerSlider").GetComponent<Slider>()
        };
        Debug.Log("EventManager: " + eventManager);

        eventManager.SubscribeToEvent("OnStatChanged", new StringFloatDelegate(ChangeSliderValue));
        eventManager.SubscribeToEvent("ChangeOverlayOpacity", new FloatDelegate(ChangeOverlayOpacity));
        eventManager.SubscribeToEvent("UpdateScore", new FloatDelegate(UpdateScore));

        overlayRenderer = _overlayRenderer;
        scoreText = _scoreText;
    }

    private void ChangeSliderValue(string _stat, float _newAmount)
    {
        if(_newAmount > sliderDict[$"{_stat}Slider"].maxValue)
        {
            sliderDict[$"{_stat}Slider"].maxValue = _newAmount;
        } 
        sliderDict[$"{_stat}Slider"].value = _newAmount;
    }

    private void ChangeOverlayOpacity(float _newAmount)
    {
        Color color = overlayRenderer.color;
        color.a = _newAmount;
        overlayRenderer.color = color;
    }

    private void UpdateScore(float _amount)
    {
        ScoreData.score += _amount;
        scoreText.text = $"Score: {ScoreData.score}";
    }
}
