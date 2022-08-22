using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace robo
{
    public class Robo
    {
        public List<Posicao> MemoriaMapa { get; set; }
        public int Bateria { get; set; }//100
        public int TamanhoMapa { get; set; }
        public int CapacidadeLixo { get; set; }
        public int LixoColetado { get; set; }
        public Robo()
        {
            this.Bateria = 10000;
            this.CapacidadeLixo = 10;
            this.LixoColetado = 0;
            this.MemoriaMapa = new List<Posicao>();
            
        }



        public (Posicao, int) Mover(Posicao pAtual, int anter = -1, int prox = 8, int travou = 0, bool soPodereto = true)
        {
            Random rdn = new Random();
            int direcao = 0;
            if (prox == 8 && anter == -1)
            {
                direcao = rdn.Next(8);
                if (soPodereto)
                {
                    if (direcao % 2 != 0)
                    {
                        direcao -= 1;
                    }
                }
            }
            else if (anter == -1)
            {
                prox = prox + 1;
                if (prox >= 8) prox = prox % 8;
                direcao = prox;
            }
            else
            {
                direcao = anter;
                if (soPodereto)
                {
                    if (direcao % 2 != 0)
                    {
                        direcao -= 1;
                    }
                }
            }

            Posicao posicaoSeguinte;
            int nextPos;
            switch (direcao)
            {
                /*
                5 6 7 
                4 8 0
                3 2 1
                */
                case 1://Diagonal - baixo - direita
                    nextPos = Program.ConsultaMapa(pAtual.Linha + 1, pAtual.Coluna + 1);
                    System.Console.WriteLine("sujeira: " + nextPos + " -- travou: " + travou);
                    System.Console.WriteLine("direcao: " + direcao);

                    if ((nextPos >= 0 && !JaLimpou(pAtual.Linha + 1, pAtual.Coluna + 1)) || (nextPos >= 0 && travou >= 9))
                    {
                        System.Console.WriteLine("direcao:  Diagonal - baixo - direita");
                        this.ReduzirBateria(nextPos);
                        this.Bateria--;
                        System.Console.WriteLine("Nível bateria: " + this.Bateria + "/100");
                        posicaoSeguinte = new Posicao(pAtual.Linha + 1, pAtual.Coluna + 1, true);
                        if (!JaLimpou(pAtual.Linha + 1, pAtual.Coluna + 1))
                        {
                            this.MemoriaMapa.Add(posicaoSeguinte);
                        }
                        return (posicaoSeguinte, direcao);
                    }
                    else
                    {
                        return Mover(pAtual, -1, direcao, ++travou);
                    }
                case 2: //Baixo - baixo - baixo
                    nextPos = Program.ConsultaMapa(pAtual.Linha + 1, pAtual.Coluna);
                    System.Console.WriteLine("sujeira: " + nextPos + " -- travou: " + travou);
                    System.Console.WriteLine("direcao: " + direcao);
                    if ((nextPos >= 0 && !JaLimpou(pAtual.Linha + 1, pAtual.Coluna)) || (nextPos >= 0 && travou >= 9))
                    {
                        System.Console.WriteLine("direcao:  Baixo - baixo - baixo");
                        this.ReduzirBateria(nextPos);
                        this.Bateria--;
                        System.Console.WriteLine("Nível bateria: " + this.Bateria + "/100");
                        posicaoSeguinte = new Posicao(pAtual.Linha + 1, pAtual.Coluna, true);
                        if (!JaLimpou(pAtual.Linha + 1, pAtual.Coluna))
                        {
                            this.MemoriaMapa.Add(posicaoSeguinte);
                        }
                        return (posicaoSeguinte, direcao);
                    }
                    else
                    {
                        return Mover(pAtual, -1, direcao, ++travou);
                    }
                case 3:// Diagonal - baixo - esquerda
                    nextPos = Program.ConsultaMapa(pAtual.Linha + 1, pAtual.Coluna - 1);
                    System.Console.WriteLine("sujeira: " + nextPos + " -- travou: " + travou);
                    System.Console.WriteLine("direcao: " + direcao);
                    if ((nextPos >= 0 && !JaLimpou(pAtual.Linha + 1, pAtual.Coluna - 1)) || (nextPos >= 0 && travou >= 9))
                    {
                        System.Console.WriteLine("direcao:  Diagonal - baixo - esquerda");
                        this.ReduzirBateria(nextPos);
                        this.Bateria--;
                        System.Console.WriteLine("Nível bateria: " + this.Bateria + "/100");
                        posicaoSeguinte = new Posicao(pAtual.Linha + 1, pAtual.Coluna - 1, true);
                        if (!JaLimpou(pAtual.Linha + 1, pAtual.Coluna - 1))
                        {
                            this.MemoriaMapa.Add(posicaoSeguinte);
                        }
                        return (posicaoSeguinte, direcao);
                    }
                    else
                    {
                        return Mover(pAtual, -1, direcao, ++travou);
                    }
                case 4://Esquerda - esquerda - esquerda
                    nextPos = Program.ConsultaMapa(pAtual.Linha, pAtual.Coluna - 1);
                    System.Console.WriteLine("sujeira: " + nextPos + " -- travou: " + travou);
                    System.Console.WriteLine("direcao: " + direcao);
                    if ((nextPos >= 0 && !JaLimpou(pAtual.Linha, pAtual.Coluna - 1)) || (nextPos >= 0 && travou >= 9))
                    {
                        System.Console.WriteLine("direcao:  Esquerda - esquerda - esquerda");
                        this.ReduzirBateria(nextPos);
                        this.Bateria--;
                        System.Console.WriteLine("Nível bateria: " + this.Bateria + "/100");
                        posicaoSeguinte = new Posicao(pAtual.Linha, pAtual.Coluna - 1, true);
                        if (!JaLimpou(pAtual.Linha, pAtual.Coluna - 1))
                        {
                            this.MemoriaMapa.Add(posicaoSeguinte);
                        }
                        return (posicaoSeguinte, direcao);
                    }
                    else
                    {
                        return Mover(pAtual, -1, direcao, ++travou);
                    }
                case 5:// Diagonal - cima - esquerda
                    nextPos = Program.ConsultaMapa(pAtual.Linha + 1, pAtual.Coluna - 1);
                    System.Console.WriteLine("sujeira: " + nextPos + " -- travou: " + travou);
                    System.Console.WriteLine("direcao: " + direcao);
                    if ((nextPos >= 0 && !JaLimpou(pAtual.Linha + 1, pAtual.Coluna - 1)) || (nextPos >= 0 && travou >= 9))
                    {
                        System.Console.WriteLine("direcao:  Diagonal - cima - esquerda");
                        this.ReduzirBateria(nextPos);
                        this.Bateria--;
                        System.Console.WriteLine("Nível bateria: " + this.Bateria + "/100");
                        posicaoSeguinte = new Posicao(pAtual.Linha + 1, pAtual.Coluna - 1, true);
                        if (!JaLimpou(pAtual.Linha + 1, pAtual.Coluna - 1))
                        {
                            this.MemoriaMapa.Add(posicaoSeguinte);
                        }
                        return (posicaoSeguinte, direcao);
                    }
                    else
                    {
                        return Mover(pAtual, -1, direcao, ++travou);
                    }
                case 6://Cima - cima - cima
                    nextPos = Program.ConsultaMapa(pAtual.Linha - 1, pAtual.Coluna);
                    System.Console.WriteLine("sujeira: " + nextPos + " -- travou: " + travou);
                    System.Console.WriteLine("direcao: " + direcao);
                    if ((nextPos >= 0 && !JaLimpou(pAtual.Linha - 1, pAtual.Coluna)) || (nextPos >= 0 && travou >= 9))
                    {
                        System.Console.WriteLine("direcao:  Cima - cima - cima");
                        this.ReduzirBateria(nextPos);
                        this.Bateria--;
                        System.Console.WriteLine("Nível bateria: " + this.Bateria + "/100");
                        posicaoSeguinte = new Posicao(pAtual.Linha - 1, pAtual.Coluna, true);
                        if (!JaLimpou(pAtual.Linha - 1, pAtual.Coluna))
                        {
                            this.MemoriaMapa.Add(posicaoSeguinte);
                        }
                        return (posicaoSeguinte, direcao);
                    }
                    else
                    {
                        return Mover(pAtual, -1, direcao, ++travou);
                    }
                case 7://Diagonal - cima - direita
                    nextPos = Program.ConsultaMapa(pAtual.Linha - 1, pAtual.Coluna + 1);
                    System.Console.WriteLine("sujeira: " + nextPos + " -- travou: " + travou);
                    System.Console.WriteLine("direcao: " + direcao);
                    if ((nextPos >= 0 && !JaLimpou(pAtual.Linha - 1, pAtual.Coluna + 1)) || (nextPos >= 0 && travou >= 9))
                    {
                        System.Console.WriteLine("direcao:  Diagonal - cima - direita");
                        this.ReduzirBateria(nextPos);
                        this.Bateria--;
                        System.Console.WriteLine("Nível bateria: " + this.Bateria + "/100");
                        posicaoSeguinte = new Posicao(pAtual.Linha - 1, pAtual.Coluna + 1, true);
                        if (!JaLimpou(pAtual.Linha - 1, pAtual.Coluna + 1))
                        {
                            this.MemoriaMapa.Add(posicaoSeguinte);
                        }
                        return (posicaoSeguinte, direcao);
                    }
                    else
                    {
                        return Mover(pAtual, -1, direcao, ++travou);
                    }
                default://Esquerda - esquerda - esquerda
                    nextPos = Program.ConsultaMapa(pAtual.Linha, pAtual.Coluna + 1);
                    System.Console.WriteLine("sujeira: " + nextPos + " -- travou: " + travou);
                    System.Console.WriteLine("direcao: " + direcao);
                    if ((nextPos >= 0 && !JaLimpou(pAtual.Linha, pAtual.Coluna + 1)) || (nextPos >= 0 && travou >= 9))
                    {
                        System.Console.WriteLine("direcao:  Esquerda - esquerda - esquerda");
                        this.ReduzirBateria(nextPos);
                        this.Bateria--;
                        System.Console.WriteLine("Nível bateria: " + this.Bateria + "/100");
                        posicaoSeguinte = new Posicao(pAtual.Linha, pAtual.Coluna + 1, true);
                        if (!JaLimpou(pAtual.Linha, pAtual.Coluna + 1))
                        {
                            this.MemoriaMapa.Add(posicaoSeguinte);
                        }
                        return (posicaoSeguinte, direcao);
                    }
                    else
                    {
                        return Mover(pAtual, -1, direcao, ++travou);
                    }
            }
        }

        public bool AmbienteLimpo()
        {
            return MemoriaMapa.FindAll(point => point.Limpo == true).Count() == this.TamanhoMapa;
        }

        public bool JaLimpou(int pLinha, int pColuna)
        {

            try
            {
                Posicao posicao = this.MemoriaMapa.Find(p => p.Linha == pLinha && p.Coluna == pColuna);
                if (posicao == null)
                {
                    return false;
                }
                return posicao.Limpo;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void ReduzirBateria(int tipoSujeira)
        {
            if (tipoSujeira <= 4 && tipoSujeira >= 0)
            {
                this.Bateria -= tipoSujeira;
                this.LixoColetado += tipoSujeira;
            }
            if (this.Bateria <= 0)
            {
                System.Console.WriteLine("ROBO IS DEAD! :(");
                Environment.Exit(0);
            }
        }
        public Posicao MoverPraBase(Posicao pAtual)
        {
            Posicao proxPosicao = new Posicao();
            int nextPos = 0;
            System.Console.WriteLine(pAtual.Linha + " - " + pAtual.Coluna + " dentro do mover pra base");
            if (pAtual.Linha > 0 && pAtual.Coluna > 0)
            {
                for (int l = -1; l < 1; l++)
                {
                    for (int c = -1; c < 1; c++)
                    {
                        nextPos = Program.ConsultaMapa(pAtual.Linha + l, pAtual.Coluna + c);
                        if (nextPos >= 0 && nextPos < 10)
                        {
                            this.ReduzirBateria(nextPos);
                            this.Bateria--;
                            System.Console.WriteLine("Nível bateria: " + this.Bateria + "/100");
                            proxPosicao = new Posicao(pAtual.Linha + l, pAtual.Coluna + c, true);
                            if (!JaLimpou(pAtual.Linha + l, pAtual.Coluna + c))
                            {
                                this.MemoriaMapa.Add(proxPosicao);
                            }
                            return proxPosicao;
                        }
                    }
                }
            }
            return proxPosicao;
        }
    }
}
