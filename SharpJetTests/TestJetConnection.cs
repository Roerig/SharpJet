// <copyright file="TestJetConnection.cs" company="Hottinger Baldwin Messtechnik GmbH">
//
// SharpJet, a library to communicate with Jet IPC.
//
// The MIT License (MIT)
//
// Copyright (C) Hottinger Baldwin Messtechnik GmbH
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
// BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
// ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
// </copyright>

namespace Hbm.Devices.Jet
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;

    public enum Behaviour
    {
        ConnectionFail,
        ConnectionSuccess
    }

    public class TestJetConnection : IJetConnection
    {
        public const string DEFAULT_SUCCESS_PATH = "success";
        private Behaviour behaviour;

        public List<string> messages;

        public TestJetConnection(Behaviour behaviour = Behaviour.ConnectionSuccess)
        {
            this.behaviour = behaviour;
            this.messages = new List<string>();
        }

        public event EventHandler<StringEventArgs> HandleIncomingMessage;

        public void Connect(Action<bool> completed, double timeoutMs)
        {
            switch (this.behaviour)
            {
                case Behaviour.ConnectionFail:
                    completed(false);
                    break;

                case Behaviour.ConnectionSuccess:
                default:
                    completed(true);
                    break;
            }
        }


        public void SendMessage(string message)
        {
            messages.Add(message);
            JToken json = JToken.Parse(message);

            JToken method = json["method"];
    
            JToken parameters = json["params"];
            JToken path = parameters["path"];

            if (path.ToString().Contains(DEFAULT_SUCCESS_PATH)) {
                EmitCallbackMessage(json);
            }
        }

        private void EmitCallbackMessage(JToken json) {
            JObject response = new JObject();
            response["jsonrpc"] = "2.0";
            response["id"] = json["id"];
      
            response["result"] = true;

            //switch (json["method"].ToString()) {
            //    case "fetch":
            //        response["fetchOnly"] = true;
            //        response["event"] = "change";
            //        response["value"] = 4242;
            //        break;

            //    default: // response
            //        response["result"] = true;
            //        break;
            //}

            HandleIncomingMessage(this, new StringEventArgs(JsonConvert.SerializeObject(response)));
        }

        public void Disconnect()
        {
        }
    }
}
