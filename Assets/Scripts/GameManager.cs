using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum GameStates
{
    Menu,
    InGame,
    GameOver,
    Win
}
public class GameManager : MonoBehaviour
{
    public Canvas canvas;
    private GameStates gamestate;

    public GameObject gameStage;
    private StageManager stageManager;

    public float menuResetTimer = 4.0f;
    public float gameStartTimer = 2.0f;
    private float currMenuTime = 0.0f;

    private Animator canvasAnimator;

    private GameObject onScreenControlObj;

    public AudioClip inGameClip;
    public AudioClip menuClip;
    public AudioClip staticClip;

    private AudioSource gameManagerAudio;

    // Start is called before the first frame update
    void Start()
    {
        stageManager = gameStage.transform.GetComponent<StageManager>();
        gamestate = GameStates.Menu;
        canvasAnimator = canvas.GetComponent<Animator>();
        onScreenControlObj = canvas.transform.Find("OnScreenControlsPanel").gameObject;

        gameManagerAudio = transform.GetComponent<AudioSource>();
        gameManagerAudio.clip = menuClip;
        gameManagerAudio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (gamestate == GameStates.Menu)
        {
            //check for player input
            //if r pressed then start game
            //if q pressed quit game
            if (Input.GetKeyUp(KeyCode.R))
            {
                gamestate = GameStates.InGame;
                Debug.Log("changing state game");
                //load in animation plays
                
                canvasAnimator.SetTrigger("startgame");
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                Debug.Log("quitting app");
                Application.Quit();
            }
        }
        else if (gamestate == GameStates.InGame)
        {
            //wait to start game
            if (currMenuTime < gameStartTimer)
            {
                currMenuTime += Time.deltaTime;

                if(currMenuTime >= gameStartTimer)
                {
                    
                    stageManager.SetStageActive();
                    onScreenControlObj.SetActive(true);

                    gameManagerAudio.Stop();
                    gameManagerAudio.clip = inGameClip;
                    gameManagerAudio.Play();
                }
            }
            

            if (stageManager.GetPlayerWin())
            {
                //load player won animation
                gamestate = GameStates.Win;
                Debug.Log("changing state won");
                currMenuTime = 0.0f;
                canvasAnimator.SetTrigger("playerwon");
                onScreenControlObj.SetActive(false);

                gameManagerAudio.Stop();
                gameManagerAudio.clip = staticClip;
                gameManagerAudio.volume = 0.3f;
                gameManagerAudio.Play();
            }

            if (stageManager.GetGameOver())
            {
                //load game over animation
                gamestate = GameStates.GameOver;
                Debug.Log("changing state lost");
                currMenuTime = 0.0f;
                canvasAnimator.SetTrigger("gameover");
                onScreenControlObj.SetActive(false);

                gameManagerAudio.Stop();
                gameManagerAudio.clip = staticClip;
                gameManagerAudio.volume = 0.3f;
                gameManagerAudio.Play();
            }
        }
        else if (gamestate == GameStates.GameOver || gamestate == GameStates.Win)
        {
            //countdown until resetting to menu
            //load menu state once countdown is over
            currMenuTime += Time.deltaTime;

            if(currMenuTime >= menuResetTimer)
            {
                currMenuTime = 0;
                gamestate = GameStates.Menu;
                Debug.Log("changing state menu");
                //load in main menu animation
                canvasAnimator.SetTrigger("menu");
                stageManager.ResetStage();

                gameManagerAudio.Stop();
                gameManagerAudio.clip = menuClip;
                gameManagerAudio.volume = 0.4f;
                gameManagerAudio.Play();
            }
        }
    }
}
