using UnityEngine;
using System.Collections;

public class PaddleController : MonoBehaviour {

    public float m_speed;
    private float m_maxX = 45.0f;

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void FixedUpdate () {
        this.transform.Translate (Input.GetAxis ("Horizontal") * m_speed, 0.0f, 0.0f);
        if (Mathf.Abs( this.transform.position.x ) > m_maxX)
        {
            this.transform.position = 
                new Vector3(Mathf.Clamp(this.transform.position.x, -m_maxX, m_maxX),
                            this.transform.position.y,
                            this.transform.position.z);
        }
    }
}
