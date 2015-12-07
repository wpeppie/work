using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace InputFileComparer
{
    public class ConfigurationValidator
    {
        public void ValidateConfiguration(Config config)
        {
            CheckMandatoryStringCollection(() => config.KeyColumns);
            CheckMandatoryString(() => config.FirstFile);
            CheckMandatoryString(() => config.SecondFile);
            CheckMandatoryString(() => config.DateColumnName);
            CheckMandatoryInt(() => config.FirstFileHeaderLine);
            config.SecondFileHeaderLine = config.SecondFileHeaderLine 
                ?? config.FirstFileHeaderLine;
            
        }

        private void CheckMandatoryStringCollection(Expression<Func<string[]>> collection)
        {
            MemberExpression body = collection.Body as MemberExpression;
            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)collection.Body;
                body = ubody.Operand as MemberExpression;
            }
            var compiledCollection = collection.Compile()();
            if (compiledCollection == null || compiledCollection.Length == 0)
            {
                throw new ArgumentException("Configuration " + body.Member.Name + " must contain at least 1 string element");
            }
        }

        private void CheckMandatoryString(Expression<Func<string>> value)
        {
            MemberExpression body = value.Body as MemberExpression;
            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)value.Body;
                body = ubody.Operand as MemberExpression;
            }

            if (string.IsNullOrEmpty(value.Compile()()))
            {
                throw new ArgumentException("Configuration " + body.Member.Name + " must be specified");
            }

        }
        private void CheckMandatoryInt(Expression<Func<int?>> value)
        {
            MemberExpression body = value.Body as MemberExpression;
            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)value.Body;
                body = ubody.Operand as MemberExpression;
            }

            if (value.Compile()() == null)
            {
                throw new ArgumentException("Configuration " + body.Member.Name + " must be specified");
            }

        }


    }
}
