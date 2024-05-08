using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;
using SantanderBPCS.Repository;

namespace SantanderBPCS
{
    class Program
    {
        public static void Main(string[] args)
        {
            var rep = new Processamento();

            Processo processo = new Processo(rep);

            int intervaloExecucaoMinutos = Convert.ToInt32(ConfigurationManager.AppSettings["IntervaloExecucaoMinutos"]);

            // Execute o serviço
            processo.ExecutaProcesso();

            // Aguarde o intervalo de tempo definido
            Thread.Sleep(TimeSpan.FromMinutes(intervaloExecucaoMinutos));
            
        }
    }
}
