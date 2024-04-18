using System;

namespace ToInject
{
    public class InterfaceEchange : MarshalByRefObject
    {
        public void Retour(object retour)
        {
            Console.WriteLine("Depuis l'autre process : " + retour.ToString());
        }

        public void Present()
        {
            Console.WriteLine("Inject present");
        }

        public void Quitte()
        {
            Console.WriteLine("Inject quitte");
        }

        public void Ping()
        {
            // Nothing to do
        }
    }
}
