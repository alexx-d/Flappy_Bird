using UnityEngine;

public class EnemyRemover : MonoBehaviour
{
    [SerializeField] private ObjectPool<Enemy> _pool;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy pipe))
        {
            _pool.PutObject(pipe);
        }
    }
}