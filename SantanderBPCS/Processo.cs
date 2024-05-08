using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using SantanderBPCS.BPCS.Model;
using SantanderBPCS.Repository;

namespace SantanderBPCS
{
    public class Processo
    {
        private readonly IRepositoryBase _repositoryBase;

        
        public Processo(IRepositoryBase repositoryBase)
        {
            _repositoryBase = repositoryBase;
        }

        public void ExecutaProcesso()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("pt-BR");
            //Busca diretório parametrizado
            var NaoProcessadas = ConfigurationManager.AppSettings["NaoProcessadas"];
            var ProcessadasOk = ConfigurationManager.AppSettings["ProcessadasOk"];
            //Obtém instância do App Service através da injeção de depedência
                
            //Processa os arquivos no diretório
            ProcessarArquivos(NaoProcessadas, ProcessadasOk);
            
            
        }

        public void ProcessarArquivos(string NaoProcessadas, string Processadas)
        {
            var Files = System.IO.Directory.GetFiles(NaoProcessadas);

            var encodingCaracter = ConfigurationManager.AppSettings["encoding"].ToString();

            var library = ConfigurationManager.AppSettings["library"].ToString();
            var tabelaBPCS = ConfigurationManager.AppSettings["tabela"].ToString();

            foreach (var arq in Files)
            {
                foreach (var linha in File.ReadAllLines(arq, System.Text.Encoding.GetEncoding(encodingCaracter)).Distinct())
                {
                    if (!string.IsNullOrEmpty(linha) && !string.IsNullOrWhiteSpace(linha))
                    {
                        var item = new Ret
                        {
                            RETRE1 = linha.Substring(0, 200),
                            RETRE2 = linha.Substring(200, 200)
                        };

                        
                        _repositoryBase.Execute($@"insert into {library}.{tabelaBPCS} (RETRE1, RETRE2) values ('{item.RETRE1}', '{item.RETRE2}')");

                    }
                }

                if (!Directory.Exists(Processadas))
                    Directory.CreateDirectory(Processadas);

                if (File.Exists(arq))
                    File.Move(arq, Processadas + "\\" + Path.GetFileName(arq));
            }
        }
    }
}
