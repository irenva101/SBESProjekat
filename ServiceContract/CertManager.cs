
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ServiceContract
{
     public class CertManager
    {
		/// <summary>
		/// Get a certificate with the specified subject name from the predefined certificate storage
		/// Only valid certificates should be considered
		/// </summary>
		/// <param name="storeName"></param>
		/// <param name="storeLocation"></param>
		/// <param name="subjectName"></param>
		/// <returns> The requested certificate. If no valid certificate is found, returns null. </returns>
		public static X509Certificate2 GetCertificateFromStorage(StoreName storeName, StoreLocation storeLocation, string subjectName)
		{
			X509Store store = new X509Store(storeName, storeLocation);
			store.Open(OpenFlags.ReadOnly);

			X509Certificate2Collection certCollection = store.Certificates.Find(X509FindType.FindBySubjectName, subjectName, true); //proverava da li ovde ima cert koji se zove isto kao subjectName

			/// Check whether the subjectName of the certificate is exactly the same as the given "subjectName"
			foreach (X509Certificate2 c in certCollection)
			{
				if (c.SubjectName.Name.Equals(string.Format("CN={0}", subjectName)))
				{
					return c;
				}
			}

			return null;
		}


		public static bool GenerateCACertificate(string subjectName)
        {

			string proces1 = "/c makecert -sv " + subjectName + ".pvk -iv SbesCA.pvk -n \"CN=" + subjectName + "\" -pe -ic SbesCA.cer " + subjectName + ".cer -sr localmachine -ss My -sky exchange";
			System.Diagnostics.Process.Start("cmd.exe", proces1).WaitForExit();

			string proces2 = "/c pvk2pfx.exe /pvk " + subjectName + ".pvk /pi 1234 /spc " + subjectName + ".cer /pfx " + subjectName + ".pfx"; //sifra od pfx-a je subjectName
			System.Diagnostics.Process.Start("cmd.exe", proces2).WaitForExit();

			// generate AES key
			using (AesManaged aes = new AesManaged())
			{
				File.WriteAllBytes(subjectName + ".key", aes.Key);
				File.WriteAllBytes(subjectName + ".IV", aes.IV);
			}

			Console.ReadLine();

			return true;
		}




		

	 }
}
