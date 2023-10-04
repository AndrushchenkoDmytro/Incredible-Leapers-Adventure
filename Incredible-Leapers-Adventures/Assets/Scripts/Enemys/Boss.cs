using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Boss : MonoBehaviour, IDamageCheck
{
    public int phase = 0;
    public float health = 100f;
    private bool isAggressive = true;
    private bool isUnprotected = false;
    [SerializeField] private bool faceRight = true;
    private float moveDirection = -1;
    [SerializeField] private float moveSpeed = 5f;
    private float aggressiveTime = 12f;
    [SerializeField] private float speedUpTime = 0;
    [SerializeField] private float unProtectedTime = 3f;
    private float preparationTime = 3f;
    private float shootingTime = 8;
    [SerializeField] private float time = 0;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;
    private ParticleSystem bulletsGun;
    private AudioSource audioSource;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioClip victory;

    [SerializeField] ResultPanel resultPanel;
    public Action<float> OnBossGetDamage;
    
    private Vector2 temp = Vector2.zero;
    private bool isPhase0 = true; 
    private bool isPhase1 = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        bulletsGun = GetComponentInChildren<ParticleSystem>();
        if (faceRight)
        {
            moveDirection = 1;
            sprite.flipX = false;
            sprite.flipY = false;
        }
        else
        {
            moveDirection = -1;
            sprite.flipX = true;
            sprite.flipY = false;
        }
    }
    void Start()
    {
        phase = 0;
        StartCoroutine(Phase0());
    }
    public float CheckForDamage(Vector3 playerPosition)
    {
        if (isUnprotected)
        {
            health -= 20f;
            OnBossGetDamage?.Invoke(health);
            AudioManager.Instance.PlayEnemyDeathAudioEffect();
            ChangePhase();
            if (health <= 0)
            {
                StartCoroutine(KillBoss());
            }
            return 0;
        }
        else if( isAggressive == true)
        {
            return 30f;
        }
        return 0f;
    }
    private void ChangePhase()
    {
        isAggressive = false;
        isUnprotected = false;
        animator.SetInteger("State", 2);
        audioSource.PlayOneShot(clips[1], 2);
        phase++;
        CheckPhase();
    }
    private void CheckPhase()
    {
        if(phase == 0)
        {
            isPhase0 = true;
            StartCoroutine(Phase0());
        }
        else if (phase == 1)
        {
            audioSource.PlayOneShot(clips[1], 2);
            isPhase0 = false;
            isPhase1 = true;
            StartCoroutine(Phase1());
        }
        else if(phase == 2)
        {
            isPhase0 = false;
            isPhase1 = false;
            StartCoroutine(Phase2());
        }
        else if(phase == 3)
        {
            isPhase1 = false;
            isPhase0 = true;
            StartCoroutine(Phase0());
        }
        else if(phase == 4)
        {
            isPhase0 = false;
            StartCoroutine(Phase3());
        }
        else if(phase == 5)
        {
            isPhase0 = false;
            isPhase1 = true;
            StartCoroutine(Phase1());
        }
        else if(phase == 6)
        {
            isPhase1 = false;
            isPhase0 = true;
            StartCoroutine(Phase0());
        }
        else
        {
            isPhase0 = false;
            isPhase1 = false;
            StopAllCoroutines();
        }
    }
    public void OnWallHit()
    {
        StartCoroutine(ChangeDirection());
    }
    IEnumerator ChangeDirection()
    {
        temp = new Vector2(rb.velocity.x * -1, 0);
        rb.velocity = temp;
        moveDirection *= -1;
        sprite.flipX = !sprite.flipX;
        audioSource.PlayOneShot(clips[2],1.5f);
        yield return new WaitForFixedUpdate();
        yield break;
    }

    IEnumerator Phase0Unprotected()
    {
        temp = new Vector2(rb.velocity.x * 0.6f, 0);
        rb.velocity = temp;
        time = 0;
        animator.SetInteger("State", 1);
        audioSource.PlayOneShot(clips[0],2f);
        yield return new WaitForSeconds(0.35f);
        isUnprotected = true;
        yield break;
    }
    IEnumerator Phase0()
    {
        yield return new WaitForSeconds(preparationTime * 0.5f);
        isAggressive = true;
        isUnprotected = false;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(preparationTime * 0.5f);
        audioSource.clip = clips[6];


        while (isPhase0)
        {
            if (isAggressive)
            {
                if (audioSource.isPlaying == false)
                {
                    audioSource.Play();
                }

                if (time < aggressiveTime)
                {
                    if (time < speedUpTime)
                    {
                        temp.x = curve.Evaluate(time) * moveDirection;
                        rb.velocity = temp;
                        yield return new WaitForFixedUpdate();
                    }
                    else
                    {
                        rb.velocity = temp;
                        yield return new WaitForFixedUpdate();
                    }
                    time += Time.fixedDeltaTime;
                }
                else
                {
                    isAggressive = false;
                    audioSource.Stop();
                    StartCoroutine(Phase0Unprotected());
                    yield return new WaitForFixedUpdate();
                }
            }
            else if (isUnprotected)
            {
                if(time < unProtectedTime)
                {
                    time += Time.fixedDeltaTime;
                    yield return new WaitForFixedUpdate();
                }
                else
                {
                    time = 0;
                    animator.SetInteger("State", 2);
                    isAggressive = true;
                    isUnprotected = false;
                    yield return new WaitForFixedUpdate();
                }
                rb.velocity = temp;
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
            Debug.Log("Phase0()");
        }
        audioSource.Stop();
        yield break;
    }
    IEnumerator Phase1()
    {
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(preparationTime * 0.5f);
        isAggressive = true;
        isUnprotected = false;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(preparationTime * 0.5f);
        isAggressive = true;
        isUnprotected = false;
        bulletsGun.Play();

        audioSource.clip = clips[3];

        while (isPhase1)
        {
            if (isAggressive)
            {
                if (audioSource.isPlaying == false)
                {
                    audioSource.Play();
                }

                if (time < shootingTime)
                {
                    time += Time.fixedDeltaTime;
                    yield return new WaitForFixedUpdate();
                }
                else
                {
                    isAggressive = false;
                    bulletsGun.Stop();
                    StartCoroutine(Phase0Unprotected());
                    yield return new WaitForFixedUpdate();
                }
            }
            else if(isUnprotected)
            {
                if (time < unProtectedTime)
                {
                    time += Time.fixedDeltaTime;
                    yield return new WaitForFixedUpdate();
                }
                else
                {
                    time = 0;
                    animator.SetInteger("State", 2);
                    audioSource.PlayOneShot(clips[1], 2);
                    bulletsGun.Play();
                    isAggressive = true;
                    isUnprotected = false;
                    yield return new WaitForFixedUpdate();
                }
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }
        yield return new WaitForFixedUpdate();
        bulletsGun.Stop();
        yield break;
    }
    IEnumerator Phase2()
    {
        yield return new WaitForSeconds(preparationTime * 0.5f);
        isAggressive = true;
        isUnprotected = false;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(preparationTime * 0.5f);

        audioSource.PlayOneShot(clips[4],1.5f);
        Vector3 newPos = new Vector3(0, 10, 0);
        while (transform.position.y != newPos.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime * 3f);
            yield return new WaitForFixedUpdate();
        }
        GameObject enemys = GameObject.Find("Enemys");
        for (int i = 0; i < 3; i++)
        {
            enemys.transform.GetChild(i * 2).gameObject.SetActive(true);
            audioSource.PlayOneShot(clips[7], 0.5f * i);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.325f);
        for (int i = 0; i < 3; i++)
        {
            enemys.transform.GetChild(i * 2 + 1).gameObject.SetActive(true);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForFixedUpdate();
        for (int i = 2; i > -1; i--)
        {
            Destroy(enemys.transform.GetChild(i * 2).gameObject);
            yield return new WaitForFixedUpdate();
        }

        while (enemys.transform.childCount > 0)
        {
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForFixedUpdate();

        animator.SetInteger("State", 2);
        audioSource.PlayOneShot(clips[4],1.5f);
        newPos = new Vector3(1, -5.4f, 0);
        while(transform.position.y != newPos.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime * 3f);
            yield return new WaitForFixedUpdate();
        }
        Debug.Log(phase);
        ChangePhase();
        yield break;
    }

    IEnumerator Phase3()
    {
        yield return new WaitForSeconds(preparationTime * 0.5f);
        isAggressive = true;
        isUnprotected = false;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(preparationTime * 0.5f);

        audioSource.PlayOneShot(clips[4], 1.5f);
        Vector3 newPos = new Vector3(0, 10, 0);
        while (transform.position.y != newPos.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime * 3f);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForFixedUpdate();

        GameObject traps = GameObject.Find("Traps");
        for (int i = 0; i < traps.transform.childCount; i++)
        {
            traps.transform.GetChild(i).gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(16.2f);
        traps.gameObject.SetActive(false);
        yield return new WaitForFixedUpdate();


        animator.SetInteger("State", 2);
        audioSource.PlayOneShot(clips[4], 1.5f);
        newPos = new Vector3(1, -5.4f, 0);
        while (transform.position.y != newPos.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime * 3f);
            yield return new WaitForFixedUpdate();
        }

        ChangePhase();
        yield break;
    }

    IEnumerator KillBoss()
    {
        rb.velocity = Vector2.zero;
        yield return new WaitForFixedUpdate();
        audioSource.PlayOneShot(clips[5], 1.5f);
        animator.SetInteger("State", 3);

        yield return new WaitForSeconds(3);

        AudioManager.Instance.PlayAudioEffect(victory, 1.25f);
        yield return new WaitForSeconds(2);
        resultPanel.ShowWinPanel();
    }
}
