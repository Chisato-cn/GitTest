using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase<TCharacter> : MonoBehaviour where TCharacter : CharacterBase<TCharacter>
{
    /// <summary>
    /// 斜坡最大倾斜角度
    /// </summary>
    protected readonly float slopingGroundAngle = 20f;
    
    protected readonly float groundOffset = 0.1f;
    
    /// <summary>
    /// 角色控制器
    /// </summary>
    public CharacterController Controller { get; protected set; }
    
    /// <summary>
    /// 角色状态机
    /// </summary>
    public CharacterStateMachine<TCharacter> StateMachine { get; protected set; }

    /// <summary>
    /// 角色配置
    /// </summary>
    public CharacterStats<TCharacter> Stats { get; protected set; }

    public RaycastHit groundHit;
    
    /// <summary>
    /// 地面法线
    /// </summary>
    public Vector3 GroundNormal { get; protected set; }
    
    /// <summary>
    /// 地面倾斜角度
    /// </summary>
    public float GroundAngle { get; protected set; }
    
    /// <summary>
    /// 是否处于地面
    /// </summary>
    public bool IsGrounded { get; private set; } = true;
    
    /// <summary>
    /// 返回当前位置（即当前地面）上的局部坡度方向
    /// </summary>
    public Vector3 LocalSlopeDirection { get; protected set; }

    /// <summary>
    /// 最后处于地面的时间戳
    /// </summary>
    public float LastGroundTime { get; protected set; }
    
    /// <summary>
    /// 旋转阻尼系数
    /// </summary>
    public float TurningDragMultiplier { get; set; } = 1f;
    
    /// <summary>
    /// 最大速率系数
    /// </summary>
    public float MaxSpeedMultiplier { get; set; } = 1f;
    
    /// <summary>
    /// 加速系数
    /// </summary>
    public float AccelerationMultiplier { get; set; } = 1f;

    /// <summary>
    /// 减速系数
    /// </summary>
    public float DecelerationMultiplier { get; set; } = 1f;

    /// <summary>
    /// 重力加速度系数
    /// </summary>
    public float GravityMultiplier { get; set; } = 1f;
    
    /// <summary>
    /// 角色碰撞盒-高度
    /// </summary>
    public float Height => Controller.height;

    /// <summary>
    /// 角色碰撞盒-半径
    /// </summary>
    public float radius => Controller.radius;
    
    
    public Vector3 Center => Controller.center;

    /// <summary>
    /// 角色碰撞盒-中心位置
    /// </summary>
    public Vector3 Position => transform.position + Center;
    
    /// <summary>
    /// 角色碰撞盒-脚底位置
    /// </summary>
    public Vector3 StepPosition => Position - transform.up * (Height * 0.5f - Controller.stepOffset);
    
    /// <summary>
    /// 总速度向量
    /// </summary>
    public Vector3 Velocity { get; set; }
    
    /// <summary>
    /// 水平速度向量
    /// </summary>
    public Vector3 LateralVelocity
    {
        get => new Vector3(Velocity.x, 0, VerticalVelocity.z); 
        set => Velocity = new Vector3(value.x, Velocity.y, value.z);
    }
    
    /// <summary>
    /// 垂直速度向量
    /// </summary>
    public Vector3 VerticalVelocity
    {
        get => new Vector3(0, Velocity.y, 0); 
        set => Velocity = new Vector3(Velocity.x, value.y, Velocity.z);
    }
    
    protected virtual void Awake()
    {
        InitController();
        InitStateMachine();
    }


    protected virtual void Update()
    {
        if (Controller.enabled)
        {
            HandleGround();
            HandleController();
            HandleStateMachine();
            OnUpdate();
        }
    }
    
    protected virtual void OnUpdate() { }

    protected virtual void LateUpdate()
    {
        
    }
    
    public virtual void InitController()
    {
        Controller = GetComponent<CharacterController>();
        if (Controller == null) Controller = gameObject.AddComponent<CharacterController>();

        Controller.skinWidth = 0.005f;
        Controller.minMoveDistance = 0;
    }
    
    public virtual void HandleController()
    {
        if (Controller.enabled) Controller.Move(Velocity * Time.deltaTime);
        else transform.position += Velocity * Time.deltaTime;
    }

    public abstract void InitStateMachine();

    public virtual void HandleStateMachine()
    {
        StateMachine.Update();
    }

    public virtual void HandleGround()
    {
        float distance = Height * 0.5f + groundOffset;

        // 成功检测到地面
        if (SphereCast(Vector3.down, distance, out RaycastHit hit) && VerticalVelocity.y <= 0f)
        {
            // 当前不处于地面状态
            if (!IsGrounded)
            {
                // 着陆判断
                if (EvaluateLanding(hit))
                {
                    EnterGround(hit);
                }
                else
                {
                    HandleHighLedge(hit);
                }
            }
            // 处于地面
            else if (IsPointUnderStep(hit.point))
            {
                UpdateGround(hit);

                // 斜坡倾角判断
                if (Vector3.Angle(hit.normal, Vector3.up) >= Controller.slopeLimit)
                {
                    HandleSlopeLimit(hit);
                }
            }
            // 边缘滑出去
            else
            {
                HandleHighLedge(hit);
            }
        }
        // 无法检测到地面
        else
        {
            ExitGround();
        }
    }

    /// <summary>
    /// 斜坡速度处理
    /// </summary>
    protected virtual void HandleSlopeLimit(RaycastHit hit)
    {
        Vector3 slopeDirection = Vector3.Cross(hit.normal, Vector3.Cross(hit.normal, Vector3.up));
        slopeDirection = slopeDirection.normalized;
        Controller.Move(slopeDirection * Stats.slideForce * Time.deltaTime);
    }

    /// <summary>
    /// 边缘速度处理
    /// </summary>
    protected virtual void HandleHighLedge(RaycastHit hit)
    {
        // 边缘给予固定方向速度,不使用hit.normal,手动计算normal
        Vector3 edgeNormal = hit.point - Position;
        Vector3 edgePushDirection = Vector3.Cross(edgeNormal, Vector3.Cross(edgeNormal, Vector3.up));
        Controller.Move(edgePushDirection * Stats.gravity * Time.deltaTime);
    }
    
    protected virtual void EnterGround(RaycastHit hit)
    {
        if (!IsGrounded)
        {
            groundHit = hit;
            IsGrounded = true;
        }
    }

    protected virtual void UpdateGround(RaycastHit hit)
    {
        if (IsGrounded)
        {
            groundHit = hit;
            GroundNormal = groundHit.normal;
            GroundAngle = Vector3.Angle(Vector3.up, groundHit.normal);
            LocalSlopeDirection = new Vector3(GroundNormal.x, 0, GroundNormal.z).normalized;
            // hit.collider.CompareTag(GameTags.Platform) ? hit.transform : 
            transform.parent = null;
        }
    }
    
    protected virtual void ExitGround()
    {
        if (IsGrounded)
        {
            IsGrounded = false;
            // 先置空,具体再分配
            transform.parent = null;
            LastGroundTime = Time.time;
            VerticalVelocity = Vector3.Max(VerticalVelocity, Vector3.zero);
        }
    }
    
    /// <summary>
    /// 计算是否与地面接触, 同时计算是否不处于斜坡,朝脚底发射射线
    /// </summary>
    protected virtual bool EvaluateLanding(RaycastHit hit)
    {
        return IsPointUnderStep(hit.point) && Vector3.Angle(hit.normal, Vector3.up) < Controller.slopeLimit;
    }
    
    public virtual bool IsPointUnderStep(Vector3 point)
    {
        return StepPosition.y > point.y;
    }

    /// <summary>
    /// 球型物理射线检测
    /// </summary>
    /// <param name="direction">方向向量</param>
    /// <param name="distance">射线长度</param>
    public virtual bool SphereCast(Vector3 direction, float distance, out RaycastHit hit, int layer = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
    {
        // 减去起点到半径的范围, 因为SphereCast是检测不到这一段距离的
        float castDistance = Mathf.Abs(distance - radius);
        return Physics.SphereCast(Position, radius, direction, out hit, castDistance, layer, queryTriggerInteraction);
    }

    /// <summary>
    /// 平滑加速
    /// </summary>
    /// <param name="direction">目标方向向量</param>
    /// <param name="turnDrag">旋转阻尼</param>
    /// <param name="acceleration">加速度</param>
    /// <param name="maxSpeed">最大速度</param>
    public virtual void Accelerate(Vector3 direction, float turnDrag, float acceleration, float maxSpeed)
    {
        // sqrMagnitude ? magnitude 选择
        // 使用平方,性能上比开方要好;平方放大差异减少精度误差
        if (direction.sqrMagnitude > 0)
        {
            // 点积:结果是一个标量（即一个没有方向的数值）,它的大小表示了两个向量在它们共同方向上的"重叠"程度,此处代表 lateralVelocity 在 direction 方向上的"速度"分量大小
            // dot product = direction.x × lateralVelocity.x + direction.y × lateralVelocity.y + direction.z × lateralVelocity.z
            float speed = Vector3.Dot(direction, LateralVelocity);
            
            // 目标最大速度,Multiplier用于动态调节最大速率（即buff增益等）
            float targetMaxSpeed = maxSpeed * MaxSpeedMultiplier;

            // speed < 0 : 反向移动;
            // LateralVelocity.magnitude < targetMaxSpeed : 速度未达到最大速度
            // 速率均继续增加
            if (speed < 0 || LateralVelocity.magnitude < targetMaxSpeed)
            {
                speed += acceleration * AccelerationMultiplier * Time.deltaTime;
                speed = Mathf.Clamp(speed, -targetMaxSpeed, targetMaxSpeed);
            }
            
            // 实际 lateralVelocity 在 direction 方向上的大小
            Vector3 velocity = direction * speed;
            // 原:两个向量之间的向量差
            Vector3 turnVelocity = LateralVelocity - velocity;
            // 每帧的旋转量: 旋转阻尼大小 * 阻尼系数 * 帧时间
            float turnDelta = turnDrag * TurningDragMultiplier * Time.deltaTime;

            // 实际 lateralVelocity 在 direction 方向上, 速度分量进行了增加后, 的大小
            velocity = direction * speed;
            // 现:两个向量之间的向量差（每帧按旋转量进行了向量差的缩小）
            turnVelocity = Vector3.MoveTowards(turnVelocity, Vector3.zero, turnDelta);
            
            // 最终水平速度向量: 速度向量 + 向量差
            LateralVelocity = velocity + turnVelocity;
        }
    }
    
    /// <summary>
    /// 平滑摩擦
    /// </summary>
    public virtual void Friction()
    {
        // 斜坡摩擦
        if (OnSlopingGround()) Decelerate(Stats.slopeFriction);
        // 一般摩擦
        else Decelerate(Stats.friction);
    }
    
    /// <summary>
    /// 急停减速
    /// </summary>
    public virtual void Decelerate()
    {
        Decelerate(Stats.deceleration);
    }

    /// <summary>
    /// 平滑减速至0
    /// </summary>
    /// <param name="deceleration">减速速率</param>
    public virtual void Decelerate(float deceleration)
    {
        float delta = deceleration * DecelerationMultiplier * Time.deltaTime;
        LateralVelocity = Vector3.MoveTowards(LateralVelocity, Vector3.zero, delta);
    }
    
    /// <summary>
    /// 斜坡检测
    /// </summary>
    public virtual bool OnSlopingGround()
    {
        // groundAngle是外部用sphere或者capsule射线打到的物体和Vector3.up计算的出的, 并不处于角色正下方,有可能边缘碰撞体碰到
        if (IsGrounded && GroundAngle > slopingGroundAngle)
        {
            // 用两倍高的长度 作为 射线长度,用于最大限度保证打中斜坡
            // 此时往正下方射线, 计算击中的物体和transform.up的夹角判断处于斜坡上
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, Height * 2f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                return Vector3.Angle(hit.normal, Vector3.up) > slopingGroundAngle;
            }
            // 没击中（例如:斜坡底部,悬崖边缘）,依旧处于斜坡
            else
            {
                return true;
            }
        }

        return false;
    }
    
    /// <summary>
    /// 平滑重力（平地/下落处理）
    /// </summary>
    public virtual void Gravity()
    {
        if (!IsGrounded && VerticalVelocity.y > -Stats.gravityMaxSpeed)
        {
            float speed = VerticalVelocity.y;
            float force = VerticalVelocity.y > 0 ? Stats.gravity : Stats.fallGravity;
            speed -= force * GravityMultiplier * Time.deltaTime;
            speed = Mathf.Max(speed, -Stats.gravityMaxSpeed);
            VerticalVelocity = new Vector3(0, speed, 0);
        }
    }

    /// <summary>
    /// 特殊重力（技能）
    /// </summary>
    /// <param name="gravity"></param>
    public virtual void Gravity(float gravity)
    {
        if (!IsGrounded)
        {
            VerticalVelocity += Vector3.down * gravity * GravityMultiplier * Time.deltaTime;
        }
    }
}
