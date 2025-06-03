using System.Collections;
using System.Collections.Generic;
using Player.Enums;
using Player.Movement;
using UnityEngine;

public class TimeRetarder : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    
    private float _timer;

    public void SlowTime(float duration, AnimationCurve animationCurve)
    {
        float adjustedDuration = CalculateRealDuration(animationCurve, duration);
        
        StartCoroutine(TimeCoroutine(adjustedDuration, animationCurve));
    } 

    private IEnumerator TimeCoroutine(float duration, AnimationCurve animationCurve)
    {
        float timeBefore = Time.timeScale;
        float fixedDeltaTimeBefore = Time.fixedDeltaTime;

        _timer = 0f;

        while (_playerMovement.PlayerMovementState == PlayerMovementState.Jump)
        {
            float t = Mathf.Clamp01(_timer / duration);
            Time.timeScale = Mathf.Clamp(animationCurve.Evaluate(t), 0.1f, 1f);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            _timer += Time.unscaledDeltaTime;

            yield return null;
        }

        Time.timeScale = timeBefore;
        Time.fixedDeltaTime = fixedDeltaTimeBefore;
    }
        
    private float CalculateRealDuration(AnimationCurve curve, float duration, int steps = 1000)
    {
        float realDuration = 0f;
        float dt = duration / steps;

        for (int i = 0; i < steps; i++)
        {
            float t = i / (float)steps;
            float timeScaleValue = Mathf.Clamp(curve.Evaluate(t), 0.01f, 1f);
            realDuration += dt / timeScaleValue;
        }

        return realDuration;
    }
}
