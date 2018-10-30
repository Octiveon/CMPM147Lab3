using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] private GameObject target = null;
    [SerializeField] private float effectSpd = 0.1f;

    [SerializeField] private bool destroy = false;
    [SerializeField] private bool useParticle = false;
    [SerializeField] private bool useSound = false;
    [SerializeField] private bool useShrink = false;
    [SerializeField] private bool useGrow = false;


    public ParticleSystem emitter;
    public AudioSource sound;

    private bool inRange = false;
    private bool hasShrunk = false;

    private Vector3 scale;

    void Start()
    {
        if(target == null)
        {
            target = transform.Find("Objects").gameObject;
        }

        if(useGrow && useShrink)
        {
            useGrow = false;
        }

        if (GetComponent<ParticleSystem>() != null)
        {
            emitter = GetComponent<ParticleSystem>();
            useParticle = true;
        }
        else { useParticle = false; }
        
        if(GetComponent<AudioSource>() != null)
        {
            sound = GetComponent<AudioSource>();
            useSound = true;
        }
        else { useSound = false; }

        scale = target.transform.localScale;
    }

    void Update()
    {
        if(inRange)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                Activate();
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            inRange = true;
            if(useShrink && !hasShrunk)
            {
                StartCoroutine(Shrink());
            }
            else if (useGrow && !hasShrunk)
            {
                StartCoroutine(Grow());
            }

        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            inRange = false;

            if (useShrink)
            {
                StartCoroutine(UnShrink());
            }else if(useGrow)
            {
                StartCoroutine(UnGrow());
            }
        }
    }

    void Activate()
    {
        if (useShrink && !hasShrunk) { StartCoroutine(Shrink()); }

        if (useParticle) { emitter.Play(); }

        if (useSound) { sound.Play(); }

        if (destroy) { Destroy(gameObject); }
    }

    IEnumerator Shrink()
    {

        Debug.Log("Shrinking");
        float newScale = target.transform.localScale.x / 2;

        while (target.transform.localScale.x > newScale && inRange)
        {
            yield return new WaitForSeconds(effectSpd);
            target.transform.localScale = new Vector3(
                target.transform.localScale.x - 0.01f,
                target.transform.localScale.y - 0.01f,
                target.transform.localScale.z - 0.01f);
        }

        hasShrunk = true;

    }

    IEnumerator UnShrink()
    {
        Debug.Log("Growing");
        float newScale = scale.x;

        if (!hasShrunk)
        {
            yield return new WaitForSeconds(effectSpd * 2);
        }

        while (target.transform.localScale.x < newScale && !inRange)
        {
            yield return new WaitForSeconds(effectSpd);
            target.transform.localScale = new Vector3(
                target.transform.localScale.x + 0.01f,
                target.transform.localScale.y + 0.01f,
                target.transform.localScale.z + 0.01f);
        }
        hasShrunk = false;
    }

    IEnumerator Grow()
    {

        Debug.Log("Growing");
        float newScale = target.transform.localScale.x * 2;

        while (target.transform.localScale.x < newScale && inRange)
        {
            yield return new WaitForSeconds(effectSpd);
            target.transform.localScale = new Vector3(
                target.transform.localScale.x + 0.01f,
                target.transform.localScale.y + 0.01f,
                target.transform.localScale.z + 0.01f);
        }

        hasShrunk = true;

    }

    IEnumerator UnGrow()
    {

        Debug.Log("Growing");
        float newScale = scale.x;

        while (target.transform.localScale.x > newScale && !inRange)
        {
            yield return new WaitForSeconds(effectSpd);
            target.transform.localScale = new Vector3(
                target.transform.localScale.x - 0.01f,
                target.transform.localScale.y - 0.01f,
                target.transform.localScale.z - 0.01f);
        }

        hasShrunk = false;

    }

}
