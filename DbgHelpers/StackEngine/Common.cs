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
        public static byte FunctionNameStart = 40;
        public static byte FrameStop = 2;
        public static string NewStackTag = "Id: ";
        public static byte ThreadIDStart = 11;
        public static char ThreadSeparator = '.';
    }




}
