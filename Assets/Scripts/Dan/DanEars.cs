using System;
using System.Collections;
using UnityEngine;

public class DanEars : MonoBehaviour
{
    [SerializeField] private float _hearingRange = 12;
    [SerializeField] private float _coolingHearEffectDelay = 1;

    private float _currentHearEffectDelay = 0;
    private bool _isCooling = true;

    public event Action<float> SomethingHeard;

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Droid droid))
        {
            print("Heard");
            SomethingHeard?.Invoke(GetDistanceTo(droid.transform.position));

            return;
            _currentHearEffectDelay += Time.deltaTime;

            if (_currentHearEffectDelay >= _coolingHearEffectDelay)
            {
                _isCooling = false;
                _currentHearEffectDelay = _coolingHearEffectDelay;
                SomethingHeard?.Invoke(GetDistanceTo(droid.transform.position));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Droid droid))
        {
            _isCooling = true;
        }
    }

    private void Update()
    {
        if (_isCooling && _currentHearEffectDelay > 0)
            _currentHearEffectDelay -= Time.deltaTime;
    }

    private float GetDistanceTo(Vector3 soundPosition) => Vector3.Distance(transform.position, soundPosition);
}