using UnityEngine;
public class SpatulaMovement : MonoBehaviour
{
    private Animator anim;
    public bool spatula2;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        anim.SetBool("Spatula2", spatula2);
    }

}
