using UnityEngine;

public class Droid : MonoBehaviour
{
    [Header("My Target")]
    [SerializeField] private DanMind _dan;

    [Header("Dependencies")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private DroidInfoDisplay _infoDisplay;
    [SerializeField] private DroidScanner _scanner;

    [Header("Params")]
    [SerializeField] private float _moveForce = 5;
    [SerializeField] private float _glideForce = 5;
    [SerializeField] private float _scanSpeed = 2;
    [SerializeField] private LayerMask _targetMask;

    private PlayerInput _input;
    private ForceMode _forceMode = ForceMode.Force;
    private bool _isAutoPilot = false;
    private float _rotateSpeed = 20;
    private float _maxRotationX = 85;

    private void Awake()
    {
        _input = new PlayerInput();
        _input.Enable();

        _rigidbody.isKinematic = true;
        _isAutoPilot = true;
    }

    private void Update()
    {
        UpdateInfoDisplay();

        if (_input.Movement.SwitchAutoPilot.triggered)
            SwitchAutoPilot();

        if (_isAutoPilot)
        {
            AutoLockAtTarget();
            AutoMove();
        }
        else
        {
            HandGlide();
            HandMove();
            HandRotate();
            HandChill();
        }
    }

    private void UpdateInfoDisplay()
    {
        _infoDisplay.SetDistance(GetDistanceToDan());
        _infoDisplay.SetParanoidLevel(_dan.ParanoidLevel);
        _infoDisplay.SetScanLevel(_scanner.ScanProgress);
    }

    private void OnCollisionEnter(Collision other)
    {
        print("BABAH!");
        gameObject.SetActive(false);
    }

    private float GetDistanceToDan() => Vector3.Distance(transform.position, _dan.transform.position);

    private void SwitchAutoPilot()
    {
        _isAutoPilot = !_isAutoPilot;
        _rigidbody.isKinematic = !_rigidbody.isKinematic;
    }

    private void AutoLockAtTarget()
    {
        if (_dan == null)
            return;

        Vector3 direction = _dan.transform.position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            float diff = Mathf.Abs(Quaternion.Angle(_rigidbody.rotation, targetRotation));
            _rigidbody.rotation = Quaternion.RotateTowards(_rigidbody.rotation, targetRotation, Time.deltaTime * _rotateSpeed * diff);
        }
    }

    private void HandChill()
    {
        if (_input.Movement.Chill.IsInProgress() == false)
            return;

        float coeff = 0.95f;
        _rigidbody.angularVelocity *= coeff;
        _rigidbody.linearVelocity *= coeff;
    }

    private void HandRotate()
    {
        Vector3 currentRotation = transform.eulerAngles;

        float mouseMoveX = Input.GetAxis("Mouse X");
        currentRotation.y += (mouseMoveX + mouseMoveX) * _rotateSpeed * _rotateSpeed * Time.deltaTime;

        float mouseMoveY = Input.GetAxis("Mouse Y");
        currentRotation.x -= (mouseMoveY + mouseMoveY) * _rotateSpeed * _rotateSpeed * Time.deltaTime;

        /*
        if (Mathf.Abs(currentRotation.x) >= _maxRotationX)
            currentRotation.x = currentRotation.x > 0 ? _maxRotationX : -_maxRotationX;
        */
       
        transform.eulerAngles = currentRotation;
    }

    private void HandGlide()
    {
        if (_input.Movement.GlideUp.inProgress)
        {
            _rigidbody.AddForce(Vector2.up * _glideForce, _forceMode);
        }
        else if (_input.Movement.GlideDown.inProgress)
        {
            _rigidbody.AddForce(Vector2.down * _glideForce, _forceMode);
        }
        else
        {
            Vector3 currentVelocity = _rigidbody.angularVelocity;
            currentVelocity.y = 0;
            _rigidbody.angularVelocity = currentVelocity;
        }
    }

    private void HandMove()
    {
        Vector2 inputVector = _input.Movement.Move.ReadValue<Vector2>();
        Vector3 moveVector = new Vector3(inputVector.x, 0, inputVector.y);

        _rigidbody.AddRelativeForce(moveVector * _moveForce, _forceMode);
    }

    private void AutoMove()
    {
        float offset = 10;
        Vector3 destination = _dan.transform.position - _dan.transform.forward * offset;
        destination.y = transform.position.y;

        transform.position = Vector3.MoveTowards(transform.position, destination, _moveForce * Time.deltaTime);
    }
}
