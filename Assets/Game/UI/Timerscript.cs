using UnityEngine;

public class NewMonoBehaviourScript : ScriptableObject
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {public float TimeLeft;
    public bool TimerOn = False;

   
        
    }

    // Update is called once per frame
    void Update()
    {
    if(TimerOn)
    {
        if(timeLeft > 0)
        {
            TimeLeft -= Time.deltaTime;
        }
    }
}

void updateTimer(float currentTime)
{
    currentTime += 1;

}
