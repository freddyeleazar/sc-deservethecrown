using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerMovementController : MonoBehaviour
{
    #region OldCode
    /*
    public float runningSpeed;//Velocidad de trote
    public float sprintSpeed;//Velocidad de carrera
    private float maxSpeed;//Velocidad que será asignada RigidBody, puede ser la de trote o carrera
    public float jumpForce;//Magnitud de la fuerza vertical aplicado al player cuando presione la barra espaciadora
    public float pushBackForce;//Magnitud de la fuerza con que sale impelido el Player, si está desprotegido, al contacto con un enemigo

    [Header(header: "States")]
    public bool isGrounded;//El player está en el piso. Se asigna en el evento OnCollisionEnter y OnCollisionExit toda vez que el layer del objeto colisionado sea "Ground"
    public bool isJumping;//El player está saltando (moviéndose ascendentemente en el eje y). Se asigna mediante corrutina SetJumpStates que comprueba variaciones en el eje y
    public bool isFalling;//El plater está cayendo (moviéndose descendentemente en el eje y). Se asigna mediante corrutina SetJumpStates que comprueba variaciones en el eje y
    public bool isStopped;//El jugador esta impedido de moverse. Se asigna true arbitrariamente cuando colisiona desprotegidamente con un enemigo

    private Rigidbody rb;//Almacena el RigidBody del Player. Se asigna en Start
    private Animator animator;//Animator del Player. Se asigna en Start
    private float xVelocity;//Velocidad con que se ordena al Player desplazarse en el eje x. Corresponde al Input.GetAxis("Horizontal") normalizado (para evitar que los desplazamientos diagonales sean desproporcionadamente rápidos).
    private float zVelocity;//Velocidad con que se ordena al Player desplazarse en el eje y. Corresponde al Input.GetAxis("Vertical") normalizado (para evitar que los desplazamientos diagonales sean desproporcionadamente rápidos).

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        isStopped = false;
    }

    private void Update()//En cada frame...
    {
        animator.SetBool("IsJumping", isJumping);//Se actualizan los valores de los parámetros del animator del Player, para que este reproduzca las animaciones pertinentes
        animator.SetBool("IsFalling", isFalling);//Ídem

        if (!isStopped)//Si el jugador no está impedido de moverse por haber perdido la partida
        {
            SetMaxSpeed(KeyCode.LeftShift);//Actualizar la velocidad de desplazamiento
        }

        if (!isStopped)//Si el jugador no está impedido de moverse por haber perdido la partida
        {
            //Obtener los valores normalizados de los axis horizontal y vertical (grado en que están siendo presionadas las teclas (o los comandos análogos) por defecto para moverse lateral y verticalmente
            Vector3 normalizedVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;//Los valores no pueden ser absolutos, debido a que los movimientos diagonales serían desproporcionados en relación con los horizontales y verticales            
            xVelocity = normalizedVelocity.x;//Asignar el valor normalizado del axis horizontal a la variable que almacena la velocidad de desplazamiento horizontal 
            zVelocity = normalizedVelocity.z;//Asignar el valor normalizado del axis horizontal a la variable que almacena la velocidad de desplazamiento horizontal
        }

        //Jump
        if (!isStopped)//Si el jugador no está impedido de moverse por haber perdido la partida
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)//Si el jugador presionó la barra espaciadora y está en el suelo (no está ya saltando)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);//Impulsarlo hacia arriba con la fuerza especificada por el valor de la variable jumpForce
            }
        }
        StartCoroutine(SetJumpStates());//Iniciar la corrutina que detecta en que momento del salto se encuentra el player (elevándose o cayendo) y asigna las variables de "States" (líneas 17 a la 20)
    }

    private void FixedUpdate()//En cada momento de actualización de la física de Unity...
    {
        //Rotate
        if (!isStopped)//Si el jugador no está impedido de moverse por haber perdido la partida
        {
            Vector3 newForward = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));//Crear una variable que almacene la dirección en la que se desplaza el player, la cual está determinada por los teclas de movimiento horizontal y vertical
            if (newForward != Vector3.zero)//Si el player efectivamente se dirige a alguna parte (es decir, si se están apretando las teclas de movimiento horizontal y vertical...
            {
                //Hacer que el player "mire" (gire) hacia la dirección en que se desplaza.
                transform.forward = newForward;//Técnicamentesi, se mueve a la derecha, entonces la derecha es su "adelante" y, por consiguiente, su rotación se ajusta a este nuevo "adelante" (forward)
            }
        }

        //Move
        if (!isStopped)//Si el jugador no está impedido de moverse por haber perdido la partida
        {
            rb.velocity = new Vector3(xVelocity * maxSpeed, rb.velocity.y, zVelocity * maxSpeed);//La velocidad de movimiento del jugador será igual a los valores normalizados obtenidos de los axis (teclas o joystick) de movimiento horizontal y vertical
            //Actualizar el valor del parámetro "Speed" del animator del Player, asignándole la magnitud física de la velocidad del RigidBody definida en la línea anterior
            animator.SetFloat("Speed", rb.velocity.magnitude / sprintSpeed);//La "magnitud" de la velocidad en un sentido físico es la sumatoria de las fuerzas que configuran el movimiento de un objeto (cuan rápido va hacia arriba o abajo, adelante o atrás y a los lados)
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == 8)//Si es piso (ground)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.layer == 8)//Si es piso (ground)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.layer == 8)//Piso
        {
            isGrounded = false;
        }
    }

    private void SetMaxSpeed(KeyCode keyCode)
    {
        if (Input.GetKey(keyCode))
        {
            maxSpeed = sprintSpeed;
        }
        else
        {
            maxSpeed = runningSpeed;
        }
    }

    private IEnumerator SetJumpStates()
    {
        while (true)
        {
            float previoudPosition = transform.position.y;
            yield return new WaitUntil(() => previoudPosition != transform.position.y);
            if (!isGrounded)
            {
                if (transform.position.y > previoudPosition + 0.05f)
                {
                    isJumping = true;
                    isFalling = false;
                }
                else if (transform.position.y < previoudPosition - 0.05f)
                {
                    isJumping = false;
                    isFalling = true;
                }
            }
            else
            {
                isJumping = false;
                isFalling = false;
            }
        }
    }
    */
    #endregion

    public void Initialize(GameObject character)
    {
        m_animator = character.GetComponent<Animator>();
        m_rigidBody = character.GetComponent<Rigidbody>();
    }

    private enum ControlMode
    {
        /// <summary>
        /// Up moves the character forward, left and right turn the character gradually and down moves the character backwards
        /// </summary>
        Tank,
        /// <summary>
        /// Character freely moves in the chosen direction from the perspective of the camera
        /// </summary>
        Direct
    }

    [SerializeField] private float m_moveSpeed = 2;
    [SerializeField] private float m_turnSpeed = 200;
    [SerializeField] private float m_jumpForce = 4;

    [SerializeField] private Animator m_animator;
    [SerializeField] private Rigidbody m_rigidBody;

    [SerializeField] private ControlMode m_controlMode = ControlMode.Direct;

    private float m_currentV = 0;
    private float m_currentH = 0;

    private readonly float m_interpolation = 10;
    private readonly float m_walkScale = 0.33f;
    private readonly float m_backwardsWalkScale = 0.16f;
    private readonly float m_backwardRunScale = 0.66f;

    private bool m_wasGrounded;
    private Vector3 m_currentDirection = Vector3.zero;

    private float m_jumpTimeStamp = 0;
    private float m_minJumpInterval = 0.25f;

    private bool m_isGrounded;

    private List<Collider> m_collisions = new List<Collider>();


    public float pushBackForce;//Magnitud de la fuerza con que sale impelido el Player, si está desprotegido, al contacto con un enemigo
    public bool isStopped;//El jugador esta impedido de moverse. Se asigna true arbitrariamente cuando colisiona desprotegidamente con un enemigo

    void Awake()
    {
        if (!m_animator) { gameObject.GetComponent<Animator>(); }
        if (!m_rigidBody) { gameObject.GetComponent<Animator>(); }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
                m_isGrounded = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            m_isGrounded = true;
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        }
        else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { m_isGrounded = false; }
    }

    void FixedUpdate()
    {
        if (!isStopped)
        {
            m_animator.SetBool("Grounded", m_isGrounded);

            switch (m_controlMode)
            {
                case ControlMode.Direct:
                    DirectUpdate();
                    break;

                case ControlMode.Tank:
                    TankUpdate();
                    break;

                default:
                    Debug.LogError("Unsupported state");
                    break;
            }

            m_wasGrounded = m_isGrounded;
        }
    }

    private void TankUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        bool walk = Input.GetKey(KeyCode.LeftShift);

        if (v < 0)
        {
            if (walk) { v *= m_backwardsWalkScale; }
            else { v *= m_backwardRunScale; }
        }
        else if (walk)
        {
            v *= m_walkScale;
        }

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
        transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);

        m_animator.SetFloat("MoveSpeed", m_currentV);

        JumpingAndLanding();
    }

    private void DirectUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Transform camera = Camera.main.transform;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            v *= m_walkScale;
            h *= m_walkScale;
        }

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

        float directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * directionLength;

        if (direction != Vector3.zero)
        {
            m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

            transform.rotation = Quaternion.LookRotation(m_currentDirection);
            transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

            m_animator.SetFloat("MoveSpeed", direction.magnitude);
        }

        JumpingAndLanding();
    }

    private void JumpingAndLanding()
    {
        bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

        if (jumpCooldownOver && m_isGrounded && Input.GetKey(KeyCode.Space))
        {
            m_jumpTimeStamp = Time.time;
            m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }

        if (!m_wasGrounded && m_isGrounded)
        {
            m_animator.SetTrigger("Land");
        }

        if (!m_isGrounded && m_wasGrounded)
        {
            m_animator.SetTrigger("Jump");
        }
    }

    public void PushBack(Transform other)
    {
        m_rigidBody.AddForce((other.forward + Vector3.up) * pushBackForce);
    }

    public void Stop()
    {
        //sprintSpeed = 0;
        //runningSpeed = 0;
        //maxSpeed = 0;
        isStopped = true;
    }
}