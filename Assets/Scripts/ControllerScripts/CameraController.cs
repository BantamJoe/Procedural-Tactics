using UnityEngine;

public class CameraController : MonoBehaviour {

	private bool doMovement = true;

	public float panSpeed = 30f;
	public float panBorderThickness = 10f;

	public float scrollSpeed = 5f;
	public float minY = 10f;
	public float maxY = 120f;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Update() 
    {

        if (Input.GetKeyDown(KeyCode.Backspace))
            transform.position = new Vector3(37.8f, 79.2f, -84.8f);

        if (Input.GetKeyDown(KeyCode.Escape))
            doMovement = !doMovement;

        if (!doMovement)
			return;


        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
		{
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.mousePosition.y <= panBorderThickness)
		{
			transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.mousePosition.x >= Screen.width - panBorderThickness)
		{
			transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.mousePosition.x <= panBorderThickness)
		{
			transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
		}

		float scroll = Input.GetAxis("Mouse ScrollWheel");

		Vector3 pos = transform.position;

		pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
		pos.y = Mathf.Clamp(pos.y, minY, maxY);

		transform.position = pos;
	}
}
