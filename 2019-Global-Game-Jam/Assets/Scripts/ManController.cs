using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManController : MonoBehaviour
{
    public GameObject bullet;
    private Rigidbody2D BulletRigidbody;
    private Rigidbody2D PlayerRigidbody;
    private Transform PlayerTransform;
    public float moveAcceleration = 0f;//角色水平移动加速度
    public float jumpVelocity = 0f;//角色垂直跳跃速度
    public float stopAcceleration = 0f;//物体停止加速度
    public float horizontalCriticalSpeed = 0f;//物体水平最大移动速度限制  
    public float moveBeginCriticalValue = 0f;//移动临界值（输入）
    public bool isGround = true;//判断物体是否处与地面
    Vector3 realtimeVelocity;
    float horizontalInput;//获取水平输入
    float verticalInput;//获取垂直输入
    public float bulletInterval = 0f;
    public bool isAttacked = false;
    public float bulletSpeed = 0f;
    private float timer = 0f;
    public float shakeInterval = 0f;
    public float shakeVelocity = 0f;
    private float shakeTimer = 0f;
    private bool isShaked = false;
    private bool shakeUp = true;
    // Start is called before the first frame update
    void Start() {
        timer = 0;
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        PlayerTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update() {
        realtimeVelocity = PlayerRigidbody.velocity;
        shakeTimer += Time.deltaTime;
        if (isGround) {
            if (shakeTimer > shakeInterval * 2 / 5) {
                realtimeVelocity.y = -shakeVelocity;
                shakeUp = false;
            }
            if (shakeTimer > shakeInterval) {
                realtimeVelocity.y = shakeVelocity;
                shakeUp = true;
                shakeTimer = 0f;
            }
        }
        if (isAttacked)
            timer += Time.deltaTime;
        if (timer > bulletInterval) {
            isAttacked = false;
        }
        //tempLocalScale = PlayerTransform.localScale;
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(horizontalInput) > moveBeginCriticalValue) {
            //realtimeVelocity.x = Mathf.Lerp(realtimeVelocity.x, horizontalCriticalSpeed * horizontalInput, Time.deltaTime * moveAcceleration);
            realtimeVelocity.x = horizontalCriticalSpeed * horizontalInput;
            //tempLocalScale.x = horizontalInput * Mathf.Abs(PlayerTransform.localScale.x);
        } else {
            realtimeVelocity.x = 0;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            //realtimeVelocity.x = Mathf.Lerp(realtimeVelocity.x, horizontalCriticalSpeed * horizontalInput, Time.deltaTime * moveAcceleration);
            realtimeVelocity.x = -horizontalCriticalSpeed;
            //tempLocalScale.x = horizontalInput * Mathf.Abs(PlayerTransform.localScale.x);
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            realtimeVelocity.x = horizontalCriticalSpeed;
        } else {
            realtimeVelocity.x = 0;
        }
        //修改目标垂直速度
        if (Input.GetKey(KeyCode.UpArrow)) {
            if (isGround) {
                realtimeVelocity.y = jumpVelocity;
                isGround = false;
            }
        }
        //修改物体目标方向与速度
        //PlayerTransform.localScale = tempLocalScale;
        PlayerRigidbody.velocity = realtimeVelocity;
        Attack();
    }

    void OnCollisionEnter2D(Collision2D _collisionObject) {
        if (_collisionObject.gameObject.tag == Tags.Ground) {
            isGround = true;
        }
    }

    /*private void OnCollisionExit2D(Collision2D _collisionObject) {
        if (_collisionObject.gameObject.tag == Tags.Ground) {
            isGround = false;
        }
    }*/

    void Attack() {
        if (!isAttacked && Input.GetMouseButtonDown(0)) {
            isAttacked = true;
            timer = 0f;
            Vector3 manWorldPos = Camera.main.WorldToScreenPoint(transform.position);
            float radian = Mathf.Atan2(Input.mousePosition.y - manWorldPos.y, Input.mousePosition.x - manWorldPos.x);
            float angle = 180 / Mathf.PI * radian;
            Vector3 tempRotation = new Vector3(0, 0, angle);
            GameObject obj = Instantiate(bullet, transform.position, Quaternion.Euler(tempRotation));
            obj.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(radian) * bulletSpeed, Mathf.Sin(radian) * bulletSpeed);
        }
    }
}
