using UnityEngine;
using System.Collections;

public class TouchUpdatePanel : MonoBehaviour {
	public float minSwipeDist, maxSwipeTime; 
	bool couldBeSwipe;
	float swipeStartTime;
	Vector2 startPos;


	public delegate void ChangePage();
	public static event ChangePage minusPage;
	public static event ChangePage plusPage;

	public void startChecks() {
		StartCoroutine (checkHorizontalSwipes ());
	}

	IEnumerator checkHorizontalSwipes () //Coroutine, wich gets Started in "Start()" and runs over the whole game to check for swipes
	{
		while (true) { //Loop. Otherwise we wouldnt check continoulsy ;-)
			foreach (Touch touch in Input.touches) { //For every touch in the Input.touches - array...
				
				switch (touch.phase) {
				case TouchPhase.Began: //The finger first touched the screen --> It could be(come) a swipe
					couldBeSwipe = true;
					
					startPos = touch.position;  //Position where the touch started
					swipeStartTime = Time.time; //The time it started
					break;

				}
				float swipeTime = Time.time - swipeStartTime; //Time the touch stayed at the screen till now.
				float swipeDist = Mathf.Abs (touch.position.x - startPos.x); //Swipedistance
				
				
				if (couldBeSwipe && swipeTime < maxSwipeTime && swipeDist > minSwipeDist) {
					// It's a swiiiiiiiiiiiipe!
					couldBeSwipe = false; //<-- Otherwise this part would be called over and over again.
					
					if (Mathf.Sign (touch.position.x - startPos.x) == 1f) { //Swipe-direction, either 1 or -1.

						// right-swipe
						if (minusPage != null){
							minusPage();
						}
						
					} else {

						// left-swipe
						if (plusPage != null){
							plusPage();
						}
					}
				} 
			}
			yield return null;
		}
	}
}
