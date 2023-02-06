using System.IO;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

public class TextureResize
{
#if UNITY_EDITOR
        [MenuItem("Tools/Check Texture 4x4")]
        private static void CheckTexture()
        {
            var strs = Directory.GetFiles(Application.dataPath, "*.png", SearchOption.AllDirectories);
            for (int i = 0; i < strs.Length; i++)
            {
                var relativePath = Path.GetRelativePath(Application.dataPath, strs[i]);
                var texture = (Texture2D)AssetDatabase.LoadAssetAtPath($"Assets/{relativePath}", typeof(Texture2D));

                if (!texture)
                    continue;

                var width = texture.width;
                var height = texture.height;

                if (width % 4 > 0 || height % 4 > 0)
                {
                    var leftOverHeight = height % 4;
                    var leftOverWidth = width % 4;

                    var newW = leftOverWidth > 0 ? width + (4 - leftOverWidth) : width;
                    var newH = leftOverHeight > 0 ? height + (4 - leftOverHeight) : height;

                    Debug.Log($"Texture can't be divided by 4 {texture}", texture);
                }
            }
        }

        [MenuItem("Tools/Resize Texture To Nearest 4x4")]
        private static void ResizeTexture()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                var texture = (Texture2D)Selection.objects[i];
                if (texture)
                {
                    var tex = ResizeAndPlaceOldTextureToCenter(texture);
                    if (tex)
                    {
                        byte[] bytes = tex.EncodeToPNG();
                        var path = AssetDatabase.GetAssetPath(texture);
                        var dirPath = Path.GetDirectoryName(path);

                        // delete or comment the line below to overwrite same file
                        path = AssetDatabase.GenerateUniqueAssetPath(path);

                        File.WriteAllBytes(path, bytes);
                    }
                }
            }
            AssetDatabase.Refresh();
        }

        private static Texture2D ResizeAndPlaceOldTextureToCenter(Texture2D oldTexture, int divider = 4)
        {
            if (oldTexture == null)
                return null;

            var oldWidth = oldTexture.width;
            var oldHeight = oldTexture.height;

            var leftOverWidth = oldWidth % divider;
            var leftOverHeight = oldHeight % divider;

            if (leftOverHeight == 0 && leftOverWidth == 0)
                return null;
            
            var newWidth = leftOverWidth > 0 ? oldWidth + (divider - leftOverWidth) : oldWidth;
            var newHeight = leftOverHeight > 0 ? oldHeight + (divider - leftOverHeight) : oldHeight;

            Texture2D newTexture = new Texture2D(newWidth, newHeight, TextureFormat.RGBA32, false);

            int paddingX = leftOverWidth / 2;
            int paddingY = leftOverHeight / 2;

            int xn = paddingX;
            int yn = paddingY;

            Color32[] newColor = newTexture.GetPixels32();
            Color32[] oldColor = oldTexture.GetPixels32();

            for (int i = 0; i < newColor.Length; i++)
            {
                newColor[i] = Color.clear;
            }

            for (int yo = 0; yo < oldHeight; yo++)
            {
                for (int xo = 0; xo < oldWidth; xo++)
                {
                    var newIndex = yn * newWidth + xn;
                    var oldIndex = yo * oldWidth + xo;
                    newColor[newIndex] = oldColor[oldIndex];
                    xn += 1;
                }
                xn = paddingX;
                yn += 1;
            }

            newTexture.SetPixelData(newColor, 0);
            
            newColor = null;
            oldColor = null;
            return newTexture;
        }
#endif
}