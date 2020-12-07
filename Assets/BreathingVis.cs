using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingVis : MonoBehaviour
{
    // Breathing Vis sides
    public GameObject VisL;
    public GameObject VisR;

    // Distance the sides will move
    public float BreathInDist;
    public float BreathOutDist;

    // The scale multiplier for middle
    public float breathInScaleMulti;
    public float breathOutScaleMulti;

    // RectTransforms for left/right sides, and rt for the middle
    private RectTransform rtL;
    private RectTransform rtR;
    private RectTransform rt;

    // The starting rotation
    private Quaternion oldRotation;

    // The starting scale
    private Vector3 startScale;



    // Breathing Sound
    AudioSource audioSource;

    // Sound effects
    public AudioClip breathInSound;
    public AudioClip breathOutSound;

    // The time for each breath, as well as the delay between them
    public float breathInTime;
    public float breathOutTime;
    public float delay;

    // The current timer for the breathing stage
    float currentTimer;

    // Breathing stages
    public enum stages {breatheIn, breatheInDelay, breatheOut, breatheOutDelay};
    public stages currentStage;

    // See if the stage has been started
    bool started;


    // Start is called before the first frame update
    void Awake()
    {
        // Get RectTrans Comp
        rtL = VisL.GetComponent<RectTransform>();
        rtR = VisR.GetComponent<RectTransform>();
        rt = gameObject.GetComponent<RectTransform>();

        // Set starting Vars
        oldRotation = rt.localRotation;
        startScale = rt.localScale;

        // Create/get audioSource Comp
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
        switch (currentStage)
        {
            case stages.breatheIn:

                if (started) {
                    currentTimer -= Time.deltaTime;
                    SetBreathingVis();

                    if (currentTimer <= 0)
                    {
                        started = false;
                        currentStage = stages.breatheInDelay;
                    }
                }
                else {
                    currentTimer = breathInTime;
                    started = true;
                    audioSource.PlayOneShot(breathInSound);
                }

                break;

            case stages.breatheInDelay:

                if (started) {
                    currentTimer -= Time.deltaTime;

                    if (currentTimer <= 0) {
                        started = false;
                        currentStage = stages.breatheOut;
                    }
                }
                else {
                    currentTimer = delay;
                    started = true;
                }

                break;

            case stages.breatheOut:

                if (started) {
                    currentTimer -= Time.deltaTime;
                    SetBreathingVis();

                    if (currentTimer <= 0) {
                        started = false;
                        currentStage = stages.breatheOutDelay;
                    }
                }
                else {
                    currentTimer = breathOutTime;
                    started = true;
                    audioSource.PlayOneShot(breathOutSound);
                }

                break;

            case stages.breatheOutDelay:

                if (started) {
                    currentTimer -= Time.deltaTime;

                    if (currentTimer <= 0) {
                        started = false;
                        currentStage = stages.breatheIn;
                        // Reset roation, set oldRotation Var
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

    // Update the breathing Visualiser
    void SetBreathingVis()
    {
        if (currentStage == stages.breatheIn) {
            // Rotate Center
            rt.Rotate(0, 0, (45 / breathInTime) * Time.deltaTime);
            // Rescale Center
            rt.localScale = new Vector3((1 - (currentTimer / breathInTime)) * (breathInScaleMulti * startScale.x - startScale.x * breathOutScaleMulti) + startScale.x * breathOutScaleMulti, (1 - (currentTimer / breathInTime)) * (breathInScaleMulti * startScale.y - startScale.y * breathOutScaleMulti) + startScale.y * breathOutScaleMulti, 1);
            // Move Outter Pieces
            rtL.localPosition = new Vector3(((1 - (currentTimer / breathInTime)) * (BreathOutDist - BreathInDist) + BreathInDist) * -1, 0, rtL.localPosition.z);
            rtR.localPosition = new Vector3(((1 - (currentTimer / breathInTime)) * (BreathOutDist - BreathInDist) + BreathInDist), 0, rtL.localPosition.z);
        }
        else if (currentStage == stages.breatheOut) {
            // Rotate Center
            rt.Rotate(0, 0, (45 / breathOutTime) * Time.deltaTime);
            // Rescale Center
            rt.localScale = new Vector3((1 - (currentTimer / breathOutTime)) * (breathOutScaleMulti * startScale.x - startScale.x * breathInScaleMulti) + startScale.x * breathInScaleMulti, (1 - (currentTimer / breathOutTime)) * (breathOutScaleMulti * startScale.y - startScale.y * breathInScaleMulti) + startScale.y * breathInScaleMulti, 1);
            // Move Outter Pieces
            rtL.localPosition = new Vector3((((currentTimer / breathOutTime)) * (BreathOutDist - BreathInDist) + BreathInDist) * -1, 0, rtL.localPosition.z);
            rtR.localPosition = new Vector3((((currentTimer / breathOutTime)) * (BreathOutDist - BreathInDist) + BreathInDist), 0, rtL.localPosition.z);
        }
        
    }
}
