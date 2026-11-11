using UnityEngine;

public class Tower : MonoBehaviour
{
    public float detectionRadius = 20f;
    public Transform rotatingPart; // Optional: For rotating only part of the tower
    public GameObject bulletPrefab, ElecBox;
    public Transform firePoint;
    public float fireRate = 1f;

    private float fireCooldown;
    private void OnEnable()
    {
        ElecBox = GameObject.FindGameObjectWithTag("EBOX");
        
    }
    void Update()
    {
        GameObject target = GetNearestZombie();

        if (target != null&& ElecBox.GetComponent<ElectricalBoxPuzzle>().puzzleComplete)
        {
            Vector3 dir = target.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);

            if (rotatingPart != null)
                rotatingPart.rotation = Quaternion.Lerp(rotatingPart.rotation, lookRotation, Time.deltaTime * 5f);
            else
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            // Shooting logic
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                Shoot(target.transform);
                fireCooldown = 1f / fireRate;
            }
        }
    }

    GameObject GetNearestZombie()
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
        GameObject nearest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject zombie in zombies)
        {
            float dist = Vector3.Distance(transform.position, zombie.transform.position);
            if (dist < shortestDistance && dist <= detectionRadius)
            {
                shortestDistance = dist;
                nearest = zombie;
            }
        }

        return nearest;
    }

    void Shoot(Transform target)
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }
}
