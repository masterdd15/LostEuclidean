using UnityEngine;

public class CratePush : Interactable
{
    [SerializeField] SimpleColorCube colorCube;
    [SerializeField] float grabDistance = 2f;
    [SerializeField] Collider crateCollider;
    [SerializeField] Rigidbody rb;

    private bool isGrabbed = false;
    Vector3 playerOffset;
    GameObject player;
    BoxCollider playerBox;

    private void Start()
    {
        
    }

    public override void Update()
    {
        base.Update();

        if (isGrabbed && colorCube.CanInteract() && InteractionEnabled)
        {
            transform.position = player.transform.position + playerOffset;
        }
        else if (isGrabbed && (!colorCube.CanInteract() || !InteractionEnabled))
        {
            isGrabbed = false;
            playerBox.enabled = false;
            crateCollider.enabled = true;
            //rb.isKinematic = false;
            crateCollider.isTrigger = false;
            player.GetComponent<Player>().IsHolding = false;
        }
    }

    public override void Interact()
    {
        if (isGrabbed)
        {
            isGrabbed = false;
            playerBox.enabled = false;
            //crateCollider.enabled = true;
            //rb.isKinematic = false;
            crateCollider.isTrigger = false;
            player.GetComponent<Player>().IsHolding = false;
        }
        else if (!isGrabbed && colorCube.CanInteract() && InteractionEnabled)
        {
            isGrabbed = true;
            player = GameObject.FindGameObjectWithTag("Player");
            playerOffset = transform.position - player.transform.position;

            Vector3 toPlayer = transform.position - player.transform.position;

            float forwardBack = Vector3.Dot(transform.forward, toPlayer.normalized);
            float leftRight = Vector3.Dot(transform.right, toPlayer.normalized);

            float xOff = 0f;
            float zOff = 0f;

            if (Mathf.Abs(forwardBack) > Mathf.Abs(leftRight))
            {
                if (forwardBack < 0f)
                {
                    transform.Translate(transform.forward * player.GetComponent<Player>().speed * Time.deltaTime, Space.World);
                    zOff = -0.1f;
                }
                else if (forwardBack > 0f)
                {
                    transform.Translate(-transform.forward * player.gameObject.GetComponent<Player>().speed * Time.deltaTime, Space.World);
                    zOff = 0.1f;
                }
            }
            else
            {
                if (leftRight < 0f)
                {
                    transform.Translate(transform.right * player.gameObject.GetComponent<Player>().speed * Time.deltaTime, Space.World);
                    xOff = -0.1f;
                }
                else if (leftRight > 0f)
                {
                    transform.Translate(-transform.right * player.gameObject.GetComponent<Player>().speed * Time.deltaTime, Space.World);
                    xOff = 0.1f;
                }
            }

            //playerOffset += new Vector3(xOff, 0f, zOff);

            // Make the player look at the cube
            player.transform.LookAt(transform);
            Vector3 playerRot = player.transform.rotation.eulerAngles;
            playerRot = new Vector3(0f, playerRot.y - 90f, 0f);
            player.transform.rotation = Quaternion.Euler(playerRot);
            player.GetComponent<Player>().IsHolding = true;

            playerBox = player.GetComponent<BoxCollider>();
            playerBox.enabled = true;
            //crateCollider.enabled = false;
            crateCollider.isTrigger = true;
            //rb.isKinematic = true;
        }

        //if (isGrabbed)
        //{
        //    // Release the crate from the player
        //    isGrabbed = false;
        //    playerTransform = null;
        //    playerRigidbody = null;

        //    // Destroy the fixed joint and reset the parent
        //    Destroy(fixedJoint);

        //    transform.SetParent(null);

        //    // Enable the crate's rigidbody 
        //    GetComponent<Rigidbody>().isKinematic = false;
        //}
        //else
        //{
        //    // Get the player's transform and rigidbody
        //    playerTransform = collision.gameObject.transform;
        //    playerRigidbody = playerTransform.gameObject.GetComponent<Rigidbody>();

        //    // Calculate the distance between the player and the crate
        //    float distance = Vector3.Distance(playerTransform.position, transform.position);


        //    // Check if the player is within grabbing distance
        //    if (distance <= grabDistance)
        //    {
        //        // Attach the crate to the player using a fixed joint
        //        fixedJoint = gameObject.AddComponent<FixedJoint>();
        //        fixedJoint.connectedBody = playerRigidbody;
        //        fixedJoint.breakForce = Mathf.Infinity;

        //        // Make the crate a child of the player
        //        transform.parent = playerTransform;

        //        // Disable the crate's rigidbody so it follows the player without physics simulation
        //        GetComponent<Rigidbody>().isKinematic = true;

        //        isGrabbed = true;
        //    }
        //}
    }
}
