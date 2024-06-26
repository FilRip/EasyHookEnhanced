﻿// EasyHook (File: EasyHook\ServiceMgmt.cs)
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

using System.IO;
using System.Threading;

namespace EasyHook;

internal static class ServiceMgmt
{
#pragma warning disable IDE0052, S4487 // Supprimer les membres privés non lus
    private static Mutex m_TermMutex = null;
#pragma warning restore IDE0052, S4487 // Supprimer les membres privés non lus
    private static HelperServiceInterface m_Interface = null;
    private static readonly object ThreadSafe = new();

    private static void Install()
    {
        lock (ThreadSafe)
        {
            // Ensure we create a new one if the existing
            // channel cannot be pinged
            try
            {
                m_Interface?.Ping();
            }
            catch
            {
                m_Interface = null;
            }

            if (m_Interface == null)
            {
                // create sync objects
                string ChannelName = RemoteHooking.GenerateName();
                EventWaitHandle Listening = new(
                    false,
                    EventResetMode.ManualReset,
                    "Global\\Event_" + ChannelName);
                Mutex TermMutex = new(true, "Global\\Mutex_" + ChannelName);

                using (TermMutex)
                {
                    // install and start service
                    NativeApi.RtlInstallService(
                        "EasyHook" + (NativeApi.Is64Bit ? "64" : "32") + "Svc",
                        Path.GetFullPath(Config.GetDependantSvcExecutableName()),
                        ChannelName);

                    if (!Listening.WaitOne(5000, true))
                        throw new EasyHookException("Unable to wait for service startup.");

                    HelperServiceInterface Interface = RemoteHooking.IpcConnectClient<HelperServiceInterface>(ChannelName);

                    Interface.Ping();

                    // now we can be sure that all things are fine...
                    m_Interface = Interface;
                    m_TermMutex = TermMutex;
                }
            }
        }
    }

    public static void Inject(
        int InHostPID,
        int InTargetPID,
        int InWakeUpTID,
        int InNativeOptions,
        string InLibraryPath_x86,
        string InLibraryPath_x64,
        bool InRequireStrongName,
        params object[] InPassThruArgs)
    {
        Install();

        m_Interface.InjectEx(
            InHostPID,
            InTargetPID,
            InWakeUpTID,
            InNativeOptions,
            InLibraryPath_x86,
            InLibraryPath_x64,
            false,
            false,
            InRequireStrongName,
            false,
            InPassThruArgs);
    }

    public static object ExecuteAsService<TClass>(
            string InMethodName,
            params object[] InParams)
    {
        Install();

        return m_Interface.ExecuteAsService<TClass>(InMethodName, InParams);
    }
}
