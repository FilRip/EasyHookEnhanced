// EasyHook (File: EasyHook\DllImport.cs)
//
// Copyright (c) 2009 Christoph Husse & Copyright (c) 2015 Justin Stenning
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// Please visit https://easyhook.github.io for more information
// about the project and latest updates.

using System;
using System.Runtime.InteropServices;

namespace EasyHook
{
    static class NativeApiX86
    {
        private const string DllName = "EasyHook32.dll";

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern string RtlGetLastErrorStringCopy();

        public static string RtlGetLastErrorString()
        {
            return RtlGetLastErrorStringCopy();
        }

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int RtlGetLastError();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern void LhUninstallAllHooks();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhInstallHook(
            IntPtr InEntryPoint,
            IntPtr InHookProc,
            IntPtr InCallback,
            IntPtr OutHandle);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhUninstallHook(IntPtr RefHandle);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhWaitForPendingRemovals();


        /*
            Setup the ACLs after hook installation. Please note that every
            hook starts suspended. You will have to set a proper ACL to
            make it active!
        */
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhSetInclusiveACL(
                    [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)]
                    int[] InThreadIdList,
                    int InThreadCount,
                    IntPtr InHandle);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhSetExclusiveACL(
                    [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)]
                    int[] InThreadIdList,
                    int InThreadCount,
                    IntPtr InHandle);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhSetGlobalInclusiveACL(
                    [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
                    int[] InThreadIdList,
                    int InThreadCount);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhSetGlobalExclusiveACL(
                    [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)]
                    int[] InThreadIdList,
                    int InThreadCount);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhIsThreadIntercepted(
                    IntPtr InHandle,
                    int InThreadID,
                    out bool OutResult);

        /*
            The following barrier methods are meant to be used in hook handlers only!

            They will all fail with STATUS_NOT_SUPPORTED if called outside a
            valid hook handler...
        */
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhBarrierGetCallback(out IntPtr OutValue);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhBarrierGetReturnAddress(out IntPtr OutValue);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhBarrierGetAddressOfReturnAddress(out IntPtr OutValue);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhBarrierBeginStackTrace(out IntPtr OutBackup);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhBarrierEndStackTrace(IntPtr OutBackup);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhBarrierGetCallingModule(out IntPtr OutValue);

        /*
            Debug helper API.
        */
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int DbgAttachDebugger();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int DbgGetThreadIdByHandle(
            IntPtr InThreadHandle,
            out int OutThreadId);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int DbgGetProcessIdByHandle(
            IntPtr InProcessHandle,
            out int OutProcessId);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int DbgHandleToObjectName(
            IntPtr InNamedHandle,
            IntPtr OutNameBuffer,
            int InBufferSize,
            out int OutRequiredSize);


        /*
            Injection support API.
        */
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern int RhInjectLibrary(
            int InTargetPID,
            int InWakeUpTID,
            int InInjectionOptions,
            string InLibraryPath_x86,
            string InLibraryPath_x64,
            IntPtr InPassThruBuffer,
            int InPassThruSize);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int RhIsX64Process(
            int InProcessId,
            out bool OutResult);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern bool RhIsAdministrator();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int RhGetProcessToken(int InProcessId, out IntPtr OutToken);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern int RtlInstallService(
            string InServiceName,
            string InExePath,
            string InChannelName);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int RhWakeUpProcess();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern int RtlCreateSuspendedProcess(
           string InEXEPath,
           string InCommandLine,
            int InProcessCreationFlags,
           out int OutProcessId,
           out int OutThreadId);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern int RhInstallDriver(
           string InDriverPath,
           string InDriverName);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int RhInstallSupportDriver();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern bool RhIsX64System();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GacCreateContext();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern void GacReleaseContext(ref IntPtr RefContext);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern bool GacInstallAssembly(
            IntPtr InContext,
            string InAssemblyPath,
            string InDescription,
            string InUniqueID);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern bool GacUninstallAssembly(
            IntPtr InContext,
            string InAssemblyName,
            string InDescription,
            string InUniqueID);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern int LhGetHookBypassAddress(IntPtr handle, out IntPtr address);
    }

    static class NativeApiX64
    {
        private const string DllName = "EasyHook64.dll";

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern string RtlGetLastErrorStringCopy();

        public static string RtlGetLastErrorString()
        {
            return RtlGetLastErrorStringCopy();
        }

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int RtlGetLastError();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern void LhUninstallAllHooks();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhInstallHook(
            IntPtr InEntryPoint,
            IntPtr InHookProc,
            IntPtr InCallback,
            IntPtr OutHandle);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhUninstallHook(IntPtr RefHandle);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhWaitForPendingRemovals();


        /*
            Setup the ACLs after hook installation. Please note that every
            hook starts suspended. You will have to set a proper ACL to
            make it active!
        */
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhSetInclusiveACL(
                    [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
                    int[] InThreadIdList,
                    int InThreadCount,
                    IntPtr InHandle);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhSetExclusiveACL(
                    [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
                    int[] InThreadIdList,
                    int InThreadCount,
                    IntPtr InHandle);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhSetGlobalInclusiveACL(
                    [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
                    int[] InThreadIdList,
                    int InThreadCount);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhSetGlobalExclusiveACL(
                    [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
                    int[] InThreadIdList,
                    int InThreadCount);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhIsThreadIntercepted(
                    IntPtr InHandle,
                    int InThreadID,
                    out bool OutResult);

        /*
            The following barrier methods are meant to be used in hook handlers only!

            They will all fail with STATUS_NOT_SUPPORTED if called outside a
            valid hook handler...
        */
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhBarrierGetCallback(out IntPtr OutValue);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhBarrierGetReturnAddress(out IntPtr OutValue);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhBarrierGetAddressOfReturnAddress(out IntPtr OutValue);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhBarrierBeginStackTrace(out IntPtr OutBackup);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhBarrierEndStackTrace(IntPtr OutBackup);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int LhBarrierGetCallingModule(out IntPtr OutValue);

        /*
            Debug helper API.
        */
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int DbgAttachDebugger();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int DbgGetThreadIdByHandle(
            IntPtr InThreadHandle,
            out int OutThreadId);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int DbgGetProcessIdByHandle(
            IntPtr InProcessHandle,
            out int OutProcessId);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int DbgHandleToObjectName(
            IntPtr InNamedHandle,
            IntPtr OutNameBuffer,
            int InBufferSize,
            out int OutRequiredSize);


        /*
            Injection support API.
        */
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern int RhInjectLibrary(
            int InTargetPID,
            int InWakeUpTID,
            int InInjectionOptions,
            string InLibraryPath_x86,
            string InLibraryPath_x64,
            IntPtr InPassThruBuffer,
            int InPassThruSize);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int RhIsX64Process(
            int InProcessId,
            out bool OutResult);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern bool RhIsAdministrator();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int RhGetProcessToken(int InProcessId, out IntPtr OutToken);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern int RtlInstallService(
            string InServiceName,
            string InExePath,
            string InChannelName);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern int RtlCreateSuspendedProcess(
           string InEXEPath,
           string InCommandLine,
            int InProcessCreationFlags,
           out int OutProcessId,
           out int OutThreadId);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int RhWakeUpProcess();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern int RhInstallDriver(
           string InDriverPath,
           string InDriverName);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int RhInstallSupportDriver();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern bool RhIsX64System();



        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GacCreateContext();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern void GacReleaseContext(ref IntPtr RefContext);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern bool GacInstallAssembly(
            IntPtr InContext,
            string InAssemblyPath,
            string InDescription,
            string InUniqueID);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern bool GacUninstallAssembly(
            IntPtr InContext,
            string InAssemblyName,
            string InDescription,
            string InUniqueID);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern int LhGetHookBypassAddress(IntPtr handle, out IntPtr address);
    }

    public static class NativeApi
    {
        public const int MAX_HOOK_COUNT = 1024;
        public const int MAX_ACE_COUNT = 128;
        public readonly static bool Is64Bit = IntPtr.Size == 8;

        [DllImport("kernel32.dll")]
        internal static extern int GetCurrentThreadId();

        [DllImport("kernel32.dll")]
        internal static extern void CloseHandle(IntPtr InHandle);

        [DllImport("kernel32.dll")]
        internal static extern int GetCurrentProcessId();

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        internal static extern IntPtr GetProcAddress(IntPtr InModule, string InProcName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr LoadLibrary(string InPath);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr GetModuleHandle(string InPath);

        [DllImport("kernel32.dll")]
        internal static extern short RtlCaptureStackBackTrace(
            int InFramesToSkip,
            int InFramesToCapture,
            IntPtr OutBackTrace,
            IntPtr OutBackTraceHash);

        public const int STATUS_SUCCESS = unchecked(0);
        public const int STATUS_INVALID_PARAMETER = unchecked((int)0xC000000DL);
        public const int STATUS_INVALID_PARAMETER_1 = unchecked((int)0xC00000EFL);
        public const int STATUS_INVALID_PARAMETER_2 = unchecked((int)0xC00000F0L);
        public const int STATUS_INVALID_PARAMETER_3 = unchecked((int)0xC00000F1L);
        public const int STATUS_INVALID_PARAMETER_4 = unchecked((int)0xC00000F2L);
        public const int STATUS_INVALID_PARAMETER_5 = unchecked((int)0xC00000F3L);
        public const int STATUS_NOT_SUPPORTED = unchecked((int)0xC00000BBL);

        public const int STATUS_INTERNAL_ERROR = unchecked((int)0xC00000E5L);
        public const int STATUS_INSUFFICIENT_RESOURCES = unchecked((int)0xC000009AL);
        public const int STATUS_BUFFER_TOO_SMALL = unchecked((int)0xC0000023L);
        public const int STATUS_NO_MEMORY = unchecked((int)0xC0000017L);
        public const int STATUS_WOW_ASSERTION = unchecked((int)0xC0009898L);
        public const int STATUS_ACCESS_DENIED = unchecked((int)0xC0000022L);

        private static string ComposeString()
        {
            return string.Format("{0} (Code: {1})", RtlGetLastErrorString(), RtlGetLastError());
        }

        internal static void Force(int InErrorCode)
        {
            switch (InErrorCode)
            {
                case STATUS_SUCCESS: return;
                case STATUS_INVALID_PARAMETER: throw new ArgumentException("STATUS_INVALID_PARAMETER: " + ComposeString());
                case STATUS_INVALID_PARAMETER_1: throw new ArgumentException("STATUS_INVALID_PARAMETER_1: " + ComposeString());
                case STATUS_INVALID_PARAMETER_2: throw new ArgumentException("STATUS_INVALID_PARAMETER_2: " + ComposeString());
                case STATUS_INVALID_PARAMETER_3: throw new ArgumentException("STATUS_INVALID_PARAMETER_3: " + ComposeString());
                case STATUS_INVALID_PARAMETER_4: throw new ArgumentException("STATUS_INVALID_PARAMETER_4: " + ComposeString());
                case STATUS_INVALID_PARAMETER_5: throw new ArgumentException("STATUS_INVALID_PARAMETER_5: " + ComposeString());
                case STATUS_NOT_SUPPORTED: throw new NotSupportedException("STATUS_NOT_SUPPORTED: " + ComposeString());
                case STATUS_INTERNAL_ERROR: throw new EasyHookException("STATUS_INTERNAL_ERROR: " + ComposeString());
                case STATUS_INSUFFICIENT_RESOURCES: throw new InsufficientMemoryException("STATUS_INSUFFICIENT_RESOURCES: " + ComposeString());
                case STATUS_BUFFER_TOO_SMALL: throw new ArgumentException("STATUS_BUFFER_TOO_SMALL: " + ComposeString());
                case STATUS_NO_MEMORY: throw new EasyHookException("STATUS_NO_MEMORY: " + ComposeString());
                case STATUS_WOW_ASSERTION: throw new EasyHookException("STATUS_WOW_ASSERTION: " + ComposeString());
                case STATUS_ACCESS_DENIED: throw new AccessViolationException("STATUS_ACCESS_DENIED: " + ComposeString());

                default: throw new EasyHookException("Unknown error code (" + InErrorCode + "): " + ComposeString());
            }
        }

        public static int RtlGetLastError()
        {
            if (Is64Bit)
                return NativeApiX64.RtlGetLastError();
            else
                return NativeApiX86.RtlGetLastError();
        }

        public static string RtlGetLastErrorString()
        {
            if (Is64Bit)
                return NativeApiX64.RtlGetLastErrorStringCopy();
            else
                return NativeApiX86.RtlGetLastErrorStringCopy();
        }

        public static void LhUninstallAllHooks()
        {
            if (Is64Bit)
                NativeApiX64.LhUninstallAllHooks();
            else
                NativeApiX86.LhUninstallAllHooks();
        }

        public static void LhInstallHook(
            IntPtr InEntryPoint,
            IntPtr InHookProc,
            IntPtr InCallback,
            IntPtr OutHandle)
        {
            if (Is64Bit)
                Force(NativeApiX64.LhInstallHook(InEntryPoint, InHookProc, InCallback, OutHandle));
            else
                Force(NativeApiX86.LhInstallHook(InEntryPoint, InHookProc, InCallback, OutHandle));
        }

        public static void LhUninstallHook(IntPtr RefHandle)
        {
            if (Is64Bit)
                Force(NativeApiX64.LhUninstallHook(RefHandle));
            else
                Force(NativeApiX86.LhUninstallHook(RefHandle));
        }

        public static void LhWaitForPendingRemovals()
        {
            if (Is64Bit)
                Force(NativeApiX64.LhWaitForPendingRemovals());
            else
                Force(NativeApiX86.LhWaitForPendingRemovals());
        }

        public static void LhIsThreadIntercepted(
                    IntPtr InHandle,
                    int InThreadID,
                    out bool OutResult)
        {
            if (Is64Bit)
                Force(NativeApiX64.LhIsThreadIntercepted(InHandle, InThreadID, out OutResult));
            else
                Force(NativeApiX86.LhIsThreadIntercepted(InHandle, InThreadID, out OutResult));
        }

        public static void LhSetInclusiveACL(
                    int[] InThreadIdList,
                    int InThreadCount,
                    IntPtr InHandle)
        {
            if (Is64Bit)
                Force(NativeApiX64.LhSetInclusiveACL(InThreadIdList, InThreadCount, InHandle));
            else
                Force(NativeApiX86.LhSetInclusiveACL(InThreadIdList, InThreadCount, InHandle));
        }

        public static void LhSetExclusiveACL(
                    int[] InThreadIdList,
                    int InThreadCount,
                    IntPtr InHandle)
        {
            if (Is64Bit)
                Force(NativeApiX64.LhSetExclusiveACL(InThreadIdList, InThreadCount, InHandle));
            else
                Force(NativeApiX86.LhSetExclusiveACL(InThreadIdList, InThreadCount, InHandle));
        }

        public static void LhSetGlobalInclusiveACL(
                    int[] InThreadIdList,
                    int InThreadCount)
        {
            if (Is64Bit)
                Force(NativeApiX64.LhSetGlobalInclusiveACL(InThreadIdList, InThreadCount));
            else
                Force(NativeApiX86.LhSetGlobalInclusiveACL(InThreadIdList, InThreadCount));
        }

        public static void LhSetGlobalExclusiveACL(
                    int[] InThreadIdList,
                    int InThreadCount)
        {
            if (Is64Bit)
                Force(NativeApiX64.LhSetGlobalExclusiveACL(InThreadIdList, InThreadCount));
            else
                Force(NativeApiX86.LhSetGlobalExclusiveACL(InThreadIdList, InThreadCount));
        }

        public static void LhBarrierGetCallingModule(out IntPtr OutValue)
        {
            if (Is64Bit)
                Force(NativeApiX64.LhBarrierGetCallingModule(out OutValue));
            else
                Force(NativeApiX86.LhBarrierGetCallingModule(out OutValue));
        }

        public static void LhBarrierGetCallback(out IntPtr OutValue)
        {
            if (Is64Bit)
                Force(NativeApiX64.LhBarrierGetCallback(out OutValue));
            else
                Force(NativeApiX86.LhBarrierGetCallback(out OutValue));
        }

        public static void LhBarrierGetReturnAddress(out IntPtr OutValue)
        {
            if (Is64Bit)
                Force(NativeApiX64.LhBarrierGetReturnAddress(out OutValue));
            else
                Force(NativeApiX86.LhBarrierGetReturnAddress(out OutValue));
        }

        public static void LhBarrierGetAddressOfReturnAddress(out IntPtr OutValue)
        {
            if (Is64Bit)
                Force(NativeApiX64.LhBarrierGetAddressOfReturnAddress(out OutValue));
            else
                Force(NativeApiX86.LhBarrierGetAddressOfReturnAddress(out OutValue));
        }

        public static void LhBarrierBeginStackTrace(out IntPtr OutBackup)
        {
            if (Is64Bit)
                Force(NativeApiX64.LhBarrierBeginStackTrace(out OutBackup));
            else
                Force(NativeApiX86.LhBarrierBeginStackTrace(out OutBackup));
        }

        public static void LhBarrierEndStackTrace(IntPtr OutBackup)
        {
            if (Is64Bit)
                Force(NativeApiX64.LhBarrierEndStackTrace(OutBackup));
            else
                Force(NativeApiX86.LhBarrierEndStackTrace(OutBackup));
        }

        public static void LhGetHookBypassAddress(IntPtr handle, out IntPtr address)
        {
            if (Is64Bit)
                Force(NativeApiX64.LhGetHookBypassAddress(handle, out address));
            else
                Force(NativeApiX86.LhGetHookBypassAddress(handle, out address));
        }

        public static void DbgAttachDebugger()
        {
            if (Is64Bit)
                Force(NativeApiX64.DbgAttachDebugger());
            else
                Force(NativeApiX86.DbgAttachDebugger());
        }

        public static void DbgGetThreadIdByHandle(
            IntPtr InThreadHandle,
            out int OutThreadId)
        {
            if (Is64Bit)
                Force(NativeApiX64.DbgGetThreadIdByHandle(InThreadHandle, out OutThreadId));
            else
                Force(NativeApiX86.DbgGetThreadIdByHandle(InThreadHandle, out OutThreadId));
        }

        public static void DbgGetProcessIdByHandle(
            IntPtr InProcessHandle,
            out int OutProcessId)
        {
            if (Is64Bit)
                Force(NativeApiX64.DbgGetProcessIdByHandle(InProcessHandle, out OutProcessId));
            else
                Force(NativeApiX86.DbgGetProcessIdByHandle(InProcessHandle, out OutProcessId));
        }

        public static void DbgHandleToObjectName(
            IntPtr InNamedHandle,
            IntPtr OutNameBuffer,
            int InBufferSize,
            out int OutRequiredSize)
        {
            if (Is64Bit)
                Force(NativeApiX64.DbgHandleToObjectName(InNamedHandle, OutNameBuffer, InBufferSize, out OutRequiredSize));
            else
                Force(NativeApiX86.DbgHandleToObjectName(InNamedHandle, OutNameBuffer, InBufferSize, out OutRequiredSize));
        }

        public const int EASYHOOK_INJECT_DEFAULT = 0x00000000;
        public const int EASYHOOK_INJECT_MANAGED = 0x00000001;

        public static int RhInjectLibraryEx(
            int InTargetPID,
            int InWakeUpTID,
            int InInjectionOptions,
            string InLibraryPath_x86,
            string InLibraryPath_x64,
            IntPtr InPassThruBuffer,
            int InPassThruSize)
        {
            if (Is64Bit)
                return NativeApiX64.RhInjectLibrary(InTargetPID, InWakeUpTID, InInjectionOptions,
                    InLibraryPath_x86, InLibraryPath_x64, InPassThruBuffer, InPassThruSize);
            else
                return NativeApiX86.RhInjectLibrary(InTargetPID, InWakeUpTID, InInjectionOptions,
                    InLibraryPath_x86, InLibraryPath_x64, InPassThruBuffer, InPassThruSize);
        }

        public static void RhInjectLibrary(
            int InTargetPID,
            int InWakeUpTID,
            int InInjectionOptions,
            string InLibraryPath_x86,
            string InLibraryPath_x64,
            IntPtr InPassThruBuffer,
            int InPassThruSize)
        {
            if (Is64Bit)
                Force(NativeApiX64.RhInjectLibrary(InTargetPID, InWakeUpTID, InInjectionOptions,
                    InLibraryPath_x86, InLibraryPath_x64, InPassThruBuffer, InPassThruSize));
            else
                Force(NativeApiX86.RhInjectLibrary(InTargetPID, InWakeUpTID, InInjectionOptions,
                    InLibraryPath_x86, InLibraryPath_x64, InPassThruBuffer, InPassThruSize));
        }

        public static void RtlCreateSuspendedProcess(
           string InEXEPath,
           string InCommandLine,
            int InProcessCreationFlags,
           out int OutProcessId,
           out int OutThreadId)
        {
            if (Is64Bit)
                Force(NativeApiX64.RtlCreateSuspendedProcess(InEXEPath, InCommandLine, InProcessCreationFlags,
                    out OutProcessId, out OutThreadId));
            else
                Force(NativeApiX86.RtlCreateSuspendedProcess(InEXEPath, InCommandLine, InProcessCreationFlags,
                    out OutProcessId, out OutThreadId));
        }

        public static void RhIsX64Process(
            int InProcessId,
            out bool OutResult)
        {
            if (Is64Bit)
                Force(NativeApiX64.RhIsX64Process(InProcessId, out OutResult));
            else
                Force(NativeApiX86.RhIsX64Process(InProcessId, out OutResult));
        }

        public static bool RhIsAdministrator()
        {
            if (Is64Bit)
                return NativeApiX64.RhIsAdministrator();
            else
                return NativeApiX86.RhIsAdministrator();
        }

        public static void RhGetProcessToken(int InProcessId, out IntPtr OutToken)
        {
            if (Is64Bit)
                Force(NativeApiX64.RhGetProcessToken(InProcessId, out OutToken));
            else
                Force(NativeApiX86.RhGetProcessToken(InProcessId, out OutToken));
        }

        public static void RhWakeUpProcess()
        {
            if (Is64Bit)
                Force(NativeApiX64.RhWakeUpProcess());
            else
                Force(NativeApiX86.RhWakeUpProcess());
        }

        public static void RtlInstallService(
            string InServiceName,
            string InExePath,
            string InChannelName)
        {
            if (Is64Bit)
                Force(NativeApiX64.RtlInstallService(InServiceName, InExePath, InChannelName));
            else
                Force(NativeApiX86.RtlInstallService(InServiceName, InExePath, InChannelName));
        }

        public static void RhInstallDriver(
           string InDriverPath,
           string InDriverName)
        {
            if (Is64Bit)
                Force(NativeApiX64.RhInstallDriver(InDriverPath, InDriverName));
            else
                Force(NativeApiX86.RhInstallDriver(InDriverPath, InDriverName));
        }

        public static void RhInstallSupportDriver()
        {
            if (Is64Bit)
                Force(NativeApiX64.RhInstallSupportDriver());
            else
                Force(NativeApiX86.RhInstallSupportDriver());
        }

        public static bool RhIsX64System()
        {
            if (Is64Bit)
                return NativeApiX64.RhIsX64System();
            else
                return NativeApiX86.RhIsX64System();
        }

        public static void GacInstallAssemblies(
            string[] InAssemblyPaths,
            string InDescription,
            string InUniqueID)
        {
            try
            {
                System.GACManagedAccess.AssemblyCache.InstallAssemblies(
                    InAssemblyPaths,
                    new System.GACManagedAccess.InstallReference(System.GACManagedAccess.InstallReferenceGuid.OpaqueGuid, InUniqueID, InDescription),
                    System.GACManagedAccess.EAssemblyCommit.Force);
            }
            catch (Exception e)
            {
                throw new EasyHookException("Unable to install assemblies to GAC, see inner exception for details", e);
            }
        }

        public static void GacUninstallAssemblies(
            string[] InAssemblyNames,
            string InDescription,
            string InUniqueID)
        {
            try
            {
                System.GACManagedAccess.AssemblyCache.UninstallAssemblies(
                    InAssemblyNames,
                    new System.GACManagedAccess.InstallReference(System.GACManagedAccess.InstallReferenceGuid.OpaqueGuid, InUniqueID, InDescription),
                    out System.GACManagedAccess.AssemblyCacheUninstallDisposition[] results);

                // disable warnings Obsolete and Obsolete("message")
#pragma warning disable 612, 618
                for (int i = 0; i < InAssemblyNames.Length; i++)
                    Config.PrintComment("GacUninstallAssembly: Assembly {0}, uninstall result {1}", InAssemblyNames[i], results[i]);
                // enable warnings for Obsolete and Obsolete("message")
#pragma warning restore 612, 618
            }
            catch (Exception e)
            {
                throw new EasyHookException("Unable to uninstall assemblies from GAC, see inner exception for details", e);
            }
        }
    }
}
