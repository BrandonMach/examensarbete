using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using TMPro;
using UnityEngine.UI;

public class QRCodeReader : MonoBehaviour
{

    [SerializeField] private RawImage _rawImageBackground; //Will be the phone camera viewport
    [SerializeField] private AspectRatioFitter _aspectRatioFitter;
    [SerializeField] private TextMeshProUGUI _textOutput;
    [SerializeField] private RectTransform _scanZone;

    private bool _isCamAvailable;
    private WebCamTexture _cameraTexture; //Access to device camera

    void Start()
    {
        SetupCamera();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraRender();
    }


    private void UpdateCameraRender()
    {

        if (!_isCamAvailable) { return; }

        float ratio = (float) _cameraTexture.width / (float) _cameraTexture.height;
        _aspectRatioFitter.aspectRatio = ratio;

        //Flip the image to show correct image on _rawImageBackground / camera visualizer
        int orientation = -_cameraTexture.videoRotationAngle;
        _rawImageBackground.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);

    }


    private void SetupCamera()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        if(devices.Length == 0) //No available cameras on the device
        {
            _isCamAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing) // if camera is not front facing 
            {
                _cameraTexture = new WebCamTexture(devices[i].name, (int)_scanZone.rect.width, (int)_scanZone.rect.height);
            }
        }

        _cameraTexture.Play(); //Starts the camera
        _rawImageBackground.texture = _cameraTexture;
        _isCamAvailable = true;
    }


    public void OnClickScan()
    {
        Scan();
    }



    private void Scan()
    {
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();

            //Takes in scan image
            Result result = barcodeReader.Decode(_cameraTexture.GetPixels32(), _cameraTexture.width, _cameraTexture.height);

            if(result != null)
            {
                _textOutput.text = result.Text;
            }
            else
            {
                _textOutput.text = "FAILED TO READ QR CODE";
            }
        }
        catch //If try fails catch exception
        {
            _textOutput.color = Color.red;
            _textOutput.text = "FAILED IN TRY";
            
        }
    }
}
