using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreeshotManager : MonoBehaviour
{
	Camera myCamera;
	static ScreeshotManager instance;
	bool takeScreenshotOnNextFrame;

	private void Awake()
	{
		instance = this;
		myCamera = GetComponent<Camera>();
	}
	private void OnPostRender()
	{
		if (takeScreenshotOnNextFrame)
		{
			takeScreenshotOnNextFrame = false;
			RenderTexture renderTexture = myCamera.targetTexture;

			Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
			Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
			renderResult.ReadPixels(rect, 0, 0);

			byte[] byteArray = renderResult.EncodeToPNG();
			System.IO.File.WriteAllBytes(Application.dataPath + "/CameraScrenshot.png", byteArray);
			Debug.Log("Screenshot Taken");

			RenderTexture.ReleaseTemporary(renderTexture);
			myCamera.targetTexture = null;
		}
	}


	void TakeScreenshot(int width, int height)
	{
		myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
		takeScreenshotOnNextFrame = true;

	}

	public static void CaptureScreenshot(int width, int height)
	{
		instance.TakeScreenshot(width, height);
	}

}
