using System.Net;


namespace App.net {
	static class WebResponseExt {

		// Methods
		public static WebResponse UpdateCookies(this WebResponse o) {
			var z = (HttpWebResponse)o;
			var h = o.Headers["Set-Cookie"];
			if (h == null) return o;
			z.Cookies.Add(h);
			return o;
		}
	}
}
