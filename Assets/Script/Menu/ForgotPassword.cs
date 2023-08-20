using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEditor;

public class ForgotPassword : MonoBehaviour
{
    public TMP_InputField txtUser,txtOTP, txtNewPassword, txtReNewPassword;
    public GameObject resetPassword, sendOTP, login;
    public TMP_Text txtError;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SendOTP()
    {
        var user = txtUser.text;
        OTPModel oTPModel = new OTPModel(user);
        StartCoroutine(SendOTPAPI(oTPModel));
        SendOTPAPI(oTPModel);
    }
    //Send OTP API
    IEnumerator SendOTPAPI(OTPModel oTPModel)
    {
        //…
        string jsonStringRequest = JsonConvert.SerializeObject(oTPModel);

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/send-otp", "POST");
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
                //thanh cong, load pannel reset
                resetPassword.SetActive(true);
                sendOTP.SetActive(false);
            }
            else
            {
                //hien thong bao that bai
                txtError.text = "Send OTP Falled!";
            }
        }
        request.Dispose();
    }
    public void ResetPassWord()
    {

        var newPassword = txtNewPassword.text;
        var reNewPassword = txtReNewPassword.text;
        if(newPassword.Equals(reNewPassword))
        {
            var user = txtUser.text;
            var otp = int.Parse(txtOTP.text);
            ResetPasswordModel resetPasswordModel = new ResetPasswordModel(user, otp, newPassword);
            StartCoroutine(ResetPasswordAPI(resetPasswordModel));
            ResetPasswordAPI(resetPasswordModel);
        }
        else
        {
            //hien thi thong bao
            Debug.Log("Reset Password - Falled");
        }
    }
    //Reset Password API
    IEnumerator ResetPasswordAPI(ResetPasswordModel resetPasswordModel)
    {
        //…
        string jsonStringRequest = JsonConvert.SerializeObject(resetPasswordModel);

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/reset-epassword", "POST");
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
                //tro ve login
                resetPassword.SetActive(false);
                login.SetActive(true);
            }
            else
            {
                //hien thi thong bao
            }
        }
        request.Dispose();
    }

}
