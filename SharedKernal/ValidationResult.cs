namespace SharedKernal
{
    public sealed class ValidationResult : Result , IValidationResult
    {
        private ValidationResult(Error[] errors)
            : base(false, IValidationResult.ValidationError)
        {
            Error.AddDetails(errors);
        }

        public static ValidationResult WithErrors(Error[] errors) => new(errors);
    }

    public sealed class ValidationResult<TValue> : Result<TValue> , IValidationResult
    {
        private ValidationResult(Error[] errors)
            : base(default, false, IValidationResult.ValidationError)
        {
            Error.AddDetails(errors);
        }

        public static ValidationResult<TValue> WithErrors(Error[] errors) => new(errors);
    }
}
