using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;
using UnityEngine.UI;
using TMPro;

public class QRCodeGenerator : MonoBehaviour
{

    [SerializeField] private RawImage _rawImageReciver;
    [SerializeField] private TMP_InputField _textInputField;

    private Texture2D _storeEncodedTexture;


    void Start()
    {
        _storeEncodedTexture = new Texture2D(256, 256);
    }







    private Color32 [] Encode(string textToEncode, int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE, //Format barcode writer to a QR code
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };

        //Return QR code with info being textToEncode / input field
        return writer.Write(textToEncode);
    }

    public void OnClickEncode()
    {
        EncodeTextToQRCode();
    }

    private void EncodeTextToQRCode()
    {
        //Check if _textInputField is empty => "You should write something" else (One-liner)
        string inputText = string.IsNullOrEmpty(_textInputField.text) ? "You should write something" : _textInputField.text;

        Color32[] convertPixelToTexture = Encode(inputText, _storeEncodedTexture.width, _storeEncodedTexture.height);


        _storeEncodedTexture.SetPixels32(convertPixelToTexture); //Set Encoded QR code to _storeEncodedTexture
        _storeEncodedTexture.Apply();

        _rawImageReciver.texture = _storeEncodedTexture;
    }

   
}
