// EasyHook (File: EasyHookSvc\InjectionService.cs)
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
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Ipc;
using System.Security.Principal;
using System.ServiceProcess;
using System.Threading;

using EasyHook;

namespace EasyHookSvc
{
    public partial class InjectionService : ServiceBase
    {

        public InjectionService(string InServiceName)
        {
            InitializeComponent();

            ServiceName = InServiceName;
        }

        private void LogError(string InMessage)
        {
            this.EventLog.WriteEntry(
                "[EasyHookSvc]: " + InMessage,
                EventLogEntryType.Error);
        }

        protected override void OnStart(string[] args)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(OnExecute), args);
        }

        public void OnExecute(object InParam)
        {
            string[] args = (string[])InParam;

            try
            {
                string ChannelName = args[0];
                Mutex TermMutex = Mutex.OpenExisting("Global\\Mutex_" + ChannelName);
                WellKnownSidType SidType = WellKnownSidType.BuiltinAdministratorsSid;

                if (!RemoteHooking.IsAdministrator)
                    SidType = WellKnownSidType.WorldSid;

                IpcServerChannel Channel = RemoteHooking.IpcCreateServer<HelperServiceInterface>(
                                                ref ChannelName,
                                                WellKnownObjectMode.SingleCall,
                                                SidType);

                // signal that service is listening
                EventWaitHandle Listening = EventWaitHandle.OpenExisting("Global\\Event_" + ChannelName);

                Listening.Set();

                try
                {
                    TermMutex.WaitOne();
                }
                catch
                {
                    // Nothing to do
                }

                // cleanup library...
                Channel.StopListening(null);

#pragma warning disable S1215 // "GC.Collect" should not be called
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
#pragma warning restore S1215 // "GC.Collect" should not be called

            }
            catch (Exception e)
            {
                LogError(e.ToString());
            }
            finally
            {
                Stop();
            }
        }

        protected override void OnStop()
        {
            // Nothing to do
        }
    }
}
