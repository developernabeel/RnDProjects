using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SBIReportUtility.Common.General;

namespace SBIReportUtility.Test
{
    [TestClass]
    public class EncryptionHelperTests
    {
        [TestMethod]
        public void EncryptTest()
        {
            string textToEncrypt = "Foobar";
            string encryptedText = EncryptionHelper.Encrypt(textToEncrypt);
            Assert.AreEqual(encryptedText, "lHiB4v9/v42CoCE5klBOVA==");
        }

        [TestMethod]
        public void DecryptTest()
        {
            string textToDecrypt = "lHiB4v9/v42CoCE5klBOVA==";
            string decryptedText = EncryptionHelper.Decrypt(textToDecrypt);
            Assert.AreEqual(decryptedText, "Foobar");
        }

        [TestMethod]
        public void GetMD5HashTest()
        {
            string valueToMD5 = "Foobar";
            string md5Hash = EncryptionHelper.GetMD5Hash(valueToMD5);
            Assert.AreEqual(md5Hash, "89d5739baabbbe65be35cbe61c88e06d");
        }

        [TestMethod]
        public void VerifyMD5HashTest()
        {
            string valueToMD5 = "Foobar";
            string md5Hash = "89d5739baabbbe65be35cbe61c88e06d";
            bool isVerified = EncryptionHelper.VerifyMD5Hash(valueToMD5, md5Hash);

            Assert.IsTrue(isVerified);
        }
    }
}
