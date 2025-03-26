using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class DanMind : MonoBehaviour
{
    private const float MaxParanoidLevel = 100;
    private const float TurnOffChanceForParanoidLevel = 0.5f;

    [Header("Roadmap")]
    [SerializeField] private Transform[] _roadmap;

    [Header("Slaveries Dan's Body Parts")]
    [SerializeField] private DanEars _ears;
    [SerializeField] private DanEyes _eyes;
    [SerializeField] private DanLegs _legs;
    [SerializeField] private DanView _view;

    public float ParanoidLevel { get; private set; }
    public float TurnOffChance => ParanoidLevel * TurnOffChanceForParanoidLevel;

    private void OnEnable()
    {
        _ears.SomethingHeard += OnSomethingHeard;
        _eyes.SomethingSeen += OnSomethingSeen;
    }

    private void OnDisable()
    {
        _ears.SomethingHeard -= OnSomethingHeard;
        _eyes.SomethingSeen -= OnSomethingSeen;
    }

    private void Start()
    {
        StartCoroutine(MovingThroughRoadmap());
    }

    private void Update()
    {
        ChillOutFromParanoid();
    }

    private void ChillOutFromParanoid()
    {
        float coeff = 2.5f;

        if (ParanoidLevel > 0)
        {
            ParanoidLevel -= Time.deltaTime * coeff * (1/ParanoidLevel);
            _view.SetAnimationSpeedMultiplier(1 + 50 / ParanoidLevel);
        }
    }

    private IEnumerator MovingThroughRoadmap()
    {
        while (true)
        {
            Vector3 nextPoint = GetNextRoadmapPoint();
            _view.PlayWalkAnimation();

            while (Vector3.Distance(transform.position, nextPoint) > transform.localScale.x)
            {
                _legs.TryMoveTo(nextPoint, ParanoidLevel);

                yield return null;
            }

            _view.PlayIdleAnimation();

            yield return new WaitForSeconds(2.28f - 0.2f * ParanoidLevel);
        }
    }

    private Vector3 GetNextRoadmapPoint()
    {
        return _roadmap[Random.Range(0, _roadmap.Length)].position;
    }

    private void OnSomethingHeard(float distance)
    {
        float coef = 0.5f;
        // ParanoidLevel = Mathf.Clamp(ParanoidLevel * 15f * Time.deltaTime + 1/distance + (1 / Mathf.Pow(distance, coef)), ParanoidLevel, MaxParanoidLevel);
        ParanoidLevel += coef;
        _view.SetAnimationSpeedMultiplier(1 + 50/ParanoidLevel);
    }

    private void OnSomethingSeen(float distance)
    {
        float coef = 0.785f;
        // ParanoidLevel = Mathf.Clamp(ParanoidLevel * 20 * Time.deltaTime + 1 / Mathf.Pow(distance, coef), ParanoidLevel, MaxParanoidLevel);
        ParanoidLevel += coef;
        _view.SetAnimationSpeedMultiplier(1 + 50/ParanoidLevel);
    }
}
