using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingVis : MonoBehaviour
{
    // Breathing Vis
    public float minDist;
    public float maxDist;

    public GameObject VisL;
    public GameObject VisR;

    private RectTransform rtL;
    private RectTransform rtR;
    private RectTransform rt;

    private Quaternion OldRotation;


    // Breathing Sound
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
        rtL = VisL.GetComponent<RectTransform>();
        rtR = VisR.GetComponent<RectTransform>();
        rt = gameObject.GetComponent<RectTransform>();
        OldRotation = rt.localRotation;


        if (gameObject.GetComponent<AudioSource>()) {
            audioSource = gameObject.GetComponent<AudioSource>();
        }
        else {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

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
                    SetBreathingVis();

                    if (currentTimer <= 0)
                    {
                        started = false;
                        stage = "breathInDelay";
                    }
                }
                else {
                    currentTimer = breathInTime;
                    started = true;
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
                }

                break;

            case "breathOut":

                if (started) {
                    currentTimer -= Time.deltaTime;
                    SetBreathingVis();

                    if (currentTimer <= 0) {
                        started = false;
                        stage = "breathOutDelay";
                    }
                }
                else {
                    currentTimer = breathOutTime;
                    started = true;
                    audioSource.PlayOneShot(breathOutSound);
                }

                break;

            case "breathOutDelay":

                if (started) {
                    currentTimer -= Time.deltaTime;

                    if (currentTimer <= 0) {
                        started = false;
                        stage = "breathIn";
                        rt.localRotation = new Quaternion(OldRotation.x, OldRotation.y, OldRotation.z - 90, OldRotation.w);
                        OldRotation = rt.localRotation;
                    }
                }
                else {
                    currentTimer = delay;
                    started = true;
                }

                break;
        }
    }

    void SetBreathingVis()
    {
        

        if (stage == "breathIn") {
            rt.Rotate(0, 0, (45 / breathInTime) * Time.deltaTime);
            rtL.localPosition = new Vector3(((1 - (currentTimer / breathInTime)) * (maxDist-minDist) + minDist) * -1, 0, rtL.localPosition.z);
            rtR.localPosition = new Vector3(((1 - (currentTimer / breathInTime)) * (maxDist - minDist) + minDist), 0, rtL.localPosition.z);
        }
        else if (stage == "breathOut") {
            rt.Rotate(0, 0, (45 / breathOutTime) * Time.deltaTime);
            rtL.localPosition = new Vector3((((currentTimer / breathOutTime)) * (maxDist - minDist) + minDist) * -1, 0, rtL.localPosition.z);
            rtR.localPosition = new Vector3((((currentTimer / breathOutTime)) * (maxDist - minDist) + minDist), 0, rtL.localPosition.z);
        }

    }
}
