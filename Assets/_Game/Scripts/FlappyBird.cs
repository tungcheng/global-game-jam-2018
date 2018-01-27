using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyBird : MonoBehaviour {

    Rigidbody2D m_RigidBody;
    bool m_IsDead = false;

    SpriteRenderer render;

	// Use this for initialization
	void Start () {
        m_RigidBody = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (m_IsDead) return;

		if(Input.GetMouseButtonDown(0) && m_RigidBody.velocity.y <= 0f)
        {
            var data = GameManager.Instance.GetDesignData();
            m_RigidBody.AddForce(new Vector2(0f, data.flyUpForce));
            GameManager.Instance.PlaySfxWing();
        }

        var velocity = m_RigidBody.velocity;
        velocity.x = (m_IsDead) ? 0 : GameManager.Instance.GetDesignData().horizontalSpeed;
        m_RigidBody.velocity = velocity;

        if(m_RigidBody.velocity.y <= 0f)
        {
            var data = GameManager.Instance.GetDesignData();
            render.sprite = data.flyDownImage;
        }
        else
        {
            var data = GameManager.Instance.GetDesignData();
            render.sprite = data.flyUpImage;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (m_IsDead) return;

        m_IsDead = true;
        var data = GameManager.Instance.GetDesignData();
        render.sprite = data.deadImage;
        render.color = data.deadColor;
        GameManager.Instance.OnBirdDead();
        GameManager.Instance.PlaySfxHit();
    }

    const string ScoreTag = "Score";
    void OnTriggerEnter2D(Collider2D other)
    {
        if (m_IsDead) return;

        if(other.gameObject.CompareTag(ScoreTag))
        {
            GameManager.Instance.OnHitNewScore();
        }
    }
}
