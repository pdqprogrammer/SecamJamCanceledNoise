using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameObject))]
public class StageManager : MonoBehaviour
{
    private List<GameObject> panels = new List<GameObject>();//list to hold all panels
    private List<GameObject> activePanels = new List<GameObject>();//list to currently active panels

    private int currActive = 4;
    private int avoidCount = 0;

    public float countdownTime = 3.0f;
    private float currCountdownTime = 0.0f;

    private bool stageActive = false;

    private bool gameOver = false;
    private bool playerWon = false;

    //add player in as a public variable
    public GameObject player;
    private PlayerMovementScript playerScript;

    //public materials for panels
    public Material secamBlack;
    public Material secamWhite;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<PlayerMovementScript>();

        foreach(Transform child in transform)
        {
            panels.Add(child.gameObject);
            Debug.Log("Added in child " + child.name);
        }

        ResetStage();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver && !playerWon && stageActive)
        {
            currCountdownTime += Time.deltaTime;

            if (currCountdownTime >= countdownTime - ((currActive - 4) * 0.1f))
            {
                IsGameOver();

                currCountdownTime = 0;
                ResetActivePanels(panels, activePanels);

                avoidCount++;

                if (avoidCount >= 3)
                {
                    currActive++;
                    avoidCount = 0;
                }

                SetActivePanels(panels, activePanels);
            }
        }
    }

    //method to add panels to active panels
    private void SetActivePanels(List<GameObject> panelList, List<GameObject> activePanelList)
    {
        if (activePanelList.Count >= currActive || panelList.Count == 0)
        {
            return;
        }

        //choose random gameobject from panelList
        int randomPanel = Random.Range(0, panelList.Count);

        GameObject panelChild = panelList[randomPanel].transform.GetChild(0).gameObject;
        panelChild.GetComponent<Renderer>().material = secamWhite;

        activePanelList.Add(panelList[randomPanel]);
        panelList.RemoveAt(randomPanel);

        SetActivePanels(panelList, activePanelList);
    }


    //method to reset active panels
    private void ResetActivePanels(List<GameObject> panelList, List<GameObject> activePanelList)
    {
        if(activePanelList.Count <= 0)
        {
            return;
        }

        GameObject activePanelChild = activePanelList[0].transform.GetChild(0).gameObject;
        activePanelChild.GetComponent<Renderer>().material = secamBlack;

        panelList.Add(activePanelList[0]);
        activePanelList.RemoveAt(0);

        ResetActivePanels(panelList, activePanelList);
    }

    private void IsGameOver()
    {
        if (panels.Count == 0)
        {
            Debug.Log("Player has won the game! Snow Man defeated!");
            playerWon = true;
            stageActive = false;
        }
        else {
            Vector2 playerPos = playerScript.GetPlayerPos();
            string playerPosString = playerPos.x + "_" + playerPos.y;

            //loop through all activel panels for player
            foreach (GameObject panel in activePanels)
            {
                //check if name matches player posString
                if (panel.name.Equals(playerPosString))
                {
                    Debug.Log("player has been hit");
                    gameOver = true;
                    stageActive = false;
                }
            }
        }
    }

    //method to check if player is on an active square when timer ends
    //game ends if player is in square
    //if player is not on it then add one to curr active
    //put active panels back onto panels list

    //public methods to handle game flow
    public void SetStageActive()
    {
        stageActive = true;
        SetActivePanels(panels, activePanels);
    }

    public void ResetStage()
    {
        gameOver = false;
        playerWon = false;

        currActive = 4;
        avoidCount = 0;
        currCountdownTime = 0.0f;
        ResetActivePanels(panels, activePanels);

        playerScript.ResetPlayer();
    }

    public float GetCurrTimer()
    {
        return currCountdownTime;
    }

    public bool GetGameOver()
    {
        return gameOver;
    }

    public bool GetPlayerWin()
    {
        return playerWon;
    }
}
