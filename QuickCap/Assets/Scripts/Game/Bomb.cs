//created using https://www.youtube.com/watch?v=SKdM2ERWy8U. many thanks.
using System;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject exp;
    public float expForce, radius;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject InstExp = Instantiate(exp, transform.position, transform.rotation);
        Destroy(InstExp, 2);
        KnockBack();
        Destroy(gameObject);
    }

    private void KnockBack()
    {
        //gets a list of colliders in a sphere starting from bomb impact
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach(Collider nearby in colliders)
        {
            //checking that the collider has a rigidbody
            Rigidbody rig = nearby.GetComponent<Rigidbody>();
            if(rig != null)
            {
                //uses unity's builtin explosions
                rig.AddExplosionForce(expForce, transform.position, radius);
            }
        }
    }
}
