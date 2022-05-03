/*
 *  Author: Calvin Soueid
 *  Date:   22/11/2021
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityAsync;
using UnityEngine;

namespace Utility
{ 
    public static class Async
    {

        public class Timer
        {
            //Limit in seconds
            public float Limit = 1f;
            public float Current = 0f;
            public bool IsRepeating;

            public event Action<float> OnElapsed;
            private CancellationTokenSource TokenSource;

            public bool Enabled = false;

            private float StartTime;
            public void Start()
            {
                Current = 0f;
                Enabled = true;
                StartTime = Time.time;
                TokenSource = new CancellationTokenSource();
                Counter();
            }
            public void Stop()
            {
                Enabled = false;
                Current = 0f;
                TokenSource?.Cancel();
            }

            async void Counter()
            {
                while (Enabled)
                {

                    bool isCancelled = await WaitForSeconds(Limit, TokenSource.Token);
                    if (isCancelled) return;

                    OnElapsed?.Invoke(Time.time - StartTime);
                    StartTime = Time.time;
                    if (!IsRepeating) break;
                }
            }

            public Timer(float seconds, Action<float> callback, bool repeat = false)
            {
                IsRepeating = repeat;
                Limit = seconds;
                OnElapsed += callback;
            }


        }

        public static async Task<bool> WaitForSeconds(float time, CancellationToken token, IProgress<float> progress = null)
        {
            float currentTime = 0;
            while (currentTime < time)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                }
                catch (OperationCanceledException)
                {
                    return true;
                }
                currentTime += Time.deltaTime;
                progress?.Report(currentTime / time);
                await Await.NextUpdate();
            }
            return false;
        }
    }
}
