using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float speed;

    public Text countText;

    public Text winText;

    private Rigidbody rb;

    private int Count { get; set; }

    private float maxY;

    void Start()
    {
        Debug.Log("" + Physics.gravity);
        rb = GetComponent<Rigidbody>();
        Count = 0;
        SetCount();
        maxY = this.gameObject.transform.position.y;
    }

    private void Update()
    {
        bool pressed = Input.GetKeyDown("space");

        if (pressed)
        {
            float speedY = Mathf.Sqrt(2.000f * (-Physics.gravity.y) * 4.000f); /* 重力と揚力はともに上が+ */
            rb.velocity = new Vector3(rb.velocity.x, speedY, rb.velocity.z);
        }

        float currentY = this.gameObject.transform.position.y;
        maxY = (currentY > maxY) ? currentY : maxY;
        countText.text = maxY + "";
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
