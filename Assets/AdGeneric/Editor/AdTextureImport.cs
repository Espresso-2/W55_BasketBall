using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;

namespace AdGeneric
{
    public class AdTextureImport:AssetPostprocessor
    {
        private const string Platform = "WebGL";

        private static readonly Regex[] HealthMatcher = new[]
        {
            new Regex(@"^.*竖版.*(\s\d*)?$"),
            new Regex(@"^.*横板.*(\s\d*)?$"),
            new Regex(@"^.*健康.*(\s\d*)?$"),
            new Regex(@"^.*忠告.*(\s\d*)?$"),
            new Regex(@"^\d{4}[A-Z]{2,3}\d{6,7}(\s\d*)?$")
        };
        private static readonly Regex[] TitleMatcher = new[]
        {
            new Regex(@"^\d{8}-\d{6}(\s\d*)?$")
        };

        private static readonly TextureImporterPlatformSettings WebGLSetting 
            =  new TextureImporterPlatformSettings
        {
            overridden = true,
            name = Platform,
            textureCompression = TextureImporterCompression.Compressed,
            format = TextureImporterFormat.DXT5Crunched,
        };
        private void OnPreprocessTexture()
        {
            var path = assetPath;
            var info = new FileInfo(path);
            var fn = Path.GetFileNameWithoutExtension(info.FullName);
            if (TitleMatcher.Any(e=>e.IsMatch(fn))||HealthMatcher.Any(e=>e.IsMatch(fn)))
            {
                if (assetImporter is TextureImporter ti)
                {
                    ti.textureType = TextureImporterType.Sprite;
                    ti.spriteImportMode = SpriteImportMode.Single;
                    ti.alphaIsTransparency = true;
                    ti.mipmapEnabled = false;
                    ti.SetPlatformTextureSettings(WebGLSetting);
                }
            }
        }
    }
}