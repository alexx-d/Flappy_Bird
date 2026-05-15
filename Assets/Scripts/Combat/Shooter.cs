using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;

    private ObjectPool<Bullet> _bulletPool;

    public bool HasPool => _bulletPool is not null;

    public void SetPool(ObjectPool<Bullet> pool)
    {
        _bulletPool = pool;
    }

    public void Shoot()
    {
        Bullet bullet = _bulletPool.GetObject();

        bullet.transform.position = _shootPoint.position;
        bullet.gameObject.SetActive(true);

        Vector3 shootDirection = transform.right;

        bullet.Initialize(shootDirection);

        bullet.NeedsToReturn += OnBulletNeedsToReturn;
    }

    private void OnBulletNeedsToReturn(Bullet bullet)
    {
        bullet.NeedsToReturn -= OnBulletNeedsToReturn;

        _bulletPool.PutObject(bullet);
    }
}