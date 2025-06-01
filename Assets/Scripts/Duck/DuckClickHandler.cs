using UnityEngine;


public class DuckClickHandler : MonoBehaviour
{
   public Animator dogAnimator; // Assign this in the Inspector


   public void PlayDogAnimation(int ducksClicked)
   {
       if (ducksClicked == 2)
       {
           dogAnimator.Play("GrabTwo"); // Play if both ducks were hit
       }
       else if (ducksClicked == 1)
       {
           dogAnimator.Play("GrabOne"); // Play if only one duck was hit
       }
       else
       {
           dogAnimator.Play("Laugh"); // Play if no ducks were hit
       }
   }
}