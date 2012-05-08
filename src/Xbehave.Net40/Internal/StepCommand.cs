﻿// <copyright file="StepCommand.cs" company="Adam Ralph">
//  Copyright (c) Adam Ralph. All rights reserved.
// </copyright>

namespace Xbehave.Internal
{
    using System;
    using Xunit.Sdk;

    internal class StepCommand : CommandBase
    {
        private readonly Step step;
        private readonly Action<IDisposable> handleResult;

        public StepCommand(MethodCall call, int? contextOrdinal, int ordinal, Step step, Action<IDisposable> handleResult)
            : base(call, contextOrdinal, ordinal, string.Concat("\"", step.Name, "\""))
        {
            this.step = step;
            this.handleResult = handleResult;
        }

        public override MethodResult Execute(object testClass)
        {
            if (this.step.Skip)
            {
                return new SkipResult(this.testMethod, this.DisplayName, this.step.SkipReason);
            }

            this.handleResult(this.step.Execute());
            return new PassedResult(this.testMethod, this.DisplayName);
        }
    }
}