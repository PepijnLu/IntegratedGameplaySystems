using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : EventUser
{
    private Dictionary<string, Slider> sliderDict;
    private delegate void ChangeSliderValueDelegate(string _stat, float _amount);
    public UIManager()
    {
        sliderDict = new()
        {
            ["HealthSlider"] = GameObject.Find("HealthSlider").GetComponent<Slider>(),
            ["ThirstSlider"] = GameObject.Find("ThirstSlider").GetComponent<Slider>(),
            ["HungerSlider"] = GameObject.Find("HungerSlider").GetComponent<Slider>()
        };
        Debug.Log("EventManager: " + eventManager);
        eventManager.SubscribeToEvent("OnStatChanged", new StringFloatDelegate(ChangeSliderValue));
    }

    private void ChangeSliderValue(string _stat, float _newAmount)
    {
        if(_newAmount > sliderDict[$"{_stat}Slider"].maxValue)
        {
            sliderDict[$"{_stat}Slider"].maxValue = _newAmount;
        } 
        sliderDict[$"{_stat}Slider"].value = _newAmount;
    }
}
