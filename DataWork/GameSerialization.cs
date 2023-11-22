using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;
using System.Windows.Markup;

namespace TableStrategy
{
	/// <summary> Сохранение и загрузка игры </summary>
	public class GameSerialization
	{
		private const string FilePath = "GameSave";
		private const string FileFormat = ".json";

		public GameSerialization()
		{}

		public bool LoadCheck() => (File.Exists(FilePath + FileFormat));

		public void DeleteSaveFile()
		{
			if(LoadCheck())
				File.Delete(FilePath + FileFormat);
		}

		public GameManager Load()
		{
			string jsonText = File.ReadAllText(FilePath + FileFormat);
			GameManager data = JsonConvert.DeserializeObject<GameManager>(jsonText);

			return data;
		}

		public void Save(GameManager game)
		{
			string serializeT = JsonConvert.SerializeObject(game);
			var file = File.Create(FilePath + FileFormat);
			file.Close();

			File.WriteAllText(FilePath + FileFormat, serializeT);
		}
	}
}
