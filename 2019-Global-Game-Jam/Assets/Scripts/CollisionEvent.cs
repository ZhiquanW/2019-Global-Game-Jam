﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEvent : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == Tags.Ground) {
            Destroy(gameObject);
        } else if (collision.gameObject.tag == Tags.Bird) {

        }
    }
}
