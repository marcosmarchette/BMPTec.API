using AutoMapper;
using BMPTec.Application.DTOs;
using BMPTec.Domain.Entities;

namespace BMPTec.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Usuario
            CreateMap<Usuario, UsuarioDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Endereco))
                .ForMember(dest => dest.CPF, opt => opt.MapFrom(src => src.CPF.Numero));

            // Conta
            CreateMap<Conta, ContaDto>()
                .ForMember(dest => dest.NumeroConta, opt => opt.MapFrom(src => src.NumeroConta.Numero))
                .ForMember(dest => dest.Agencia, opt => opt.MapFrom(src => src.NumeroConta.Agencia))
                .ForMember(dest => dest.TipoConta, opt => opt.MapFrom(src => src.TipoConta.ToString()))
                .ForMember(dest => dest.SaldoDisponivel, opt => opt.MapFrom(src => src.ObterSaldoDisponivel()));

            // Transferencia
            CreateMap<Transferencia, TransferenciaDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.TipoTransferencia, opt => opt.MapFrom(src => src.TipoTransferencia.ToString()));

            // Extrato
            CreateMap<Extrato, ExtratoDto>()
                .ForMember(dest => dest.TipoOperacao, opt => opt.MapFrom(src => src.TipoOperacao.ToString()));
        }
    }
}
