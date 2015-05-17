using UnityEngine;
using System.Collections;

public class Tay : MonoBehaviour
{

    public float moveSpeed = 0.01f;
    
    float trueX;
    float trueY;

    // Use this for initialization
    void Start()
    {
        trueX = this.transform.position.x;
        trueY = this.transform.position.y;
    }
    
    // Update is called once per frame
    void Update()
    {
        // X-axis movement
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            
            Vector3 position = this.transform.position;
            trueX -= moveSpeed * Time.smoothDeltaTime;
            position.x = Mathf.Round(trueX * 100f) / 100f;
            this.transform.position = position;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 position = this.transform.position;
            trueX += moveSpeed * Time.smoothDeltaTime;
            position.x = Mathf.Round(trueX * 100f) / 100f;
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 position = this.transform.position;
            trueY += moveSpeed * Time.smoothDeltaTime;
            position.y = Mathf.Round(trueY * 100f) / 100f;
            this.transform.position = position;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 position = this.transform.position;
            trueY -= moveSpeed * Time.smoothDeltaTime;
            position.y = Mathf.Round(trueY * 100f) / 100f;
            this.transform.position = position;
        }
        
        // Snap to pixel when movement stops
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) {
            Vector3 position = this.transform.position;
            trueX = Mathf.Round(trueX * 100f) / 100f;
            position.x = trueX;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)) {
            Vector3 position = this.transform.position;
            trueY = Mathf.Round(trueY * 100f) / 100f;
            position.y = trueY;
        }
    }
}
