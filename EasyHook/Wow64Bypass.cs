// EasyHook (File: EasyHook\WOW64Bypass.cs)
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

using System.Diagnostics;
using System.Threading;

namespace EasyHook
{
    internal static class Wow64Bypass
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
                    string ChannelName = RemoteHooking.GenerateName();
                    string SvcExecutablePath = (Config.DependencyPath.Length > 0 ? Config.DependencyPath : Config.GetProcessPath()) + Config.GetWOW64BypassExecutableName();

                    Process Proc = new();
                    ProcessStartInfo StartInfo = new(
                            SvcExecutablePath, "\"" + ChannelName + "\"");

                    // create sync objects
                    EventWaitHandle Listening = new(
                        false,
                        EventResetMode.ManualReset,
                        "Global\\Event_" + ChannelName);

                    m_TermMutex = new Mutex(true, "Global\\Mutex_" + ChannelName);

                    // start and connect program
                    StartInfo.CreateNoWindow = true;
                    StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                    Proc.StartInfo = StartInfo;

                    Proc.Start();

                    if (!Listening.WaitOne(5000, true))
                        throw new EasyHookException("Unable to wait for service application due to timeout.");

                    HelperServiceInterface Interface = RemoteHooking.IpcConnectClient<HelperServiceInterface>(ChannelName);

                    Interface.Ping();

                    m_Interface = Interface;
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
                true,
                InRequireStrongName,
                false,
                InPassThruArgs);
        }
    }
}
