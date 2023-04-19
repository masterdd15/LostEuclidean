using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFloat : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 upPosition;
    private Vector3 downPosition;
    private Vector3 target;
    [SerializeField] bool isUp;
    [SerializeField] bool isDown;
    [SerializeField] float speed;
    [SerializeField] float distance;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = gameObject.transform.position;
        upPosition = new Vector3(startPosition.x, startPosition.y + distance, startPosition.z);
        downPosition = new Vector3(startPosition.x, startPosition.y - distance, startPosition.z);
        target = upPosition;
        isDown = true;


    }

    // Update is called once per frame
    void Update()
    {
        if(upPosition.y - transform.position.y <= 0.3)
        {
            isUp = true;
            isDown = false;
        }
        else if(downPosition.y - transform.position.y >= -0.3 )
        {
            isUp = false;
            isDown = true;
        }

        if(isUp)
        {
            target = downPosition;
            timer = 0;
        }
        else if(isDown)
        {
            target = upPosition;
            timer = 0;
        }

        timer += Time.deltaTime;
        float percentageComplete = timer / speed;

        transform.position = Vector3.Lerp(transform.position, target, percentageComplete);
    }
}
