using UnityEngine;

public class CameraSeguir : MonoBehaviour
{
    public Transform alvo;
    public Vector3 distancia = new Vector3(0f, 8f, -8f);
    public float velocidadeSuavizacao = 8f;

    private void LateUpdate()
    {
        if (alvo == null) return;

        Vector3 destino = alvo.position + distancia;

        transform.position = Vector3.Lerp(transform.position, destino, velocidadeSuavizacao * Time.deltaTime);
        transform.LookAt(alvo);
    }
}
