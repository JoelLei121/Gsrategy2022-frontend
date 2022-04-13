using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool doMovement = true;

    public float panSpeed = 30.0f;
    public float panBorderThickness = 10f;

    public float scrollSpeed = 5f;
    public float maxY = 35f;
    public float minY = 5f;

    private float lastMouseX, lastMouseY, currentMouseX, currentMouseY;

    void Start()
    {
        lastMouseX = lastMouseY = currentMouseX = currentMouseY = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        lastMouseX = currentMouseX;
        lastMouseY = currentMouseY;
        currentMouseX = Input.mousePosition.x;
        currentMouseY = Input.mousePosition.y;

        // rotation needed
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Vector3 rotation = Vector3.zero;
        //     if(lastMouseX - currentMouseX > 0)
        //     {
        //         transform.Rotate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        //     }
        //     else if (lastMouseX - currentMouseX < 0)
        //     {

        //     }

        //     if(lastMouseY - currentMouseY > 0)
        //     {
        //         transform.Rotate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        //     }
        //     else if (lastMouseY - currentMouseY < 0)
        //     {
                
        //     }

        // }

        if (Input.GetKeyDown(KeyCode.L))
        {
            doMovement = !doMovement;
        }

        if (!doMovement)
        {
            return;
        }

        if (Input.GetKey("w")) // || Input.mousePosition.y >= Screen.height - panBorderThickness
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey("s")) // || Input.mousePosition.y <= panBorderThickness
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey("a") ) //|| Input.mousePosition.x <= panBorderThickness
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey("d") ) //|| Input.mousePosition.x >= Screen.width - panBorderThickness
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.Self);
        }

        // update needed
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 pos = transform.position;
        pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;


    }
}
