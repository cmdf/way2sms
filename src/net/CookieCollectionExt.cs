using System.Net;


namespace src.net {
	static class CookieCollectionExt {

		// Methods
		public static CookieCollection Add(this CookieCollection o, string cookies) {
			var cps = cookies.Split(';');
			foreach(var c in cps[0].Split(',')) {
				var kv = c.Split('=');
				if (kv.Length < 2) continue;
				o.Add(new Cookie(kv[0], kv[1]));
			}
			return o;
		}
	}
}
