using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour {

    public static FlockManager instance;
    public int birds_num = 0;
    public int inner_radius = 0;
    public int outter_radius = 0;
    public float min_velocity = 0;
    public float max_velocity = 0;
    public List<Transform> bird_list;
    public Transform bird_prefab;
    private Transform flock_transform;
    public CircleCollider2D inner_collider;
    public CircleCollider2D outter_collider;
    private void Awake() {
        instance = this;
        flock_transform = this.GetComponent<Transform>();
        inner_collider.radius = inner_radius;
        outter_collider.radius = outter_radius;
    }
    // Start is called before the first frame update

    void Start() {
        float tmp_len = 0;
        float tmp_radian = 0;
        for (int i = 0; i < birds_num; ++i) {
            tmp_len = Random.Range(inner_radius, outter_radius);
            tmp_radian = Random.Range(0,2f * Mathf.PI);
            Vector3 tmp_offset = new Vector3(Mathf.Cos(tmp_radian)*tmp_len,Mathf.Sin(tmp_radian)*tmp_len, 0);

            float tmp_rotation_radian= Random.Range(0f, 2f * Mathf.PI);
            Transform tmp_bird = Instantiate(bird_prefab, flock_transform.position + tmp_offset, Quaternion.Euler(new Vector3(0,0,180/Mathf.PI* tmp_rotation_radian - 90)));
            float tmp_velocity = Random.Range(min_velocity, max_velocity);
            Vector3 tmp_rotation = new Vector2(Mathf.Cos(tmp_rotation_radian),Mathf.Sin(tmp_rotation_radian));
            tmp_bird.GetComponent<Rigidbody2D>().velocity = tmp_rotation * tmp_velocity;
            tmp_bird.SetParent(this.transform);
            bird_list.Add(tmp_bird);
        }
    }

    // Update is called once per frame
    void Update() {


    }

}