using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Cosas pra el score del jugador
    int currentScore;

    //Cosas para el tiempo de la partida.
    float RemainigTime;
    float maxTime;
   //Singleton
   static public GameManager instance;

   

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this);
    }


    void Start()
    {

        currentScore = 0;     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore()
    {
        currentScore++; 
       UI_Manager.instance.UpdateTextScore(currentScore);
    }


}
