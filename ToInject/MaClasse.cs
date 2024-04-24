using System;
using System.Threading;
using System.Windows.Forms;

using EasyHook;

namespace ToInject;

public class MaClasse : IEntryPoint
{
    private readonly InterfaceEchange _interface;

#pragma warning disable IDE0060 // Supprimer le paramètre inutilisé
    public MaClasse(RemoteHooking.IContext context, string channelName)
#pragma warning restore IDE0060 // Supprimer le paramètre inutilisé
    {
        if (!string.IsNullOrWhiteSpace(channelName))
        {
            _interface = RemoteHooking.IpcConnectClient<InterfaceEchange>(channelName);
        }
    }

#pragma warning disable IDE0060 // Supprimer le paramètre inutilisé
    public void Run(RemoteHooking.IContext context, string channelName)
#pragma warning restore IDE0060 // Supprimer le paramètre inutilisé
    {
        _interface.Present();
        _interface.Retour(AppDomain.CurrentDomain.FriendlyName);
        if (Application.OpenForms.Count > 0)
            foreach (Form window in Application.OpenForms)
                _interface.Retour("Fenetre " + window.Name + " ouverte");
        else
            _interface.Retour("Aucune fenetre trouvée");
        while (true)
        {
            try
            {
                _interface.Ping();
                Thread.Sleep(1000);
            }
            catch
            {
                break;
            }
        }
        _interface.Quitte();
        LocalHook.Release();
    }
}
