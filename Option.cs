using System;
using System.Collections.Generic;


namespace App {
	class Option {

		// Data
		public IList<string> Command = new List<string>();
		public string Username;
		public string Password;
		public string Input;
		public string To;
		public bool O;


		// Methods
		public Option(string[] args) {
			for (var i = 0; i < args.Length;) {
				var a = args[i++];
				if (a.StartsWith("--")) Flag(args, ref i, a.Substring(2));
				else if (a.StartsWith("-")) { foreach (var v in a.Substring(1)) Flag(args, ref i, v.ToString()); }
				else Command.Add(a);
			}
		}

		private void Flag(string[] a, ref int i, string v) {
			switch (v.ToLower()) {
				case "u":
				case "username":
					Username = i < a.Length ? a[i++] : "";
					break;
				case "p":
				case "password":
					Password = i < a.Length ? a[i++] : "";
					break;
				case "i":
				case "input":
					Input = i < a.Length ? a[i++] : "";
					break;
				case "t":
				case "to":
					To = i < a.Length ? a[i++] : "";
					break;
				case "0":
					O = true;
					break;
				default:
					throw new ArgumentException(string.Format("Bad flag: \"{0}\"", v));
			}
		}
	}
}
