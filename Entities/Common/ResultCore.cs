using System;
            namespace OmniPulse.Entities.Common;
            public interface IResult { bool Success { get; } string Message { get; } }
            public interface IDataResult<out T> : IResult { T Data { get; } }
            public class SuccessResult : IResult { public bool Success => true; public string Message { get; } public SuccessResult(string m = "OK") => Message = m; }
            public class ErrorResult : IResult { public bool Success => false; public string Message { get; } public ErrorResult(string m = "FAIL") => Message = m; }
            public class SuccessDataResult<T> : SuccessResult, IDataResult<T> { public T Data { get; } public SuccessDataResult(T data, string m = "OK") : base(m) => Data = data; }
            public class ErrorDataResult<T> : ErrorResult, IDataResult<T> { public T Data => default!; public ErrorDataResult(string m = "FAIL") : base(m) { } }