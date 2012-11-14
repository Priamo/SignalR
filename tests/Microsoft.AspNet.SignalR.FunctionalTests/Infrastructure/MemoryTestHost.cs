﻿using System;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Client.Transports;
using Microsoft.AspNet.SignalR.Hosting.Memory;

namespace Microsoft.AspNet.SignalR.FunctionalTests.Infrastructure
{
    public class MemoryTestHost : ITestHost
    {
        private readonly MemoryHost _host;

        public MemoryTestHost(MemoryHost host)
        {
            _host = host;
        }

        public string Url
        {
            get
            {
                return "http://memoryhost";
            }
        }

        public IClientTransport Transport { get; set; }

        public void Initialize(int? keepAlive,
                               int? connectonTimeOut,
                               int? hearbeatInterval,
                               bool enableAutoRejoiningGroups)
        {
            if (keepAlive != null)
            {
                _host.Configuration.KeepAlive = TimeSpan.FromSeconds(keepAlive.Value);
            }
            else
            {
                _host.Configuration.KeepAlive = null;
            }

            if (connectonTimeOut != null)
            {
                _host.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(connectonTimeOut.Value);
            }

            if (hearbeatInterval != null)
            {
                _host.Configuration.HeartbeatInterval = TimeSpan.FromSeconds(hearbeatInterval.Value);
            }

            if (enableAutoRejoiningGroups)
            {
                _host.HubPipeline.EnableAutoRejoiningGroups();
            }

            _host.MapHubs();
            _host.MapConnection<MyBadConnection>("/ErrorsAreFun");
            _host.MapConnection<MyGroupEchoConnection>("/group-echo");
            _host.MapConnection<MySendingConnection>("/multisend");
            _host.MapConnection<MyReconnect>("/my-reconnect");
            _host.MapConnection<MyGroupConnection>("/groups");
            _host.MapConnection<MyRejoinGroupsConnection>("/rejoin-groups");
            _host.MapConnection<FilteredConnection>("/filter");
        }

        public void Dispose()
        {
            _host.Dispose();
        }
    }
}
