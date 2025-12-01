using BMPTec.Domain.Common;
using BMPTec.Domain.Enums;
using BMPTec.Domain.Exceptions;

namespace BMPTec.Domain.Entities
{
    public class Feriado : BaseEntity
    {
        public DateTime Data { get; private set; }
        public string Nome { get; private set; }
        public TipoFeriado TipoFeriado { get; private set; }
        public bool Recorrente { get; private set; }

      
        private Feriado() : base()
        {
            Nome = string.Empty;
        }

        private Feriado(DateTime data, string nome, TipoFeriado tipoFeriado, bool recorrente)
            : base()
        {
            ValidarFeriado(nome);

            Data = data.Date;
            Nome = nome;
            TipoFeriado = tipoFeriado;
            Recorrente = recorrente;
        }

        public static Feriado Criar(DateTime data, string nome, TipoFeriado tipoFeriado, bool recorrente = false)
        {
            return new Feriado(data, nome, tipoFeriado, recorrente);
        }

        private static void ValidarFeriado(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new DomainException("Nome do feriado não pode ser vazio.");

            if (nome.Length > 100)
                throw new DomainException("Nome do feriado não pode ter mais de 100 caracteres.");
        }
    }
}
