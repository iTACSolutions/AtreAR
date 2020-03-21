using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BreadAPI : MonoBehaviour
{
    [SerializeField] private Text _logText;
    [SerializeField] private Canvas _canvas;

    [SerializeField] private GameObject _hukidashi;

    void Start()
    {
//        StartCoroutine(Upload(Application.dataPath + "/Images/SS.png"));
//       StartCoroutine(UploadScreenShot()); 
//        Debug.Log(Application.dataPath);
//        StartCoroutine(UploadScreenShot());
    }

    public IEnumerator UploadScreenShot()
    {
        while (true)
        {
            yield return StartCoroutine(UploadScreenShotCoroutine());
        }
    }

    byte[] MakeScreenShot()
    {
        var texture = Camera.main.targetTexture;
        var tex2d = new Texture2D(texture.width, texture.height);
        RenderTexture.active = texture;
        tex2d.ReadPixels(new Rect(0, 0, tex2d.width, tex2d.height), 0, 0);
        tex2d.Apply();
        return tex2d.EncodeToPNG();
    }

    IEnumerator UploadScreenShotCoroutine()
    {
        Debug.Log(Application.persistentDataPath);
        var fileName = "SS.png";
        var filePath = System.IO.Path.Combine(Application.persistentDataPath, fileName);

//        _canvas.gameObject.SetActive(false);
        _hukidashi.SetActive(false);
        yield return null;

        ScreenCapture.CaptureScreenshot(fileName);

//        _canvas.gameObject.SetActive(true);
        _hukidashi.SetActive(true);
        
        yield return new WaitForSeconds(0.5f);
        
        StartCoroutine(Upload(filePath));
    }

    IEnumerator Upload(string filePath)
    {
        // 画像ファイルをbyte配列に格納
        byte[] img = File.ReadAllBytes(filePath);
        Debug.Log(img.Length);
//        byte[] img = MakeScreenShot();

        var formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormFileSection("img", img, "img.png", "application/png"));

        var www = UnityWebRequest.Post("http://3.113.21.180:9012/api/coord/", formData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            _logText.text = www.error;
        }
        else
        {
            Debug.Log("Form upload complete!");
            Debug.Log(www.downloadHandler.text);
            _logText.text = www.downloadHandler.text;
            var json = new JSONObject(www.downloadHandler.text);
            var datas = www.downloadHandler.text.Split('[', ',', ' ', ']');
            var bread = new Bread()
            {
                Name = datas[2], XMin = int.Parse(datas[4]), YMin = int.Parse(datas[6]), XMax = int.Parse(datas[8]),
                YMax = int.Parse(datas[10])
            };
            _hukidashi.SetActive(true);
            _hukidashi.transform.position =
                new Vector3((bread.XMin + bread.XMax) / 2f, Screen.height - (bread.YMin + bread.YMax) / 2f, 0);
        }
    }

    Sprite ByteToSprite(byte[] bytes)
    {
        int width = 2048;
        int height = 2048;

        //byteからTexture2D作成
        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(bytes);

        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }
}