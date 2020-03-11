using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 0;
    public float Speed
    {
        get { return speed; }
        set
        {
            if (value > 0.5)
                speed = value;
        }
    }
    [SerializeField] private float force;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private GroundDetection groundDetection;
    [SerializeField] private float nextFire = 3;
    public float NextFire
    {
        get { return nextFire; }
    }
    [SerializeField] private bool isReadyFire = true;
    [SerializeField] private bool isCheatMode;
    [SerializeField] private float minimalHeight = -10;
    public float minHeight
    {
        get { return minimalHeight; }
        set
        {
            if (value < 0 && value > -12)
                speed = value;
        }
    }
    [SerializeField] private Arrow arrow;
    [SerializeField] private Transform arrowSpawnPoint;
    [SerializeField] private float shootForce;
    [SerializeField] private float arrowCount;
    [SerializeField] private Health health;
    [SerializeField] private float forceBonus;
    [SerializeField] private int armorBonus;
    [SerializeField] private int damageBonus;

    public Health Health {  get { return health; } }
    private Arrow curArrow;
    private List<Arrow> arrowsPool;
    private Vector3 direction;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    [SerializeField] private BuffReciever buffReciever;
    private bool isJumping;
    private UICharacterController controller;
    [SerializeField] private ReloadFire reloadFire;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        arrowsPool = new List<Arrow>();
        for (int i = 0; i < arrowCount; i++)
        {
            var arrowTemp = Instantiate(arrow, arrowSpawnPoint);
            arrowsPool.Add(arrowTemp);
            arrowTemp.gameObject.SetActive(false);
        }
        buffReciever.OnBuffsChanged += BuffHendler; 
    }

    #region Singleton
    public static Player Instance { get; set; }
    #endregion
    public void InitUIController(UICharacterController uiController)
    {
        controller = uiController;
        controller.Jump.onClick.AddListener(Jump);
        controller.Fire.onClick.AddListener(CheckShoot);
    }

    private void BuffHendler(Buff buff)
    {
        if (buff.type == BuffType.Force)
            forceBonus = buff.additiveBonus;
        if (buff.type == BuffType.Damage)
            damageBonus = (int)buff.additiveBonus;
        if (buff.type == BuffType.Armor)
        {
            armorBonus = (int)buff.additiveBonus;
            health.SetHealth(armorBonus);
        }
    }
    void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
#endif
    }

    private void Move()
    {
        animator.SetBool("isGrounded", groundDetection.isGrounded);
        if (!isJumping && !groundDetection.isGrounded)
            animator.SetTrigger("StartFall");
        isJumping = isJumping && !groundDetection.isGrounded;
        direction = Vector3.zero;
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.A))
            direction = Vector3.left;
        if (Input.GetKey(KeyCode.D))
            direction = Vector3.right;
#endif
        if (controller.Left.IsPressed)
            direction = Vector3.left;
        if (controller.Right.IsPressed)
            direction = Vector3.right;
        direction *= speed;
        direction.y = rigidbody.velocity.y;
        rigidbody.velocity = direction;


        if (direction.x > 0)
            spriteRenderer.flipX = false;
        if (direction.x < 0)
            spriteRenderer.flipX = true;

        animator.SetFloat("speed", Mathf.Abs(direction.x));
        CheckFall();

    }
    private void Jump()
    {
        if (groundDetection.isGrounded)
        {
            rigidbody.AddForce(Vector2.up * (force + forceBonus), ForceMode2D.Impulse);
            animator.SetTrigger("StartJump");
            isJumping = true;
        }
    }

    void CheckShoot()
    {

        if (isReadyFire)
        {
            animator.SetTrigger("attack");
        } 
    }

    public void InitArrow()
    {
        curArrow = GetArrowFromPool();
        curArrow.SetImpulse(Vector2.right, 0, 0, this);
    }

    private void Attack()
    {
        curArrow.SetImpulse
            (Vector2.right, spriteRenderer.flipX ? force * -shootForce : force * shootForce, damageBonus, this);

        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        reloadFire.ReloadTime();
        isReadyFire = false;
        yield return new WaitForSeconds(nextFire);
        isReadyFire = true;
    }

    private Arrow GetArrowFromPool()
    {
        if (arrowsPool.Count > 0)
        {
            var arrowTemp = arrowsPool[0];
            arrowsPool.Remove(arrowTemp);
            arrowTemp.gameObject.SetActive(true);
            arrowTemp.transform.parent = null;
            arrowTemp.transform.position = arrowSpawnPoint.position;
            return arrowTemp;
        }
        return Instantiate(arrow, arrowSpawnPoint.position, Quaternion.identity);
    }

    public void ReturnArrowToPool(Arrow arrowTemp)
    {
        if (!arrowsPool.Contains(arrowTemp))
            arrowsPool.Add(arrowTemp);
        arrowTemp.transform.parent = arrowSpawnPoint;
        arrowTemp.transform.position = arrowSpawnPoint.transform.position;
        arrowTemp.gameObject.SetActive(false);
    }
    void CheckFall()
    {
        if (transform.position.y < minimalHeight && isCheatMode)
        {
            rigidbody.velocity = new Vector2(0, 0);
            transform.position = new Vector2(0, 0);
        }
        else if (transform.position.y < minimalHeight && !isCheatMode)
            Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (GameManager.Instance.coinContainer.ContainsKey(col.gameObject))
        {
            PlayerInventory.Instance.coinsCount++;
            PlayerInventory.Instance.coinsText.text = PlayerInventory.Instance.coinsCount.ToString();
            var coin = GameManager.Instance.coinContainer[col.gameObject];
            coin.StartDestroy();
        }

        if (GameManager.Instance.itemsContainer.ContainsKey(col.gameObject))
        {
            var itemComponent = GameManager.Instance.itemsContainer[col.gameObject];
            PlayerInventory.Instance.Items.Add(itemComponent.Item);
            itemComponent.Destroy(col.gameObject);
        }
    }

}

