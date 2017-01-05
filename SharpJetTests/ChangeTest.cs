using System;

namespace SharpJetTests
{
    using Hbm.Devices.Jet;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;

    [TestFixture]
    class ChangeTest
    {
        private bool changeCallbackCalled;
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
        public void ChangeTestSuccess() {
            JValue stateValue = new JValue(42);

            JObject addMessage = peer.AddState(TestJetConnection.successPath, stateValue, OnChange, ResponseCallback, 3000);
            JObject changeMessage = peer.Change(TestJetConnection.successPath, stateValue, ResponseCallback, 3000);

            Assert.True(this.responseCallbackCalled, "ChangeCallback was not called");
            Assert.True(this.responseSuccess, "Changecallback was completed successfully");
        }

        [Test]
        public void ChangeNotOnOwnState() {
            Assert.Throws<ArgumentException>(delegate {
                JValue stateValue = new JValue(42);
                JObject changeMessage = peer.Change(TestJetConnection.successPath, stateValue, ResponseCallback, 3000);
            }, "Change a state wich not the state of himself");

        }

        private void OnConnect(bool completed) { }

        private JToken OnChange(string path, JToken newValue) {
            
            return null;
        }

        private void ResponseCallback(bool completed, JToken response) {
            responseCallbackCalled = true;
            responseSuccess = completed;
            
        }
    }
}
