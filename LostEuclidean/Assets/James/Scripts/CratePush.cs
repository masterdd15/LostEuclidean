using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CratePush : MonoBehaviour
{
    [SerializeField] float slowdown;
    //[SerializeField] Rigidbody rb_Cube;

    Vector3 moveVec;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector3 pos = transform.InverseTransformPoint(collision.transform.position);

            Vector3 toPlayer = collision.transform.position - transform.position;

            float forwardBack = Vector3.Dot(transform.forward, toPlayer.normalized);
            float leftRight = Vector3.Dot(transform.right, toPlayer.normalized);

            if (Mathf.Abs(forwardBack) > Mathf.Abs(leftRight))
            {
                if (forwardBack < 0f)
                {
                    //moveVec = transform.forward * collision.gameObject.GetComponent<Player>().speed * slowdown;
                    transform.Translate(transform.forward * collision.gameObject.GetComponent<Player>().speed * slowdown * Time.deltaTime, Space.World);
                }
                else if (forwardBack > 0f)
                {
                    //moveVec = -transform.forward * collision.gameObject.GetComponent<Player>().speed * slowdown;
                    transform.Translate(-transform.forward * collision.gameObject.GetComponent<Player>().speed * slowdown * Time.deltaTime, Space.World);
                }
            }
            else
            {
                if (leftRight < 0f)
                {
                    //moveVec = transform.right * collision.gameObject.GetComponent<Player>().speed * slowdown;
                    transform.Translate(transform.right * collision.gameObject.GetComponent<Player>().speed * slowdown * Time.deltaTime, Space.World);
                }
                else if (leftRight > 0f)
                {
                    //moveVec = -transform.right * collision.gameObject.GetComponent<Player>().speed * slowdown;
                    transform.Translate(-transform.right * collision.gameObject.GetComponent<Player>().speed * slowdown * Time.deltaTime, Space.World);
                }
            }
        }
    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        moveVec = Vector3.zero;
    //    }
    //}
}
