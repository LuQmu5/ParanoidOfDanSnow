using UnityEngine;

public class DroidScanner : MonoBehaviour
{
    [SerializeField] private float _clearScanDistance = 15;
    [SerializeField] private float _blockedVisionScanDistance = 5; // можно еще по слоям помножать в зависимости от того какой прочности и сопротивляемости
    [SerializeField] private float _scanSpeed = 2;
    [SerializeField] private LayerMask _danMask;

    private float _scanProgress = 0;
    private float _blockedScanCoeff = 0.5f;

    public float ScanProgress => _scanProgress;

    private void OnTriggerStay(Collider other)
    {
        float coeff = 0.1f;

        if (other.TryGetComponent(out DanMind dan))
        {
            print("detected");
            Ray ray = new Ray(transform.position, dan.transform.position - transform.position);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, _clearScanDistance))
            {
                if (hitInfo.collider && hitInfo.collider.TryGetComponent(out DanMind danMind))
                {
                    _scanProgress += Time.deltaTime * _scanSpeed * coeff * Vector2.Distance(transform.position, dan.transform.position);
                }
            }
            else if (Physics.Raycast(ray, out hitInfo, _blockedVisionScanDistance, _danMask))
            {
                if (hitInfo.collider && hitInfo.collider.TryGetComponent(out DanMind danMind))
                {
                    _scanProgress += Time.deltaTime * _scanSpeed * _blockedScanCoeff * coeff * Vector2.Distance(transform.position, dan.transform.position);
                }
            }
        }
    }
}
