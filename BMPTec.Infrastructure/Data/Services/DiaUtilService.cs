using BMPTec.Domain.Interfaces;

namespace BMPTec.Infrastructure.Data.Services
{
    public class DiaUtilService
    {
        private readonly IFeriadoRepository _feriadoRepository;

        public DiaUtilService(IFeriadoRepository feriadoRepository)
        {
            _feriadoRepository = feriadoRepository;
        }

        /// <summary>
        /// Verifica se a data é um dia útil (não é fim de semana nem feriado)
        /// </summary>
        public async Task<bool> EhDiaUtilAsync(DateTime data, CancellationToken cancellationToken = default)
        {
            // Verifica se é fim de semana
            if (data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday)
                return false;

            // Verifica se é feriado
            var ehFeriado = await _feriadoRepository.DataEhFeriadoAsync(data.Date, cancellationToken);
            return !ehFeriado;
        }

        /// <summary>
        /// Obtém o próximo dia útil a partir de uma data
        /// </summary>
        public async Task<DateTime> ObterProximoDiaUtilAsync(DateTime data, CancellationToken cancellationToken = default)
        {
            var proximaData = data.Date.AddDays(1);

            while (!await EhDiaUtilAsync(proximaData, cancellationToken))
            {
                proximaData = proximaData.AddDays(1);
            }

            return proximaData;
        }
    }
}
