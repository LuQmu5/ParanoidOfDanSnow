using System;
using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;

public class DanEyes : MonoBehaviour
{
    [SerializeField] private float _visionRange;

    public event Action<float> SomethingSeen;

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Droid droid))
        {
            print("detected");
            Ray ray = new Ray(transform.position, droid.transform.position - transform.position);
            Physics.Raycast(ray, out RaycastHit hitInfo, _visionRange);

            if (hitInfo.collider.TryGetComponent(out Droid droidCollider))
            {
                print("Seen");
                SomethingSeen?.Invoke(GetDistanceTo(droidCollider.transform.position));
            }
        }
    }

    private float GetDistanceTo(Vector3 seenDroidPosition) => Vector3.Distance(transform.position, seenDroidPosition);
}
