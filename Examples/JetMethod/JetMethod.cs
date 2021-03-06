﻿// <copyright file="JetMethod.cs" company="Hottinger Baldwin Messtechnik GmbH">
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

namespace JetExamples
{
    using System;
    using System.Net;
    using Hbm.Devices.Jet;
    using Newtonsoft.Json.Linq;

    public class JetMethod
    {
        private const string MethodName = "theMethod";
        private JetPeer peer;

        private JetMethod()
        {
            // var connection = new WebSocketJetConnection("wss://172.19.1.1");
            // var connection = new WebSocketJetConnection("ws://172.19.1.1:11123");
            IPAddress[] ips;
            ips = Dns.GetHostAddresses("172.19.1.1");
            var connection = new SocketJetConnection(ips[0], 11122);
            this.peer = new JetPeer(connection);
            this.peer.Connect(this.OnConnect, 5000);
        }

        public static void Main(string[] args)
        {
            var method = new JetMethod();
            Console.ReadKey(true);
            method.Stop();
            Console.ReadKey(true);
        }

        internal void Stop()
        {
            this.peer.RemoveMethod(MethodName, this.RemoveMethodCallback, 5000);
            this.peer.Disconnect();
        }

        private void OnConnect(bool completed)
        {
            if (completed)
            {
                Console.WriteLine("Successfully connected to Jet daemon!");
                JValue stateValue = new JValue(12);
                this.peer.AddMethod(MethodName, this.MethodCallback, this.AddResponseCallback, 5000);
            }
            else
            {
                Console.WriteLine("Connection to Jet daemon failed!");
            }
        }

        private JToken MethodCallback(string path, JToken value)
        {
            return new JValue(42);
        }

        private void AddResponseCallback(bool completed, JToken response)
        {
            Console.WriteLine("Method \"" + MethodName + "\" successfully added!");
        }

        private void RemoveMethodCallback(bool completed, JToken response)
        {
            Console.WriteLine("Method \"" + MethodName + "\" successfully removed!");
        }
    }
}
