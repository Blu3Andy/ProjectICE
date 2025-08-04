using UnityEngine;

public class IsometricAiming : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;

    private Camera mainCamera;

    public bool isInverted = false;

    private void Start()
    {
        // Cache the camera, Camera.main is an expensive operation.
        mainCamera = Camera.main;
    }

    private void Update()
    {
            Aim();
        
        //DIGGA SETZT AUF DEN BODEN AUF DEN GROUND LAYER SONST GEHT YIER GAR NICHTS
        //DU HATTEST JA RECHT MAN 18.02.2024
        //jo btw wenn du die ground layer erstellst, dann bitte ordne den Boden auch zu dieserLayer hinzu, danke
    }

    private void Aim()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            // Calculate the direction
            var direction = position - transform.position;

            // You might want to delete this line.
            // Ignore the height difference.
            direction.y = 0;

            // Make the transform look in the direction.

            if(isInverted)
            {
                transform.forward = -direction;
            }
            else
            {
                transform.forward = direction;
            }
            
        }
    }

    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            // The Raycast hit something, return with the position.
            return (success: true, position: hitInfo.point);
        }
        else
        {
            // The Raycast did not hit anything.
            return (success: false, position: Vector3.zero);
        }
    }
}