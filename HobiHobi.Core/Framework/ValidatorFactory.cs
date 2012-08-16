using System;
using System.Web.Mvc;
using Autofac.Features.Indexed;
using FluentValidation;

namespace HobiHobi.Core.Framework
{
    public class ValidatorFactory : IValidatorFactory
    {
        readonly IIndex<Type, IValidator> _validators;

        public ValidatorFactory(IIndex<Type, IValidator> validators)
        {
            _validators = DependencyResolver.Current.GetService<IIndex<Type, IValidator>>();
        }

        #region IValidatorFactory Members

        public IValidator GetValidator(Type type)
        {
            IValidator validator;
            if (_validators.TryGetValue(type, out validator))
                return validator;
            else
                return null;
        }

        public IValidator<T> GetValidator<T>()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
