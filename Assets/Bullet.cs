using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public enum BulletType { Homing, Area, Laser }
    public BulletType bulletType = BulletType.Homing;

    [Header("General")]
    public float speed = 30f;
    public float damage = 10f;
    public float lifeTime = 5f;

    [Header("Homing")]
    public float rotateSpeed = 10f;

    [Header("Area")]
    public float explosionRadius = 5f;

    [Header("Laser")]
    public LineRenderer laserLine;
    public float laserDuration = 0.1f;

    private Transform target;

    public void Seek(Transform _target)
    {
        target = _target;

        if (bulletType == BulletType.Laser)
        {
            FireLaser();
        }
    }

    void Update()
    {
        if (bulletType == BulletType.Laser) return; // Laser is instant

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f) Destroy(gameObject);

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (bulletType == BulletType.Homing)
        {
            Vector3 direction = dir.normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else // straight
        {
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        }
    }

    void HitTarget()
    {
        if (bulletType == BulletType.Area)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }

        Destroy(gameObject);
    }

    void Damage(Transform enemy)
    {
        if (enemy != null)
        {
            enemy.GetComponent<ZombieAI>()?.TakeDamage(damage);
            Debug.Log($"Damaged {enemy.name} for {damage} damage.");
        }
    }

    void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider col in hits)
        {
            if (col.CompareTag("Zombie"))
            {
                Damage(col.transform);
            }
        }
    }

    void FireLaser()
    {
        if (target == null) return;

        Vector3 start = transform.position;
        Vector3 end = target.position;

        if (laserLine != null)
        {
            laserLine.SetPosition(0, start);
            laserLine.SetPosition(1, end);
            StartCoroutine(DisableLaser());
        }

        Damage(target);
        Destroy(gameObject);
    }

    IEnumerator DisableLaser()
    {
        laserLine.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        laserLine.enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        if (bulletType == BulletType.Area)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }

    // ? Trigger-based hit detection (useful for homing and straight bullets)
    private void OnTriggerEnter(Collider other)
    {
        if (bulletType == BulletType.Laser) return;

        // ?? Hit a zombie
        if (other.CompareTag("Zombie"))
        {
            target = other.transform;
            HitTarget();
            return;
        }

        Destroy(gameObject);
    }

}
