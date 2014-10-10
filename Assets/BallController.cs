using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BallController : MonoBehaviour {
    public float m_speed;
    public GameObject m_leftWall;
    public GameObject m_rightWall;
    public GameObject m_topWall;
    public GameObject m_paddle;
    public GameObject m_bricksParent;
    public GameObject m_scoreBoard;
    public GameObject m_livesBoard;
    public GameObject m_gameOverText;

    private Vector2 m_move;
    private int m_score = 0;
    private int m_lives = 3;
    private bool m_isPlaying = false;

    private float rad2deg(float rad) {
        return (rad * 180.0f) / Mathf.PI;
    }

    private float deg2rad(float rad) {
        return (rad * Mathf.PI) / 180.0f;
    }

    private static bool doesCollide(Bounds thisBounds, GameObject go) {
        Bounds thatBounds = go.GetComponent<SpriteRenderer> ().bounds;
        return thisBounds.Intersects(thatBounds);
    }

    private void setDeltas(float angle) {
        float dx = m_speed * Mathf.Cos(deg2rad (angle));
        float dy = m_speed * Mathf.Sin(deg2rad (angle));
        m_move = new Vector2(dx, dy);
    }

    private void UpdateScoreBoard() {
        Text scoreText = m_scoreBoard.GetComponent<Text>();
        scoreText.text = System.String.Format("Score:{0}", m_score);        
    }

    private void UpdateLivesBoard() {
        Text livesBoard = m_livesBoard.GetComponent<Text>();
        livesBoard.text = System.String.Format("Lives:{0}", m_lives);        
    }
    
    void Start () {        
        m_move = new Vector2(0.0f,0.0f);
        UpdateScoreBoard();
        UpdateLivesBoard();
    }
    
    void Update () {
        Bounds thisBounds = this.GetComponent<SpriteRenderer> ().bounds;
        if (this.doesCollide (thisBounds, m_paddle)) {
            if (m_move.y < 0) {
                float ratio = 
                    (this.transform.position.x - m_paddle.transform.position.x) / 
                    (0.5f * m_paddle.GetComponent<SpriteRenderer>().bounds.size.x);
                ratio = Mathf.Clamp(ratio, -1.0f, 1.0f);
                float angle = (-1.0f * 80.0f * ratio) + 90.0f;
                this.setDeltas(angle);              
            }
        } else if (this.doesCollide (thisBounds, m_rightWall)) {
            if (m_move.x > 0) {
                m_move.x *= -1.0f;
            }
        } else if (this.doesCollide (thisBounds, m_leftWall)) {
            if (m_move.x < 0) {
                m_move.x *= -1.0f;
            }
        } else if (this.doesCollide (thisBounds, m_topWall)) {
            if (m_move.y > 0) {
                m_move.y *= -1.0f;
            }
        } else {
            bool found = false;
            IEnumerator en = m_bricksParent.transform.GetEnumerator();
            while(!found && en.MoveNext())
            {
                Transform brickTransform = en.Current as Transform;
                if (this.doesCollide (thisBounds, brickTransform.gameObject)) {
                    m_move.y *= -1.0f;
                    Destroy(brickTransform.gameObject);
                    m_score += 100;
                    UpdateScoreBoard();                    
                    found = true;
                }                
            }
        }

        if (m_isPlaying && this.transform.position.y < -5.0f)
        {
            m_move = new Vector2(0.0f, 0.0f);
            m_lives -= 1;
            UpdateLivesBoard();
            m_isPlaying = false;

            if (m_lives <= 0)
            {
                m_gameOverText.GetComponent<CanvasGroup>().alpha = 1.0f;
            }
        }
        

        if (Input.GetButton("Fire1")) {
            if (!m_isPlaying)
            {
                if (m_lives <=0)
                {
                    Application.LoadLevel ("Level1");
                }
                else
                {
                    this.setDeltas (315.0f);                
                    this.transform.position = new Vector3(-15.0f, 25.0f, 0.0f);                          
                    m_isPlaying = true;                    
                }
            } 
        }
    }

    void FixedUpdate() {
        this.transform.Translate (Time.fixedDeltaTime * m_move.x, 
                                  Time.fixedDeltaTime * m_move.y, 
                                  0);
    }
}
