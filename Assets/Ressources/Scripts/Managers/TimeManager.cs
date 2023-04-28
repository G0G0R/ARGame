using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    private static TimeManager instance;
    public static TimeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TimeManager>();
            }

            return instance;
        }
    }

    private bool timeFrozen = false;


    private int tick;


    public int Tick { get { return tick; } set { tick = value; } }


    public Action OnMinuteChanged { get => onMinuteChanged; set => onMinuteChanged = value; }

    private float minuteToRealTime = 1f;
    private float timer;

    private Action onMinuteChanged;


    // Start is called before the first frame update
    void Start()
    {
        tick = 0;

        timer = minuteToRealTime;

        
    }

    // Update is called once per frame
    void Update()
    {

        if (timeFrozen == false)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                tick++;

                if (tick % 5 == 0)
                {
                    GameManager.Instance.MAJRessources();
                }

                onMinuteChanged?.Invoke();
                timer = minuteToRealTime;
            }
        }

    }

    public void StartTime()
    {
        timeFrozen = false;
    }

    public void StopTime()
    {
        timeFrozen = true;
    }

    public void ResetTime()
    {
        Tick = 0;
    }
}
