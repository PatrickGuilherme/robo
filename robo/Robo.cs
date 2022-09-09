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
            this.Bateria = 100;
            this.CapacidadeLixo = 100;
            this.LixoColetado = 0;
            this.MemoriaMapa = new List<Posicao>();
            this.MemoriaMapa.Add(new Posicao(0, 0, true));
        }



        public (Posicao, int, int) Mover(Posicao pAtual, int anter = -1, int prox = 8, int travou = 0, bool soPodereto = true)
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
            switch (direcao)
            {
                /*
                5 6 7 
                4 8 0
                3 2 1
                */
                case 1://Diagonal - baixo - direita
                    return Movimentacao(pAtual, +1, +1, travou, direcao);
                case 2: //Baixo - baixo - baixo
                    return Movimentacao(pAtual, +1, 0, travou, direcao);
                case 3:// Diagonal - baixo - esquerda
                    return Movimentacao(pAtual, +1, -1, travou, direcao);
                case 4://Esquerda - esquerda - esquerda
                    return Movimentacao(pAtual, 0, -1, travou, direcao);
                case 5:// Diagonal - cima - esquerda
                    return Movimentacao(pAtual, -1, -1, travou, direcao);
                case 6://Cima - cima - cima
                    return Movimentacao(pAtual, -1, 0, travou, direcao);
                case 7://Diagonal - cima - direita
                    return Movimentacao(pAtual, -1, +1, travou, direcao);
                default://Esquerda - esquerda - esquerda
                    return Movimentacao(pAtual, 0, +1, travou, direcao);
            }
        }


        private (Posicao, int, int) Movimentacao(Posicao pAtual, int linha, int coluna, int travou, int direcao)
        {
            int nextPos = Program.ConsultaMapa(pAtual.Linha + linha, pAtual.Coluna + coluna);
            // System.Console.WriteLine("sujeira: " + nextPos + " -- travou: " + travou);
            // System.Console.WriteLine("direcao: " + direcao);

            if ((nextPos >= 0 && !JaLimpou(pAtual.Linha + linha, pAtual.Coluna + coluna)) || (nextPos >= 0 && travou >= 9))
            {
                int lixoDaCasa = this.ColetaLixo(nextPos);
                this.ReduzirBateria(nextPos - lixoDaCasa);
                System.Console.Clear();
                System.Console.WriteLine("nivel de lixo: " + this.LixoColetado + "/" + this.CapacidadeLixo);
                System.Console.WriteLine("Nível bateria: " + this.Bateria + "/100");
                System.Console.WriteLine(pAtual.Linha + " - " + pAtual.Coluna + " //antes de mover");
                Posicao posicaoSeguinte;
                if (lixoDaCasa == 0)
                    posicaoSeguinte = new Posicao(pAtual.Linha + linha, pAtual.Coluna + coluna, true);
                else
                    posicaoSeguinte = new Posicao(pAtual.Linha + linha, pAtual.Coluna + coluna, false);

                if (!JaPassou(pAtual.Linha + linha, pAtual.Coluna + coluna))
                {
                    this.MemoriaMapa.Add(posicaoSeguinte);
                }
                System.Console.WriteLine(posicaoSeguinte.Linha + " - " + posicaoSeguinte.Coluna + " //movendo-se");
                return (posicaoSeguinte, direcao, lixoDaCasa);
            }
            else
            {
                return Mover(pAtual, -1, direcao, ++travou);
            }
        }

        private (Posicao, int) MovimentacaoToBase(Posicao pAtual, int l, int c)
        {
            int nextPos = Program.ConsultaMapa(pAtual.Linha + l, pAtual.Coluna + c);
            if (nextPos >= 0 && nextPos < 10)
            {
                int lixoDaCasa = this.ColetaLixo(nextPos);
                this.ReduzirBateria(lixoDaCasa - nextPos);
                Posicao proxPosicao;
                if (lixoDaCasa == 0)
                    proxPosicao = new Posicao(pAtual.Linha + l, pAtual.Coluna + c, true);
                else
                    proxPosicao = new Posicao(pAtual.Linha + l, pAtual.Coluna + c, false);

                if (!JaPassou(pAtual.Linha + l, pAtual.Coluna + c))
                {
                    this.MemoriaMapa.Add(proxPosicao);
                }
                System.Console.WriteLine("nivel de lixo: " + this.LixoColetado + "/" + this.CapacidadeLixo);
                System.Console.WriteLine(pAtual.Linha + " - " + pAtual.Coluna + "//voltando pra base");
                System.Console.WriteLine("Nível bateria: " + this.Bateria + "/100");
                return (proxPosicao, lixoDaCasa);
            }
            return (null, 0);
        }
        public bool lixeiraCheia()
        {
            return this.LixoColetado >= this.CapacidadeLixo;
        }

        public int ColetaLixo(int lixo)
        {
            int nivel = this.LixoColetado + lixo;

            if (nivel >= this.CapacidadeLixo)
            {
                nivel -= this.CapacidadeLixo;
                this.LixoColetado += lixo - nivel;
                // System.Console.WriteLine("\nnivel dentro do if" + nivel + "-  lixo " + lixo);
                return nivel;
            }
            else if (this.Bateria >= 25)
            {
                this.LixoColetado += lixo;
                // System.Console.WriteLine("\nnivel " + nivel + "-  lixo " + lixo);
                return 0;
            }
            else
            {
                return lixo;
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
        public bool JaPassou(int pLinha, int pColuna)
        {
            Posicao posicao = this.MemoriaMapa.Find(p => p.Linha == pLinha && p.Coluna == pColuna);
            return posicao != null ? true : false;
        }
        public void ReduzirBateria(int tipoSujeira)
        {
            if (tipoSujeira <= 4 && tipoSujeira >= 0)
            {
                this.Bateria -= tipoSujeira;
                this.Bateria--;
            }
            if (this.Bateria <= 0)
            {
                System.Console.WriteLine("ROBO IS DEAD! :(");
                Environment.Exit(0);
            }
        }
        public (Posicao, int) MoverPraBase(Posicao pAtual)
        {
            Posicao proxPosicao = new Posicao();
            int lixoDaCasa = 0;
            System.Console.Clear();
            if (pAtual.Linha >= 0 && pAtual.Coluna >= 0)
            {
                for (int l = -1; l < 1; l++)
                {
                    for (int c = -1; c < 1; c++)
                    {
                        (proxPosicao, lixoDaCasa) = MovimentacaoToBase(pAtual, l, c);
                        if (proxPosicao != null){
                            return (proxPosicao, lixoDaCasa);
                        }
                    }
                }
            }
            if (pAtual.Linha < 0 && pAtual.Coluna >= 0)
            {
                for (int l = -1; l < 1; l++)
                {
                    for (int c = 0; c < 2; c++)
                    {
                        (proxPosicao, lixoDaCasa) = MovimentacaoToBase(pAtual, l, c);
                        if (proxPosicao != null){
                            return (proxPosicao, lixoDaCasa);
                        }
                    }
                }
            }
            if (pAtual.Linha < 0 && pAtual.Coluna < 0)
            {
                for (int l = 0; l < 2; l++)
                {
                    for (int c = 0; c < 2; c++)
                    {
                        (proxPosicao, lixoDaCasa) = MovimentacaoToBase(pAtual, l, c);
                        if (proxPosicao != null){
                            return (proxPosicao, lixoDaCasa);
                        }
                    }
                }
            }
            if (pAtual.Linha >= 0 && pAtual.Coluna < 0)
            {
                for (int l = 0; l < 2; l++)
                {
                    for (int c = -1; c < 1; c++)
                    {
                        (proxPosicao, lixoDaCasa) = MovimentacaoToBase(pAtual, l, c);
                        if (proxPosicao != null){
                            return (proxPosicao, lixoDaCasa);
                        }
                    }
                }
            }
            return (proxPosicao, 0);
        }
    }
}
