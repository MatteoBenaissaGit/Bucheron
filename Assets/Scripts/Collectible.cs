using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace
{
    public class Collectible : MonoBehaviour
    {
        [SerializeField] private Transform _shadow;
        [ReadOnly] public bool CanBePickedUp = true;
        [ReadOnly] public bool HasBeenPickedUp;
        
        private Vector3 _shadowScale;
        protected float JumpTime = 1f;
        protected Rigidbody2D Rigidbody2D;

        private void Awake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
            CanBePickedUp = false;
        }
        
        private void Start()
        {
            Jump();
        }

        public virtual void Jump()
        {
            transform.localScale = Vector3.zero;
            _shadowScale = _shadow.localScale;
            _shadow.localScale = Vector3.zero;
            const float animTime = 0.25f;
            transform.DOScale(Vector3.one, animTime);

            //jump
            System.Random random = new System.Random();
            double randomX = random.NextDouble();
            Rigidbody2D.velocity = new Vector2(randomX < 0.5f ? -(float)random.Next(100, 300) / 100 : (float)random.Next(100, 300) / 100, (float)random.Next(300, 600) / 100);
        }

        private void Update()
        {
            JumpTime -= Time.deltaTime;
            if (JumpTime <= 0)
            {
                _shadow.DOScale(_shadowScale, 0.3f);
                Rigidbody2D.gravityScale = 0;
                Rigidbody2D.velocity = Vector2.zero;
                CanBePickedUp = true;
            }
        }
        
        public void PickedUp()
        {
            CanBePickedUp = false;
            HasBeenPickedUp = true;
            transform.DOScale(0, 0.5f).SetEase(Ease.OutCubic).OnComplete(Destroy);
        }

        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}