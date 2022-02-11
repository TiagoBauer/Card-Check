using System;
using System.Collections.Generic;
using System.Linq;

namespace CardVerify
{
    class Program
    {
        static void Main(string[] args)
        {
            var cards = new List<Int64>();
            long card = 0;
            int processamento = 0;
            string cardNumber = "";
            do
            {
                Console.Clear();
                Console.WriteLine("Olá, \n Indique o processo que será executado:");
                Console.WriteLine("1 - Incluir cartão na lista");
                Console.WriteLine("2 - Listar cartões cadastrados");
                Console.WriteLine("3 - Executar registros de controle");
                Console.WriteLine("0 - Finalizar execução");
                processamento = int.Parse(Console.ReadLine());
                switch (processamento)
                {
                    case 1:
                        Console.WriteLine("Informe o código do cartão");
                        cardNumber = Console.ReadLine();
                        try
                        {
                            card = long.Parse(cardNumber.Replace(" ", ""));
                            if (incluiCartao(cards, card))
                            {
                                Console.WriteLine("Cartão inserido, precione enter para retornar ao menu!");
                                Console.ReadLine();
                            }
                        }
                        catch (Exception) {
                            Console.WriteLine("Erro na inserção do cartão, precione enter para retornar ao menu!");
                            Console.ReadLine();
                        }
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("--------- Catões Inseridos ---------- \n\n" + listaCartoes(cards) + "\n\n Precione enter para retornar ao menu!");
                        Console.ReadLine();
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("--------- Catões Teste ---------- \n\n" + listaCartoesTeste() + "\n\n Precione enter para retornar ao menu!");
                        Console.ReadLine();
                        break;
                    case 0:
                        break;
                    default:
                        Console.WriteLine("Comando inválido, precione enter para retornar ao menu!");
                        Console.ReadLine();
                        break;
                }
                
            } while (processamento > 0);
        }
        private static bool incluiCartao(List<long> cards, long card) {
            try
            {
                cards.Add(card);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        private static string listaCartoes(List<long> cards) {
            var cartoes = "";            

            foreach (long card in cards) {
                cartoes = cartoes + validaBandeira(card) + card.ToString() + validaCartao(card) + "\n";
            }
            
            return cartoes;
        }

        private static string validaBandeira(long card) {
            // BANDEIRA   | COMEÇO DO CARTÃO | COMPRIMENTO
            // AMEX       | 34 ou 37         | 15
            // Discover   | 6011             | 16
            // MasterCard | 51 ao 55         | 16
            // Visa       | 4                | 13 ou 16
            // VALIDA BANDEIRA

            string cartaoString = card.ToString();
            string inicio = cartaoString.Substring(0, 4);
            if (((inicio.Substring(0, 2) == "34") || (inicio.Substring(0, 2) == "37")) && (cartaoString.Length == 15))
            {
                return "AMEX........: ";
            }
            else if ((inicio == "6011") && (cartaoString.Length == 16))
            {
                return "DISCOVER....: ";
            }
            else if (((int.Parse(inicio.Substring(0,2)) >= 51) && (int.Parse(inicio.Substring(0, 2)) <= 55)) && (cartaoString.Length == 16))
            {
                return "MASTERCARD..: ";
            }
            else if ((inicio.Substring(0, 1) == "4") && ((cartaoString.Length == 13) || (cartaoString.Length == 16)))
            {
                return "VISA........: ";
            }
            else
            {
                return "DESCONHECIDO: ";
            }
        }

        private static string validaCartao(long card) {
            //1. Tome uma sequência de números inteiros positivos e a inverta.
            //2. Começando pelo segundo número, dobre o valor de cada número de forma alternada("24145... = "28185...).
            //3. Para dígitos maiores que 9 será necessário que some cada dígito("10", 1 + 0 = 1) ou subtraia por 9("10", 10 - 9 = 1).
            //4. Some todos os números da sequência.
            //5. Se o total for múltiplo de 10, o número é válido.
            string inverte = new string (card.ToString().Reverse().ToArray());
            char[] numerosCartao = inverte.Trim().ToCharArray();
            int dobra = 1;
            int somaValor = 0;
            foreach (char number in numerosCartao) {
                int auxNum = 0;
                if (int.TryParse(number.ToString(), out auxNum))
                {
                    if (dobra % 2 == 0)
                    {
                        auxNum = auxNum * 2;
                        if (auxNum > 9)
                        {
                            auxNum -= 9;
                        }
                    }
                    dobra++;
                    somaValor += auxNum;
                }                
            }

            if (somaValor % 10 == 0)
            {
                return "(Válido)";
            }
            else {
                return "(Inválido)";
            }

        }

        public static string listaCartoesTeste() {
            var testCards = new List<Int64>();

            testCards.Add(4111111111111111); 
            testCards.Add(4111111111111);
            testCards.Add(4012888888881881);
            testCards.Add(378282246310005);
            testCards.Add(6011111111111117);
            testCards.Add(5105105105105100);
            testCards.Add(5105105105105106);
            testCards.Add(9111111111111111);

            return listaCartoes(testCards);
        }
    }
}
