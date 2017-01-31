using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Spashfade : MonoBehaviour {

	public Image splashImage;
	public string loadLevel;
	private bool _isLoading = true;

	IEnumerator ClickStart(){
		
		//splashImage.canvasRenderer.SetAlpha (0.0f);
//		FadeIn ();
		yield return new WaitForSeconds (2.5f);
		FadeOut ();
		yield return new WaitForSeconds (1.0f);
		SceneManager.LoadScene (loadLevel);
	}



	// Update is called once per frame
	void Update () {
		if (_isLoading) {
			splashImage.canvasRenderer.SetAlpha (0.0f);
			FadeIn ();
			_isLoading = false;
		}
		else if (Input.GetMouseButton (0)) {
			StartCoroutine (ClickStart ());
		}
	}

	private void FadeIn(){
		splashImage.CrossFadeAlpha (1.0f, 1.0f, false);
	}

	private void FadeOut(){
		splashImage.CrossFadeAlpha (0.0f, 1.0f, false);
	}
}
