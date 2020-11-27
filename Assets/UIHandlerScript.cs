using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandlerScript : MonoBehaviour
{
    float CurrentTime = 0.0f;
    public float MaxTime = 15.0f;
    public Object UIMessage;

    // Start is called before the first frame update
    void Start()
    {
        CurrentTime = MaxTime;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTime -= 1 * Time.deltaTime;
        if(CurrentTime <= 0.0f)
        {
            Destroy(UIMessage);
        }
    }
}
