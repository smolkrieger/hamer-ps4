using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorGameScript : MonoBehaviour
{
    public RectTransform cursorTransform;

    float horizontalInput;
    float verticalInput;
    public float cursorSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("LeftStickY");
        verticalInput = Input.GetAxisRaw("LeftStickX");
        Vector3 moveInput = new Vector3((horizontalInput * cursorSpeed), (verticalInput * cursorSpeed), 0f);
        Vector3 newPosition = cursorTransform.position + moveInput / 1;

        cursorTransform.position = newPosition;
        //Debug.Log("leftstickx = " + Input.GetAxis("LeftStickY") + " leftsticky = " + Input.GetAxis("LeftStickX"));
    }
}
