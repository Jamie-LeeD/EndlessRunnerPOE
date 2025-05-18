using UnityEngine;

public class Timer : MonoBehaviour
{
    float counter;
    float timerEnd;

    bool runTimer = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (runTimer) 
        {
            counter += Time.deltaTime;

        }
    }

    public void startTimer(float endTime)
    {
        runTimer = true;
        timerEnd = endTime;
    }
}
