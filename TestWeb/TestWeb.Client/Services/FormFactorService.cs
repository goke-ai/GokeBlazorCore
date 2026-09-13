using Goke.Core.Interfaces;

namespace TestWeb.Client.Services
{
    public class FormFactorService : IFormFactor
    {
        public string GetFormFactor()
        {
            return "WebAssembly";
        }

        public string GetPlatform()
        {
            return Environment.OSVersion.ToString();
        }
    }
}
