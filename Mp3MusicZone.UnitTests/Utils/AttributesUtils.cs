namespace Mp3MusicZone.UnitTests.Utils
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class AttributesUtils
    {
        public static bool MethodHasAttribute(Expression<Action> expression, Type attrType)
        {
            MethodInfo methodInfo = ((MethodCallExpression)expression.Body).Method;

            return methodInfo.GetCustomAttributes(attrType)
                .Any();
        }
    }
}
