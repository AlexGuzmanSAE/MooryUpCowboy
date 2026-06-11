using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Cosas pra el score del jugador
    int currentScore;

    //Cosas para el tiempo de la partida.
    float RemainigTime;

    //Singleton
    static public GameManager instance;

    public RectTransform GameOverLay;
    public TextMeshProUGUI scoreTxt;
    

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }


    void Start()
    {
        RemainigTime = 60.0f;
        currentScore = 0;
        GameOverLay.gameObject.SetActive(false);
    }

    void Update()
    {
        SubstractTime();
    }

    public void AddScore()
    {
        currentScore++;
        UI_Manager.instance.UpdateTextScore(currentScore);
    }

    void SubstractTime()
    {
        if (RemainigTime > 0)
        {
            RemainigTime -= Time.deltaTime;
           
        }
        else
        {
            
            GameOverLay.gameObject.SetActive(true);
            scoreTxt.text = string.Format("Score: {0}", currentScore);
        }
    }

    public void ResetGame()
    {
        //Destruir vacas
        //SpawnPosPLayer
        //reset tiempo, puntos

    }

}