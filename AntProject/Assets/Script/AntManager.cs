using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntManager : MonoBehaviour
{
    [Tooltip("蚂蚁的速度")]
    public float velocity;

    [Tooltip("控制旋转的时间间隔")]
    public float rotateTimeInterval;

    [Tooltip("控制旋转的开始角度")]
    public float rotateRangeStart;

    [Tooltip("箭头的旋转速度,多少秒从min->max")]
    public float rotateVelocity;

    public Transform AntTrans;
    public Transform arrowBoxTrans;
    public Transform arrowTrans;
    public Rigidbody antRigidbody;
    public List<Transform> CheckPointList = new List<Transform>();
    public Transform CheckPoint;

    //下次控制旋转的时间
    private float m_rotateTime;

    private float m_rotateY;

    private bool m_rotateRight = true;

    private bool rotating = false;

    private Vector3 m_curVelocity = new Vector3();

    private static float s_angle2PI = 360;
    private void Start()
    {
        m_curVelocity = new Vector3(0, 0, velocity);
        m_rotateTime = Time.time + rotateTimeInterval;
    }
    private void Update()
    {
        CheckShowArrow();
        CheckRotate();
        RotateArrow();
        CheckOrder();
    }

    private void FixedUpdate()
    {
        Vector3 newVelocity = m_curVelocity - antRigidbody.velocity;
        antRigidbody.AddForce(newVelocity, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            ContactPoint contact = collision.contacts[0];
            var dis = AntTrans.forward + contact.normal;
            m_curVelocity = (contact.normal + dis).normalized * velocity;
            Vector3 lookAtPosition = AntTrans.position + m_curVelocity;
            AntTrans.LookAt(lookAtPosition);
            return;
        }
        //if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        //{
        //    CheckWall();
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Resource"))
        {
            Debug.Log(111);
            return;
        }
    }

    private void CheckShowArrow()
    {
        if (rotating)
        {
            arrowBoxTrans.gameObject.SetActive(true);
        }
        else
        {
            arrowBoxTrans.gameObject.SetActive(false);
        }
    }

    private void CheckRotate()
    {
        if (!rotating && Time.time > m_rotateTime)
        {
            rotating = true;
            // 开始旋转控制
            m_rotateY = rotateRangeStart;
            arrowBoxTrans.localRotation = Quaternion.Euler(0, rotateRangeStart, 0);
        }
    }
    private void RotateArrow()
    {
        if (rotating)
        {
            var dis = s_angle2PI / rotateVelocity * Time.deltaTime;
            m_rotateY += dis * (m_rotateRight ? 1 : -1);
            if (m_rotateY > s_angle2PI)
            {
                m_rotateY -= s_angle2PI;
            }
            else if (m_rotateY < 0)
            {
                m_rotateY = s_angle2PI - m_rotateY;
            }
            //arrowBoxTrans.rotation = Quaternion.RotateTowards(arrowBoxTrans.rotation, Quaternion.Euler(0, 0, m_rotateY), 1f);
            arrowBoxTrans.localRotation = Quaternion.Euler(0, m_rotateY, 0);
        }
    }

    private void CheckOrder()
    {
        if (rotating && Input.GetKeyDown(KeyCode.Space))
        {
            m_rotateTime = Time.time + rotateTimeInterval;
            rotating = false;
            StartCoroutine(RotateTransFormNextFrame(Quaternion.Euler(0, arrowBoxTrans.rotation.eulerAngles.y, 0)));
        
        }
    }

    private IEnumerator RotateTransFormNextFrame(Quaternion q)
    {
        yield return null;
        AntTrans.rotation = q;
        yield return null;
        m_curVelocity = AntTrans.forward * velocity;
    }

    private void CheckWall()
    {
        foreach(var trans in CheckPointList)
        {
            Ray ray = new Ray(trans.position, trans.forward);
            Debug.DrawRay(ray.origin, ray.direction * 0.8f);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 0.8f, LayerMask.GetMask("Wall")))
            {
                var dis = AntTrans.forward + hit.normal;
                m_curVelocity = (hit.normal + dis).normalized * velocity;

                Vector3 lookAtPosition = AntTrans.position + m_curVelocity;
                AntTrans.LookAt(lookAtPosition);
                return;
            }
        }
    }

}
