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
            startMapa(mapa);
            robo.TamanhoMapa = (tamanhoMatriz-2)*(tamanhoMatriz-2);

            //setando base do robô 
            mapa[baseLinha, baseColuna] = 9;
            robo.MemoriaMapa.Add(new Posicao(baseLinha, baseColuna, true));
            posicaoAtual.Coluna = 1;
            posicaoAtual.Linha = 1;
            /*
             - Ajustar coleta de lixo *** criar funcao que verifique a casa e quantidade lixo se pode pegar ou nao
             - Ajustar voltar pra base *** unico caso funcional é quando a base está no 0,0
             - Criar próximas passagens *** após recarregar, limpar lixo e/ou finalizar limpeza, recriar passagem
            */

            ImprimirMapa(mapa, posicaoAtual);
            int direcao = -1;
            while(robo.Bateria >= 25 && !robo.AmbienteLimpo() && robo.LixoColetado <= robo.CapacidadeLixo)
            {
                System.Console.WriteLine("numero de memoria gravada: " + robo.MemoriaMapa.Count);
                mapa[posicaoAtual.Linha, posicaoAtual.Coluna] = 0; //limpando casa anterior
                (posicaoAtual, direcao) = robo.Mover(posicaoAtual, direcao);
                System.Console.WriteLine("direcao na main: " + direcao);
                System.Console.WriteLine(posicaoAtual.Linha + " - " + posicaoAtual.Coluna);
                mapa[posicaoAtual.Linha, posicaoAtual.Coluna] = 66;
                ImprimirMapa(mapa, posicaoAtual);
                Thread.Sleep(500);
            }
            System.Console.WriteLine("voltando pra base ---------------");
            while (posicaoAtual.Linha != baseLinha || posicaoAtual.Coluna != baseColuna)
            {
                mapa[posicaoAtual.Linha, posicaoAtual.Coluna] = 0;
                posicaoAtual = robo.MoverPraBase(posicaoAtual);
                mapa[posicaoAtual.Linha, posicaoAtual.Coluna] = 66;
                ImprimirMapa(mapa, posicaoAtual);
                Thread.Sleep(500);
            }
        }

        private static void startMapa(int[,] mapa)
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
            SetAmbiente(mapa);

        }
        //PAREDE = -1
        //REGIÃO LIVRE = 0
        //ROBO = 8
        //BASE = 9
        //ROBO NA BASE = 10
        //LIXO = 1,2,3


        private static void SetAmbiente(int[,] mapa)
        {
            Random rnd = new Random();
            int l, c, sujeira;
            for (l = 0; l < tamanhoMatriz; l++)
            {
                for (c = 0; c < tamanhoMatriz; c++)
                {
                    if (mapa[l, c] >= 0)
                    {
                        sujeira = rnd.Next(10);
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
            if (linha <= tamanhoMatriz && coluna <= tamanhoMatriz)
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
                        System.Console.Write("  " + mapa[l, c]);
                    else
                        System.Console.Write(" " + mapa[l, c]);
                }
                System.Console.WriteLine();
            }
        }
    }
}