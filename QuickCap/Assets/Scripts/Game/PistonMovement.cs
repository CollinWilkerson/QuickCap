using System.Collections;
using UnityEngine;

public class PistonMovement : MonoBehaviour
{
    public float dropTime;
    //this reall isnt working
    //option 1 is to manually create each piston and put it in the list
    //option 2 is to make a list of gameobjects and a list of bools and just give them the same ids for the coroutine
    public GameObject[] pistonList;
    public bool[] upList;
    private Animator anim;
    float lastRunTime = 0f;

    private void Start()
    {
        if (dropTime <= 0)
            dropTime = 8.0f;
        StartCoroutine(PistonMove());
    }

    // Update is called once per frame
    //this cant be a coroutine
    void Update()
    {
        
    }

    private bool anyTrue(bool[] list)
    {
        foreach(bool b in list)
        {
            if (b)
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator PistonMove()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(7f, 10f));
            if (anyTrue(upList))
            {
                //selects a piston that isnt already moving
                int activePiston = Random.Range(0, pistonList.Length);
                while (!upList[activePiston])
                    activePiston = Random.Range(0, pistonList.Length);//sets the anim to the animation controller of the current piston
                anim = pistonList[activePiston].GetComponent<Animator>();

                //moves the piston down and tells the progam that it cannot be moved
                anim.SetBool("Up", false);
                upList[activePiston] = false;
                Debug.Log("start wait");
                yield return new WaitForSeconds(Random.Range(3.0f, 6.0f)); //stays down for a random number of seconds

                Debug.Log("end wait");

                //raises piston and allows it to be moved again
                anim.SetBool("Up", true);
                yield return new WaitForSeconds(3); //time of the animation
                upList[activePiston] = true;
            }
        }

    }
}

