﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEvent : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == Tags.Ground) {
            Destroy(gameObject);
        } else if (collision.gameObject.tag == Tags.Bird) {
            collision.GetComponent<BirdController>().Dead();
            Destroy(gameObject);
        }
        if (_colliderObject.gameObject.tag == Tags.Wall) {
            Destroy(gameObject);
        }
        if (_colliderObject.gameObject.tag == Tags.Bird) {
            _colliderObject.GetComponent<BirdController>().Dead();
            Destroy(gameObject);
        }
    }
}
