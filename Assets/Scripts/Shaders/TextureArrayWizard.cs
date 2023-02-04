namespace Shaders
{
	/*public class TextureArrayWizard : ScriptableWizard
	{
		[SerializeField, Min(16)] private int _resolution;
		[SerializeField, Min(1)] private int _cellsWide;
		[SerializeField, Min(1)] private int _cellsHeight;

		[SerializeField] private Texture2D _spriteSheet;
		
		[MenuItem("Assets/Create/Texture Array")]
		public static void CreateWizard() 
		{
			DisplayWizard<TextureArrayWizard>( "Create Texture Array", "Create");
		}
		
		void OnWizardCreate()
		{
			if (_spriteSheet == null)
				return;
			
			string path = EditorUtility.SaveFilePanelInProject(
				"Save Texture Array", "Texture Array", "asset", "Save Texture Array"
			);
			
			if (path.Length == 0)
				return;

			Texture[] uvs = new Texture[_cellsWide * _cellsHeight];

			int index = -1;
			for (int y = 0; y < _cellsHeight; y++)
			{
				for (int x = 0; x < _cellsWide; x++)
				{
					index++;
					// uvs[index] = GenerateTexture()...;
					uvs[index] = new Texture2D(x*_resolution,(_cellsHeight-1 - y)*_resolution);
				}
			}
			
			
			// Texture2D t = _textures[0];
			Texture2DArray textureArray = new Texture2DArray(
				_spriteSheet.width, _spriteSheet.height, _textures.Length, _spriteSheet.format, _spriteSheet.mipmapCount > 1);
			
			textureArray.anisoLevel = t.anisoLevel;
			textureArray.filterMode = t.filterMode;
			textureArray.wrapMode = t.wrapMode;
			
			for (int i = 0; i < _textures.Length; i++) 
			{
				for (int m = 0; m < t.mipmapCount; m++) 
				{
					Graphics.CopyTexture(_textures[i], 0, m, textureArray, i, m);
				}
			}
			
			AssetDatabase.CreateAsset(textureArray, path);
			
		}
	}*/
}