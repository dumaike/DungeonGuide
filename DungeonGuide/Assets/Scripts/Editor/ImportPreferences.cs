using UnityEngine;
using UnityEditor;
using System;

public class ImportPreferences : AssetPostprocessor 
{
	private void OnPostprocessTexture(Texture2D texture)
	{
		if (assetPath.Contains("CharacterSprites"))
		{
			int lastSlashIndex = assetPath.LastIndexOf("/");
			Log.Print("Importing texture \"" + assetPath.Substring(lastSlashIndex + 1) + "\" with custom character sprite settings", LogChannel.IMPORT);
			TextureImporter texImporter = assetImporter as TextureImporter;
			TextureImporterSettings textureSettings = new TextureImporterSettings();
			textureSettings.ApplyTextureType(TextureImporterType.Sprite, true);
			textureSettings.spritePixelsPerUnit = 100;
			textureSettings.textureFormat = TextureImporterFormat.AutomaticCompressed;
			textureSettings.maxTextureSize = 512;
			textureSettings.mipmapEnabled = false;
			textureSettings.spriteMode = 1;
			texImporter.spritePackingTag = "characters";
			texImporter.SetTextureSettings(textureSettings);
		}		
	}
}