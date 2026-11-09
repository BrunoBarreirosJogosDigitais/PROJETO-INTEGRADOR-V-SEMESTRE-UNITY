
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class JogadorMovimento3D : MonoBehaviour
{
    [Header("Movimentação")]
    public float velocidade = 5f;
    public float gravidade = -20f;
    public float alturaPulo = 1.2f;

    [Header("Câmera")]
    public Transform cameraPrincipal;

    [Header("Animações")]
    public Animator animator;

    private CharacterController controlador;
    private Vector3 velocidadeVertical;

    private void Awake()
    {
        controlador = GetComponent<CharacterController>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
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

        Vector3 movimento = frente * vertical +
                            direita * horizontal;

        if (movimento.sqrMagnitude > 1f)
            movimento.Normalize();

     
        bool consertando = Input.GetKey(KeyCode.R);
        bool mexendo = Input.GetKey(KeyCode.F)
                       && !consertando;

        bool trabalhando = consertando || mexendo;

        if (!trabalhando)
        {
            controlador.Move(
                movimento * velocidade * Time.deltaTime
            );
        }
        if (controlador.isGrounded &&
            velocidadeVertical.y < 0f)
        {
            velocidadeVertical.y = -2f;
        }

        if (Input.GetButtonDown("Jump") &&
            controlador.isGrounded &&
            !trabalhando)
        {
            velocidadeVertical.y =
                Mathf.Sqrt(alturaPulo * -2f * gravidade);
        }

        velocidadeVertical.y += gravidade * Time.deltaTime;

        controlador.Move(
            velocidadeVertical * Time.deltaTime
        );

        if (!trabalhando &&
            movimento.sqrMagnitude > 0.01f)
        {
            Quaternion rotacao =
                Quaternion.LookRotation(movimento);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                rotacao,
                10f * Time.deltaTime
            );
        }
        if (animator != null)
        {
            bool andando =
                movimento.sqrMagnitude > 0.01f &&
                !trabalhando;

            animator.SetBool("Andando", andando);

            animator.SetBool(
                "ConsertandoMaquinas",
                consertando
            );

            animator.SetBool(
                "MexendoMaquinas",
                mexendo
            );
        }
    }
}
