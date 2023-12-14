using Application.Core.Error;

namespace Application.Core;

public class Result<T>{
    public bool IsSuccess{ get; set; }
    public T Value{ get; set; }
    public ApplicationRequestError Error{ get; set; }

    public static Result<T> Success(T value) => new Result<T>{ IsSuccess = true, Value = value};
    
    public static Result<T> Failure(ApplicationRequestError error) => new Result<T>{ IsSuccess = false, Error = error};
}