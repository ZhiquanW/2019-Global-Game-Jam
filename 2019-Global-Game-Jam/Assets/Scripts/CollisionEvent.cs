using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEvent : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D _colliderObject) {
        if (_colliderObject.gameObject.tag == Tags.Ground) {
            Destroy(gameObject);
        }
    }
}
