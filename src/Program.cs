using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using src.core;
using src.text;


namespace src {
	class Program {

		// Data
		public static readonly string NOrg = "0rez";
		public static readonly string NApp = "cmd-way2sms";
		public static readonly string PUser = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		public static readonly string PRoot = Path.Combine(PUser, NOrg, NApp);
		public static readonly string ERoot = "OWAY2SMS_";
		private static readonly IDictionary<string, Action<Option>> FnDo = new Dictionary<string, Action<Option>>() {
			["get"] = DoGet, ["set"] = DoSet, ["send"] = DoSend
		};


		// Methods
		static void Main(string[] args) {
			Directory.CreateDirectory(PRoot);
			try { Do(new Option(args)); }
			catch (Exception e) { Console.Error.WriteLine(e.Message); }
		}

		private static void Do(Option o) {
			if (o.Command.Count == 0) o.Command.Add("");
			var cmd = o.Command[0].ToLower();
			o.Command.RemoveAt(0);
			if (FnDo.ContainsKey(cmd)) FnDo[cmd](o);
			else throw new NotSupportedException(string.Format("Bad command: \"{0}\"", cmd));
		}

		private static void DoSend(Option o) {
			o = OptionDefs(o);
			if (o.O) {
				Console.WriteLine("SEND SMS");
				OptionGet(o, "username", ref o.Username);
				OptionGet(o, "password", ref o.Password);
				OptionGet(o, "to", ref o.To);
				OptionGet(o, "input", ref o.Input, true);
			}
			if (string.IsNullOrEmpty(o.Username)) throw new ArgumentException("Bad username: \"\"");
			if (string.IsNullOrEmpty(o.Password)) throw new ArgumentException("Bad password: \"\"");
			if (string.IsNullOrEmpty(o.To)) throw new ArgumentException("Bad to: \"\"");
			var api = new Api(o.Username, o.Password);
			api.Send(o.To, o.Input);
		}

		private static void DoGet(Option o) {
			if (o.O) Console.WriteLine("GET CONFIGURATION");
			var all = o.Username == null && o.Password == null;
			if (all || o.Username != null) ConfigGet(o, "username");
			if (all || o.Password != null) ConfigGet(o, "password");
		}

		private static void DoSet(Option o) {
			if (o.O) Console.WriteLine("SET CONFIGURATION");
			var all = o.Username == null && o.Password == null;
			if (o.Input != null) { foreach (var l in Regex.Split(o.Input, "\r\n|\r|\n")) ConfigSet(o, l); }
			ConfigSet(o, "username", o.Username, all);
			ConfigSet(o, "password", o.Password, all);
		}

		private static void OptionGet(Option o, string k, ref string v, bool iall = false) {
			if (!o.O) return;
			Console.Write("{0}: {1}", k.ToCapital(), iall ? "\n" : "");
			if (v == null) v = iall ? Console.In.ReadToEnd() : Console.ReadLine();
			else Console.WriteLine(v);
		}

		private static Option OptionDefs(Option o) {
			if (o.Input == null && !o.O) o.Input = Console.In.ReadToEnd();
			if (o.Username == null) o.Username = Environment.GetEnvironmentVariable(ERoot + "USERNAME");
			if (o.Password == null) o.Password = Environment.GetEnvironmentVariable(ERoot + "PASSWORD");
			if (o.Username == null) o.Username = Config.Get("username");
			if (o.Password == null) o.Password = Config.Get("password");
			return o;
		}

		private static void ConfigGet(Option o, string k) {
			if (k == "") return;
			if (o.O) Console.WriteLine("{0}: {1}", k.ToCapital(), Config.Get(k));
			else Console.WriteLine("{0}={1}", k, Config.Get(k));
		}

		private static void ConfigSet(Option o, string kv) {
			var p = kv.Split('=');
			ConfigSet(o, p[0], p.Length > 1 ? p[1] : "");
		}

		private static void ConfigSet(Option o, string k, string v, bool inp = false) {
			if (k == "" || (v == null && !inp)) return;
			if (!o.O) { Config.Set(k, v); return; }
			Console.Write("{0}: ", k.ToCapital());
			if (v == null) v = Console.ReadLine();
			else Console.WriteLine(v);
			Config.Set(k, v);
		}
	}
}
