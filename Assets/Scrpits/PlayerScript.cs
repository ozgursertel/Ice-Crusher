using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

public class PlayerScript : MonoBehaviour
{

    public GameObject boxPrefab;
    public readonly float PlayerSpeed = 40;
    private bool canShoot;
    private float forceMultipler;
    Rigidbody rb;
    List<GameObject> boxes;
    Animator animator;
    private SwerveInputSystem _swerveInputSystem;
    private Swipe swipe;
    [SerializeField] private float swerveSpeed = 0.5f;
    [SerializeField] private float maxSwerveAmount = 1f;

    [SerializeField] private Transform handTransform;
    [SerializeField] private GameObject endCorn;

    public TextMeshProUGUI AcornCount;

    public float tiltingPower;

    public float maxZRotation;
    public float minZRotation;

    public float hitDistance;

    private bool isSwerveMechanics;
    private bool isCatWalkingMechanics;
    private bool isDead;

    [SerializeField]private Transform endAreaPos;



    private Ray ray;
    private RaycastHit hit;

    public bool CanShoot { get => canShoot; set => canShoot = value; }
    public bool IsDead { get => isDead; set => isDead = value; }
    public List<GameObject> Acorns { get => boxes; set => boxes = value; }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        _swerveInputSystem = GetComponent<SwerveInputSystem>();
        Acorns = new List<GameObject>();
        swipe = GetComponent<Swipe>();
        isSwerveMechanics = true;
        CanShoot = true;
        forceMultipler = PlayerSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead)
        {
            return;
        }
        AcornCount.text = Acorns.Count.ToString();
    }

    private void FixedUpdate()
    {
        if (IsDead)
        {
            return;
        }
        if (GameController.Instance.IsGameStarted && !GameController.Instance.IsLevelEnded)
        {
            
               Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.forward * hitDistance, Color.green);
               ray = new Ray(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.forward);
               rb.AddForce(Vector3.forward * 20 * forceMultipler);

            //Swerve
            if (isSwerveMechanics)
            {
                float swerveAmount = Time.deltaTime * swerveSpeed * _swerveInputSystem.MoveFactorX;
                swerveAmount = Mathf.Clamp(swerveAmount, -maxSwerveAmount, maxSwerveAmount);
                rb.AddForce(new Vector3(swerveAmount * 1000, 0, 0));
            }

            //CatWalk
            if (isCatWalkingMechanics)
            {
                if (Vector3.Angle(Vector3.right, transform.right) >= minZRotation && Vector3.Angle(Vector3.right, transform.right) <= maxZRotation)
                {
                    transform.Rotate(0, 0, Input.acceleration.x * -2);
                }
                else
                {
                    FallPlayer();
                }
            }

            //RayCast For Shooting
            Shoot("WallBox");

        }
    }

    public void Shoot(string tag)
    {
        if (Physics.Raycast(ray, out hit, hitDistance))
        {
            if (hit.transform.gameObject.tag == tag && CanShoot && Acorns.Count > 0)
            {
                CanShoot = false;
                StartCoroutine(boxes[boxes.Count - 1].GetComponent<BoxScript>().ThrowBox(this.transform, hit.transform));
            }
        } 
    }

    private void FallPlayer()
    {
        IsDead = true;
        StartCoroutine(GameController.Instance.GameOver());
    }


    public void setThrowingAnim(bool b)
    {
        animator.SetBool("Throwing", b);

    }
    public void setEndLevelDanceAnim(bool b)
    {
        animator.SetBool("End Level Dance", b);

    }
    public void setCarefullWalkAnim(bool b)
    {
        animator.SetBool("Carefull Walk", b);
    }
    public void setJumpingAnim(bool b)
    {
        animator.SetBool("Jumping", b);
    }
    public void setWaitingAnim(bool b)
    {
        animator.SetBool("Waiting", b);
    }
    public void setRunningAnim(bool b)
    {
        animator.SetBool("Running", b);
    }
    public void PlayerGameStarted()
    {
        setRunningAnim(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsDead)
        {
            return;
        }

        if (other.gameObject.tag == "Acorn" && !Acorns.Contains(other.gameObject))
        {
            AudioManager.instance.Play("Box Collect");
            other.gameObject.GetComponent<BoxScript>().PlayerCollision(handTransform);
            Acorns.Add(other.gameObject);
        }

        if (other.gameObject.tag == "EndOfLevel")
        {
            GameController.Instance.IsLevelEnded = true;
            StartCoroutine(endGame());
        }

        if (other.gameObject.tag == "CatWalk")
        {
            if (isCatWalkingMechanics && !isSwerveMechanics)
            {
                return;
            }
            this.transform.DOMoveX(0, 0.3f);
            rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionX;
            isCatWalkingMechanics = true;
            isSwerveMechanics = false;
            setCarefullWalkAnim(true);
            forceMultipler = PlayerSpeed / 2;
        }

        if (other.gameObject.tag == "DeadZone")
        {
            FallPlayer();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (IsDead)
        {
            return;
        }
        if (other.gameObject.tag == "CatWalk")
        {
            isCatWalkingMechanics = false;
            isSwerveMechanics = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            transform.DORotate(new Vector3(0, 0, 0), 0.1f);
            setCarefullWalkAnim(false);
            forceMultipler = PlayerSpeed;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead)
        {
            return;
        }
        //Debug.Log(gameObject.name + " colide with -->" + collision.gameObject.name);
        if (collision.gameObject.tag == "WallBox")
        {
            FallPlayer();
        }

        if (collision.gameObject.tag == "+5 Box")
        {
            BoxAdder(collision.transform, 5);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "+1 Box")
        {
            BoxAdder(collision.transform, 1);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "+10 Box")
        {

            BoxAdder(collision.transform, 10);
            Destroy(collision.gameObject);

        }
        if (collision.gameObject.tag == "-5 Box")
        {
            BoxRemove(5);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "-3 Box")
        {
            BoxRemove(3);
            Destroy(collision.gameObject);
        }
    }

    public void BoxAdder(Transform t, int addCount)
    {
        for (int i = 0; i < addCount; i++)
        {
            GameObject obj = Instantiate(boxPrefab, t.position, Quaternion.identity);
            obj.GetComponent<BoxScript>().PlayerCollision(handTransform);
            boxes.Add(obj);
        }
    }

    public void BoxRemove(int removeCount)
    {
        for (int i = 0; i < removeCount; i++)
        {
            if (boxes.Count > 0)
            {
                GameObject obj = boxes[boxes.Count - 1];
                removeBoxFromList(boxes[boxes.Count - 1]);
                Destroy(obj);
            }
        }
    }

    public void removeBoxFromList(GameObject obj)
    {
        if (Acorns.Contains(obj))
        {
            Acorns.Remove(obj);
        }
    }

    public IEnumerator endGame()
    {
        //Elimizde kalan palamutlarý çarparak endCornun scale ný büyüt
        int count = Mathf.Clamp(Acorns.Count, 2, 300);
        transform.DOMove(endAreaPos.position,0.2f);
        float totalGrowTime = Acorns.Count * 0.2f;
        yield return new WaitForSeconds(0.2f);
        setRunningAnim(false);
        endCorn.transform.DOScale(endCorn.transform.localScale * count * 0.5f,totalGrowTime);
        foreach (GameObject obj in Acorns)
        {
            obj.GetComponent<BoxScript>().SetMeshRenderer(true);
            obj.transform.DOMove(endCorn.transform.position, 0.2f);
            endCorn.transform.DOMoveY(endCorn.transform.position.y + 0.3f, 0.2f);
            yield return new WaitForSeconds(0.2f);
            obj.GetComponent<BoxScript>().SetMeshRenderer(false);
        }
        yield return new WaitForSeconds(1f);
        GameController.Instance.LevelEnded(count);
    }


}
