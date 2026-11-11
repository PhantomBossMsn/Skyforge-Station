using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    [Header("Stats")]
    public float health = 100f;
    public float moveSpeed = 3.5f;

    [Header("Target (auto-assigned)")]
    public Transform target;

    [Header("Appearance Customization")]
    [Tooltip("Set 3 game objects for visual appearance.")]
    public GameObject[] appearanceOptions = new GameObject[3]; // 0 = common, 1 = uncommon, 2 = alternate
    [Range(1, 2)] public int appearanceType = 1;

    [Header("Rare Visuals (10% chance each)")]
    public GameObject rareVisual1;
    public GameObject rareVisual2;

    [Header("Animator")]
    public Animator animator;
    [Range(1, 2)] public int zombieState = 1; // 1 = idle/walk, 2 = angry/run

    [Header("Attack")]
    public float attackRange = 2.5f;
    public float damageAmount = 5f;
    public float damageInterval = 3f;
    private float damageCooldown = 0f;

    private NavMeshAgent agent;
    public GameObject ShopManger;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    void OnEnable()
    {
        ShopManger = GameObject.FindGameObjectWithTag("Shop");
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
            target = playerGO.transform;
        else
            Debug.LogWarning("ZombieAI: No GameObject with tag 'Player' found.");

        if (target != null)
            agent.SetDestination(target.position);

        SetupAppearance();
        TryEnableRareVisuals();
        ApplyZombieState();
    }

    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);

            float distance = Vector3.Distance(transform.position, target.position);
            if (distance <= attackRange)
            {
                TryDealDamage();
            }
        }

        // Decrease cooldown every frame
        if (damageCooldown > 0f)
        {
            damageCooldown -= Time.deltaTime;
        }
    }

    void TryDealDamage()
    {
        if (damageCooldown <= 0f)
        {
            // Send damage to target if it has a matching method
            target.SendMessage("TakeDamage", damageAmount, SendMessageOptions.DontRequireReceiver);
            damageCooldown = damageInterval;
        }
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0f)
            Die();
    }

    void Die()
    {
        switch (appearanceType)
        {
            case 1: ShopManger.GetComponent<ShopManager>().AddPoints(20); break;
            case 2: ShopManger.GetComponent<ShopManager>().AddPoints(25); break;
            case 3: ShopManger.GetComponent<ShopManager>().AddPoints(30); break;
            case 4: ShopManger.GetComponent<ShopManager>().AddPoints(40); break;
        }

        Destroy(gameObject);
    }

    void SetupAppearance()
    {
        foreach (GameObject obj in appearanceOptions)
            if (obj != null) obj.SetActive(false);

        int indexToEnable = -1;

        if (appearanceType == 1)
        {
            indexToEnable = (Random.value <= 0.7f) ? 0 : 1;
        }
        else if (appearanceType == 2)
        {
            indexToEnable = 2;
        }

        if (indexToEnable >= 0 && indexToEnable < appearanceOptions.Length && appearanceOptions[indexToEnable] != null)
            appearanceOptions[indexToEnable].SetActive(true);
    }

    void TryEnableRareVisuals()
    {
        if (rareVisual1 != null)
            rareVisual1.SetActive(Random.value <= 0.1f);

        if (rareVisual2 != null)
            rareVisual2.SetActive(Random.value <= 0.1f);
    }

    void ApplyZombieState()
    {
        if (animator != null)
        {
            int stateValue = (zombieState == 2) ? 1 : 0;
            animator.SetInteger("states", stateValue);
        }
        else
        {
            Debug.LogWarning("ZombieAI: Animator is not assigned.");
        }
    }

    void OnDrawGizmosSelected()
    {
        if (agent != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
