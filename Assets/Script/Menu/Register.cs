using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class Register : MonoBehaviour
{
    public GameObject login, register;
    public TMP_InputField username,password;
    public TMP_Text txtError;    
            
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void CheckRegister()
    {
        var user = username.text;
        var pass = password.text;
        UserModel userModel = new UserModel(user, pass);

        StartCoroutine(RegisterAPI(userModel));
        RegisterAPI(userModel);
    }
    IEnumerator RegisterAPI(UserModel userModel)
    {
        //…
        string jsonStringRequest = JsonConvert.SerializeObject(userModel);

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/register", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonStringRequest);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            var jsonString = request.downloadHandler.text.ToString();
            ReponseModel reponseModel = JsonConvert.DeserializeObject<ReponseModel>(jsonString);
            if(reponseModel.status == 1)
            {
                register.SetActive(false);
                login.SetActive(true);
            }
            else
            {
                //thong bao
                txtError.text = reponseModel.notification;
            }
        }
        request.Dispose();
    }


}
