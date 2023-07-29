using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] float velocity = 5;
    [SerializeField] float swipeThreshold = 125;
    [SerializeField] float velocityMax = 200;

    Rigidbody2D rigidbody2d;
    private Transform myTransform;
    Vector3 startPosition;
    bool moving;
    float horizontal;
    float vertical;
    bool caught;
    List<Vector2> movements = new List<Vector2>();
    private Vector2 startTouch;
    private bool drag;
    private float androidVelocity = 10;

    public static Action OnDied;

    private bool canGoUp = true;
    private bool canGoDown = true;
    private bool canGoRight = true;
    private bool canGoLeft = true;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        myTransform = transform;
        startPosition = myTransform.position;
    }
    private void Update()
    {
        if (!caught)
        {
            #region Standalone Controls
            #if UNITY_STANDALONE
            if (Input.GetButtonDown("MoveRight"))
            {
                movements.Add(new Vector2(1, 0));
            }
            else if (Input.GetButtonDown("MoveLeft"))
            {
                movements.Add(new Vector2(-1, 0));
            }
            else if (Input.GetButtonDown("MoveUp"))
            {
                movements.Add(new Vector2(0, 1));
            }
            else if (Input.GetButtonDown("MoveDown"))
            {
                movements.Add(new Vector2(0, -1));
            }
            #endif
            #endregion

            #region Mobile Controls
            #if UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    startTouch = touch.position;
                    drag = true;
                }
                else if(touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                {
                    startTouch = Vector2.zero;
                    drag = false;
                }

                if (drag)
                {
                    var distance = touch.position - startTouch;
                    if(distance.magnitude > swipeThreshold)
                    {
                        if(Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
                        {
                            if(distance.x > 0)
                            {
                                movements.Add(new Vector2(1,0));
                            }
                            else
                            {
                                movements.Add(new Vector2(-1,0));
                            }
                        }
                        else
                        {
                            if (distance.y > 0)
                            {
                                movements.Add(new Vector2(0,1));
                            }
                            else
                            {
                                movements.Add(new Vector2(0,-1));
                            }
                        }

                        startTouch = touch.position;
                    }
                }
            }
            #endif
            #endregion
        }
    }

    void FixedUpdate()
    {
        if(!caught)
        {
            moving = Mathf.Abs(rigidbody2d.velocity.y) > 0.5f;

            if (!moving)
            {
                #if UNITY_STANDALONE
                //rigidbody2d.velocity = Vector2.zero;
                #endif

                if (movements.Count > 0)
                {
                    horizontal = movements[0].x;
                    vertical = movements[0].y;
                    movements.RemoveAt(0);
                }
                else
                {
                    horizontal = 0;
                    vertical = 0;
                }
            }

            #if UNITY_STANDALONE
            rigidbody2d.AddForce(new Vector2(horizontal, vertical) * velocity*150);
            //rigidbody2d.velocity += new Vector2(velocity * horizontal, velocity * vertical);
            /*if(rigidbody2d.velocity.x > velocityMax)
            {
                rigidbody2d.velocity = new Vector2(velocityMax, rigidbody2d.velocity.y);
            }
            else if (rigidbody2d.velocity.x < -velocityMax)
            {
                rigidbody2d.velocity = new Vector2(-velocityMax, rigidbody2d.velocity.y);
            }
            else if (rigidbody2d.velocity.y > velocityMax)
            {
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, velocityMax);
            }
            else if (rigidbody2d.velocity.y < -velocityMax)
            {
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, -velocityMax);
            }*/
            #endif

            #if UNITY_ANDROID
            if (horizontal > 0 && canGoRight || horizontal < 0 && canGoLeft || vertical > 0 && canGoUp || vertical < 0 && canGoDown)
            {
                myTransform.Translate(horizontal * androidVelocity * Time.deltaTime, vertical * androidVelocity * Time.deltaTime, 0);
                moving = true;
                androidVelocity += 1;
            }
            else
            {
                moving = false;
                androidVelocity = 10;
            }
            #endif
        }
    }

    public void SetUp(bool value)
    {
        canGoUp = value;
    }
    public void SetDown(bool value)
    {
        canGoDown = value;
    }
    public void SetRight(bool value)
    {
        canGoRight = value;
    }
    public void SetLeft(bool value)
    {
        canGoLeft = value;
    }

    public void Die()
    {
        StartCoroutine(WaitForRestart());
    }

    IEnumerator WaitForRestart()
    { 
        #if UNITY_STANDALONE
        rigidbody2d.velocity = Vector2.zero;
        #endif

        moving = false;
        var movementsSaved = movements.Count;
        for (int i = 0; i < movementsSaved; i++)
        {
            movements.RemoveAt(0);
        }
        caught = true;
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(1.5f);

        Time.timeScale = 1;
        transform.position = startPosition;
        caught = false;
        var inventory = GetComponent<Inventory>();
        if (inventory.HasKey())
            inventory.ReturnKey();

        yield return new WaitForSecondsRealtime(0.1f);

        OnDied?.Invoke();
    }
}
