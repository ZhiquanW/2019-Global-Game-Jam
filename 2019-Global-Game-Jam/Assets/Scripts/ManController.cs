using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManController : MonoBehaviour
{
    public GameObject pistol;
    public GameObject bullet;
    private Rigidbody2D BulletRigidbody;
    private Rigidbody2D PlayerRigidbody;
    private Transform PlayerTransform;
    public float moveAcceleration = 0f;//��ɫˮƽ�ƶ����ٶ�
    public float jumpVelocity = 0f;//��ɫ��ֱ��Ծ�ٶ�
    public float stopAcceleration = 0f;//����ֹͣ���ٶ�
    public float horizontalCriticalSpeed = 0f;//����ˮƽ����ƶ��ٶ�����  
    public float moveBeginCriticalValue = 0f;//�ƶ��ٽ�ֵ�����룩
    public float armLength = 0f;
    public bool isGround = true;//�ж������Ƿ������
    Vector3 realtimeVelocity;
    float horizontalInput;//��ȡˮƽ����
    float verticalInput;//��ȡ��ֱ����
    public float bulletInterval = 0f;
    public bool isAttacked = false;
    public float bulletSpeed = 0f;
    private float timer = 0f;
    public float shakeInterval = 0f;
    public float shakeVelocity = 0f;
    private float shakeTimer = 0f;
    private bool isShaked = false;
    private bool shakeUp = true;
    private bool isWall = false;
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
        //�޸�Ŀ�괹ֱ�ٶ�
        if (!isWall && Input.GetKey(KeyCode.UpArrow)) {
            if (isGround) {
                realtimeVelocity.y = jumpVelocity;
                isGround = false;
            }
        }
        //�޸�����Ŀ�귽�����ٶ�
        //PlayerTransform.localScale = tempLocalScale;
        PlayerRigidbody.velocity = realtimeVelocity;
        Attack();
    }

    void OnCollisionEnter2D(Collision2D _collisionObject) {
        if (_collisionObject.gameObject.tag == Tags.Ground) {
            isGround = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == Tags.Wall) {
            isWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D _collisionObject) {
        /*if (_collisionObject.gameObject.tag == Tags.Ground) {
            isGround = false;
        }*/
        if (_collisionObject.gameObject.tag == Tags.Wall) {
            isWall = false;
        }
    }

    void Attack() {
        Vector3 manWorldPos = Camera.main.WorldToScreenPoint(transform.position);
        float radian;
        float angle;
        Vector3 tempRotation;
        if (Input.mousePosition.x - manWorldPos.x > 0) {
            if (pistol.transform.localScale.y < 0)
                pistol.transform.localScale = new Vector3(pistol.transform.localScale.x,
                    -pistol.transform.localScale.y, pistol.transform.localScale.z);
            radian = Mathf.Atan2(Input.mousePosition.y - manWorldPos.y, Input.mousePosition.x - manWorldPos.x);
            angle = 180 / Mathf.PI * radian;
            tempRotation = new Vector3(0, 0, angle);
            pistol.transform.rotation = Quaternion.Euler(tempRotation);
            pistol.transform.position = this.transform.position + new Vector3(armLength * Mathf.Cos(radian), armLength * Mathf.Sin(radian) + 0.3f, 0);
        } else {
            if (pistol.transform.localScale.y > 0)
                pistol.transform.localScale = new Vector3(pistol.transform.localScale.x,
                    -pistol.transform.localScale.y, pistol.transform.localScale.z);
            radian = Mathf.Atan2(Input.mousePosition.y - manWorldPos.y, Input.mousePosition.x - manWorldPos.x);
            angle = 180 / Mathf.PI * radian;
            tempRotation = new Vector3(0, 0, angle);
            pistol.transform.rotation = Quaternion.Euler(tempRotation);
            pistol.transform.position = this.transform.position + new Vector3(armLength * Mathf.Cos(radian), armLength * Mathf.Sin(radian) + 0.3f, 0);
        }

        if (!isAttacked && Input.GetMouseButtonDown(0)) {
            isAttacked = true;
            timer = 0f;
            GameObject obj = Instantiate(bullet, pistol.transform.position, Quaternion.Euler(tempRotation));
            obj.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(radian) * bulletSpeed, Mathf.Sin(radian) * bulletSpeed);
        }
    }
}
