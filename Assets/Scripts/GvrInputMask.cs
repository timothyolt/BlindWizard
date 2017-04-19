using UnityEngine;

public static class GvrInputMask
{
    public static bool AppButtonDown
        => Input.GetKeyDown(KeyCode.C) || GvrController.AppButtonDown;

    public static bool ClickButtonDown
        => Input.GetKeyDown(KeyCode.Space) || GvrController.ClickButtonDown;
}