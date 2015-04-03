using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AddressValidation.Tests
{
    // from https://github.com/bbraithwaite/MSTestExtensions
    public static class ExceptionAssert
    {
        public static T Throws<T>(Action task) where T : Exception
        {
            try
            {
                task();
            }
            catch (Exception ex)
            {
                return (T)ex;
            }

            if (typeof(T).Equals(new Exception().GetType()))
            {
                Assert.Fail("Expected exception but no exception was thrown.");
            }
            else
            {
                Assert.Fail(string.Format("Expected exception of type {0} but no exception was thrown.", typeof(T)));
            }

            return default(T);
        }
    }
}
