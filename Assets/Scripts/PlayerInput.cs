using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float _cameraMoveSpeed;
    [SerializeField] private float _cameraZoomSpeed;
    [SerializeField] private float _cameraZoomMin;
    [SerializeField] private float _cameraZoomMax;
    [SerializeField] private Vector3 _focusDifference;
    [SerializeField] private GameObject _selectionBoxPrefab;

    private Controls _controls;
    private Camera _camera;
    private GameObject _selectionBox;
    private SpriteRenderer _selectionBoxRenderer;
    private bool _clickLeft = false;
    private bool _dragLeft = false;
    private bool _clickRight = false;
    private bool _dragRight = false;
    private bool _clickMiddle = false;
    private bool _dragMiddle = false;
    private bool _ctrlHold = false;
    private bool _shiftHold = false;
    private bool _altHold = false;
    private Vector2 _cursorPosition;
    private Vector2 _clickLeftOrigin;
    private Vector2 _clickRightOrigin;
    private Vector2 _clickMiddleOrigin;
    private Vector2 _cameraMovement;
    private float _cameraZoom;

    private void Awake()
    {
        _controls = new Controls();
        _camera = GetComponent<Camera>();
        _selectionBox = Instantiate(_selectionBoxPrefab);
        _selectionBoxRenderer = _selectionBox.GetComponent<SpriteRenderer>();
        _selectionBoxRenderer.enabled = false;
    }

    private void Update()
    {
        _cursorPosition = _camera.ScreenToWorldPoint(_controls.player.cursor.ReadValue<Vector2>());
        _cameraMovement = _controls.player.move.ReadValue<Vector2>();
        _cameraZoom = _controls.player.zoom.ReadValue<float>();
    }
    private void FixedUpdate()
    {
        Vector3 cameraPositionDifference = (Vector3)(Time.deltaTime * _cameraMoveSpeed * _camera.orthographicSize * _cameraMovement);

        Selectable focus = SelectionManager.Instance.Focused;
        if (focus != null)
        {
            _focusDifference += cameraPositionDifference;
            Vector3 focusRelative = focus.transform.position + _focusDifference;

            this.transform.position = new Vector3(focusRelative.x, focusRelative.y, this.transform.position.z);
        }
        else
        {
            _focusDifference = Vector3.zero;
            this.transform.position = this.transform.position + cameraPositionDifference;
        }

        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize + 
            _camera.orthographicSize * Time.deltaTime * _cameraZoomSpeed * _cameraZoom, 
            _cameraZoomMin, _cameraZoomMax);

        if (!_dragLeft && _clickLeft && _clickLeftOrigin != _cursorPosition)
            _dragLeft = true;
        if (_dragLeft)
        {
            Vector2 cursorDifference = _cursorPosition - _clickLeftOrigin;
            _selectionBox.transform.localScale = (Vector3)cursorDifference + Vector3.forward;
            _selectionBox.transform.position = (Vector3)(_clickLeftOrigin + 0.5f * cursorDifference)
                + _selectionBox.transform.position.z * Vector3.forward;
        }
        else
        {
            _selectionBox.transform.position = _cursorPosition;;
        }

        if (!_dragRight && _clickRight && _clickRightOrigin != _cursorPosition) 
            _dragRight = true;
    }

    private void MouseLeftDown(InputAction.CallbackContext context)
    {
        _clickLeft = true;
        _clickLeftOrigin = _cursorPosition;
        _selectionBoxRenderer.enabled = true;
        _selectionBox.transform.position = _clickLeftOrigin;
        _selectionBox.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
    }
    private void MouseLeftUp(InputAction.CallbackContext context)
    {
        _clickLeft = false;
        if (_dragLeft)
        {
            _dragLeft = false;
            if (_ctrlHold)
            {
                SelectionManager.Instance.SelectHovered();
            }
            else
            {
                SelectionManager.Instance.SelectMoreHovered();
            }
        }
        else
        {
            if (_ctrlHold)
            {
                SelectionManager.Instance.SelectAnother();
            }
            else
            {
                SelectionManager.Instance.SelectOne();
            }
        }
        _selectionBoxRenderer.enabled = false;
        _selectionBox.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
    }

    private void MouseRightDown(InputAction.CallbackContext context)
    {
        _clickRight = true;
        _clickRightOrigin = _cursorPosition;
    }
    private void MouseRightUp(InputAction.CallbackContext context)
    {
        _clickRight = false;
        HashSet<Selectable> selected = SelectionManager.Instance.Selected;

        Task task = new Task();
        task.target = null;

        if (_dragRight)
        {
            // Formation.Instance.SetDestination(_clickRightOrigin, (_cursorPosition - _clickRightOrigin).normalized, selected);
            task.destination = _clickRightOrigin;
            task.direction = (_cursorPosition - _clickRightOrigin).normalized;
            _dragRight = false;
        }
        else
        {
            // Formation.Instance.SetDestination(_cursorPosition, new Vector2(), selected);
            task.destination = _cursorPosition;
            task.direction = Vector2.zero;
        }
        task.halt = true;
        task.repeat = _altHold;
        SelectionManager.Instance.Assign(task, _shiftHold);
    }

    private void CtrlDown(InputAction.CallbackContext context)
    {
        _ctrlHold = true;
    }
    private void CtrlUp(InputAction.CallbackContext context)
    {
        _ctrlHold = false;
    }

    private void ShiftDown(InputAction.CallbackContext context)
    {
        _shiftHold = true;
    }
    private void ShiftUp(InputAction.CallbackContext context)
    {
        _shiftHold = false;
    }

    private void AltDown(InputAction.CallbackContext context)
    {
        _altHold = true;
    }
    private void AltUp(InputAction.CallbackContext context)
    {
        _altHold = false;
    }

    private void OnEnable()
    {
        _controls.Enable();
        _controls.player.clickLeft.started += MouseLeftDown;
        _controls.player.clickLeft.canceled += MouseLeftUp;
        _controls.player.clickRight.started += MouseRightDown;
        _controls.player.clickRight.canceled += MouseRightUp;
        _controls.player.ctrl.started += CtrlDown;
        _controls.player.ctrl.canceled += CtrlUp;
        _controls.player.shift.started += ShiftDown;
        _controls.player.shift.canceled += ShiftUp;
        _controls.player.alt.started += AltDown;
        _controls.player.alt.canceled += AltUp;
    }
    private void OnDisable()
    {
        _controls.Disable();
        _controls.player.clickLeft.started -= MouseLeftDown;
        _controls.player.clickLeft.canceled -= MouseLeftUp;
        _controls.player.clickRight.started -= MouseRightDown;
        _controls.player.clickRight.canceled -= MouseRightUp;
        _controls.player.ctrl.started -= CtrlDown;
        _controls.player.ctrl.canceled -= CtrlUp;
        _controls.player.shift.started -= ShiftDown;
        _controls.player.shift.canceled -= ShiftUp;
        _controls.player.alt.started -= AltDown;
        _controls.player.alt.canceled -= AltUp;
    }
}
