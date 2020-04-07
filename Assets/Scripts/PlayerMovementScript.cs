using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerMovementScript : MonoBehaviour
{
    public GameObject gameStage;

    public Vector2 playerPos = new Vector2(0, 0);

    private AudioSource playerMoveSound;

    private void Start()
    {
        playerMoveSound = transform.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //poll for user input
        if (Input.GetKeyDown(KeyCode.D)){
            Vector2 changePos = playerPos;
            changePos.x += 1;

            MovePlayer(changePos, true);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector2 changePos = playerPos;
            changePos.x -= 1;

            MovePlayer(changePos, true);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Vector2 changePos = playerPos;
            changePos.y += 1;

            MovePlayer(changePos, true);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Vector2 changePos = playerPos;
            changePos.y -= 1;

            MovePlayer(changePos, true);
        }
    }

    private void MovePlayer(Vector2 changePos, bool playSound)
    {
        string panel = changePos.x + "_" + changePos.y;

        Debug.Log("Current panel is " + panel);

        if (gameStage.transform.Find(panel) != null)
        {
            GameObject nextPanel = gameStage.transform.Find(panel).gameObject;
            Vector3 transformPos = transform.position;

            transformPos.x = nextPanel.transform.position.x;
            transformPos.z = nextPanel.transform.position.z;

            transform.position = transformPos;
            playerPos = changePos;

            if(playSound)
                playerMoveSound.PlayOneShot(playerMoveSound.clip);
        } else
        {
            Debug.Log("Unable to find " + panel);
        }
    }

    public Vector2 GetPlayerPos()
    {
        return playerPos;
    }

    public void ResetPlayer()
    {
        MovePlayer(new Vector2(0,0), false);
    }
}
