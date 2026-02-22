using UnityEngine;

public class InputManager : MonoBehaviour {

    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask interactableLayer;

    private void Update() {

#if UNITY_EDITOR
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
    }

    private void HandleTouchInput() {

        if(Input.touchCount == 0) {
            return;
        }

        Touch touch = Input.GetTouch(0);

        if(touch.phase == TouchPhase.Began) {
            ProcessTap(touch.position);
        }
    }

    private void HandleMouseInput() {

        if(Input.GetMouseButtonDown(0)) {
            ProcessTap(Input.mousePosition);
        }
    }

    private void ProcessTap(Vector2 screenPosition) {

        Ray ray = mainCamera.ScreenPointToRay(screenPosition);

        if(Physics.Raycast(ray, out RaycastHit hit, 100f, interactableLayer)) {

            ITappable tappable = hit.collider.GetComponentInParent<ITappable>();

            if(tappable == null) {
                tappable = hit.collider.GetComponent<ITappable>();
            }

            if(tappable != null) {
                tappable.OnTap();
            }
        }
    }
}