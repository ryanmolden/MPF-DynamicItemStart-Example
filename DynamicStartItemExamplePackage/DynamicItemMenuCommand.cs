﻿using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;

namespace ExampleInc.DynamicStartItemExamplePackage
{
    /// <summary>
    /// Handles DynamicItemStart items by delegating the dynamic matching of a given id to this object to a user
    /// supplied predicate.
    /// </summary>
    class DynamicItemMenuCommand : OleMenuCommand
    {
        private Predicate<int> matches;

        public DynamicItemMenuCommand(CommandID rootId, Predicate<int> matches, EventHandler invokeHandler, EventHandler beforeQueryStatusHandler) : base(invokeHandler, null /*changeHandler*/, beforeQueryStatusHandler, rootId)
        {
            if (matches == null)
            {
                throw new ArgumentNullException("matches");
            }

            this.matches = matches;
        }

        public override bool DynamicItemMatch(int cmdId)
        {
            //Call our supplied predicate to test if the given cmdId is 'good', if so remember the command id
            //in MatchedCommandid (for use by any BeforeQueryStatus handlers) and then return that it is a match,
            //otherwise clear our any previously remembered matched command id and return that it is not a match.
            if (this.matches(cmdId)) 
            {
                this.MatchedCommandId = cmdId;
                return true;
            }

            this.MatchedCommandId = 0;
            return false;
        }
    }
}
