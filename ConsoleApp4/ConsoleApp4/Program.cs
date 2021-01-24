using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    using webServiceCalculo;
    class Program
    {
        static void Main(string[] args)
        {
            string nVlComprimento = "0";
            string nVlAltura = "0";
            string nVlLargura = "0";
            string nVlDiametro = "0";

            Console.WriteLine("Insira o CEP de origem:");
            string sCepOrigem = Console.ReadLine();
            Console.WriteLine("Insira o CEP de Destino:");
            string sCepDestino = Console.ReadLine();
            Console.WriteLine("Insira o Formato da Caixa (1 - Caixa, 2- Rolo, 3 - Envelope) :");
            string nCdFormato = Console.ReadLine();

            if (nCdFormato.Equals("1"))
            {
                Console.WriteLine("Insira a Comprimento");
                nVlComprimento = Console.ReadLine();
                Console.WriteLine("Insira a altura:");
                nVlAltura = Console.ReadLine();
                Console.WriteLine("Insira a largura: ");
                nVlLargura = Console.ReadLine();
            }
            else if (nCdFormato.Equals("2")) 
            {
                Console.WriteLine("Insira a altura:");
                nVlComprimento = Console.ReadLine();
                Console.WriteLine("Insira o diametro");
                nVlDiametro = Console.ReadLine();
            }
            Console.WriteLine("Insira o Peso da Caixa:");
            string nVlPeso = Console.ReadLine();
            Console.WriteLine("A encomenda será entregue com o serviço adicional mão própria ? (S - SIM, N - Não) :");
            string sCdMaoPropria = Console.ReadLine();  

            var resultadoCalculo = CalculoValorTotalServico(sCepOrigem, sCepDestino, nCdFormato, nVlComprimento, nVlAltura, nVlLargura, nVlDiametro, sCdMaoPropria, nVlPeso);

            if (resultadoCalculo.MsgErro == "")
            {
                Console.WriteLine("Valor: " + resultadoCalculo.Valor + " Prazo De Entrega:" + resultadoCalculo.PrazoDeEntrega + " dias apos a data de postagem");
                Console.ReadLine();
            }
            else 
            {
                Console.WriteLine("Erro: " + resultadoCalculo.MsgErro);
                Console.ReadLine();
            }
        }

        public static calcPreco CalculoValorTotalServico(string sCepOrigem, string sCepDestino, string nCdFormato, string nVlComprimento, string nVlAltura, string nVlLargura, string nVlDiametro, string sCdMaoPropria, string nVlPeso)
        {
            var calcPrecoVarivaeis = new calcPreco();
            var nCdEmpresa = "";
            var sDsSenha = "";
            var nCdServico = "04014"; //Sedex a vista    
            var nVlValorDeclarado = "0";
            var sCdAvisoRecebimento = "N";
           
            webServiceCalculo.CalcPrecoPrazoWSSoapClient client = new webServiceCalculo.CalcPrecoPrazoWSSoapClient();

            var resp = client.CalcPrecoPrazo(nCdEmpresa, sDsSenha, nCdServico, sCepOrigem, sCepDestino, nVlPeso, int.Parse(nCdFormato), decimal.Parse(nVlComprimento), decimal.Parse(nVlAltura), decimal.Parse(nVlLargura), decimal.Parse(nVlDiametro), sCdMaoPropria,decimal.Parse(nVlValorDeclarado), sCdAvisoRecebimento);
            
            calcPrecoVarivaeis.MsgErro = resp.Servicos[0].MsgErro;
            calcPrecoVarivaeis.Valor = resp.Servicos[0].Valor;
            calcPrecoVarivaeis.ValorMaoPropria = resp.Servicos[0].ValorMaoPropria;
            calcPrecoVarivaeis.PrazoDeEntrega = resp.Servicos[0].PrazoEntrega;

            return calcPrecoVarivaeis;
        }
    }
}

