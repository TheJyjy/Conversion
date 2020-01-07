using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    [Header("Deplacement")]
    public float vitesse;
    private bool canmove = true;
    private Rigidbody rb;
    [Header("Camera")]
    public GameObject cameraplayer;
    public float sensi;
    public float limiteHaut;
    public float limiteBas;
    [Header("Saut")]
    public int DoubleSaut;
    public float vitessesaut;
    [Header("Dash")]
    public float pulse;
    public float cooldownpulse;
    private Vector3 Myposition;
    public float distancemax;
    private bool dashstate = false;
    private bool candash = true;
    public float newgravity;
    private float currentgravity;
    private bool canconverse = true;
    public float cooldownconverse;
    public Transform pieds;
    private float rotationSpeed;
    private bool inconverse = false;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(Physics.gravity.x, newgravity, Physics.gravity.z);
        rb = GetComponent<Rigidbody>();
        Cursor.visible = false;
        currentgravity = Physics.gravity.y;

    }

    // Update is called once per frame
    void Update()
    {

        Conversense();
        Impulsion();

        float axeX = Input.GetAxisRaw("Horizontal") * vitesse;
        float axeZ = Input.GetAxisRaw("Vertical") * vitesse;
        //Debug.Log("L'axe x =" + axeX);

        Vector3 mouvementenX = transform.right * axeX;
        Vector3 mouvementenZ = transform.forward * axeZ;

        if (DoubleSaut > 0)
        {

            Saut();

        }

        if (canmove == true)
        {

            rb.velocity = new Vector3(0, rb.velocity.y, 0) + mouvementenX + mouvementenZ;
        }

        float sourisX = Input.GetAxis("Mouse X") * sensi;
        float sourisY = Input.GetAxis("Mouse Y") * sensi;

        transform.Rotate(0, sourisX, 0);
        //transform.rotation = Quaternion.Euler(0, sourisX, 0);

        cameraplayer.transform.Rotate(sourisY, 0, 0);

        Vector3 currentRotation = cameraplayer.transform.localRotation.eulerAngles;

        if (currentRotation.x > 180f)

        {

            currentRotation.x -= 360f;


        }

        currentRotation.x = Mathf.Clamp(currentRotation.x, limiteBas, limiteHaut);
        cameraplayer.transform.localRotation = Quaternion.Euler(currentRotation);

        if (dashstate == true)
        {

            if (Vector3.Distance(Myposition, transform.position) >= distancemax || rb.velocity == Vector3.zero)
            {

                rb.velocity = Vector3.zero;
                dashstate = false;
                canmove = true;



            }

       

        }

        if (inconverse)
        {

            if(currentgravity > 0)
            {

               float playerZRot = transform.rotation.eulerAngles.z ;
               transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Lerp(playerZRot, 180f, rotationSpeed * Time.deltaTime)));

                if(transform.rotation.eulerAngles.z >= 180f)
                {
                    inconverse = false;
                }

            }
            else
            {
                float playerZRot = transform.rotation.eulerAngles.z;
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Lerp(playerZRot, 0f, rotationSpeed * Time.deltaTime)));

                if (transform.rotation.eulerAngles.z <= 0f)
                {
                    inconverse = false;
                }
            }

        }


    }
    void Saut()
    {


        if (Input.GetKeyDown(KeyCode.Space))

        {

            rb.velocity = rb.velocity + transform.up * vitessesaut;
            DoubleSaut--;
        }

    }

    void Impulsion()
    {

        if (Input.GetKeyDown(KeyCode.E)&& candash == true)


        {
            candash = false;
            dashstate = true;
            Myposition = transform.position;
            rb.velocity = rb.velocity + transform.forward * pulse;

            

            canmove = false;
            StartCoroutine(waitimpulsion());


        }

    }

    public void Resetsaut()
    {

        DoubleSaut = 2;

    }

    IEnumerator waitimpulsion()
    {
        yield return new WaitForSeconds(cooldownpulse);
        candash = true;
        
        

    }

    void Conversense()
    {
        if (Input.GetKeyDown(KeyCode.X)&& canconverse == true)
        {
            RaycastHit hit ;

            if(Physics.Raycast(pieds.position, transform.up, out hit, 300f))
            {
                if(hit.collider != null)

                {
                    Debug.Log(hit.collider.gameObject); 
                    float distanceConverse = Vector3.Distance(pieds.position, hit.point);
                    Debug.Log(distanceConverse);

                        //rotationSpeed = (180f - transform.rotation.eulerAngles.z) / distanceConverse;
                        rotationSpeed = Mathf.Abs(currentgravity) / distanceConverse;
                        inconverse = true;
                        Debug.Log(rotationSpeed);
                }
            }

            canconverse = false;
        Physics.gravity = new Vector3(0, currentgravity *=-1, 0);

            StartCoroutine(Waitconverse());
            
        }



    }

    IEnumerator Waitconverse()
    {

        yield return new WaitForSeconds(cooldownconverse);
        canconverse = true;

    }





}




