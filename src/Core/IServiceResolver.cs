﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace CnSharp.Updater.Server
{
    public interface IServiceResolver
    {
        object Resolve(Type type);
    }
}