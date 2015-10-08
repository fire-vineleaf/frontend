using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using vllib;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {

        public string _localhost = "http://localhost/vineleaf/backend/";
        public string _localPassword = "hallo";

        [TestMethod]
        public void CanCreateMessage()
        {
            // a
            vllib.vllib lib = new vllib.vllib(_localhost, "player2@hallo.de", _localPassword);
            SimpleJSON.JSONNode message;
            // a
            message = lib.createMessage("12", "hallo", "was geht");
            // a
            Assert.AreNotEqual("", message["messageId"].Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ApiException))]
        public void CannotCreateMessageAuthError()
        {
            vllib.vllib lib = new vllib.vllib(_localhost, "player2@hallo.de", _localPassword+"bad");
            lib.createMessage("12", "hallo", "was geht");
        }

    }
}
