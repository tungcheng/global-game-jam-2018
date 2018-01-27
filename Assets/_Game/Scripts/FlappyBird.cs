﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyBird : MonoBehaviour {

    Rigidbody2D m_RigidBody;
    bool m_IsDead = false;

	// Use this for initialization
	void Start () {
        m_RigidBody = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        if (m_IsDead) return;

		if(Input.GetMouseButtonDown(0) && m_RigidBody.velocity.y <= 0f)
        {
            var data = GameManager.Instance.GetDesignData();
            m_RigidBody.AddForce(new Vector2(0f, data.flyUpForce));
        }

        var velocity = m_RigidBody.velocity;
        velocity.x = (m_IsDead) ? 0 : GameManager.Instance.GetDesignData().horizontalSpeed;
        m_RigidBody.velocity = velocity;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (m_IsDead) return;

        m_IsDead = true;
        GameManager.Instance.OnBirdDead();
    }
}