using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float speed;

    public Text countText;

    public Text winText;

    public ParticleSystem pickedUpEffect;

    private Rigidbody rb;

    private int Count { get; set; }

    void Start()
    {
        Debug.Log("" + Physics.gravity);
        rb = GetComponent<Rigidbody>();
        Count = 0;
        SetCount();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            Count++;
            SetCount();
            pickedUpEffect.transform.position = this.transform.position;
            pickedUpEffect.Play();
        }
    }
    
    private void SetCount()
    {
        countText.text = "Count:" + Count;
        if (Count == 12)
        {
            winText.gameObject.SetActive(true);
        }
    }
}
