using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool doMovement = true;

    public float panSpeed = 30.0f;
    public float panBorderThickness = 10f;

    public float scrollSpeed = 5f;
    public float maxY = 150f;
    public float minY = 5f;


    // Update is called once per frame
    void Update()
    {
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
            transform.Translate(Vector3.up * panSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey("s")) // || Input.mousePosition.y <= panBorderThickness
        {
            transform.Translate(Vector3.down * panSpeed * Time.deltaTime, Space.Self);
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
        return;
    }
}
