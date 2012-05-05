﻿// <copyright file="ThreadContext.cs" company="Adam Ralph">
//  Copyright (c) Adam Ralph. All rights reserved.
// </copyright>

namespace Xbehave.Internal
{
    using System;
    using System.Collections.Generic;
    using Xbehave.Infra;
    using Xunit.Sdk;

    internal static class ThreadContext
    {
        private static ICommandFactory commandFactory = new CommandFactory(new Disposer());

        [ThreadStatic]
        private static Scenario scenario;

        public static Scenario Scenario
        {
            get { return scenario ?? (scenario = new Scenario(commandFactory)); }
        }

        // NOTE: I've tried to move this into Scenario, with the finally block clearing the steps but it just doesn't seem to work
        public static IEnumerable<ITestCommand> CreateTestCommands(MethodCall call, Action registerSteps)
        {
            try
            {
                registerSteps();
                return Scenario.GetTestCommands(call);
            }
            finally
            {
                scenario = null;
            }
        }
    }
}