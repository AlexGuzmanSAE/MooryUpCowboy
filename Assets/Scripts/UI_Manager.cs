using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _cowCounterText;

    [SerializeField]
    private Image _fillDayNightMeter;

    private int _cowCount = 0;


    private void OnEnable()
    {
        Cow_Collector collector = FindObjectOfType<Cow_Collector>();
        collector.colectionEvent += UpdateCowCounter;
    }

    void Start()
    {
        _cowCounterText.text = string.Format("X {0}", _cowCount);
        _fillDayNightMeter.fillAmount = 1;
    }

    private void UpdateCowCounter(int amount)
    {
        _cowCount += amount;
        _cowCounterText.text = string.Format("X {0}", _cowCount);
    }

    // This is a placeholder for the day/night meter, it will be updated in the future when the day/night cycle is implemented

}
