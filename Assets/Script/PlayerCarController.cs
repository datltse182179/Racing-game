using UnityEngine;

public class PlayerCarController : MonoBehaviour
{
    [Header("Wheels collider")]
    public WheelCollider frontRightWheelCollider;
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider backRightWheelCollider;
    public WheelCollider backLeftWheelCollider;

    [Header("Wheels Transform")]
    public Transform frontRightWheelTransform;
    public Transform frontLeftWheelTransform;
    public Transform backRightWheelTransform;
    public Transform backLeftWheelTransform;

    [Header("Car Engigne")]
    public float acclerationForce = 20000f; // Tăng cực mạnh lực tăng tốc
    public float maxSpeed = 50f; // Tốc độ tối đa (km/h)
    public bool showSpeedometer = true; // Hiển thị tốc độ trong console
    public float breakingForce = 8000f; // Tăng lực phanh
    private float presentBreakForce = 0f;
    private float presentAcceleration = 0f;

    [Header("Car Steering")]
    public float wheelsTorque = 300f; // Tăng góc rẽ CỰC LỚN
    public float steeringSensitivity = 20f; // Tăng độ nhạy steering CỰC CAO
    public float steeringSmoothing = 40f; // Smoothing CỰC NHANH
    public float speedBasedSteeringMultiplier = 1.2f; // Tăng hệ số steering ở tốc độ cao
    public float driftFactor = 0.95f; // Hệ số drift (0.9-1.0)
    public float driftThreshold = 0.5f; // Ngưỡng drift
    private float presentTurnAngle = 0f;
    private float targetTurnAngle = 0f;

    [Header("Car Sounds")]
    public AudioSource audioSource;
    public AudioClip acceleraionSound;
    public AudioClip slowAccelerationSound;
    public AudioClip stopSound;

    [Header("Car Stability")]
    public float maxSteerAngle = 100f; // Tăng góc rẽ tối đa cực lớn
    public bool speedBasedSteering = false; // Tắt speed-based steering để rẽ nhanh khi chạy
    public float stabilityForce = 1000f; // Lực ổn định xe
    private Rigidbody carRigidbody;
    private float driftAngle;
    private bool isDrifting;

    private void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        // Điều chỉnh center of mass để xe ổn định hơn
        carRigidbody.centerOfMass = new Vector3(0, -0.5f, 0);
        
        // Thiết lập drag để xe ổn định hơn
        carRigidbody.linearDamping = 0f; // Không có drag để xe nhanh nhất
        carRigidbody.angularDamping = 0f; // Không có drag góc để xoay nhanh
        
