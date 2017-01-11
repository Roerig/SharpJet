

namespace SharpJetTests
{
    using Hbm.Devices.Jet;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;

    [TestFixture]
    class FetchTest
    {
        private JetPeer peer;
        private bool addResponseCallbackCalled = false;
        private bool addSuccess = false;

        private TestJetConnection connection;
        

        [SetUp]
        public void SetUp() {
            connection = new TestJetConnection();
            peer = new JetPeer(connection);
            peer.Connect(OnConnect, 3000);
        }

        [Test]
        public void SimpleFetchSuccessTest() {
            FetchId id = new FetchId(1);
            Matcher matcher = new Matcher();
            matcher.Contains = TestJetConnection.DEFAULT_SUCCESS_PATH;
            JObject fetchMessage = peer.Fetch(out id, matcher, ValueChanged, AddResponseCallback, 3000);

            Assert.True(addResponseCallbackCalled, "Add-Response is never called");
            Assert.True(addSuccess, "Add-response is not successfully");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void ValueChanged(JToken obj) {
            // TODO: Implements a methodology to test the value-change-callback
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="completed"></param>
        /// <param name="response"></param>
        private void AddResponseCallback(bool completed, JToken response) {
            addSuccess = completed;
            addResponseCallbackCalled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        private JToken OnChangeState(string path, JToken newValue) {
            return null;
        }

        private void OnConnect(bool completed) {

        }
    }
}
