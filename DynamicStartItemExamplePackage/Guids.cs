// Guids.cs
// MUST match guids.h
using System;

namespace ExampleInc.DynamicStartItemExamplePackage
{
    static class GuidList
    {
        public const string guidDynamicStartItemExamplePackagePkgString = "19d301f8-ea0a-459d-aa0e-1ddfadff33d6";
        public const string guidDynamicStartItemExamplePackageCmdSetString = "75222791-3179-4902-8830-64211a64061d";
        public const string guidToolWindowPersistanceString = "995c34bf-44cf-4f1b-8eaa-fb11766f5b28";

        public static readonly Guid guidDynamicStartItemExamplePackageCmdSet = new Guid(guidDynamicStartItemExamplePackageCmdSetString);
    };
}