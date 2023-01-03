using UnityEngine;

namespace DefaultNamespace
{
    public class Carrot : Collectible
    {
        public override void Jump()
        {
            JumpTime = 0.5f;
            base.Jump();
            //jump
            System.Random random = new System.Random();
            double randomX = random.NextDouble();
            Rigidbody2D.velocity = new Vector2(randomX < 0.5f ? -(float)random.Next(100, 300) / 100 : (float)random.Next(50, 150) / 100, (float)random.Next(100, 300) / 100);
        }
    }
}