﻿using System;
using Microsoft.Extensions.Logging;

namespace Gelf.Extensions.Logging
{
    public class GelfLoggerProvider : ILoggerProvider
    {
        private readonly GelfLoggerOptions _options;
        private readonly GelfMessageProcessor _messageProcessor;
        private readonly IDisposable _gelfClient;

        public GelfLoggerProvider(GelfLoggerOptions options)
        {
            var gelfClient = new UdpGelfClient(options);

            _options = options;
            _gelfClient = gelfClient;
            _messageProcessor = new GelfMessageProcessor(gelfClient);
            _messageProcessor.Start();
        }

        public ILogger CreateLogger(string name)
        {
            return new GelfLogger(name, _messageProcessor, _options);
        }

        public void Dispose()
        {
            _messageProcessor.Stop();
            _gelfClient.Dispose();
        }
    }
}
