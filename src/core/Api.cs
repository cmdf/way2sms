using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using src.net;

namespace src.core {
	class Api {

		// Constants
		private static readonly Uri URoot = new Uri("http://site24.way2sms.com");
		private static readonly Uri ULogin = new Uri(URoot, "Login1.action");
		private static readonly Uri USend = new Uri(URoot, "smstoss.action");
		private static readonly string HContentType = "application/x-www-form-urlencoded";
		private static readonly string CNRoot = "JSESSIONID";
		private static readonly string CCountry = "91";
		private static readonly int LMsgMax = 140;

		// Data
		public Cookie CRoot;


		// Constructors
		public Api(Cookie c) {
			if (c == null) throw new ArgumentException("Bad cookie: \"\"");
			CRoot = c;
		}

		public Api(string usr, string pwd) {
			var t = (HttpWebRequest)WebRequest.Create(ULogin);
			t.Method = "POST";
			t.ContentType = HContentType;
			t.AllowAutoRedirect = false;
			using (var st = new StreamWriter(t.GetRequestStream()))
				st.WriteLine("username={0}&password={1}", usr, pwd);
			var r = (HttpWebResponse)t.GetResponse().UpdateCookies();
			if (r.Cookies.Count > 0) CRoot = r.Cookies[CNRoot];
			else throw new WebException(string.Format("Bad login: username=\"{0}\"", usr));
		}


		// Methods
		public void Send(string to, string msg) {
			for (var i = 0; i < msg.Length; i += LMsgMax)
				SendOne(to, msg.Substring(i, i + LMsgMax < msg.Length ? LMsgMax : msg.Length - i));
		}
		
		public void SendOne(string to, string msg) {
			var t = (HttpWebRequest)WebRequest.Create(USend);
			t.Method = "POST";
			t.ContentType = HContentType;
			t.AllowAutoRedirect = false;
			t.CookieContainer = new CookieContainer();
			t.CookieContainer.Add(USend, CRoot);
			var i = CRoot.Value.IndexOf('~');
			var tkn = CRoot.Value.Substring(i + 1);
			to = Regex.Replace(to, @"\D", "");
			to = to.Length > 10 ? to : CCountry + to;
			var lmsg = LMsgMax - msg.Length;
			using (var st = new StreamWriter(t.GetRequestStream()))
				st.WriteLine("Token={0}&mobile={1}&name=&ssaction=qs&message={2}&msgLen={3}", tkn, to, msg, lmsg);
			var r = (HttpWebResponse)t.GetResponse();
			var hl = r.Headers["Location"];
			if (hl != null && hl.Contains("Token")) return;
			throw new WebException(string.Format("Bad cookie: \"{0}={1}\"", CRoot.Name, CRoot.Value));
		}
	}
}
