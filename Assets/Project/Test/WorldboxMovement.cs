using UnityEngine;

public class WorldBoxMovement : MonoBehaviour
{
    [Header("Cài đặt Di chuyển")]
    [Tooltip("Tốc độ di chuyển của nhân vật")]
    public float moveSpeed = 2f;
    
    [Tooltip("Phạm vi lang thang tính từ điểm bắt đầu")]
    public float wanderRadius = 5f;
    
    [Tooltip("Thời gian nghỉ tối thiểu giữa các lần di chuyển")]
    public float minWaitTime = 1f;
    
    [Tooltip("Thời gian nghỉ tối đa giữa các lần di chuyển")]
    public float maxWaitTime = 3f;

    [Header("Cài đặt Animation")]
    [Tooltip("Tên biến Float trong Animator")]
    public string paramName = "Move";

    // Các thành phần cần thiết
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Biến trạng thái nội bộ
    private Vector2 startPosition; // Vị trí gốc để AI không đi quá xa
    private Vector2 targetPosition; // Điểm đến hiện tại
    private bool isMoving = false;
    private float waitTimer;
    
    // Lưu trạng thái hướng nhìn dọc (true = nhìn lên, false = nhìn xuống)
    // Mặc định là false (nhìn xuống)
    private bool isFacingUp = false; 

    private void Awake()
    {
        // Lấy các component tự động
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Lưu vị trí ban đầu làm tâm của vòng tròn di chuyển
        startPosition = transform.position;
        
        // Bắt đầu bằng việc chọn điểm đến ngay
        PickNewPosition();
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveToTarget();
        }
        else
        {
            WaitAtPosition();
        }

        UpdateAnimation();
    }

    // Hàm xử lý di chuyển
    private void MoveToTarget()
    {
        // Di chuyển nhân vật về phía mục tiêu
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Kiểm tra xem đã đến nơi chưa (khoảng cách rất nhỏ)
        if (Vector2.Distance(transform.position, targetPosition) < 0.05f)
        {
            isMoving = false;
            // Random thời gian nghỉ ngơi
            waitTimer = Random.Range(minWaitTime, maxWaitTime);
        }
    }

    // Hàm xử lý chờ đợi
    private void WaitAtPosition()
    {
        waitTimer -= Time.deltaTime;
        if (waitTimer <= 0)
        {
            PickNewPosition();
        }
    }

    // Hàm chọn điểm đến ngẫu nhiên
    private void PickNewPosition()
    {
        // Tạo một điểm ngẫu nhiên trong vòng tròn (Random.insideUnitCircle)
        // Cộng với vị trí gốc để đảm bảo nó ở quanh khu vực ban đầu
        Vector2 randomPoint = startPosition + Random.insideUnitCircle * wanderRadius;
        
        targetPosition = randomPoint;
        isMoving = true;
    }

    // Hàm cập nhật Animation và FlipX
    private void UpdateAnimation()
    {
        // 1. Xử lý Flip X (Trái/Phải)
        // Nếu mục tiêu ở bên trái (x nhỏ hơn) -> Flip = true, ngược lại false
        if (targetPosition.x < transform.position.x)
        {
            spriteRenderer.flipX = true; 
        }
        else if (targetPosition.x > transform.position.x)
        {
            spriteRenderer.flipX = false;
        }

        // 2. Xác định hướng dọc (Lên/Xuống) để chọn Animation
        // Nếu đang di chuyển, cập nhật hướng dựa trên vị trí mục tiêu so với vị trí hiện tại
        if (isMoving)
        {
            if (targetPosition.y > transform.position.y)
                isFacingUp = true;  // Đang đi lên
            else
                isFacingUp = false; // Đang đi xuống
        }

        // 3. Gửi giá trị vào Animator dựa trên Blend Tree của bạn
        // Quy tắc của bạn:
        // 0: IdleDown, 1: IdleUp, 2: RunDown, 3: RunUp
        
        float animValue = 0;

        if (isMoving)
        {
            // Đang chạy (Run)
            if (isFacingUp) 
                animValue = 3; // RunUp
            else 
                animValue = 2; // RunDown
        }
        else
        {
            // Đang đứng yên (Idle)
            if (isFacingUp) 
                animValue = 1; // IdleUp
            else 
                animValue = 0; // IdleDown
        }

        // Cập nhật giá trị cho Animator
        if (animator != null)
        {
            animator.SetFloat(paramName, animValue);
        }
    }
    
    // Vẽ vòng tròn phạm vi trong Scene view để dễ debug (Tùy chọn)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        // Nếu chưa chạy game, dùng vị trí hiện tại, nếu chạy rồi dùng startPosition
        Vector2 center = Application.isPlaying ? startPosition : (Vector2)transform.position;
        Gizmos.DrawWireSphere(center, wanderRadius);
    }
}