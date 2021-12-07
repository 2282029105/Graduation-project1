using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("补偿速度")]
    public float lightSpeed;
    public float heavySpeed;
    [Header("打击感")]
    public float shakeTime;
    public int lightPause;
    public float lightStrength;
    public int heavyPause;
    public float heavyStrength;

    [Header("冲刺参数")]
    public float dashTime;//冲刺时间
    private float dashTimeLeft;//冲刺剩余时间
    private float lastDash = -10f;//上一次冲刺的时间点
    public float dashCoolDown;//冲刺的CD
    public float dashSpeed;//冲刺的强度


    [Space]
    public float interval = 2f;//设置允许连续攻击的时间
    private float timer;//连续攻击计时
    private bool isAttack;//判断是否进入攻击状态
    private string attackType;//判断攻击类型
    private int comboStep;//设置函数用以攻击计数


    public float moveSpeed;//玩家移动速度
    public bool isDashing;//判断冲刺
    public float jumpForce;//玩家跳跃高度
    new private Rigidbody2D rigidbody;
    private Animator animator;

    private BoxCollider2D playerFeet;
    private float input;
    private bool isGround;
    [SerializeField] private LayerMask layer;

    [SerializeField] private Vector3 check;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Dash();
        if (isDashing)
            animator.SetBool("Dash", isDashing);
            return;

    }

    void Update()
    {
        input = Input.GetAxisRaw("Horizontal");
        isGround = Physics2D.OverlapCircle(transform.position + new Vector3(check.x, check.y, 0), check.z, layer);

        animator.SetFloat("Horizontal", rigidbody.velocity.x);//水平
        animator.SetFloat("Vertical", rigidbody.velocity.y);//竖直
        animator.SetBool("isGround", isGround);//地面

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Time.time >= (lastDash + dashCoolDown))
            {
                //可以执行dash
                ReadToDash();
            }
        }

        Jump();
        Move();
        Attack();
        //Run();
        SwitchAnimation();
        //CheckGround(); 
    }

    //void Run()
    //{
    //    float moveDir = Input.GetAxis("Horizontal");//判断正轴
    //    Vector2 playerVel = new Vector2(moveDir * moveSpeed, rigidbody.velocity.y);//移动方向*移动速度
    //    rigidbody.velocity = playerVel;//玩家速度
    //    bool playerHasXAxisSpeed = Mathf.Abs(rigidbody.velocity.x) > Mathf.Epsilon;
    //    animator.SetBool("Run", playerHasXAxisSpeed);//如果在x轴上有速度的话就会调动Run动画
    //}
    #region 移动和攻击移动补偿
    void Move()
    {
        if (!isAttack)
            rigidbody.velocity = new Vector2(input * moveSpeed, rigidbody.velocity.y);
        else
        {
            if (attackType == "Light")//按照相应的补偿速度相面朝向移动
                rigidbody.velocity = new Vector2(transform.localScale.x * lightSpeed, rigidbody.velocity.y);
            else if (attackType == "Heavy")
                rigidbody.velocity = new Vector2(transform.localScale.x * heavySpeed, rigidbody.velocity.y);//攻击向前方进行小距离移动
        }

        if (rigidbody.velocity.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (rigidbody.velocity.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
    }
    #endregion

    #region 跳跃
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGround)//触发跳跃，判断若为地面则进行一次跳跃
        {
            rigidbody.velocity = new Vector2(0, jumpForce);
            animator.SetTrigger("Jump");
        }
        //if (Input.GetButtonDown("Jump"))
        //{
        //    if (isGround)//判断是否为地面，若为地面则只可以跳一次
        //    {
        //        animator.SetBool("Jump", true);
        //        Vector2 jumpVel = new Vector2(0.0f, jumpForce);
        //        rigidbody.velocity = Vector2.up * jumpVel;
        //    }
        //}
    }
    #endregion

    #region 动画切换
    void SwitchAnimation()
    {
        animator.SetBool("Idle", false);
        if (animator.GetBool("Jump"))
        {
            if (rigidbody.velocity.y < 0.0f)
            {
                animator.SetBool("Jump", false);
                animator.SetBool("Fall", true);//判断玩家是否到达最高点，若到达最高点，切换动画
            }
        }
        else if (isGround)
        {
            animator.SetBool("Fall", false);
            animator.SetBool("Land", true);//落到地面时切换动画
        }
        else
        {
            animator.SetBool("Idle", true);
        }
    }
    #endregion

    //void CheckGround()
    //{
    //    isGround = playerFeet.IsTouchingLayers(LayerMask.GetMask("Ground"));
    //    Debug.Log(isGround);
    //}

    #region 攻击设置
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.J) && !isAttack)//轻击
        {
            isAttack = true;
            attackType = "Light";
            comboStep++;
            if (comboStep > 3)
                comboStep = 1;//当combo超过最大连击数重置为1，目前攻击最大连击数设置为3
            timer = interval;
            animator.SetTrigger("LightAttack");
            animator.SetInteger("ComboStep", comboStep);
        }
        if (Input.GetKeyDown(KeyCode.K) && !isAttack)//重击
        {
            isAttack = true;
            attackType = "Heavy";
            comboStep++;
            if (comboStep > 3)
                comboStep = 1;
            timer = interval;
            animator.SetTrigger("HeavyAttack");
            animator.SetInteger("ComboStep", comboStep);
        }

        //判断是否开始计数，若timer值不为0则持续减去当前帧时间，当timer归0时重置计时器和计数器
        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                comboStep = 0;
            }
        }
    }
    #endregion

    #region 关闭攻击以及判断攻击情况
    //结束攻击，在动画轴中创建帧事件（AttackOver）处理攻击的时间用来执行帧事件指定的函数，即结束攻击
    public void AttackOver()
    {
        isAttack = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))//判断接触的物体
        {
            if (attackType == "Light")
            {
                AttackSense.Instance.HitPause(lightPause);
                AttackSense.Instance.CameraShake(shakeTime, lightStrength);
            }
            else if (attackType == "Heavy")
            {
                AttackSense.Instance.HitPause(heavyPause);
                AttackSense.Instance.CameraShake(shakeTime, heavyStrength);
            }

            if (transform.localScale.x > 0)
                other.GetComponent<Enemy>().GetHit(Vector2.right);
            else if (transform.localScale.x < 0)
                other.GetComponent<Enemy>().GetHit(Vector2.left);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + new Vector3(check.x, check.y, 0), check.z);
    }
    #endregion

    void ReadToDash()
    {

        isDashing = true;

        dashTimeLeft = dashTime;

        lastDash = Time.time;
    }

    void Dash()
    {


        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                rigidbody.velocity = new Vector2(gameObject.transform.localScale.x * dashSpeed, rigidbody.velocity.y);

                dashTimeLeft -= Time.deltaTime;

                ShadowPool.instance.GetfromPool();


            }//判断冲刺剩余时间，若还有剩余时间还可以继续冲刺
        }

    }
    //void Dash()
    //{
    //    if (isDashing)
    //    {
    //        if (dashTimeLeft > 0)
    //        {

    //            if (rigidbody.velocity.y > 0 && !isGround)
    //            {
    //                rigidbody.velocity = new Vector2(dashSpeed * gameObject.transform.localScale.x, jumpForce);//在空中Dash向上
    //            }
    //            rigidbody.velocity = new Vector2(dashSpeed * gameObject.transform.localScale.x, rigidbody.velocity.y);//地面Dash

    //            dashTimeLeft -= Time.deltaTime;

    //            ShadowPool.instance.GetfromPool();
    //        }
    //        if (dashTimeLeft <= 0)
    //        {
    //            isDashing = false;
    //            if (!isGround)
    //            {
    //                //目的为了在空中结束 Dash 的时候可以接一个小跳跃
    //                rigidbody.velocity = new Vector2(dashSpeed * gameObject.transform.localScale.x, jumpForce);
    //            }
    //        }
    //    }

    //}

}