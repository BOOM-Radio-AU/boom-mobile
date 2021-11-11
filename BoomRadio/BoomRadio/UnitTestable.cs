using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BoomRadio
{
    /// <summary>
    /// A class that allows it's private methods to be unit tested.
    /// </summary>
    public abstract class UnitTestable
    {
        /// <summary>
        /// Calls a private method for unit testing purposes.
        /// Based on https://stackoverflow.com/questions/9122708/unit-testing-private-methods-in-c-sharp#answer-67409568
        /// </summary>
        /// <typeparam name="TReturn">Return type of method</typeparam>
        /// <param name="methodName">Name of method</param>
        /// <param name="parameters">Parameters to pass to method</param>
        /// <returns></returns>
        public TReturn CallPrivateMethod<TReturn>(string methodName, params object[] parameters)
        {
            Type type = this.GetType();
            BindingFlags bindingAttr = BindingFlags.NonPublic | BindingFlags.Instance;
            MethodInfo method = type.GetMethod(methodName, bindingAttr);

            try { 
                return (TReturn)method.Invoke(this, parameters);
            } catch (Exception ex)
            {
                // Re-throw the original exception, rather than the exception that .Invoke() 
                // wraps around it
                throw ex.InnerException;
            }
        }
    }
}
