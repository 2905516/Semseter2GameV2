using UnityEngine;
using UnityEngine.EventSystems;

public class playerAni : MonoBehaviour
{
    public Rigidbody pRb;
    public Animator pAnim;
    private Vector3 moveDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pRb = GetComponent<Rigidbody>();
        pRb = GetComponentInParent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector3(pRb.linearVelocity.x, 0, pRb.linearVelocity.z);

        if (moveDirection.magnitude > 0.1f)
        {
            pAnim.SetFloat("Move", 1);
        }
        else
        {
            pAnim.SetFloat("Move", 0);
        }
    }

    
}
