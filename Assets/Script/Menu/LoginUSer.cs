using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Newtonsoft.Json;
using System.Text;
using UnityEngine.Networking;

public class LoginUSer : MonoBehaviour
{
    public TMP_InputField edtUser, edtPass;
    public TMP_Text txtError;
    public Selectable first;
    private EventSystem eventSystem;

    public Button btnLogin;
    public static LoginReponseModel loginReponseModel;
    // Start is called before the first frame update
    void Start()
    {
        eventSystem = EventSystem.current;
        first.Select();

      
 
    }

    // Update is called once per frame 
    void Update()
    {
        if(Input.GetKey(KeyCode.Return)) 
        {
               btnLogin.onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = eventSystem
                .currentSelectedGameObject
                .GetComponent<Selectable>()
                .FindSelectableOnDown();
            if(next != null)  next.Select();
        }
        if(Input.GetKey(KeyCode.LeftShift))
        {
            Selectable next = eventSystem
               .currentSelectedGameObject
               .GetComponent<Selectable>()
               .FindSelectableOnUp();
            if (next != null) next.Select();
        }
    }
    public void CheckLogin()
    {
        var user = edtUser.text;
        var pass = edtPass.text;

        UserModel userModel = new UserModel(user,pass);
        StartCoroutine(Login(userModel));
        Login(userModel);
        // //goi API
        //   if(user.Equals("tiin") && pass.Equals("123"))
        //   {
        //      SceneManager.LoadScene(1);
        //  }
        //  else
        //  {
        //      txtError.text = "Login Failed!";
        //   }
    }
    IEnumerator Login(UserModel userModel)
    {
        //…
        string jsonStringRequest = JsonConvert.SerializeObject(userModel);
        

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/login", "POST");
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
           loginReponseModel = JsonConvert.DeserializeObject<LoginReponseModel>(jsonString);
            if(loginReponseModel.status == 1)
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                txtError.text = loginReponseModel.notification;
            }
        }
        request.Dispose();
    }



}
