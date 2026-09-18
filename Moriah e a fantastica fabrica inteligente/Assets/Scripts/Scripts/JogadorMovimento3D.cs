using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class JogadorMovimento3D : MonoBehaviour
{
    public float velocidade = 5f;
    public float gravidade = -20f;
    public float alturaPulo = 1.2f;

    public Transform cameraPrincipal;

    private CharacterController controlador;
    private Vector3 velocidadeVertical;

    private void Awake()
    {
        controlador = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 frente = cameraPrincipal != null
            ? cameraPrincipal.forward
            : Vector3.forward;

        Vector3 direita = cameraPrincipal != null
            ? cameraPrincipal.right
            : Vector3.right;

        frente.y = 0f;
        direita.y = 0f;
        frente.Normalize();
        direita.Normalize();

        Vector3 movimento = frente * vertical + direita * horizontal;

        if (movimento.sqrMagnitude > 1f)
            movimento.Normalize();

        controlador.Move(movimento * velocidade * Time.deltaTime);

        if (controlador.isGrounded && velocidadeVertical.y < 0f)
            velocidadeVertical.y = -2f;

        if (Input.GetButtonDown("Jump") && controlador.isGrounded)
            velocidadeVertical.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);

        velocidadeVertical.y += gravidade * Time.deltaTime;
        controlador.Move(velocidadeVertical * Time.deltaTime);

        if (movimento.sqrMagnitude > 0.01f)
        {
            Quaternion rotacao = Quaternion.LookRotation(movimento);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                rotacao,
                10f * Time.deltaTime
            );
        }
    }
}
