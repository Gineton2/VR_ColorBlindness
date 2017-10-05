﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wilberforce;
using TMPro;

public class GameLogic2 : MonoBehaviour {
    public int ripeApples; //total ripe apples
    public int collectedApples; //ripe apples collected

    public int normalScore = 0;
    public int colorblindScore = 0;

    public GenerateScene fruitGenerator;

    public GameObject scoreSign; //score sign model
    public GameObject timeSign;  //time sign model

    public Canvas scoreCanvas;
    public Canvas timeCanvas;

    public TextMeshProUGUI timeText; //time text pro UGUI
    public TextMeshProUGUI scoreText; //score text pro UGUI

    public GameObject parkSign; // info screen sign
    public GameObject[] canvasObjs; //all canvas gameobjects

    public AudioSource sound; // Audio source on player camera
    public AudioClip yuck; // success noise
    public AudioClip yum; // failure noise

    public Camera mainCamera;

    public bool gameRunning;

    public float timeSetting = 10.0f;
    public float timeRemaining; //length of timer

    public int level = 0;

    void Start()
    {
        /*timeSign = GameObject.FindGameObjectWithTag("SignTime");
        timeText = timeSign.GetComponentInChildren<TextMeshProUGUI>();
        //timeCanvas = timeSign.GetComponent<Canvas>();
        //timeText = timeCanvas.GetComponent<TextMeshProUGUI>();

        scoreSign = GameObject.FindGameObjectWithTag("SignScore");
        scoreText = scoreSign.GetComponentInChildren<TextMeshProUGUI>();
        //scoreCanvas = scoreSign.GetComponent<Canvas>();
        //scoreText = scoreCanvas.GetComponent<TextMeshProUGUI>();*/

        StartGame();
       // sound = mainCamera.GetComponent<AudioSource>();

    }

    void StartGame()
    {
        level = 0;
        //disable timer/score signs
        scoreSign.SetActive(false);
        timeSign.SetActive(false);

        //show park sign
        parkSign.SetActive(true);

        //show explanation canvas
        canvasObjs[0].SetActive(true);
        canvasObjs[1].SetActive(false);
        canvasObjs[2].SetActive(false);
        canvasObjs[3].SetActive(false);


        //show instructions/play canvas

        

        //play game normal
    }

    public void gameNormal()
    {
        //generate fruit
        fruitGenerator.spawnApples();
        fruitGenerator.spawnFlowers();

        //set score
        collectedApples = 0;

        //set possible score
        ripeApples = GameObject.FindGameObjectsWithTag("RipeApple").Length;
        Debug.Log("Ripe Apples: " + ripeApples);

        //set timer
        timeRemaining = timeSetting;

        //turn off park sign
        parkSign.SetActive(false);

        //turn on score sign
        scoreSign.SetActive(true);
        timeSign.SetActive(true);

        gameRunning = true;
        level++;
    }

    public void gameColorblind()
    {
        //remove old fruit
        collectedApples = 0;
        
        //generate fruit
        fruitGenerator.spawnApples();
        fruitGenerator.spawnFlowers();

        //set timer
        timeRemaining = timeSetting;

        //turn off park sign
        parkSign.SetActive(false);

        //turn on score sign
        scoreSign.SetActive(true);
        timeSign.SetActive(true);

        //turn on protanopia vision
        Colorblind colorblindsetting = mainCamera.GetComponent<Colorblind>();
        colorblindsetting.Type = 2;

        gameRunning = true;
        level++;
    }

    public void StopGame(int endedLevel)
    {
        gameRunning = false;

        //turn onpark sign
        parkSign.SetActive(true);

        //turn on score sign
        scoreSign.SetActive(false);
        timeSign.SetActive(false);

        if (endedLevel == 1)
        {
            normalScore = collectedApples;
            Debug.Log("Normal Score: " + normalScore);
            //show red-green canvas
            canvasObjs[0].SetActive(false);
            canvasObjs[1].SetActive(false);
            canvasObjs[2].SetActive(true);
            canvasObjs[3].SetActive(false);
        }
        else if (endedLevel == 2)
        {
            colorblindScore = collectedApples;
            Debug.Log("Colorblind Score: " + colorblindScore);
            //show exit canvas
            canvasObjs[0].SetActive(false);
            canvasObjs[1].SetActive(false);
            canvasObjs[2].SetActive(false);
            canvasObjs[3].SetActive(true);

            //turn on normal vision
            Colorblind colorblindsetting = mainCamera.GetComponent<Colorblind>();
            colorblindsetting.Type = 0;
        }
    }

    void FixedUpdate()
    {
        if (gameRunning)
        {
            Debug.Log("Game is running");
            DecreaseTime();
        } else
        {
            Debug.Log("Game has stopped");
        }
        //UpdateTimer();
    }

    void DecreaseTime()
    {
        if (timeRemaining > 0.0f)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            StopGame(level);
        }

       UpdateTimer();
    }

    void UpdateTimer()
    {
        timeText.SetText("Time: " + timeRemaining.ToString("F2"));
    }

    void UpdateScore()
    {
        Debug.Log("Score: " + collectedApples);
        scoreText.SetText("Score: " + collectedApples);
    }

    // Triggered on click:
    // If apple is ripe, increment collectedApples, give positive audio feedback,
    // Else give negative audio feedback.
    // update updateHUD.
    public void OnAppleClick(GameObject other)
    {
        if (other.gameObject.CompareTag("RipeApple"))
        {
            // play "yum" noise
            /*sound.enabled = true;
            sound.clip = yum;
            sound.Play();*/
            collectedApples++;
            if (collectedApples == ripeApples)
            {
                Debug.Log("All apples collected");
                StopGame(level);
            }
        }
        else if (other.gameObject.CompareTag("UnripeApple"))
        {
            // play "yuck" noise
            /*sound.enabled = true;
            sound.clip = yuck;
            sound.Play();*/

            //assign some sort of penalty, like decreasing time
        }

        UpdateScore();
    }

    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }

}
