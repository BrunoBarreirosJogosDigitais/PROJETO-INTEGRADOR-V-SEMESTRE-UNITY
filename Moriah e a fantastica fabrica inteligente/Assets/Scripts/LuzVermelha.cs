using UnityEngine;

public class LuzVermelha : MonoBehaviour
{
    public Light luz;
    public float velocidade = 1f;

    void Start()
    {
        luz.color = Color.red;
    }

    void Update()
    {
        luz.enabled = Mathf.PingPong(Time.time * velocidade, 1f) > 0.5f;
    }
}
