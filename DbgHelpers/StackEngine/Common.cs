using System;

namespace DbgHelpers.StackEngine
{
    public enum Platform:byte
    {
        x86 = 0,
        x64
    }

    public static class StackHelper
    {
        public const byte FunctionNameStart = 40;
        public const byte FrameStop = 2;
        public const string NewStackTag = "Id: ";
        public const byte ThreadIDStart = 11;
        public const char ThreadSeparator = '.';
    }




}
