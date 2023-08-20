using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using System.Text;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    public float MovementSpeed = 0f;
    public float Jumpforce = 0f;
    float dirX = 0f;


    public float Falling = 0f;

    private bool Grounded = true;
    private bool Jumping = false;
    private bool DoubleJump = false;
    public float delayBeforeDoubleJump;




    private Vector2 velocity;
    //menu
    public GameObject menu;
    private bool isPlaying = true;

    //dem coin
    private int countCoin = 0;
    public TMP_Text txtCoin, txtLife, txtBullet;
    //fog
    public ParticleSystem fog;

    //ban dan
    private bool isRight = true;
    private int countBullet = 0;

    //mang
    private int Lifecount = 3;

    private enum MovementState { Idle, Running, Jumping, Falling }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        //load diem, load vi tri
        //if (LoginUSer.loginReponseModel.score >= 0)
        //{
        //    countCoin = LoginUSer.loginReponseModel.score;
        //    txtCoin.text = countCoin + " x";
        //}
        //if (LoginUSer.loginReponseModel.positionX != "")
        //{
        //    var posX = float.Parse(LoginUSer.loginReponseModel.positionX);
        //    var posY = float.Parse(LoginUSer.loginReponseModel.positionY);
        //    var posZ = float.Parse(LoginUSer.loginReponseModel.positionZ);
        //    transform.position = new Vector3(posX, posY, posZ);
        //}


    }

    // Update is called once per frame
    void Update()
    {
        txtBullet.SetText(countBullet.ToString());
        txtLife.SetText(Lifecount.ToString());

        Move();
        Jump();
        UpdateAnimationState();

        Pause();
        Bullet();
        ChangeMap();
       

        // Vector2 scale = transform.localScale;   change facing
    }
    //Di chuyen trai phai
    private void Move()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * MovementSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        if (Input.GetKey("space") && !Jumping)
        {
            rb.velocity = new Vector3(rb.velocity.x, Jumpforce);
            Jumping = true;
            //falling ket hop gravity
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (Falling - 1) * Time.deltaTime;
            }
        }

    }
    void EnableDoubleJump()
    {
        DoubleJump = true;
    }
    //getkey: Nhan giu nut
    //getKeydown: Nhan phim 1 lan
    //getKeyup: Nhan roi tha ra moi bat dau hoat dong
    //Animation
    private void UpdateAnimationState()
    {
        MovementState state;
        Quaternion rotation = fog.transform.localRotation; ///bui duoi chan

        if (dirX > 0f && Grounded)
        {
            state = MovementState.Running;
            sprite.flipX = false; // change facing
                                  //anim.SetBool("isGrounded", true);
                                  //scale.x = 1; change facing
            rotation.y = 180; //bui duoi chan

            state = MovementState.Running;

            isRight = true;
        }
        else if (dirX < 0f && Grounded)
        {
            state = MovementState.Running;
            sprite.flipX = true; // change facing
                                 //  anim.SetBool("isGrounded", true);
                                 //  anim.SetBool("isFalling", false);

            //scale.x = -1; change facing
            rotation.y = 0;
            isRight = false;
       
        }
        else
        {
            state = MovementState.Idle;
            //   anim.SetBool("isGrounded", true);
            // anim.SetBool("isFalling", false);

        }

        //check ground, chuyen trang thai jump sang falling
        if (Jumping)
        {
            if (rb.velocity.y > .1f && Grounded)
            {
                //anim.SetInteger("state", 2);
                state = MovementState.Jumping;
            }
            if (rb.velocity.y < 0)
            {

                state = MovementState.Falling;
            }
        }

        anim.SetInteger("state", (int)state);
        fog.transform.localRotation = rotation;
        fog.Play();
    }

    //ban dan
    public void Bullet()
    {

        if (Input.GetKeyDown(KeyCode.J) && countBullet >0)
        {
            countBullet--;
            var x = transform.position.x + (isRight ? 1f : -1f) ;
            var y = transform.position.y + (isRight ? 1f : -1f);
            var z = transform.position.z;
            GameObject gameObject = (GameObject)Instantiate(
                Resources.Load("Prefabs/bullet 1"),
                new Vector3(x, y, z),
                Quaternion.identity
                );
            gameObject.GetComponent<BulletScript>().setIsRight(isRight);


        }


    }

    //pause game & show menu
    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPlaying)
            {
                menu.SetActive(true);
                Time.timeScale = 0;
                isPlaying = false;
            }
            else
            {
                menu.SetActive(false);
                Time.timeScale = 1;
                isPlaying = true;
            }

        }
    }

    public void ShowMenu()
    {
        if (isPlaying)
        {
            menu.SetActive(true);
            Time.timeScale = 0;
            isPlaying = false;
        }
        else
        {
            menu.SetActive(false);
            Time.timeScale = 1;
            isPlaying = true;
        }
    }

    // cham vao xu se xoa xu, tang xu cho nhan vat
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "coin")
        {
            countCoin += 1;
            txtCoin.text = countCoin + " x";
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.tag == "CheckPoint")
        {
            SavePosittion();
        }
        if (collision.gameObject.tag == "Left_Enemy" )
        {
            //kill player
         
            
            Lifecount -= 1;
            if(Lifecount == 0)
            {
                SceneManager.LoadScene(1);
            }
           

        }
        if (collision.gameObject.tag == "Up_enemy")
        {
            //kill enemy
         
            var name = collision.attachedRigidbody.name;
            Destroy(GameObject.Find(name));
        }

        if(collision.gameObject.tag == "addBullet")
        {
            countBullet += 3;
            Destroy(collision.gameObject);
        }

        if(collision.gameObject.tag == "Next")
        {
            SceneManager.LoadScene(2);
        }
        }

    //ground check
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Grounded = true;
            Jumping = false;
            DoubleJump = false;
        }
        if(collision.gameObject.tag == "Brick" || collision.gameObject.tag == "Itembox") {
            Grounded = true;
            Jumping = false;
            DoubleJump = false;
        }
    }

    public void ChangeMap()
    {
        if (Input.GetKey(KeyCode.N))
        {
            
            SceneManager.LoadScene(2);
        }
    }
    public void SaveScore()
    {
        var user = LoginUSer.loginReponseModel.username;
        ScoreModel score = new ScoreModel(user, countCoin);
        StartCoroutine(SaveScoreAPI(score));
        SaveScoreAPI(score);
    }
    //Save score API
    IEnumerator SaveScoreAPI(ScoreModel score)
    {
        //…
        string jsonStringRequest = JsonConvert.SerializeObject(score);

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/save-score", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonStringRequest);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            var jsonString = request.downloadHandler.text.ToString();
            ReponseModel scoreReponseModel = JsonConvert.DeserializeObject<ReponseModel>(jsonString);
            if(scoreReponseModel.status == 1)
            {
                ShowMenu();
            }
            else
            {
                //thong bao
            }
        }
        request.Dispose();
    }

    public void SavePosittion()
    {
        var user = LoginUSer.loginReponseModel.username;
        var x = transform.position.x;
        var y = transform.position.y;
        var z = transform.position.z;
        PositionModel positionModel = new PositionModel(user, x.ToString(), y.ToString(), z.ToString());

        StartCoroutine(SavePositionAPI(positionModel));
        SavePositionAPI(positionModel);
    }
    //Save Posittion API
    IEnumerator SavePositionAPI(PositionModel posittionModel)
    {
        //…
        string jsonStringRequest = JsonConvert.SerializeObject(posittionModel);

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/save-position", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonStringRequest);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            var jsonString = request.downloadHandler.text.ToString();
            ReponseModel positionReponseModel = JsonConvert.DeserializeObject<ReponseModel>(jsonString);
            if (positionReponseModel.status == 1)
            {
                //done
                Debug.Log("checkPosittion - done");
            }
            else
            {
                //goi lai API luu posittion
                Debug.Log("checkPosittion - " + positionReponseModel.status); ;

            }
        }
        request.Dispose();
    }

    //Check life
   

}
