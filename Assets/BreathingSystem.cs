using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingSystem : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip breathInSound;
    public AudioClip breathOutSound;

    public float breathInTime;
    public float breathOutTime;
    public float delay;

    float currentTimer;

    string stage = "breathIn";

    bool started;



    // Start is called before the first frame update
    void Awake()
    {
        if (gameObject.GetComponent<AudioSource>()){
            audioSource = gameObject.GetComponent<AudioSource>();
        }
        else {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        //audioSource.PlayOneShot(breathInSound, 1);
    }

    // Update is called once per frame
    void Update()
    {

        // Each stage of breathing
        switch (stage)
        {
            case "breathIn":

                if (started) {
                    currentTimer -= Time.deltaTime;

                    if (currentTimer <= 0) {
                        started = false;
                        stage = "breathInDelay";
                    }
                }
                else {
                    currentTimer = breathInTime;
                    started = true;
                    print(stage);
                    audioSource.PlayOneShot(breathInSound);
                }
                
                break;

            case "breathInDelay":

                if (started) {
                    currentTimer -= Time.deltaTime;

                    if (currentTimer <= 0) {
                        started = false;
                        stage = "breathOut";
                    }
                }
                else {
                    currentTimer = delay;
                    started = true;
                    print(stage);
                }

                break;

            case "breathOut":

                if (started) {
                    currentTimer -= Time.deltaTime;

                    if (currentTimer <= 0) {
                        started = false;
                        stage = "breathOutDelay";
                    }
                }
                else {
                    currentTimer = breathOutTime;
                    started = true;
                    print(stage);
                    audioSource.PlayOneShot(breathOutSound);
                }

                break;

            case "breathOutDelay":

                if (started) {
                    currentTimer -= Time.deltaTime;

                    if (currentTimer <= 0) {
                        started = false;
                        stage = "breathIn";
                    }
                }
                else {
                    currentTimer = delay;
                    started = true;
                    print(stage);
                }

                break;
        }
    }
}
