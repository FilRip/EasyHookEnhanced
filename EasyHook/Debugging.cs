// EasyHook (File: EasyHook\Debugging.cs)
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
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace EasyHook
{
    public partial class LocalHook
    {
        private static readonly NameBuffer Buffer = new();

        /// <summary>
        /// RIP relocation is disabled by default. If you want to enable it,
        /// just call this method which will attach a debugger to the current
        /// process. There may be circumstances under which this might fail
        /// and this is why it is not done by default. On 32-Bit system this
        /// method will always succeed and do nothing...
        /// </summary>
        public static void EnableRIPRelocation()
        {
            NativeApi.DbgAttachDebugger();
        }

        /// <summary>
        /// Tries to get the underlying thread ID for a given handle.
        /// </summary>
        /// <remarks>
        /// This is not always possible. The handle has to be opened with <c>THREAD_QUERY_INFORMATION</c>
        /// access. 
        /// </remarks>
        /// <param name="InThreadHandle">A valid thread handle.</param>
        /// <returns>A valid thread ID associated with the given thread handle.</returns>
        /// <exception cref="AccessViolationException">
        /// The given handle was not opened with <c>THREAD_QUERY_INFORMATION</c> access.</exception>
        /// <exception cref="ArgumentException">
        /// The handle is invalid. 
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Should never occur and just notifies you that a handle to thread ID conversion is not
        /// available on the current platform.
        /// </exception>
        public static int GetThreadIdByHandle(IntPtr InThreadHandle)
        {

            NativeApi.DbgGetThreadIdByHandle(InThreadHandle, out int Result);

            return Result;
        }

        /// <summary>
        /// Tries to get the underlying process ID for a given handle.
        /// </summary>
        /// <remarks>
        /// This is not always possible. The handle has to be opened with <c>PROCESS_QUERY_INFORMATION</c>
        /// access. 
        /// </remarks>
        /// <param name="InProcessHandle">A valid process handle.</param>
        /// <returns>A valid process ID associated with the given process handle.</returns>
        /// <exception cref="AccessViolationException">
        /// The given handle was not opened with <c>PROCESS_QUERY_INFORMATION</c> access.</exception>
        /// <exception cref="ArgumentException">
        /// The handle is invalid. 
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Should never occur and just notifies you that a handle to thread ID conversion is not
        /// available on the current platform.
        /// </exception>
        public static int GetProcessIdByHandle(IntPtr InProcessHandle)
        {

            NativeApi.DbgGetProcessIdByHandle(InProcessHandle, out int Result);

            return Result;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private sealed class UnicodeString
        {
            public short Length;
            public short MaximumLength;
            public string privateBuffer;
        }

        internal class NameBuffer : CriticalFinalizerObject
        {
            public IntPtr internalBuffer;
            public int Size;

            public NameBuffer()
            {
                Size = 0;
                internalBuffer = Marshal.AllocCoTaskMem(0);
            }

            public void Alloc(int InDesiredSize)
            {
                IntPtr Tmp;

                if (InDesiredSize < Size)
                    return;

                if ((Tmp = Marshal.ReAllocCoTaskMem(internalBuffer, InDesiredSize)) == IntPtr.Zero)
                    throw new EasyHookException("Unable to allocate unmanaged memory.");

                internalBuffer = Tmp;
                Size = InDesiredSize;
            }

            ~NameBuffer()
            {
                if (internalBuffer != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(internalBuffer);

                internalBuffer = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Reads the kernel object name for a given windows usermode handle.
        /// Executes in approx. 100 micro secounds.
        /// </summary>
        /// <remarks><para>
        /// This allows you to translate a handle back to the associated filename for example.
        /// But keep in mind that such names are only valid for kernel service routines, like
        /// <c>NtCreateFile</c>. You won't have success when calling <c>CreateFile</c> on such
        /// object names! The regular windows user mode API has some methods that will allow
        /// you to convert such kernelmode names back into usermode names. I know this because I did it
        /// some years ago but I've already forgotten how it has to be done! I can only give you
        /// some hints: <c>FindFirstVolume()</c>, <c>FindFirstVolumeMountPoint()</c>,
        /// <c>QueryDosDevice()</c>, <c>GetVolumePathNamesForVolumeName()</c>
        /// </para>
        /// <param name="InHandle">A valid usermode handle.</param>
        /// </remarks>
        /// <returns>The kernel object name associated with the given handle.</returns>
        /// <exception cref="ArgumentException">
        /// The given handle is invalid or could not be accessed for unknown reasons.
        /// </exception>
        public static string GetNameByHandle(IntPtr InHandle)
        {

            NativeApi.DbgHandleToObjectName(
                InHandle,
                IntPtr.Zero,
                0,
                out int RequiredSize);


            lock (Buffer)
            {
                Buffer.Alloc(RequiredSize + 1);

                NativeApi.DbgHandleToObjectName(
                    InHandle,
                    Buffer.internalBuffer,
                    RequiredSize,
                    out RequiredSize);

                UnicodeString Result = new();

                Marshal.PtrToStructure(Buffer.internalBuffer, Result);

                return Result.privateBuffer;
            }
        }
    }
}
