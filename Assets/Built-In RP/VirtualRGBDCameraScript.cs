using System;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Built_In_RP
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class VirtualRGBDCameraScript : MonoBehaviour
    {
        public Vector2Int imageResolution;
        public Material depthMaterial;
        public RenderTexture depthTexture;
        public RenderTexture colorTexture;

        public RawImage colorDisplay, depthDisplay;
        public string imageSavePath;

        private Camera _camera;
    
        // private void OnRenderImage(RenderTexture source, RenderTexture destination)
        // {
        //     Graphics.Blit(source, destination, depthMaterial);
        // }

        void Start()
        {
            // GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
            _camera = GetComponent<Camera>();
            InitRenderTextures();
            
        }

        void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CaptureColorImage();
                CaptureDepthImage();
            }
            
        }

        void InitRenderTextures()
        {
            if(colorTexture != null)
                colorTexture.Release();
            colorTexture = new RenderTexture(imageResolution.x, imageResolution.y, 0, RenderTextureFormat.Default);
            
            if(depthTexture != null)
                depthTexture.Release();
            depthTexture = new RenderTexture(imageResolution.x, imageResolution.y, 24, RenderTextureFormat.Depth);
            
            _camera.SetTargetBuffers(colorTexture.colorBuffer, depthTexture.depthBuffer);
            
            colorDisplay.texture = colorTexture;
            depthDisplay.texture = depthTexture;
        }

        private void CaptureColorImage()
        {
            RenderTexture activeRenderTexture = RenderTexture.active;
            RenderTexture.active = colorTexture;
 
            _camera.Render();
 
            Texture2D image = new Texture2D(colorTexture.width, colorTexture.height);
            image.ReadPixels(new Rect(0, 0, colorTexture.width, colorTexture.height), 0, 0);
            image.Apply();
            RenderTexture.active = activeRenderTexture;
 
            byte[] bytes = image.EncodeToPNG();
            Destroy(image);
 
            File.WriteAllBytes( imageSavePath + "/color-" + DateTime.Now.ToString("0:MM-dd-yy_H-mm-ss") + ".png", bytes);

        }
        
        private void CaptureDepthImage()
        {
            RenderTexture activeRenderTexture = RenderTexture.active;
            RenderTexture.active = colorTexture;
 
            _camera.Render();
 
            Texture2D image = new Texture2D(colorTexture.width, colorTexture.height);
            image.ReadPixels(new Rect(0, 0, colorTexture.width, colorTexture.height), 0, 0);
            image.Apply();
            RenderTexture.active = activeRenderTexture;
 
            byte[] bytes = image.EncodeToPNG();
            Destroy(image);
 
            File.WriteAllBytes( imageSavePath + "/depth-" + DateTime.Now.ToString("0:MM-dd-yy_H-mm-ss") + ".png", bytes);
            
        }
    }
}
