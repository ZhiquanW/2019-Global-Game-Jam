using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{

    public float influence_num = 0;
    public float repulsive_radius = 0;
    public List<Vector3> velocity_list;
    public Vector2 ave_velocity = new Vector2(0, 0);
    public float repulsive_velocity = 0;
    public float timer = 0;
    public float frame_inteval = 0;
    public Vector2 target_v = new Vector2(0, 0);
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        timer += Time.deltaTime;
        if (timer > frame_inteval) {
            timer = 0;
            ave_velocity = new Vector2(0, 0);
            Vector2 repulsive_dir = new Vector2(0, 0);
            foreach (Transform tmp in FlockManager.instance.bird_list) {

                float tmp_dis = Vector3.Distance(tmp.position, this.transform.position);

                if (tmp_dis == 0) {
                    continue;

                }
                //print(tmp_dis);
                if (tmp_dis < repulsive_radius) {
                    Vector2 tmp_dir = (this.transform.position - tmp.transform.position).normalized;
                    repulsive_dir += tmp_dir * repulsive_velocity * tmp_dis * tmp_dis;
                    //print(repulsive_dir);
                }
                velocity_list.Add(tmp.GetComponent<Rigidbody2D>().velocity);
            }
            List<Transform> tmp_list = new List<Transform>(FlockManager.instance.bird_list);
            Vector3 ave_pos = new Vector3(0, 0, 0);
            for (int i = 0; i < influence_num; ++i) {
                float tmp_min = Mathf.Infinity;
                int tmp_index = 0;
                if (tmp_list.Count == 0) {
                    continue;
                }
                for (int j = 0; j < tmp_list.Count; ++j) {
                    float tmp_dis_0 = Vector3.Distance(tmp_list[j].transform.position, this.transform.position);
                    if (tmp_dis_0 < tmp_min) {
                        tmp_min = tmp_dis_0;
                        tmp_index = j;
                    }
                }

                ave_velocity += tmp_list[tmp_index].GetComponent<Rigidbody2D>().velocity;
                ave_pos += tmp_list[tmp_index].transform.position;
                tmp_list.RemoveAt(tmp_index);
            }
            ave_velocity /= influence_num;


            ave_pos /= influence_num;
            Vector2 ave_pos2 = ave_pos - this.transform.position;
           
            Vector2 in_vel = new Vector2(0, 0);
            float tmp_center_dis = (this.transform.position - FlockManager.instance.transform.position).magnitude;
            if (tmp_center_dis> FlockManager.instance.outter_radius) {
                in_vel = FlockManager.instance.transform.position - this.transform.position;
            }
            target_v = (ave_pos2.normalized + ave_velocity.normalized) * ave_velocity.magnitude + repulsive_dir + in_vel* tmp_center_dis * repulsive_velocity/10;
            if(target_v.magnitude > FlockManager.instance.max_velocity) {
                target_v *= 0.8f;
            }
            
        }
        this.GetComponent<Rigidbody2D>().velocity = Vector3.Lerp(this.GetComponent<Rigidbody2D>().velocity,target_v,0.07f);
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0,
                180 * Mathf.Atan2(this.GetComponent<Rigidbody2D>().velocity.y, this.GetComponent<Rigidbody2D>().velocity.x) / Mathf.PI - 90f));
        //print(Mathf.Atan2(this.GetComponent<Rigidbody2D>().velocity.y, this.GetComponent<Rigidbody2D>().velocity.x));
    }
    
}