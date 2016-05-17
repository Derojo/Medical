using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.IO;
using UnityEngine.UI;

namespace Gamedonia.Backend {
	
	public class ProfilePicture : MonoBehaviour {

		public static Texture2D ProfileImage;
		Sprite sprite;
		public Image p_image;
		public Loader loader;
		private bool ImageIsLoaded = false;
		private string fbpictureurl = "";

		WWW www;

		void Start() {

			if(PlayerManager.I.player.fbuserid != "") {
				fbpictureurl = "http://graph.facebook.com/" + PlayerManager.I.player.fbuserid + "/picture?type=normal";
				StartCoroutine (setProfilePicture());
			}
//			if(PlayerPrefs.HasKey("profile_path")) {
//				StartCoroutine (getProfilePicture (Application.persistentDataPath + "/profile.png"));
//				ProfileImage = new Texture2D(www.texture.width, www.texture.height, TextureFormat.ARGB32, false);
//				ProfileImage.SetPixels32 (www.texture.GetPixels32 ());
//				ProfileImage.Apply();
//				p_image.sprite = Sprite.Create(ProfileImage, new Rect(0, 0, ProfileImage.width, ProfileImage.height), new Vector2(0.5f, 0.5f));
//			}
		}

		private IEnumerator setProfilePicture() {
			WWW www = new  WWW (fbpictureurl);
			yield return www;

			Sprite sprite = new Sprite ();
			sprite = Sprite.Create (www.texture, new Rect (0, 0, www.texture.width, www.texture.height), new Vector2 (0.5f, 0.5f), 100.0f);
			p_image.sprite = sprite;
			www.LoadImageIntoTexture (p_image.sprite.texture);
//
//			ProfileImage = new Texture2D(www.texture.width, www.texture.height, TextureFormat.DXT1, false);
//			p_image.sprite = Sprite.Create(ProfileImage, new Rect(0, 0, ProfileImage.width, ProfileImage.height), new Vector2(0.5f, 0.5f));


		}

		public void GetImageFromGallery() {
			loader.enableLoader ();
				//createImage("file://D:\\Afbeeldingen\\wallpaper-1217815.jpg");
				AndroidJNI.AttachCurrentThread ();
//				//AndroidJNIHelper.debug = true;
				AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
				AndroidJavaObject profClass = new AndroidJavaObject ("com.gamedonia.medical.UnityBinder");
				profClass.CallStatic ("OpenGallery", unity.GetStatic<AndroidJavaObject> ("currentActivity"));
		}

		public void OnPhotoPick(string filepath) {
			createImage (filepath);
		}
			
		public void createImage(string filepath) {
			StartCoroutine (getProfilePicture (filepath));
			ProfileImage = new Texture2D(www.texture.width, www.texture.height, TextureFormat.ARGB32, false);
			ProfileImage.SetPixels32 (www.texture.GetPixels32 ());
			ProfileImage.Apply();

			int new_width =  (int)p_image.GetComponent<RectTransform> ().rect.width;
			int new_height = Mathf.RoundToInt(((float)ProfileImage.height/(float)ProfileImage.width) * new_width);

			TextureScale.Bilinear (ProfileImage, new_width, new_height);
			byte[] bytes = ProfileImage.EncodeToPNG ();
			File.WriteAllBytes (Application.persistentDataPath + "/profile.png", bytes);

			PlayerPrefs.SetString ("profile_path", Application.persistentDataPath + "/profile.png");

			p_image.sprite = Sprite.Create(ProfileImage, new Rect(0, 0, new_width, new_height), new Vector2(0.5f, 0.5f));
			loader.disableLoader ();
		}

		public IEnumerator getProfilePicture (string filepath) {
			www = new WWW ("file://" + filepath);
			while (!www.isDone) {
				yield return null;
			}

			yield break;

		}




	}
}