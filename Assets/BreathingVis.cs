using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingVis : MonoBehaviour
{
    // Breathing Vis
    public float minDist;
    public float maxDist;

    public float breathInSizeMulti;
    public float breathOutSizeMulti;


    public GameObject VisL;
    public GameObject VisR;

    private RectTransform rtL;
    private RectTransform rtR;
    private RectTransform rt;

    private Quaternion oldRotation;

    private Vector3 startScale;


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
        oldRotation = rt.localRotation;
        startScale = rt.localScale;

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
                        rt.localRotation = new Quaternion(oldRotation.x, oldRotation.y, oldRotation.z - 90, oldRotation.w);
                        oldRotation = rt.localRotation;
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
            // Rotate Center
            rt.Rotate(0, 0, (45 / breathInTime) * Time.deltaTime);
            // Rescale Center
            rt.localScale = new Vector3((1 - (currentTimer / breathInTime)) * (breathInSizeMulti * startScale.x - startScale.x * breathOutSizeMulti) + startScale.x * breathOutSizeMulti, (1 - (currentTimer / breathInTime)) * (breathInSizeMulti * startScale.y - startScale.y * breathOutSizeMulti) + startScale.y * breathOutSizeMulti, 1);
            // Move Outter Pieces
            rtL.localPosition = new Vector3(((1 - (currentTimer / breathInTime)) * (maxDist - minDist) + minDist) * -1, 0, rtL.localPosition.z);
            rtR.localPosition = new Vector3(((1 - (currentTimer / breathInTime)) * (maxDist - minDist) + minDist), 0, rtL.localPosition.z);
        }
        else if (stage == "breathOut") {
            // Rotate Center
            rt.Rotate(0, 0, (45 / breathOutTime) * Time.deltaTime);
            // Rescale Center
            rt.localScale = new Vector3((1 - (currentTimer / breathOutTime)) * (breathOutSizeMulti * startScale.x - startScale.x * breathInSizeMulti) + startScale.x * breathInSizeMulti, (1 - (currentTimer / breathOutTime)) * (breathOutSizeMulti * startScale.y - startScale.y * breathInSizeMulti) + startScale.y * breathInSizeMulti, 1);
            // Move Outter Pieces
            rtL.localPosition = new Vector3((((currentTimer / breathOutTime)) * (maxDist - minDist) + minDist) * -1, 0, rtL.localPosition.z);
            rtR.localPosition = new Vector3((((currentTimer / breathOutTime)) * (maxDist - minDist) + minDist), 0, rtL.localPosition.z);
        }
        
    }
}
