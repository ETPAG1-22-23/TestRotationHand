using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.GraphicsBuffer;

public class Movement2D : MonoBehaviour
{
    float horizontalValue;
    Rigidbody2D rb;
    Vector3 m_Velocity = Vector3.zero; 
    [SerializeField] private float m_MovementSmoothing = .05f;
    float speed = 40f;
    private float m_JumpForce = 800f;
    private bool jumping = false;
    public GameObject pivotGun;
    public GameObject spawnPoint;
    public Object bulletRef;
    public SpriteRenderer bras;
    public int JumpCount;

    
    

    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.y == 0)

       {

            JumpCount = 0;
            
       
        }
        horizontalValue = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump") && jumping==false && JumpCount<2)
        {
            jumping = true;

        }
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 v_diff = (target - transform.position).normalized;
        float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
        pivotGun.transform.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);
        
        if (pivotGun.transform.rotation.eulerAngles.z > 90 && pivotGun.transform.rotation.eulerAngles.z < 270)
        {
            bras.flipY=true;
            
            spawnPoint.GetComponent<Transform>().localPosition = new Vector2(1.9f, -0.35f);
        }
        else
        {
            bras.flipY = false;
            
            spawnPoint.GetComponent<Transform>().localPosition = new Vector2(1.9f, 0.3f);
        }
        
       //Debug.Log(v_diff.x);
        
        sr.flipX = (v_diff.x < 0);
        if (Input.GetButtonDown("Fire1"))
        {
            
            Instantiate(bulletRef,spawnPoint.transform.position, pivotGun.transform.rotation);
        }

    }

    private void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector2(horizontalValue * 10f * speed * Time.fixedDeltaTime, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        if (jumping)
        {
            rb.AddForce(new Vector2(0f, m_JumpForce));
            JumpCount ++;                    
            jumping = false;
        }

    }
    
       
    }
   

