using UnityEngine;

public enum DanAnimations
{
    Idle = 0,
    Walk = 1
}

public class DanView : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private const string SpeedMultiplier = nameof(SpeedMultiplier);
    private const float MaxAnimSpeed = 2;

    public void PlayWalkAnimation()
    {
        _animator.SetTrigger(DanAnimations.Walk.ToString());
    }

    public void PlayIdleAnimation()
    {
        _animator.SetTrigger(DanAnimations.Idle.ToString());
    }

    public void SetAnimationSpeedMultiplier(float value)
    {
        if (value > MaxAnimSpeed)
            value = MaxAnimSpeed;

        _animator.SetFloat(SpeedMultiplier, value);
    }
}