using System.IO;


namespace App.core {
	class Config {

		// Constants
		private static readonly string PRoot = Program.PRoot;


		// Methods
		public static string Get(string k) {
			var f = Path.Combine(PRoot, k);
			return File.Exists(f) ? File.ReadAllText(f) : null;
		}

		public static void Set(string k, string v) {
			var f = Path.Combine(PRoot, k);
			File.WriteAllText(f, v);
		}
	}
}
