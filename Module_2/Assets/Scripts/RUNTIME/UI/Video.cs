using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;

public class Video : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private bool isOver = false;

    void Start()
    {
        videoPlayer = transform.GetComponent<VideoPlayer>();
    }

    void Update()
    {
        checkOver();
    }

    void checkOver()
    {
        long currentFrame = videoPlayer.frame;
        long frameCount = Convert.ToInt64(videoPlayer.frameCount);

        if (currentFrame > frameCount - 5)
        {
            isOver = true;
        }

    }

    public bool IsOver()
    {
        return isOver;
    }

    public void Reset()
    {
        isOver = false;
    }

    public void Play()
    {
        videoPlayer.Prepare();
        videoPlayer.Play();
    }
}
