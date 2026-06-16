using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Cosas pra el score del jugador
    int currentScore;

    //Cosas para el tiempo de la partida.
   public float RemainigTime;

    //Singleton
    static public GameManager instance;

    public RectTransform GameOverLay;
    public TextMeshProUGUI scoreTxt;


    public Transform spawnPlayer;

    public GameObject playerReference;

    public bool isStarted;

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
        isStarted = false;
        GameOverLay.gameObject.SetActive(false);
        StartGame();
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
        if (RemainigTime > 0 && isStarted)
        {
            RemainigTime -= Time.deltaTime;

        }
        else
        {
            End();
        }
    }



    public void StartGame()
    {

        GameOverLay.gameObject.SetActive(false);
        isStarted = true;
    }

    public void End()
    {
        GameOverLay.gameObject.SetActive(true);
        isStarted = false;
    }

    public void Reset()
    {
        print("reset");
        CowSpawner.instance.EmptyListOfCows();
        if (playerReference != null)
        {
            playerReference.transform.position = spawnPlayer.position;
        }
        RemainigTime = 60.0f;
        currentScore = 0;
        UI_Manager.instance.UpdateTextScore(0);

        GameOverLay.gameObject.SetActive(false);
        isStarted = true;
    }

}
