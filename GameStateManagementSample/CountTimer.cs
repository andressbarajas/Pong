using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework; 

namespace GameStateManagement
{
    public class CountTimer
    {
        private int endTimer; 
        private int countTimerRef; 
        public bool isRunning { get; private set; } 
        public bool isFinished { get; private set; } 

        public CountTimer() 
        { 
            endTimer = 0; 
            countTimerRef = 0; 
            isRunning = false; 
            isFinished = false; 
        } 

        public void Start(int seconds) 
        { 
            endTimer = seconds; 
            isRunning = true; 
        } 
          
        public bool CheckTime(GameTime gameTime) 
        { 
            countTimerRef += (int)gameTime.ElapsedGameTime.TotalMilliseconds; 
            if (!isFinished) 
            { 
                if (countTimerRef >= 1000.0f) 
                { 
                    endTimer = endTimer - 1; 
                    countTimerRef = 0; 
                    if (endTimer <= 0) 
                    { 
                        endTimer = 0; 
                        isFinished = true; 
                    } 
                } 
            } 
            return isFinished; 
        }

        public void Reset() 
        { 
            isRunning = false; 
            isFinished = false; 
            countTimerRef = 0; 
            endTimer = 0; 
        }     
    }
}

