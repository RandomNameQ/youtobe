using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _startCameraHeight = 15f;
    [SerializeField] private bool _cameraLockOn;
    [SerializeField] private bool _freeCameraControl;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float _heightChangeSpeed = 5f;
    [SerializeField] private float _speedChangePosWithLeftClick = 20f;
    [SerializeField] private float _arrowSpeed = 5f;

    [SerializeField] private float _doubleClickTimeThreshold = 0.2f;
    [SerializeField] private float _edgeMoveSpeed = 10f;
    [SerializeField] private float _edgeThreshold = 20f;
    [SerializeField] private bool _edgeScreenMove;

    private float _time;
    private bool _isDoubleClick;
    private bool _isDoubleClickFix;


    
    private GameObject _player;
    private GameObject _target;

    private void Update()
    {



        if (Input.GetKeyDown(KeyCode.F1))
        {
            _freeCameraControl = false;
            _cameraLockOn = true;
            _target = _player;
        }

        ArrowsControl();
        PressWheelRotation();
        MouseWheelHeight();
        DoubleClickChangeTarget();
        CameraFollowTarget();
        LeftClicMove();
        ScreenEdgeMoveCam();
    }


    public void UpdateCameraOwner(GameObject newPlayer)
    {
        _cameraLockOn = true;
        _freeCameraControl = false;
        _player = newPlayer;
        _target = _player;
        transform.position = new Vector3(
            _player.transform.position.x + 2f,
            _startCameraHeight,
            _player.transform.position.z - 5f
        );
    }

    private void CameraFollowTarget()
    {
        if (_cameraLockOn && !_freeCameraControl)
        {
            float x = _target.transform.position.x + 2f;
            float y = transform.position.y;
            float z = _target.transform.position.z - 5f;
            transform.position = new Vector3(x, y, z);
        }
    }

    private void BreakFreeControl()
    {
        _freeCameraControl = true;
    }

    private void ArrowsControl()
    {

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (moveX != 0f || moveZ != 0f)
        {

            Vector2 direction = new Vector2(moveX, moveZ);
            ChangeCameraPos(direction, true);
        }
    }



    

    private void ChangeCameraPos(Vector2 direction, bool isArrow = false)
    {


        if (isArrow)
        {
            BreakFreeControl();
        }

        direction *= Time.deltaTime * _arrowSpeed;

        Vector3 movement = transform.forward * direction.y + transform.right * direction.x;
        movement *= _speedChangePosWithLeftClick;

        Vector3 newPosition = transform.position + movement;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }

    private void PressWheelRotation()
    {
        if (Input.GetMouseButton(2))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            Vector3 rotation = new Vector3(-mouseY, mouseX, 0f) * rotationSpeed;
            Quaternion newRotation = transform.rotation * Quaternion.Euler(rotation);
            newRotation.eulerAngles = new Vector3(newRotation.eulerAngles.x, newRotation.eulerAngles.y, 0f);
            transform.rotation = newRotation;
        }
    }

    private void MouseWheelHeight()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            BreakFreeControl();
            Vector3 position = transform.position;
            position.y -= scroll * _heightChangeSpeed;
            transform.position = position;
        }
    }

    private void LeftClicMove()
    {
        if (Input.GetMouseButton(0) && !_isDoubleClickFix)
        {
            BreakFreeControl();

            float mouseX = -Input.GetAxis("Mouse X");
            float mouseY = -Input.GetAxis("Mouse Y");

            Vector3 forward = transform.forward * Time.deltaTime;
            Vector3 right = transform.right * Time.deltaTime;
            forward.y = 0f;

            Vector3 movement = (forward * mouseY + right * mouseX) * _speedChangePosWithLeftClick;
            Vector3 newPosition = transform.position + movement;
            newPosition.y = transform.position.y;
            transform.position = newPosition;
        }
    }

    private void DoubleClickChangeTarget()
    {
        _time += Time.deltaTime;

        // for left click movement fix
        if (_isDoubleClickFix && _time >= _doubleClickTimeThreshold)
        {
            _isDoubleClickFix = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _isDoubleClick = _time <= _doubleClickTimeThreshold;
            _time = 0f;
        }

        if (_isDoubleClick)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject newTarget = hit.transform.gameObject;
                if (newTarget != null)
                {
                    _target = newTarget;
                    _freeCameraControl = false;
                    /* Debug.Log(_target.name + " TEST"); */
                    _isDoubleClick = false;
                    _isDoubleClickFix = true;
                }
            }
        }
    }

    private void ScreenEdgeMoveCam()
    {
        Debug.Log("X: "+Input.mousePosition.x+"      Y: "+Input.mousePosition.y);
        Debug.Log("WIDTH: "+Screen.width + "    HEIGHT: "+Screen.height);

        if (!_freeCameraControl)
        {
            return;
        }

        if (!_edgeScreenMove)
        {
            return;
        }
        Vector3 mousePosition = Input.mousePosition;
        float leftEdge = _edgeThreshold;
        float rightEdge = Screen.width - _edgeThreshold;
        float topEdge = Screen.height - _edgeThreshold;
        float bottomEdge = _edgeThreshold;

        bool nearLeftEdge = mousePosition.x <= leftEdge;
        bool nearRightEdge = mousePosition.x >= rightEdge;
        bool nearTopEdge = mousePosition.y >= topEdge;
        bool nearBottomEdge = mousePosition.y <= bottomEdge;

        float moveX = 0f;
        float moveZ = 0f;

        if (nearLeftEdge)
        {
            moveX = -1f;
        }
        else if (nearRightEdge)
        {
            moveX = 1f;
        }

        if (nearTopEdge)
        {
            moveZ = 1f;
        }
        else if (nearBottomEdge)
        {
            moveZ = -1f;
        }

        Vector2 movement = new Vector2(moveX, moveZ) * _edgeMoveSpeed * Time.deltaTime;
        ChangeCameraPos(movement);
    }
}