        // Đảm bảo xe không bị lật dễ dàng
        carRigidbody.mass = 500f; // Giảm mass để xe nhanh hơn
    }

    private void Update()
    {
        MoveCar();
        CarSteering();
        ApplyBreaks();
        ApplyStabilityControl();
        CheckDrift();
    }

    private void MoveCar()
    {   
        // Tính toán tốc độ hiện tại (m/s)
        float currentSpeed = carRigidbody.linearVelocity.magnitude;
        float currentSpeedKmh = currentSpeed * 3.6f; // Chuyển đổi sang km/h
        
        // Hiển thị tốc độ trong console (nếu bật)
        if (showSpeedometer && Time.frameCount % 60 == 0) // Hiển thị mỗi giây
        {
            Debug.Log($"Tốc độ hiện tại: {currentSpeedKmh:F1} km/h / Tối đa: {maxSpeed} km/h");
        }
        
        // Tính toán lực tăng tốc
        float inputAcceleration = Input.GetAxis("Vertical");
        
        // Logic tăng tốc đơn giản
        if (currentSpeedKmh < maxSpeed)
        {
            // Tăng tốc mạnh mẽ cho đến khi đạt tốc độ tối đa
            presentAcceleration = acclerationForce * inputAcceleration;
        }
        else
        {
            // Dừng tăng tốc khi đạt tốc độ tối đa
            presentAcceleration = 0f;
        }
        
        // Áp dụng lực cho tất cả bánh xe (FWD)
        frontLeftWheelCollider.motorTorque = presentAcceleration;
        frontRightWheelCollider.motorTorque = presentAcceleration;
        backLeftWheelCollider.motorTorque = presentAcceleration;
        backRightWheelCollider.motorTorque = presentAcceleration;

        if (presentAcceleration > 0)
        {
            audioSource.PlayOneShot(acceleraionSound, 0.2f);
        }
        else if (presentAcceleration < 0)
        {
            audioSource.PlayOneShot(slowAccelerationSound, 0.2f);
        }
        else if (presentAcceleration == 0) 
        {
            audioSource.PlayOneShot(stopSound, 0.1f);

        }
    }

    private void CarSteering()
    {
        // Lấy input từ người chơi
        float horizontalInput = Input.GetAxis("Horizontal");
        
        // Tính toán tốc độ hiện tại
        float currentSpeed = carRigidbody.linearVelocity.magnitude;
        float currentSpeedKmh = currentSpeed * 3.6f;
        
        // Tăng độ nhạy CỰC MẠNH cho input nhẹ - xe sẽ bẻ lái ngay cả khi input nhỏ
        float inputSensitivity = 1f;
        if (Mathf.Abs(horizontalInput) > 0.01f && Mathf.Abs(horizontalInput) < 0.5f)
        {
            // Tăng độ nhạy CỰC MẠNH cho input nhẹ
            inputSensitivity = 4f; // Tăng từ 2f lên 4f
        }
        else if (Mathf.Abs(horizontalInput) >= 0.5f)
        {
            // Tăng độ nhạy cho input mạnh
            inputSensitivity = 2f;
        }
        
        // Tính toán góc rẽ mục tiêu với độ nhạy cao
        float baseSteerAngle = wheelsTorque * horizontalInput * steeringSensitivity * inputSensitivity;
        
        // Áp dụng speed-based steering - TĂNG CƯỜNG steering ở mọi tốc độ
        float speedFactor = 1f;
        if (currentSpeedKmh > 5f) // Bắt đầu điều chỉnh steering từ 5 km/h
        {
            // TĂNG CƯỜNG steering ở mọi tốc độ
            speedFactor = Mathf.Lerp(1.2f, 1.8f, (currentSpeedKmh - 5f) / 25f);
            speedFactor = Mathf.Clamp(speedFactor, 1.0f, 2.0f); // Từ 100% đến 200%
        }
        else
        {
            speedFactor = 1.5f; // 150% khi đứng yên
        }
        
        baseSteerAngle *= speedFactor;
        
        // Giới hạn góc rẽ tối đa
        targetTurnAngle = Mathf.Clamp(baseSteerAngle, -maxSteerAngle, maxSteerAngle);
        
        // Rẽ CỰC NHANH với smoothing - CỰC NHANH cho input nhẹ
        float smoothingSpeed = steeringSmoothing;
        if (Mathf.Abs(horizontalInput) > 0.01f && Mathf.Abs(horizontalInput) < 0.5f)
        {
            smoothingSpeed = steeringSmoothing * 2f; // CỰC NHANH cho input nhẹ
        }
        else if (Mathf.Abs(horizontalInput) >= 0.5f)
        {
            smoothingSpeed = steeringSmoothing * 1.5f; // Nhanh cho input mạnh
        }
        
        presentTurnAngle = Mathf.Lerp(presentTurnAngle, targetTurnAngle, smoothingSpeed * Time.deltaTime);
        
        // Áp dụng góc rẽ cho bánh trước
        frontLeftWheelCollider.steerAngle = presentTurnAngle;
        frontRightWheelCollider.steerAngle = presentTurnAngle;

        // Cập nhật visual của bánh xe
        SteeringWheels(frontLeftWheelCollider, frontLeftWheelTransform);
        SteeringWheels(frontRightWheelCollider, frontRightWheelTransform);
        SteeringWheels(backLeftWheelCollider, backRightWheelTransform);
        SteeringWheels(backRightWheelCollider, backRightWheelTransform);
        
        // Áp dụng drift physics
        ApplyDriftPhysics();
        
        // Debug info
        if (showSpeedometer && Time.frameCount % 60 == 0)
        {
            Debug.Log($"Steering: Speed={currentSpeedKmh:F1}km/h, Input={horizontalInput:F2}, Sensitivity={inputSensitivity:F1}, Factor={speedFactor:F2}, Angle={presentTurnAngle:F1}°");
        }
    }

    void SteeringWheels(WheelCollider WC, Transform WT)
    {
        Vector3 position;
        Quaternion rotation;

        WC.GetWorldPose(out position, out rotation);

        WT.position = position; 
        WT.rotation = rotation;
    }

    public void ApplyBreaks()
    {
        // Kiểm tra nếu đang drift (Space + W + D)
        bool isDriftBraking = Input.GetKey(KeyCode.Space) && Input.GetAxis("Vertical") > 0.1f;
        bool isNormalBraking = Input.GetKey(KeyCode.Space) && Input.GetAxis("Vertical") <= 0.1f;
        
        if (isDriftBraking)
        {
            // Phanh nhẹ khi drift để không cản trở
            presentBreakForce = breakingForce * 0.2f; // Phanh nhẹ cho drift
        }
        else if (isNormalBraking)
        {
            // Phanh mạnh khi phanh thường
            presentBreakForce = breakingForce * 0.8f; // Phanh mạnh
        }
        else
        {
            presentBreakForce = 0f;
        }

        // Phanh tất cả bánh xe
        frontLeftWheelCollider.brakeTorque = presentBreakForce;
        frontRightWheelCollider.brakeTorque = presentBreakForce;
        backLeftWheelCollider.brakeTorque = presentBreakForce;
        backRightWheelCollider.brakeTorque = presentBreakForce;
    }

    private void ApplyStabilityControl()
    {
        // Tính toán góc nghiêng của xe
        float tiltAngle = Vector3.Dot(transform.up, Vector3.up);
        
        // Nếu xe nghiêng quá nhiều, áp dụng lực ổn định
        if (tiltAngle < 0.8f) // Nếu góc nghiêng > ~37 độ
        {
            Vector3 stabilityTorque = Vector3.Cross(transform.up, Vector3.up) * stabilityForce;
            carRigidbody.AddTorque(stabilityTorque, ForceMode.Force);
        }
        
        // Hỗ trợ bẻ lái ở mọi tốc độ
        float currentSpeed = carRigidbody.linearVelocity.magnitude;
        float currentSpeedKmh = currentSpeed * 3.6f;
        float horizontalInput = Input.GetAxis("Horizontal");
        
        // Giảm angular damping để bẻ lái dễ hơn ở mọi tốc độ
        if (currentSpeedKmh > 5f) // Khi có tốc độ
        {
            carRigidbody.angularDamping = 0.2f;
        }
        else
        {
            carRigidbody.angularDamping = 0.1f; // Rất thấp khi đứng yên
        }
        
        // Thêm lực hỗ trợ xoay CỰC MẠNH khi có input steering (dù nhẹ)
        if (Mathf.Abs(horizontalInput) > 0.01f) // Ngay cả input rất nhẹ
        {
            float assistForce = stabilityForce * 0.8f; // Tăng từ 0.3f lên 0.8f
            
            // Tăng lực hỗ trợ CỰC MẠNH cho input nhẹ
            if (Mathf.Abs(horizontalInput) < 0.5f)
            {
                assistForce *= 3f; // Tăng 200% cho input nhẹ (từ 1.5f lên 3f)
            }
            else
            {
                assistForce *= 2f; // Tăng 100% cho input mạnh
            }
            
            Vector3 steeringAssist = Vector3.up * horizontalInput * assistForce;
            carRigidbody.AddTorque(steeringAssist, ForceMode.Force);
        }
        
        // Giảm tốc độ xoay ngang nhẹ để tránh lạng lạng
        Vector3 angularVelocity = carRigidbody.angularVelocity;
        angularVelocity.y *= 0.95f; // Giảm 5% mỗi frame
        carRigidbody.angularVelocity = angularVelocity;
    }
    
    private void CheckDrift()
    {
        // Tính toán góc drift
        Vector3 velocity = carRigidbody.linearVelocity.normalized;
        Vector3 forward = transform.forward;
        
        driftAngle = Vector3.Angle(forward, velocity);
        
        // Kiểm tra xem có đang drift không
        bool isMoving = carRigidbody.linearVelocity.magnitude > 2f;
        bool isSteering = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f;
        bool isBraking = Input.GetKey(KeyCode.Space);
        bool isAccelerating = Input.GetAxis("Vertical") > 0.1f;
        bool isDriftBraking = isBraking && isAccelerating;
        bool isNormalBraking = isBraking && !isAccelerating;
        
        // Drift khi: đang chạy + rẽ + (có phanh hoặc góc drift lớn)
        // Đặc biệt: Space + W + D = drift có kiểm soát
        isDrifting = isMoving && isSteering && (isBraking || driftAngle > driftThreshold || (isBraking && isAccelerating));
        
        if (isDrifting && showSpeedometer && Time.frameCount % 60 == 0)
        {
            string brakeText = isBraking ? " [BRAKE DRIFT]" : " [NATURAL DRIFT]";
            string accelText = isAccelerating ? " + ACCEL" : "";
            string brakeType = isDriftBraking ? " (DRIFT BRAKE)" : isNormalBraking ? " (NORMAL BRAKE)" : "";
            Debug.Log($"DRIFT! Góc drift: {driftAngle:F1}°{brakeText}{accelText}{brakeType}");
        }
    }
    
    private void ApplyDriftPhysics()
    {
        // Tính toán tốc độ hiện tại
        float currentSpeed = carRigidbody.linearVelocity.magnitude;
        float currentSpeedKmh = currentSpeed * 3.6f;
        
        // Điều chỉnh friction dựa trên tốc độ - giảm friction ở tốc độ cao để bẻ lái dễ hơn
        float speedFactor = Mathf.Clamp(currentSpeedKmh / 40f, 0.3f, 1.0f);
        
        if (isDrifting)
        {
            // Kiểm tra nếu đang nhấn Space + W + D (drift có kiểm soát)
            bool isBrakeDrift = Input.GetKey(KeyCode.Space) && Input.GetAxis("Vertical") > 0.1f;
            
            if (isBrakeDrift)
            {
                // Drift có kiểm soát: grip tốt hơn ở tốc độ cao
                frontLeftWheelCollider.sidewaysFriction = new WheelFrictionCurve
                {
                    extremumSlip = 0.2f,
                    extremumValue = 0.8f * speedFactor, // Tăng grip theo tốc độ
                    asymptoteSlip = 0.5f,
                    asymptoteValue = 0.9f * speedFactor,
                    stiffness = 0.8f * speedFactor // Tăng stiffness theo tốc độ
                };
            }
            else
            {
                // Drift tự nhiên: grip vừa phải
                frontLeftWheelCollider.sidewaysFriction = new WheelFrictionCurve
                {
                    extremumSlip = 0.2f,
                    extremumValue = 0.6f * speedFactor,
                    asymptoteSlip = 0.5f,
                    asymptoteValue = 0.8f * speedFactor,
                    stiffness = 0.6f * speedFactor
                };
            }
            
            frontRightWheelCollider.sidewaysFriction = frontLeftWheelCollider.sidewaysFriction;
            backLeftWheelCollider.sidewaysFriction = frontLeftWheelCollider.sidewaysFriction;
            backRightWheelCollider.sidewaysFriction = frontLeftWheelCollider.sidewaysFriction;
        }
        else
        {
            // Grip bình thường - giảm friction ở tốc độ cao để bẻ lái dễ hơn
            frontLeftWheelCollider.sidewaysFriction = new WheelFrictionCurve
            {
                extremumSlip = 0.2f,
                extremumValue = 0.7f * speedFactor, // Giảm grip ở tốc độ cao
                asymptoteSlip = 0.5f,
                asymptoteValue = 0.6f * speedFactor,
                stiffness = 0.8f * speedFactor // Giảm stiffness để bẻ lái dễ hơn
            };
            
            frontRightWheelCollider.sidewaysFriction = frontLeftWheelCollider.sidewaysFriction;
            backLeftWheelCollider.sidewaysFriction = frontLeftWheelCollider.sidewaysFriction;
            backRightWheelCollider.sidewaysFriction = frontLeftWheelCollider.sidewaysFriction;
        }
    }
}



