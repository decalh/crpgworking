/*
 * This Time System Is Initialized In The CameraController
 * 
 */

using System;
using UnityEngine;

public static class TimeSystem
{ 

    public class OnTickEventArgs : EventArgs
    {
        public int tick;
    }
    public static event EventHandler<OnTickEventArgs> OnTick;
    public static event EventHandler<OnTickEventArgs> OnTick_6;


    private const float TICK_TIMER_MAX = .5f;
    private static int tick;




    private static GameObject timeSystemGameObject;
    public static void Create()
    {
        if (timeSystemGameObject == null)
        {
            timeSystemGameObject = new GameObject("TimeSystem");
            timeSystemGameObject.AddComponent<TimeSystemObject>();
        }
    }

    public static int GetTick()
    {
        return tick;
    }


    private class TimeSystemObject : MonoBehaviour
    {
        private float tickTimer;


        private void Awake()
        {
            tick = 0;
        }


        void Update()
        {
            tickTimer += Time.deltaTime;
            if (tickTimer >= TICK_TIMER_MAX)
            {
                tickTimer -= TICK_TIMER_MAX;
                tick++;
                OnTick?.Invoke(this, new OnTickEventArgs { tick = tick });

                if (tick % 6 == 0)
                {
                    OnTick_6?.Invoke(this, new OnTickEventArgs { tick = tick });
                }
            }
        }
    }
}
