using UnityEngine;

public static class VrInputHelper
{
	public static bool Secondary
	#if UNITY_ANDROID
        => Input.GetKeyDown(KeyCode.C) || GvrController.AppButtonDown;
	#else
		=> Input.GetKeyDown(KeyCode.C);
	#endif

    public static bool Primary
	#if UNITY_ANDROID
        => Input.GetKeyDown(KeyCode.Space) || GvrController.ClickButtonDown || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
	#else
		=> Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
	#endif
}