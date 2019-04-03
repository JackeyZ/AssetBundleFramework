using UnityEngine;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEditor;

public class Movie : MonoBehaviour
{
    //Raw Image to Show Video Images [Assign from the Editor]
    public RawImage image;
    public Text text;
    //Video To Play [Assign from the Editor]
    public VideoClip videoToPlay;

    private VideoPlayer videoPlayer;
    private VideoSource videoSource;

    //Audio
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        //Application.runInBackground = true;
        StartCoroutine(playVideo());
    }

    IEnumerator playVideo()
    {
        //Add VideoPlayer to the GameObject
        videoPlayer = gameObject.AddComponent<VideoPlayer>();

        //Add AudioSource
        //audioSource = gameObject.AddComponent<AudioSource>();

        //Disable Play on Awake for both Video and Audio
        videoPlayer.playOnAwake = false;
        //audioSource.playOnAwake = false;

        //We want to play from video clip not from url
        videoPlayer.source = VideoSource.VideoClip;

        //Set video To Play then prepare Audio to prevent Buffering
        videoPlayer.clip = videoToPlay;

        //Debug.Log("Done Preparing Video");

        //Set Audio Output to AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.Direct;//VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        //videoPlayer.EnableAudioTrack(0, true);
        //videoPlayer.SetTargetAudioSource(0, audioSource);


        //Google到的解决方案
        //这里一定要让以上工作完成后才能开始准备videoPlayer  并且赋值视频输出Texture
        videoPlayer.Prepare();

        //Wait until video is prepared
        while (!videoPlayer.isPrepared)
        {
            Debug.Log("Preparing Video");
            yield return null;
        }

        //Set Raw Image to Show Video Images
        image.texture = videoPlayer.texture;
        //Play Video
        videoPlayer.Play();

        //Play Sound
        //audioSource.Play();

        //Debug.Log("Playing Video");
        while (videoPlayer.isPlaying)
        {
            Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
            yield return null;
        }

        Debug.Log("Done Playing Video");
    }

    void Update()
    {
        double curPlayTime = videoPlayer.time;
        text.text = (curPlayTime - (curPlayTime % 1)).ToString() + " / " + (videoPlayer.clip.length - videoPlayer.clip.length % 1).ToString();
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            double curTime = videoPlayer.time + videoPlayer.clip.length / 100;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                curTime += videoPlayer.clip.length / 10;
            }
            if (curTime > videoPlayer.clip.length)
            {
                curTime = videoPlayer.clip.length;
            }
            videoPlayer.time = curTime;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            double curTime = videoPlayer.time - videoPlayer.clip.length / 100;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                curTime -= videoPlayer.clip.length / 10;
            }
            if (curTime < 0)
            {
                curTime = 0;
            }
            videoPlayer.time = curTime;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
            }
            else
            {
                videoPlayer.Play();
            }
        }
    }
}