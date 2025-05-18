using System.Collections.Generic;
using UnityEngine;
using Anonim.Systems.DungeonSystem;

namespace Anonim.Systems.MovementSystem
{
    public class AStarPathfinder : Singleton<AStarPathfinder>
    {
        private class Node
        {
            public Vector2Int Position;
            public Node Parent;
            public int G, H;
            public int F => G + H;
        }

        public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
        {
            var openList = new List<Node>();
            var closedSet = new HashSet<Vector2Int>();
            int maxIterations = 1000;
            int iteration = 0;

            Node startNode = new Node { Position = start, G = 0, H = Heuristic(start, goal) };
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                if (++iteration > maxIterations)
                {
                    Debug.Log("Pathfinding iteration limit reached.");
                    return null;
                }

                if (GlobalPlayer.Instance.IsPlayerBlocked())
                {
                    return null;
                }

                openList.Sort((a, b) => a.F.CompareTo(b.F));
                Node current = openList[0];
                openList.RemoveAt(0);

                if (current.Position == goal)
                    return ReconstructPath(current);

                closedSet.Add(current.Position);

                foreach (var neighbor in GetNeighbors(current.Position))
                {
                    if (closedSet.Contains(neighbor)) continue;
                    if (!DungeonGenerator.Instance.IsFloor(neighbor.x, neighbor.y)) continue;
                    if (EnemySpawner.Instance.GetEnemyInTile(neighbor) != null) continue;

                    int tentativeG = current.G + 1;

                    Node existing = openList.Find(n => n.Position == neighbor);
                    if (existing == null)
                    {
                        Node newNode = new Node
                        {
                            Position = neighbor,
                            Parent = current,
                            G = tentativeG,
                            H = Heuristic(neighbor, goal)
                        };
                        openList.Add(newNode);
                    }
                    else if (tentativeG < existing.G)
                    {
                        existing.G = tentativeG;
                        existing.Parent = current;
                    }
                }
            }

            return null; // path yok
        }

        private List<Vector2Int> ReconstructPath(Node node)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            while (node != null)
            {
                path.Add(node.Position);
                node = node.Parent;
            }
            path.Reverse();
            return path;
        }

        private int Heuristic(Vector2Int a, Vector2Int b) =>
            Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // Manhattan distance

        private IEnumerable<Vector2Int> GetNeighbors(Vector2Int pos)
        {
            yield return pos + Vector2Int.up;
            yield return pos + Vector2Int.down;
            yield return pos + Vector2Int.left;
            yield return pos + Vector2Int.right;
        }
    }
}