// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

namespace CnSharp.Updater.Server.Logging
{
    public class NullLogger
        : ILogger
    {
        public void Log(LogLevel level, string message, params object[] args)
        {
        }
    }
}