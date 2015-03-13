using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Excelsion.Enemies;

public class TurretTargeting : MonoBehaviour {

    public GameObject projectile;

    public float detectionRadius;
    private bool DO_DEBUG = true;

    public List<Enemy> potentialTargets;

    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Update()
    {
        potentialTargets.Clear();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Enemy")
            {
                Enemy newEnemy = hitColliders[i].GetComponent<Enemy>();
                potentialTargets.Add(newEnemy);
            }
            i++;
        }
        //Debug.Log(potentialTargets.Count);
    }

    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void OnDrawGizmos()
    {
        if (DO_DEBUG)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);

            if (potentialTargets == null) return;

            foreach (Enemy e in potentialTargets) //This causes errors as enemies are destroyed. Ignore it or turn off debug.
            {
                /*
                if (e == activeTarget)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(transform.position, e.transform.position);
                    continue;
                }*/
                Gizmos.color = Color.grey;
                Gizmos.DrawLine(transform.position, e.transform.position);
            }

        }
    }

    public virtual Enemy FilterTargetPriority()
    {
        //By default, target closest to objective.
        float closest = Mathf.Infinity;
        Enemy returnEnemy = null;
        foreach (Enemy e in potentialTargets)
        {
            //float distance = Vector3.Distance( DefenseController.Get().enemyObjective.transform.position, e.transform.position );
            float distance = Vector3.Distance(transform.position, e.transform.position);
            if (distance < closest)
            {
                closest = distance;
                returnEnemy = e;
            }
        }
        return returnEnemy;
    }
}
