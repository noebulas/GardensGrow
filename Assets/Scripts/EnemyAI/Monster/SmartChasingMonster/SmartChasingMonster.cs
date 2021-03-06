﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Vision))]
public class SmartChasingMonster : SmartChasingMonsterAbstractFSM {
    [Header("DelayParameters")]
    public float idleDelay = 2.0f;
    public float attackDelay = 1.0f;
    public float pathingDelay = 1.0f;
    [Range(0, 10)]
    public int speed = 2;
    Vision visionModule;

    [Header("PathFindingRequirements")]
    public TileMap tileMap;
    public GameObject targetObject;

    [Header("Status")]
    public bool isDisabled;

    [Header("Debug")]
    public bool debug = false;

    AStar astarAlgorithm;

    // Path found by astar
    private List<Globals.Direction> path;
    private int currentPathIndex = 0;

    // Tile that the monster is on
    private Tile monsterTile;
    // Tile that the target object is standing on
    private Tile targetTile;

    // Does the path need to be re-evaluated
    private bool needsReevaluation;
    // Is my attack on cooldown.
    private bool attackOnCooldown;

    private Vector2 startPosition;

     // Use this for initialization
    protected override void Start ()
    {
        isDisabled = false;
        attackOnCooldown = false;
        needsReevaluation = false;
        path = new List<Globals.Direction>();
        astarAlgorithm = new AStar(tileMap);
        visionModule = GetComponent<Vision>();
        base.Start();
	}

    public void OnDrawGizmos()
    {
        if (!debug) return;

        Vector2 acc = startPosition;
        foreach (var v in path)
        {
            var v_real = Globals.DirectionToVector(v);
            Gizmos.DrawRay(acc,v_real);
            acc += v_real;
        }
    }


	

    public override void Reset() { }

    // TODO: change this so instead of checking every step it reacts to colliders instead
    protected override bool CanAttack()
    {
        // Check if there is something killable in collider facing direction
        AttackCollider edgeTrigger = getHitColliderFromDirection(direction);

        List<KillableGridObject> killList = edgeTrigger.GetKillList();

        // Check if there is anything to kill
        if (killList.Count > 0)
        {
            // Check if any of the killables are an enemy
            foreach(KillableGridObject target in killList)
            {
                if (target.faction != this.faction)
                    return true;
            }
        }

        return false;
    }

    protected override bool CanSeePlayer()
    {
        return visionModule.CanSeePlayer(direction);
    }

    protected override bool IsDisabled()
    {
        return isDisabled;
    }

    protected override IEnumerator ExecuteActionAttack()
    {
        if(!attackOnCooldown)
        {
            attackOnCooldown = true;
            Attack();
        }
        yield return null;
    }

    protected override IEnumerator ExecuteActionChasePlayer()
    {
        for(int i = 0; i < speed; i++)
        {
            Move(direction);
        }
        yield return null;
    }

    protected override IEnumerator ExecuteActionDisabled()
    {
        yield return null;
    }

    /// <summary>
    /// Check if our path is still valid.
    /// The path might not be valid anymore if the target has moved
    /// or target is dead.
    /// </summary>
    protected override IEnumerator ExecuteActionEvaluatePath()
    {
        // Check if the target is still alive
        if(targetObject)
        {
            // Check if the target is still on the tile we last found a path to
            Tile currTargetTile = tileMap.GetNearestTile(targetObject.transform.position);
            if(currTargetTile == targetTile)
            {
                needsReevaluation = false;

                // Next step in the path
                currentPathIndex++;
            }
            else
            {
                needsReevaluation = true;
            }
        }
        else
        {
            // target is dead
            needsReevaluation = true;
        }

        yield return null;
    }

    /// <summary>
    /// Find a path from the monster to the target.
    /// </summary>
    protected override IEnumerator ExecuteActionPathFind()
    {
        // If the target still exists, find a path toward it
        if(targetObject)
        {
            // Find where monster is on the tile map
            monsterTile = tileMap.GetNearestTile(this.transform.position);

            // Find where monster has to go
            targetTile = tileMap.GetNearestTile(targetObject.transform.position);

            // Find a path
            path = astarAlgorithm.FindPath(monsterTile, targetTile);
            startPosition = monsterTile.transform.position;

            // We are on the first step of the path
            currentPathIndex = 0;
        }

        yield return null;
    }

    /// <summary>
    /// The monster takes a step on the path to the player.
    /// </summary>
    protected override IEnumerator ExecuteActionTakeStep()
    {
        if (path.Count == 0)
            yield return null;

        for(int i = 0; i < speed; i+= 1)
        {
            if (currentPathIndex < path.Count)
                Move(path[currentPathIndex]);
        }
        yield return null;
    }

    protected override bool IsAttackFinished()
    {
        if (TimeInState() > attackDelay)
        {
            attackOnCooldown = false;
            isAttacking = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// The path is the list of directions from start to target.
    /// The path is finished if we took all steps or the path
    /// needs to be re-evaluated due to player dying/moving.
    /// </summary>
    /// <returns>Are we done with the current path?</returns>
    protected override bool IsPathFinished()
    {
        return currentPathIndex >= path.Count || needsReevaluation;
    }

    /// <summary>
    /// A step is one direction in the list of directions for a path.
    /// This check adds a slight delay before the next step is taken.
    /// </summary>
    /// <returns>Has enough time passed for the delay?</returns>
    protected override bool IsStepFinished()
    {
        return TimeInState() > pathingDelay;
    }

}
