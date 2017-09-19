using Common.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Cookbook.Domain.Exceptions
{
    public static class EntityValidatorExtensions
    {
        public static EntityValidator<T> Verify<T, TReturn>(this T item, Expression<Func<T, TReturn>> path, TReturn value, params object[] args) where T : Entity
        {
            var propertyInfo = (PropertyInfo)((MemberExpression)path.Body).Member;

            var errors = propertyInfo.GetCustomAttributes(true)
                .OfType<ValidationAttribute>()
                .Where(x => !x.IsValid(value))
                .Select(x => new EntityValidator<T>.ValidationError
                {
                    Name = propertyInfo.Name,
                    Error = x.ErrorMessage,
                    Value = value,
                    Args = new object[] { value }.Union(args).ToArray(),
                    SubType = EntityValidator<T>.SelectSubTypeByType(x)
                })
                .ToArray();
            return new EntityValidator<T>(item, errors);
        }

        /// <summary>
        /// If predicate says true no error is thrown
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="validator"></param>
        /// <param name="key"></param>
        /// <param name="subType"></param>
        /// <returns></returns>
        public static EntityValidator<T> Verify<T>(this T item, Func<T, bool> validator, string key,
            ValidationException.ValidationErrors subType = ValidationException.ValidationErrors.Generic) where T : Entity
        {
            return validator(item)
                ? new EntityValidator<T>(item)
                : new EntityValidator<T>(item, key, subType);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="validator"></param>
        /// <param name="key"></param>
        /// <param name="subType"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static EntityValidator<T> Verify<T>(this T item, Func<bool> validator, string key,
            ValidationException.ValidationErrors subType = ValidationException.ValidationErrors.Generic, params object[] args) where T : Entity
        {
            return validator()
                ? new EntityValidator<T>(item)
                : new EntityValidator<T>(item, key, subType, args);
        }



    }

    public class EntityValidator<T> where T : Entity
    {
        private List<ValidationError> _errors = new List<ValidationError>();
        private readonly string Namespace = typeof(EntityValidator<>).Namespace;

        public EntityValidator(T item)
        {
            Entity = item;
        }

        public EntityValidator(T item, string key, ValidationException.ValidationErrors subType) : this(item)
        {
            _errors.Add(new ValidationError { Name = string.Empty, Error = key, Value = null, SubType = subType });
        }

        public EntityValidator(T item, string key, ValidationException.ValidationErrors subType, params object[] args) : this(item)
        {
            _errors.Add(new ValidationError { Name = string.Empty, Error = key, Value = null, SubType = subType, Args = args });
        }

        public EntityValidator(T item, ValidationError[] errors) : this(item)
        {
            _errors.AddRange(errors);
        }

        public T Entity { get; private set; }

        public EntityValidator<T> Verify<TReturn>(Expression<Func<T, TReturn>> path, TReturn value, params object[] args)
        {
            var propertyInfo = (PropertyInfo)((MemberExpression)path.Body).Member;

            _errors.AddRange(propertyInfo.GetCustomAttributes(true)
                .OfType<ValidationAttribute>()
                .Where(x => !x.IsValid(value))
                .Select(x => new EntityValidator<T>.ValidationError
                {
                    Name = propertyInfo.Name,
                    Error = x.ErrorMessage,
                    Value = value,
                    SubType = SelectSubTypeByType(x),
                    Args = args
                }));
            return this;
        }

        /// <summary>
        /// If predicate says true no error is thrown
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="key"></param>
        /// <param name="subType"></param>
        /// <returns></returns>
        public EntityValidator<T> Verify(Func<T, bool> validator, string key, ValidationException.ValidationErrors subType = ValidationException.ValidationErrors.Generic)
        {
            if (!validator(Entity))
            {
                _errors.Add(new ValidationError { Name = string.Empty, Error = key, Value = null, SubType = subType });
            }
            return this;
        }

        /// <summary>
        /// If predicate says true no error is thrown
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="key"></param>
        /// <param name="subType"></param>
        /// <returns></returns>
        public EntityValidator<T> Verify(Func<bool> validator, string key, ValidationException.ValidationErrors subType = ValidationException.ValidationErrors.Generic, params object[] args)
        {
            if (!validator())
            {
                _errors.Add(new ValidationError { Name = string.Empty, Error = key, Value = null, SubType = subType, Args = args });
            }
            return this;
        }

        public struct ValidationError
        {
            public ValidationException.ValidationErrors SubType;
            public string Name;
            public string Error;
            public object Value;
            public object[] Args;
        }

        public void ThrowIfInvalid()
        {
            if (!_errors.Any()) return;
            var myNamespace = typeof(MijnOrbisFactory).Namespace + ".";
            var type = typeof(T).FullName.Replace(myNamespace, "").Replace(".", "_");
            if (_errors.Count == 1)
            {
                throw CreateException(_errors.Single(), type);
            }
            throw new ValidationException(_errors.Select(x => CreateException(x, type)).ToArray());
        }

        private ValidationException CreateException(ValidationError errorMessage, string typeKey)
        {
            var message = new List<string>();
            message.Add(typeKey);
            if (!string.IsNullOrWhiteSpace(errorMessage.Name))
            {
                message.Add(errorMessage.Name);
            }
            if (!string.IsNullOrWhiteSpace(errorMessage.Error))
            {
                message.Add(errorMessage.Error);
            }
            return new ValidationException(errorMessage.SubType, string.Join("_", message), errorMessage.Args);
        }

        internal static ValidationException.ValidationErrors SelectSubTypeByType(ValidationAttribute x)
        {
            return x is RequiredAttribute
                ? ValidationException.ValidationErrors.RequiredField
                : x is MaxLengthAttribute || x is StringLengthAttribute
                ? ValidationException.ValidationErrors.TooLong
                : ValidationException.ValidationErrors.InvalidValue;
        }
    }
}
