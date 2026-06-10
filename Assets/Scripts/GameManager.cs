using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Cosas pra el score del jugador
    int currentScore;

    //Cosas para el tiempo de la partida.
    float RemainigTime;

    //Singleton
    static public GameManager instance;


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
    }

    // Update is called once per frame
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
            //Se acabo el juego.
        }
    }
}