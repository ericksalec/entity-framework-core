using System;
using System.Collections.Generic;
using System.Text;

namespace EFC.ValueObjects
{
    public enum TipoFrete
    {
        //remetente paga
        CIF,
        //destinatário paga
        FOB,
        //retirar na loja
        SemFrete,
    }
}
