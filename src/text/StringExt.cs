using System;


namespace src.text {
	static class StringExt {

		// Methods
		public static string ToCapital(this string o) {
			return o.Length > 0 ? Char.ToUpper(o[0]) + o.Substring(1).ToLower() : "";
		}
	}
}
