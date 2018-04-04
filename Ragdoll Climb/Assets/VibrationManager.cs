using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class VibrationManager : MonoBehaviour
{
    // Which controller to vibrate
    internal PlayerIndex playerIndex;
    
    enum VibrationStates {none, increasing, vibrating, decreasing};
    VibrationStates state = VibrationStates.none;
    
    float timer = 0;
    float duration = 0;

    float currentStrengthLeft = 0f;
    float currentStrengthRight = 0f;
    float desiredStrengthLeft = 0f;
    float desiredStrengthRight = 0f;

    // The priority value of the current vibration
    int currentPriority = 0;

    // Offset from desired viration strength before we can go to next state
    float desiredOffset = 0.005f;

    float increaseSpeed = 1f;
    float decreaseSpeed = 1f;


    void Update ()
    {
		switch (state)
        {
            // Start of vibration, increases strength to desired one
            case VibrationStates.increasing:
                // Increases strength with lerping
                currentStrengthLeft = Mathf.Lerp(currentStrengthLeft, desiredStrengthLeft, increaseSpeed * Time.deltaTime);
                currentStrengthRight = Mathf.Lerp(currentStrengthRight, desiredStrengthRight, increaseSpeed * Time.deltaTime);

                // If both left and right dtrength is close enough to desired value
                if (currentStrengthLeft >= desiredStrengthLeft - desiredOffset && currentStrengthRight >= desiredStrengthRight - desiredOffset)
                {
                    // Prepares for next state
                    currentStrengthLeft = desiredStrengthLeft;
                    currentStrengthRight = desiredStrengthRight;
                    state = VibrationStates.vibrating;
                }

                // Sets vibration
                GamePad.SetVibration(playerIndex, currentStrengthLeft, currentStrengthRight);

                break;

            
            // State of constant and consistent vibration
            case VibrationStates.vibrating:
                timer += Time.deltaTime;

                // If it's time to stop
                if (timer >= duration)
                {
                    state = VibrationStates.decreasing;
                }

                // Sets viration anew if the duration is longer than 2.5 seconds
                // This prevents Xbox One controllers from stopping long vibrations
                if (duration >= 2.5f)
                    GamePad.SetVibration(playerIndex, currentStrengthLeft, currentStrengthRight);

                break;


            // End of vibration, decreases strength to 0
            case VibrationStates.decreasing:
                // Decreases strength with lerping
                currentStrengthLeft = Mathf.Lerp(currentStrengthLeft, 0f, decreaseSpeed * Time.deltaTime);
                currentStrengthRight = Mathf.Lerp(currentStrengthRight, 0f, decreaseSpeed * Time.deltaTime);
                
                // If both left and right dtrength is close enough to 0
                if (currentStrengthLeft <= 0 + desiredOffset && currentStrengthRight <= 0f + desiredOffset)
                {
                    // Prepares vibration shutdown
                    currentStrengthLeft = 0f;
                    currentStrengthRight = 0f;
                    state = VibrationStates.none;
                    // Resets current priority value to lowest possible so any vibration can start next
                    currentPriority = Mathf.RoundToInt(Mathf.NegativeInfinity);
                }

                // Sets vibration
                GamePad.SetVibration(playerIndex, currentStrengthLeft, currentStrengthRight);
                break;


            case VibrationStates.none:
                break;
        }
    }


    // Sets vibration, each side of controller has equal strength. No automation.
    public void VibrationManual(float strength, int priority)
    {
        // If this new vibration has higher or equal priority than the current one
        if (priority >= currentPriority)
        {
            state = VibrationStates.none;

            currentPriority = priority;

            GamePad.SetVibration(playerIndex, strength, strength);
        }
    }
    // Sets vibration, each side of controller can have seperate strength. No automation.
    public void VibrationManual(float leftStrength, float rightStrength, int priority)
    {
        // If this new vibration has higher or equal priority than the current one
        if (priority >= currentPriority)
        {
            state = VibrationStates.none;

            currentPriority = priority;

            GamePad.SetVibration(playerIndex, leftStrength, rightStrength);
        }
    }


    // Sets vibration, each side of controller has equal strength. Turns of automatically after set time.
    public void VibrateTimed(float strength, float time, int priority)
    {
        // If this new vibration has higher or equal priority than the current one
        if (priority >= currentPriority)
        {
            timer = 0;
            duration = time;
            desiredStrengthLeft = strength;
            desiredStrengthRight = strength;

            increaseSpeed = Mathf.Infinity;
            decreaseSpeed = Mathf.Infinity;

            currentPriority = priority;

            state = VibrationStates.increasing;
        }
    }
    // Sets vibration, each side of controller can have seperate strength. Turns of automatically after set time.
    public void VibrateTimed(float leftStrength, float rightStrength, float time, int priority)
    {
        // If this new vibration has higher or equal priority than the current one
        if (priority >= currentPriority)
        {
            timer = 0;
            duration = time;
            desiredStrengthLeft = leftStrength;
            desiredStrengthRight = rightStrength;

            increaseSpeed = Mathf.Infinity;
            decreaseSpeed = Mathf.Infinity;

            currentPriority = priority;

            state = VibrationStates.increasing;
        }
    }


    // Sets vibration, each side of controller have equal strength. Strength is lerped when increasing and decreasing. Begins to decrease automatically after set time after increase is done.
    public void VibrateSmoothTimed(float strength, float time, float strengthIncreaseSpeed, float strengthDecreaseSpeed, int priority)
    {
        // If this new vibration has higher or equal priority than the current one
        if (priority >= currentPriority)
        {
            timer = 0;
            duration = time;
            desiredStrengthLeft = strength;
            desiredStrengthRight = strength;

            increaseSpeed = strengthIncreaseSpeed;
            decreaseSpeed = strengthDecreaseSpeed;

            currentPriority = priority;

            state = VibrationStates.increasing;
        }
    }
    // Sets vibration, each side of controller can have seperate strength. Strength is lerped when increasing and decreasing. Begins to decrease automatically after set time after increase is done.
    public void VibrateSmoothTimed(float leftStrength, float rightStrength, float time, float strengthIncreaseSpeed, float strengthDecreaseSpeed, int priority)
    {
        // If this new vibration has higher or equal priority than the current one
        if (priority >= currentPriority)
        {
            timer = 0;
            duration = time;
            desiredStrengthLeft = leftStrength;
            desiredStrengthRight = rightStrength;

            increaseSpeed = strengthIncreaseSpeed;
            decreaseSpeed = strengthDecreaseSpeed;

            currentPriority = priority;

            state = VibrationStates.increasing;
        }
    }


    // Stops current vibration
    public void StopVibration(int priority)
    {
        // If this new vibration has higher or equal priority than the current one
        if (priority >= currentPriority)
        {
            GamePad.SetVibration(playerIndex, 0f, 0f);
            
            currentPriority = Mathf.RoundToInt(Mathf.NegativeInfinity);

            state = VibrationStates.none;
        }
    }
}
