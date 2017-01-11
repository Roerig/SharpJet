using System;

namespace SharpJetTests
{
    using Hbm.Devices.Jet;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;

    [TestFixture]
    class ChangeTest
    {
        private bool responseSuccess;
        private bool responseCallbackCalled;
        private JetPeer peer;
        
        [SetUp]
        public void SetUp() {
            var connection = new TestJetConnection();
            peer = new JetPeer(connection);
            peer.Connect(OnConnect, 3000);
        }

        [Test]
        public void ChangeSuccessTest() {
            JValue stateValue = new JValue(42);

            JObject addMessage = peer.AddState(TestJetConnection.DEFAULT_SUCCESS_PATH, stateValue, OnChangeState, ResponseCallback, 3000);
            JObject changeMessage = peer.Change(TestJetConnection.DEFAULT_SUCCESS_PATH, stateValue, ResponseCallback, 3000);

            Assert.True(this.responseCallbackCalled, "ChangeCallback was not called");
            Assert.True(this.responseSuccess, "Changecallback was completed successfully");
        }

        [Test]
        public void ChangeNotOnOwnStateTest() {
            Assert.Throws<ArgumentException>(delegate {
                JValue stateValue = new JValue(42);
                JObject changeMessage = peer.Change(TestJetConnection.DEFAULT_SUCCESS_PATH, stateValue, ResponseCallback, 3000);
            }, "Change a state that is owned by the peer");
        }

        private void OnConnect(bool completed) { }

        private JToken OnChangeState(string path, JToken newValue) {

            Console.WriteLine("Change-Event is called \n path: " + path);
            return null;
        }

        private void ResponseCallback(bool completed, JToken response) {
            responseCallbackCalled = true;
            responseSuccess = completed;
            
        }
    }
}
