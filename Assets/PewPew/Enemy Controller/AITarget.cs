using UnityEngine;
using UnityEngine.AI; // NavMeshAgent için bu kütüphane şart

public class AITarget : MonoBehaviour
{
   private bool isDead = false;

   public AudioClip footstepSound;

   private AudioSource audioSource; 

   private Animator animator;

   public Transform firePoint;

   public NavMeshAgent agent;

   public Transform player;

   public LayerMask whatIsGround, whatIsPlayer;
   
   public float health;

   //Patroling
   public Vector3 walkPoint;
   bool walkPointSet;
   public float walkPointRange;
   
   //Attacking 
   public float timeBetweenAttacks;
   bool alreadyAttacked;
   public GameObject projectile;

   [Header("AI Behavior")] // Inspector'da düzenli durması için başlık
    public float patrolSpeed = 1.2f; // Devriye atarkenki yürüme hızı
    public float patrolWaitTime = 3f; // Devriye noktasında bekleme süresi

    private float agentMaxSpeed; // Kovalamada kullanılacak maks hız (Inspector'dan alınacak)
    private bool isWaiting = false;
   


    // States 
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    
    private void Awake()
{
    player = GameObject.FindGameObjectWithTag("Player").transform; 
    agent = GetComponent<NavMeshAgent>();

    // Animator'u bul (Modelin bir alt obje olduğunu varsayarak)
    animator = GetComponentInChildren<Animator>(); 

    audioSource = GetComponent<AudioSource>();

    agentMaxSpeed = agent.speed;
}

   private void Update()
    {
        if (isDead) return;

        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // --- YENİ MANTIK BLOĞU ---

        // 1. Durum: SALDIRI
        // Oyuncu hem görüş menzilinde hem de saldırı menzilindeyse
        if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
        }

