using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Launchpal.Website.IAP
{
    public sealed class RSAPKCS1SHA256SignatureDescription : SignatureDescription
    {
        public RSAPKCS1SHA256SignatureDescription()
        {
            base.KeyAlgorithm = typeof(RSACryptoServiceProvider).FullName;
            base.DigestAlgorithm = typeof(SHA256Managed).FullName;
            base.FormatterAlgorithm = typeof(RSAPKCS1SignatureFormatter).FullName;
            base.DeformatterAlgorithm = typeof(RSAPKCS1SignatureDeformatter).FullName;
        }

        public override AsymmetricSignatureDeformatter CreateDeformatter(AsymmetricAlgorithm key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var deformatter = new RSAPKCS1SignatureDeformatter(key);
            deformatter.SetHashAlgorithm("SHA256");
            return deformatter;
        }

        public override AsymmetricSignatureFormatter CreateFormatter(AsymmetricAlgorithm key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var formatter = new RSAPKCS1SignatureFormatter(key);
            formatter.SetHashAlgorithm("SHA256");
            return formatter;
        }

    }

    internal class ReceiptVerification
    {

        // Utility function to read the bytes from an HTTP response
        private static int ReadResponseBytes(byte[] responseBuffer, Stream resStream)
        {
            var count = 0;

            var numBytesRead = 0;
            int numBytesToRead = responseBuffer.Length;

            do
            {
                count = resStream.Read(responseBuffer, numBytesRead, numBytesToRead);

                numBytesRead += count;
                numBytesToRead -= count;

            } while (count > 0);

            return numBytesRead;
        }

        public static X509Certificate2 RetrieveCertificate(string certificateId)
        {
            const int maxCertificateSize = 10000;

            // We are attempting to retrieve the following url. The getAppReceiptAsync website at 
            // http://msdn.microsoft.com/library/windows/apps/windows.applicationmodel.store.currentapp.getappreceiptasync.aspx
            // lists the following format for the certificate url.
            var certificateUrl = $"https://go.microsoft.com/fwlink/?LinkId=246509&cid={certificateId}";

            // Make an HTTP GET request for the certificate
            var request = (HttpWebRequest)WebRequest.Create(certificateUrl);
            request.Method = "GET";

            var response = (HttpWebResponse)request.GetResponse();

            // Retrieve the certificate out of the response stream
            var responseBuffer = new byte[maxCertificateSize];
            var resStream = response.GetResponseStream();
            int bytesRead = ReadResponseBytes(responseBuffer, resStream);

            if (bytesRead < 1)
            {
                return null;
                //TODO: Handle error here
            }

            return new X509Certificate2(responseBuffer);
        }

        private static bool ValidateXml(XmlDocument receipt, X509Certificate2 certificate)
        {
            // Create the signed XML object.
            var sxml = new SignedXml(receipt);

            // Get the XML Signature node and load it into the signed XML object.
            var dsig = receipt.GetElementsByTagName("Signature", SignedXml.XmlDsigNamespaceUrl)[0];
            if (dsig == null)
            {
                // If signature is not found return false
                return false;
            }

            sxml.LoadXml((XmlElement)dsig);

            // Check the signature
            bool isValid = sxml.CheckSignature(certificate, true);

            return isValid;
        }

        /// <summary>
        /// Validates if a receipt is valid from a purchase
        /// </summary>
        /// <param name="xmlDoc">Receipt in XML format</param>
        /// <returns>True if valid, false if validation fails or encounter error</returns>
        internal static bool VerifyReceiptIsValid(XmlDocument xmlDoc)
        {
            // .NET does not support SHA256-RSA2048 signature verification by default, so register this algorithm for verification
            CryptoConfig.AddAlgorithm(typeof(RSAPKCS1SHA256SignatureDescription), "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256");

            // The certificateId attribute is present in the document root, retrieve it
            XmlNode node = xmlDoc.DocumentElement;
            if (node?.Attributes != null)
            {
                string certificateId = node.Attributes["CertificateId"].Value;

                // Retrieve the certificate from the official site.
                // NOTE: For sake of performance, you would want to cache this certificate locally.
                //       Otherwise, every single call will incur the delay of certificate retrieval.
                var verificationCertificate = RetrieveCertificate(certificateId);

                if (verificationCertificate == null)
                    return false;

                try
                {
                    // Validate the receipt with the certificate retrieved earlier
                    return ValidateXml(xmlDoc, verificationCertificate);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
