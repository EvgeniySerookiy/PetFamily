namespace PetFamily.Domain.Shared;

public class Result
{
     public string? Error { get; set; }
     public bool IsSuccess { get; }
     public bool IsFailure => !IsSuccess;

     public Result(bool isSuccess, string? error)
     {
          if (isSuccess && error is not null)
          {
               throw new InvalidOperationException();
          }

          if (!isSuccess && error == null)
          {
               throw new InvalidOperationException(); 
          }
          
          IsSuccess = isSuccess;
          Error = error;
     }

     public static Result Success()
     {
          return new Result(true, null);
     }

     public static Result Failure(string error)
     {
          return new Result(false, error);
     }
     
     public static implicit operator Result(string error)
     {
          return new ( false, error);
     }
}

public class Result<TValue> : Result
{
     private readonly TValue _value;

     public TValue Value
     {
          get
          {
               if (!IsSuccess)
               {
                    return _value;
               }

               else
               {
                    throw new InvalidOperationException("The value of a failure result can not be accessed.");
               }
          }
     }
     
     public Result(TValue value, bool isSuccess, string? error) 
          : base(isSuccess, error)
     {
          _value = value;
     }

     public static Result<TValue> Success(TValue value)
     {
          return new Result<TValue>(value, true, null);
     }

     public new static Result<TValue> Failure(string error)
     {
          return new Result<TValue>(default!, false, error);
     }

     public static implicit operator Result<TValue>(TValue value)
     {
          return new (value, true, null);
     }

     public static implicit operator Result<TValue>(string error)
     {
          return new (default!, false, error);
     }
}