        // 2. Durum: KOVALAMA
        // Oyuncu görüş menzilinde, FAKAT saldırı menzilinde DEĞİLSE
        else if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }

        // 3. Durum: DEVRİYE
        // Oyuncu görüş menzilinde DEĞİLSE (diğer tüm durumlar)
        else if (!playerInSightRange)
        {
            Patroling();
        }

        // --- ESKİ MANTIK (Artık buna gerek yok) ---
        // if(!playerInSightRange && !playerInAttackRange) Patroling();
        // if(playerInSightRange && playerInAttackRange) ChasePlayer();
        // if(playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        // --- HIZ VE ANİMASYON AYARI ---
        agent.speed = patrolSpeed; // Hızı yürüme hızına (1.2) düşür
        animator.SetBool("isAttacking", false); // Saldırı modunda değil
        animator.SetBool("isRunning", false);
        // ---

        // 1. BEKLEME DURUMU
        if (isWaiting)
        {
            animator.SetBool("isWalking", false); // Beklerken yürüme animasyonu durur
            return; // Fonksiyondan çık (yeni nokta arama)
        }

        // 2. YENİ NOKTA ARAMA
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
            
        // 3. NOKTAYA GİTME
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            animator.SetBool("isWalking", true); // Yürürken yürüme animasyonu başlar
        }

        // 4. NOKTAYA VARMA KONTROLÜ
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Noktaya vardığımızda (ve hala beklemiyorsak)
        if (walkPointSet && distanceToWalkPoint.magnitude < 1.5f)
        {
            walkPointSet = false; // Hedef kalmadı
            isWaiting = true;     // Bekleme moduna geç
            
            // Beklemeyi bitirmek için 'patrolWaitTime' saniye sonrasına zamanlayıcı kur
            Invoke(nameof(StopWaiting), patrolWaitTime);
        }
    }

    private void SearchWalkPoint()
    {
        
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
    
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        walkPointSet = true;

    }   


    private void ChasePlayer()
    {
        // --- HIZ VE ANİMASYON AYARI ---
        agent.speed = agentMaxSpeed; // Hızı maksimuma (koşu hızı) çıkar
        animator.SetBool("isAttacking", false); // Saldırı modunda değil
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", true); // Koşma animasyonu
        // ---

        agent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        // --- HIZ VE ANİMASYON AYARI ---
        animator.SetBool("isAttacking", true); 
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);

        // --- 1. VÜCUDU DÖNDÜRME ---
        // Botun 'LookAt' yaparken devrilmemesi için sadece Y ekseninde (yatay) dönmesini sağla
        Vector3 directionToLook = player.position - transform.position;
        directionToLook.y = 0; // Y (dikey) farkı sıfırla
        
        // Aniden dönmesin, 'Angular Speed' ayarına uysun veya yumuşak dönsün
        Quaternion lookRotation = Quaternion.LookRotation(directionToLook);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f); 
        
        // Düşmanın hareket etmesini engelle
        agent.SetDestination(transform.position); 

        // --- 2. ATEŞ ETME ZAMANLAMASI ---
        if(!alreadyAttacked)
        {
            animator.SetTrigger("Attack");

            // --- 3. MERMİ YÖNÜNÜ HESAPLAMA (EN ÖNEMLİ KISIM) ---

            // Oyuncunun pivotu (ayakları) yerine merkezini (gövdesini) hedef al
            // (Character Controller'ın varsayılan merkezi (Center Y) genelde 1'dir)
            Vector3 targetPosition = player.position + Vector3.up * 1f;

            // Namlu ucundan (firePoint) hedefin merkezine (targetPosition) giden
            // 'gerçek' yönü hesapla
            Vector3 directionToFire = (targetPosition - firePoint.position).normalized;

            // 4. MERMİYİ ATEŞLEME
            
            // Mermiyi oluştur, ama rotasyonunu 'firePoint'in kendi rotasyonu DEĞİL,
            // hesapladığımız bu 'directionToFire' yönünün rotasyonu yap
            GameObject bullet = Instantiate(projectile, firePoint.position, Quaternion.LookRotation(directionToFire));
            
            // Rigidbody'sini al
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            
            // Mermiyi, 'firePoint.forward' (namlunun baktığı yer) yönünde DEĞİL,
            // hesapladığımız 'directionToFire' (gerçek hedef) yönünde fırlat
            rb.AddForce(directionToFire * 32f, ForceMode.Impulse); 
            
            ///
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;

    }

    public void TakeDamage(int damage)
{
    // Zaten ölmüşse tekrar hasar almasın
    if (isDead) return;

    health -= damage;

    if (health <= 0)
    {
        Die(); // 'DestroyEnemy' yerine 'Die' fonksiyonunu çağır
    }
}

    private void Die()
{
    isDead = true;

    // Ölme animasyonunu tetikle
    animator.SetTrigger("Die");

    // Botun hareketini ve çarpışmasını durdur
    agent.enabled = false; 
    GetComponent<Collider>().enabled = false; // Modelinizin ana çarpıştırıcısını (örn. Capsule Collider) kapatır

    // Botun "Audio Source"unu sustur (eğer varsa)
    if (audioSource != null)
        audioSource.Stop();

    // 3 saniye sonra (veya animasyonunuz ne kadar sürüyorsa)
    // objeyi oyundan tamamen sil
    Destroy(gameObject, 3f); 
}
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    // Bu fonksiyon, 'Walk_N' animasyonundaki 'OnFootstep' olayı tarafından çağrılacak
public void OnFootstep()
{
    // Eğer bir ayak sesi atanmışsa ve hoparlör (audioSource) varsa...
    if (footstepSound != null && audioSource != null)
    {
        // ...o sesi bir kez çal.
        // PlayOneShot, mevcut sesi kesmeden üstüne yeni ses çalabilmenizi sağlar.
        // Adım sesleri için mükemmeldir.
        audioSource.PlayOneShot(footstepSound);
    }
}

// 'Invoke' tarafından 'patrolWaitTime' saniye sonra çağrılır
    private void StopWaiting()
    {
        isWaiting = false;
    }





}