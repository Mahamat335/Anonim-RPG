using Anonim;
using Anonim.Systems.CombatSystem;
using Anonim.Systems.DungeonSystem;
using Anonim.Systems.StatSystem;
using UnityEngine;

namespace Anonim
{
    public class GlobalPlayer : Singleton<GlobalPlayer>
    {
        public StatComponent PlayerStatComponent;
        public Vector2Int PlayerPosition;
        public DamageableComponent PlayerDamageableComponent;
        private void Start()
        {
            PlayerPosition = DungeonGenerator.Instance.GetWorldToGridPosition(transform.position);
        }

        public bool IsPlayerBlocked()
        {
            // 4 yön: yukarı, aşağı, sol, sağ
            Vector2Int[] directions = new Vector2Int[]
            {
                Vector2Int.up,
                Vector2Int.down,
                Vector2Int.left,
                Vector2Int.right
            };

            foreach (var dir in directions)
            {
                Vector2Int neighbor = PlayerPosition + dir;

                // Eğer bu tile yürünebilir ve boşsa, player tamamen kapalı değildir
                if (DungeonGenerator.Instance.IsFloor(neighbor.x, neighbor.y) &&
                    EnemySpawner.Instance.GetEnemyInTile(neighbor) == null)
                {
                    return false; // en az bir açık komşu var
                }
            }

            return true; // tüm komşular dolu ya da duvar
        }
    }

}