using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The monobehaviour used for generating raindrops texture
namespace NekoPunch.Raindrop
{
    public class CaptureNormal : MonoBehaviour
    {
        //Material used for output the normals and masks of raindrops
        public Material _NormalMaterial;
        //Raindrop mesh
        public Mesh _NormalMesh;
        //Resolution of output texture
        public int _OutputSize = 512;
        //The path your output texture will be saved
        public string _OutputPath = "E:/output.png";
        //The range of scales in X and Y axis
        public float _XScaleMin = 0.8f;
        public float _XScaleMax = 1.2f;
        public float _YScaleMin = 0.7f;
        public float _YScaleMax = 1.2f;
        //The range of x offset
        public float _XOffsetMin = 0.1f;
        public float _XOffsetMax = 0.17f;

        //Temporary render target used for rendering raindrops' normal and mask 
        private RenderTexture _TargetTexture = null;

        private void OnDestroy()
        {
            if (_TargetTexture != null)
                _TargetTexture.Release();
        }

        //The function of generating raindrop map
        public void Capture()
        {
            /*Vector3[] verts = _NormalMesh.vertices;
            float minY = verts[0].y;
            float maxY = verts[0].y;
            for(int i = 0; i < verts.Length; ++i)
            {
                minY = Mathf.Min(verts[i].y, minY);
                maxY = Mathf.Max(verts[i].y, maxY);
            }
            Debug.Log("MinY:" + minY.ToString());
            Debug.Log("MaxY:" + maxY.ToString());*/

            //get object space mesh center
            Bounds localBounds = _NormalMesh.bounds;
            float width = localBounds.size.x;
            float height = localBounds.size.y;
            Vector3 localCenter = localBounds.center;

            //Use a 4x supersample to remove aliases
            int superSize = _OutputSize * 4;
            if (_TargetTexture == null || _TargetTexture.width != superSize)
            {
                if (_TargetTexture != null)
                    _TargetTexture.Release();
                _TargetTexture = new RenderTexture(superSize, superSize, 24, RenderTextureFormat.ARGB32);
            }

            //Set active RT
            RenderTexture.active = _TargetTexture;
            GL.PushMatrix();
            GL.LoadIdentity();
            //simple projection matrix
            var proj = Matrix4x4.Ortho(-1, 1, -1, 1, -1, 100);
            GL.LoadProjectionMatrix(proj);
            //Clear RT,XY stores the normal.xy,while z stores the raindrops' mask
            GL.Clear(true, true, new Color(0.5f, 0.5f, 0.0f, 1.0f));
            //setup material pass
            _NormalMaterial.SetPass(0);

            float xOffMin = _XOffsetMin;
            float xOffMax = _XOffsetMax;
            float xStart = -1.0f + Random.Range(xOffMin, xOffMax);
            //Render drops from left to right until exceed the right bound
            while (xStart < 1.0f)
            {
                float itemYScale = Random.Range(_YScaleMin, _YScaleMax);
                float itemXScale = Random.Range(_XScaleMin, _XScaleMax);
                Matrix4x4 scaleMat = Matrix4x4.Scale(new Vector3(itemXScale, itemYScale, itemXScale));

                float itemHeight = height * itemYScale;
                float itemWidth = width * itemXScale;
                float xOffset = xStart;
                float yOffset = Random.Range(-1.5f, 1.5f);
                float yMin = yOffset + localCenter.y - itemHeight * 0.5f;
                float yMax = yOffset + localCenter.y + itemHeight * 0.5f;
                float xMin = xOffset + localCenter.x - itemWidth * 0.5f;
                float xMax = xOffset + localCenter.x + itemWidth * 0.5f;


                Matrix4x4 transMat = Matrix4x4.Translate(new Vector3(xOffset, yOffset, -5.0f));
                Graphics.DrawMeshNow(_NormalMesh, transMat * scaleMat, 0);

                if (yMin < -1)
                {
                    transMat = Matrix4x4.Translate(new Vector3(xOffset, yOffset + 2.0f, -5.0f));
                    Graphics.DrawMeshNow(_NormalMesh, transMat * scaleMat, 0);
                }

                if (yMax > 1)
                {
                    transMat = Matrix4x4.Translate(new Vector3(xOffset, yOffset - 2.0f, -5.0f));
                    Graphics.DrawMeshNow(_NormalMesh, transMat * scaleMat, 0);
                }

                if (xMin < -1)
                {
                    transMat = Matrix4x4.Translate(new Vector3(xOffset + 2.0f, yOffset, -5.0f));
                    Graphics.DrawMeshNow(_NormalMesh, transMat * scaleMat, 0);
                }

                if (xMax > 1)
                {
                    transMat = Matrix4x4.Translate(new Vector3(xOffset - 2.0f, yOffset, -5.0f));
                    Graphics.DrawMeshNow(_NormalMesh, transMat * scaleMat, 0);
                }

                xOffMin = Mathf.Max(_XOffsetMin, itemWidth);
                xOffMax = Mathf.Max(_XOffsetMax, itemWidth);
                xStart += Random.Range(_XOffsetMin, _XOffsetMax);
            }

            GL.PopMatrix();

            Texture2D virtualPhoto =
                        new Texture2D(superSize, superSize, TextureFormat.ARGB32, false);
            // false, meaning no need for mipmaps
            virtualPhoto.ReadPixels(new Rect(0, 0, superSize, superSize), 0, 0);

            //downsample
            Texture2D virtualPhoto2 =
                        new Texture2D(_OutputSize, _OutputSize, TextureFormat.ARGB32, false, true);

            Color[] srcColors = virtualPhoto.GetPixels();
            Color[] dstColors = new Color[_OutputSize * _OutputSize];
            for (int y = 0; y < superSize; y += 4)
                for (int x = 0; x < superSize; x += 4)
                {
                    Color tColor = new Color(0.0f, 0.0f, 0.0f);
                    tColor += srcColors[y * superSize + x];
                    tColor += srcColors[y * superSize + x + 1];
                    tColor += srcColors[y * superSize + x + 2];
                    tColor += srcColors[y * superSize + x + 3];
                    tColor += srcColors[(y + 1) * superSize + x];
                    tColor += srcColors[(y + 1) * superSize + x + 1];
                    tColor += srcColors[(y + 1) * superSize + x + 2];
                    tColor += srcColors[(y + 1) * superSize + x + 3];
                    tColor += srcColors[(y + 2) * superSize + x];
                    tColor += srcColors[(y + 2) * superSize + x + 1];
                    tColor += srcColors[(y + 2) * superSize + x + 2];
                    tColor += srcColors[(y + 2) * superSize + x + 3];
                    tColor += srcColors[(y + 3) * superSize + x];
                    tColor += srcColors[(y + 3) * superSize + x + 1];
                    tColor += srcColors[(y + 3) * superSize + x + 2];
                    tColor += srcColors[(y + 3) * superSize + x + 3];
                    tColor /= 16.0f;
                    dstColors[(y / 4) * _OutputSize + (x / 4)] = tColor.linear;
                }
            virtualPhoto2.SetPixels(dstColors);
            byte[] bytes;
            bytes = virtualPhoto2.EncodeToPNG();

            string savePath = _OutputPath;
            System.IO.File.WriteAllBytes(
                savePath, bytes);

            RenderTexture.active = null;
        }
    }
}
