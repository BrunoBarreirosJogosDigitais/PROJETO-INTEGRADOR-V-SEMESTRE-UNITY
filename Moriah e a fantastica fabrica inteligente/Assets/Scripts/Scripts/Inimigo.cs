using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public GameObject alvo;
    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        alvo = GameObject.Find("PlacaMae");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = alvo.transform.position - this.transform.position;
        Vector3 velocidade = pos.normalized * speed;
        this.transform.Translate(velocidade * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name.StartsWith("PlacaMae"))
        {
            Destroy(this.gameObject);
        }
    }
}
