
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;//Método Singleton

    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private ParticleSystem explosionParticle;
    //Particulas de poeira pes
    [SerializeField] private ParticleSystem particulaD;
    [SerializeField] private ParticleSystem particulaE;

    //Audio
    private AudioSource playerAudio;




    private Rigidbody playerRb;

    public float gravityModifier = 1f;
    public float jumpForce = 10f;
    public bool isOnGround = true;

    //animação
    private Animator playerAnim;

    //Game Over
    public bool gameOver = false;



    private void Awake()
    {
        // Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    { 
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    public void OnJump(InputValue value)
    {
        if(value.isPressed && isOnGround)
        {
            playerAudio.PlayOneShot(playerAudio.clip, 1.0f);

            //Parar particulas de poeira
            particulaD.Stop();
            particulaE.Stop();


            playerRb.AddForce(
                Vector3.up * jumpForce,ForceMode.Impulse);

            isOnGround = false;

            //animação
            playerAnim.SetTrigger("Jump_trig");
        }
    }


   private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground") && !gameOver)
        {
            //Ativar particulas de poeira
            particulaD.Play();
            particulaE.Play();


            isOnGround = true;
        }
        //Morte do jogador
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            //Parar particulas de poeira
            particulaD.Stop();
            particulaE.Stop();

            gameOver = true;
            gameOverText.gameObject.SetActive(true);
            explosionParticle.Play();
            Destroy(collision.gameObject);
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
        }
    }



    void FixedUpdate()
    {
        playerRb.AddForce(
            Vector3.down * (gravityModifier -1)
            * Physics.gravity.magnitude, ForceMode.Acceleration);
    
       
    }

}
