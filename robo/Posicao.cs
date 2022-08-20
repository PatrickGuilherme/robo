using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace robo
{
    public class Posicao
    {
        public int Linha { get; set; }
        public int Coluna { get; set; }
        public bool Limpo { get; set; }

        public Posicao(int linha, int coluna, bool limpo){
            this.Linha = linha;
            this.Coluna = coluna;
            this.Limpo = limpo;
        }
        public Posicao(){}

    }
}
