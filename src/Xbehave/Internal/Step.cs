﻿// <copyright file="Step.cs" company="Adam Ralph">
//  Copyright (c) Adam Ralph. All rights reserved.
// </copyright>

namespace Xbehave.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class Step : StepBase<Action>
    {
        public Step(string message, Action action)
            : base(message, action)
        {
        }

        public void Execute()
        {
            if (this.MillisecondsTimeout > 0)
            {
                var result = this.Action.BeginInvoke(null, null);

                // NOTE: we do not call the WaitOne(int) overload because it wasn't introduced until .NET 3.5 SP1 and we want to support pre-SP1
                if (!result.AsyncWaitHandle.WaitOne(this.MillisecondsTimeout, false))
                {
                    throw new Xunit.Sdk.TimeoutException(this.MillisecondsTimeout);
                }
                else
                {
                    this.Action.EndInvoke(result);
                }
            }
            else
            {
                this.Action();
            }
        }
    }
}