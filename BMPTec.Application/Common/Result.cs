using System;
using System.Collections.Generic;
using System.Text;

namespace BMPTec.Application.Common
{
    public  class Result
    {
        public bool Sucesso { get; private set; }
        public string? Mensagem { get; private set; }
        public List<string> Erros { get; private set; }

        protected Result(bool sucesso, string? mensagem = null, List<string>? erros = null)
        {
            Sucesso = sucesso;
            Mensagem = mensagem;
            Erros = erros ?? new List<string>();
        }

        public static Result Success(string? mensagem = null)
            => new(true, mensagem);

        public static Result Failure(string erro)
            => new(false, null, new List<string> { erro });

        public static Result Failure(List<string> erros)
            => new(false, null, erros);
    }

    public class Result<T> : Result
    {
        public T? Data { get; private set; }

        private Result(bool sucesso, T? data, string? mensagem = null, List<string>? erros = null)
            : base(sucesso, mensagem, erros)
        {
            Data = data;
        }

        public static Result<T> Success(T data, string? mensagem = null)
            => new(true, data, mensagem);

        public static new Result<T> Failure(string erro)
            => new(false, default, null, new List<string> { erro });

        public static new Result<T> Failure(List<string> erros)
            => new(false, default, null, erros);
    }
}
