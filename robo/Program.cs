using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace robo
{
    public class Program
    {
        private const int tamanhoMatriz = 12;
        private const int baseLinha = 1;
        private const int baseColuna = 1;
        private static int[,] mapa = new int[tamanhoMatriz, tamanhoMatriz];

        public static void Main(string[] args)
        {
            //criar robo e iniciar mapa
            Robo robo = new Robo();
            Posicao posicaoAtual = new Posicao();
            int lixoLocal = 0;
            Console.WriteLine("Qual o nivel de sujeira da casa que voce quer? [entre 0 e 5]");
            int nivelSujeira = Convert.ToInt32(Console.ReadLine());
            if (nivelSujeira < 0)
                nivelSujeira = 0;
            else if (nivelSujeira > 5)
                nivelSujeira = 5;
            startMapa(mapa, nivelSujeira);
            robo.TamanhoMapa = (tamanhoMatriz - 2) * (tamanhoMatriz - 2);

            //setando base do robô 
            mapa[baseLinha, baseColuna] = 9;
            posicaoAtual.Coluna = baseColuna;
            posicaoAtual.Linha = baseLinha;
            /*
             -check = Ajustar coleta de lixo *** criar funcao que verifique a casa e quantidade lixo se pode pegar ou nao
             - Ajustar voltar pra base *** unico caso funcional é quando a base está no 0,0
             - Criar próximas passagens *** após recarregar, limpar lixo e/ou finalizar limpeza, recriar passagem
            */

            ImprimirMapa(mapa, posicaoAtual);
            int direcao = -1;
            int continueProgram = 1;
            while (continueProgram != 0)
            {
                while (robo.Bateria >= 25 && !robo.AmbienteLimpo() && !robo.lixeiraCheia())
                {

                    mapa[posicaoAtual.Linha, posicaoAtual.Coluna] = lixoLocal; //limpando casa anterior
                    (posicaoAtual, direcao, lixoLocal) = robo.Mover(new Posicao(posicaoAtual.Linha - baseLinha, posicaoAtual.Coluna - baseColuna, false), direcao);
                    posicaoAtual = new Posicao(posicaoAtual.Linha + baseLinha, posicaoAtual.Coluna + baseColuna, posicaoAtual.Limpo);
                    mapa[posicaoAtual.Linha, posicaoAtual.Coluna] = 66;
                    ImprimirMapa(mapa, posicaoAtual);
                    // robo.MemoriaMapa.ForEach(p => System.Console.Write("| " + p.Linha + " - " + p.Coluna + " |"));
                    // System.Console.Write('\n');
                    Thread.Sleep(500);
                }

                System.Console.WriteLine("voltando pra base ---------------");
                while (posicaoAtual.Linha != baseLinha || posicaoAtual.Coluna != baseColuna)
                {
                    mapa[posicaoAtual.Linha, posicaoAtual.Coluna] = lixoLocal;
                    (posicaoAtual, lixoLocal) = robo.MoverPraBase(new Posicao(posicaoAtual.Linha - baseLinha, posicaoAtual.Coluna - baseColuna, posicaoAtual.Limpo));
                    posicaoAtual = new Posicao(posicaoAtual.Linha + baseLinha, posicaoAtual.Coluna + baseColuna, posicaoAtual.Limpo);
                    mapa[posicaoAtual.Linha, posicaoAtual.Coluna] = 66;
                    ImprimirMapa(mapa, posicaoAtual);
                    // robo.MemoriaMapa.ForEach(p => System.Console.Write("| " + p.Linha + " - " + p.Coluna + " |"));
                    // System.Console.Write('\n');
                    Thread.Sleep(500);
                }
                if (robo.AmbienteLimpo())
                {
                    Console.WriteLine("tudo limpo");
                    break;
                }
                while (posicaoAtual.Linha == baseLinha && posicaoAtual.Coluna == baseColuna)
                {
                    Console.WriteLine("Carregando... ");
                    robo.Bateria += 10;
                    if (robo.Bateria >= 100)
                    {
                        robo.Bateria = 100;
                        Console.WriteLine("bateria " + robo.Bateria + "%");
                        string option = "";
                        do
                        {
                            Console.WriteLine("Quer limpar o arumba? [s/n]");
                            option = Console.ReadLine();
                        }
                        while (option != "s" && option != "n");
                        if (option == "s")
                        {
                            robo.LixoColetado = 0;
                        }
                        break;
                    }
                    Console.WriteLine("bateria " + robo.Bateria + "%");
                    Thread.Sleep(1000);

                }

                // System.Console.WriteLine("ultima posicao");
                // ImprimirMapa(mapa, posicaoAtual);
            }
        }

        private static void startMapa(int[,] mapa, int nivelSujeira)
        {
            int l, c;
            for (l = 0; l < tamanhoMatriz; l++)
            {
                for (c = 0; c < tamanhoMatriz; c++)
                {
                    if (l == 0 || l == tamanhoMatriz - 1 || c == 0 || c == tamanhoMatriz - 1)
                    {
                        mapa[l, c] = -1;
                    }
                    else
                    {
                        mapa[l, c] = 00;
                    }

                }
            }
            SetAmbiente(mapa, nivelSujeira);

        }
        //PAREDE = -1
        //REGIÃO LIVRE = 0
        //ROBO = 8
        //BASE = 9
        //ROBO NA BASE = 10
        //LIXO = 1,2,3


        private static void SetAmbiente(int[,] mapa, int nivelSujeira)
        {
            Random rnd = new Random();
            int l, c, sujeira;
            for (l = 0; l < tamanhoMatriz; l++)
            {
                for (c = 0; c < tamanhoMatriz; c++)
                {
                    if (mapa[l, c] >= 0)
                    {
                        sujeira = rnd.Next(10 - nivelSujeira);
                        if (sujeira < 4 && sujeira > 0)
                        {
                            mapa[l, c] = sujeira;
                        }
                        else
                        {
                            mapa[l, c] = 0;
                        }
                    }
                }
            }
        }

        public static int ConsultaMapa(int linha, int coluna)
        {
            linha += baseLinha;
            coluna += baseColuna;
            // System.Console.WriteLine("consulta Mapa : linha " + linha + " - coluna " + coluna);
            if (linha <= tamanhoMatriz && linha >= 0 && coluna <= tamanhoMatriz && coluna >= 0)
            {
                return mapa[linha, coluna];
            }
            return -2;
        }

        private static void ImprimirMapa(int[,] mapa, Posicao posicaoAtual)
        {
            int l, c;
            if (posicaoAtual.Linha == baseLinha && posicaoAtual.Coluna == baseColuna)
            {
                mapa[baseLinha, baseColuna] = 10;

            }
            else
            {
                mapa[baseLinha, baseColuna] = 9;
            }
            for (l = 0; l < tamanhoMatriz; l++)
            {
                for (c = 0; c < tamanhoMatriz; c++)
                {

                    if (mapa[l, c] >= 0 && mapa[l, c] < 10)
                    {
                        if (mapa[l, c] == 0)
                            System.Console.Write("   ");
                        else if (mapa[l, c] == 9)
                            System.Console.Write(" [ ");
                        else if (mapa[l, c] == 1)
                            System.Console.Write(" . ");
                        else if (mapa[l, c] == 2)
                            System.Console.Write(" : ");
                        else if (mapa[l, c] == 3)
                            System.Console.Write(".:.");
                        else if (mapa[l, c] > 0)
                            System.Console.Write(" " + mapa[l, c] + " ");
                    }
                    else
                    {
                        if (mapa[l, c] == 66)
                            System.Console.Write(" O ");
                        else if (mapa[l, c] == -1)
                            System.Console.Write(" # ");
                        else
                            System.Console.Write(" " + mapa[l, c]);
                    }
                }
                System.Console.WriteLine();
            }
        }
    }
}