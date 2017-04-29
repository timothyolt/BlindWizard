using UnityEngine;

public static class VrInputHelper
{
    public static bool Primary
        => Input.GetKeyDown(KeyCode.C) || GvrController.AppButtonDown;

    public static bool Secondary
        => Input.GetKeyDown(KeyCode.Space) || GvrController.ClickButtonDown || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
}