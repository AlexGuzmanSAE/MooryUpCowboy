using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;
    [SerializeField]
    private TextMeshProUGUI _cowCounterText;

    [SerializeField]
    private Image _fillDayNightMeter;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else
            Destroy(this);
    }



    void Start()
    {
        if(_cowCounterText)
        {
            _cowCounterText.text = 0.ToString();
        }
    }

    public void UpdateTextScore(int newValue)
    {
        _cowCounterText.text = newValue.ToString();
    }




}
