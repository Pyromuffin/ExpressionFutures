﻿// Prototyping extended expression trees for C#.
//
// bartde - October 2015

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.CSharp.Expressions
{
    /// <summary>
    /// Represents a call to either static or an instance method.
    /// </summary>
    public sealed class MethodCallCSharpExpression : CSharpExpression
    {
        internal MethodCallCSharpExpression(Expression @object, MethodInfo method, ReadOnlyCollection<ParameterAssignment> arguments)
        {
            Object = @object;
            Method = method;
            Arguments = arguments;
        }

        /// <summary>
        /// Returns the node type of this <see cref="CSharpExpression" />. (Inherited from <see cref="CSharpExpression" />.)
        /// </summary>
        /// <returns>The <see cref="CSharpExpressionType"/> that represents this expression.</returns>
        public sealed override CSharpExpressionType CSharpNodeType
        {
            get { return CSharpExpressionType.Call; }
        }

        /// <summary>
        /// Gets the <see cref="Expression" /> that represents the instance for instance method calls or null for static method calls.
        /// </summary>
        public Expression Object { get; }

        /// <summary>
        /// Gets the <see cref="MethodInfo" /> for the method to be called.
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// Gets a collection of argument assignments.
        /// </summary>
        public ReadOnlyCollection<ParameterAssignment> Arguments { get; }

        /// <summary>
        /// Dispatches to the specific visit method for this node type.
        /// </summary>
        /// <param name="visitor">The visitor to visit this node with.</param>
        /// <returns>The result of visiting this node.</returns>
        protected internal override Expression Accept(CSharpExpressionVisitor visitor)
        {
            return visitor.VisitMethodCall(this);
        }

        /// <summary>
        /// Creates a new expression that is like this one, but using the supplied children. If all of the children are the same, it will return this expression.
        /// </summary>
        /// <param name="object">The <see cref="Object" /> property of the result.</param>
        /// <param name="arguments">The <see cref="Arguments" /> property of the result.</param>
        /// <returns>This expression if no children changed, or an expression with the updated children.</returns>
        public MethodCallCSharpExpression Update(Expression @object, IEnumerable<ParameterAssignment> arguments)
        {
            if (@object == Object && arguments == Arguments)
            {
                return this;
            }

            return CSharpExpression.Call(@object, Method, arguments);
        }
    }

    partial class CSharpExpression
    {
        /// <summary>
        /// Creates a <see cref="MethodCallCSharpExpression" /> that represents a call to a static method.
        /// </summary>
        /// <param name="method">The <see cref="MethodInfo" /> that represents the target method.</param>
        /// <param name="arguments">An array of one or more of <see cref="ParameterAssignment" /> that represents the call arguments.</param>
        ///<returns>A <see cref="MethodCallCSharpExpression" /> that has the <see cref="P:Microsoft.CSharp.Expressions.CSharpExpression.CSharpNodeType" /> property equal to <see cref="F:Microsoft.CSharp.Expressions.CSharpExpressionType.Call" /> and the <see cref="P:Microsoft.CSharp.Expressions.MethodCallCSharpExpression.Object" />, <see cref="P:Microsoft.CSharp.Expressions.MethodCallCSharpExpression.Method" />, and <see cref="P:Microsoft.CSharp.Expressions.MethodCallCSharpExpression.Arguments" /> properties set to the specified values.</returns>
        public static MethodCallCSharpExpression Call(MethodInfo method, params ParameterAssignment[] arguments)
        {
            return Call(null, method, (IEnumerable<ParameterAssignment>)arguments);
        }

        /// <summary>
        /// Creates a <see cref="MethodCallCSharpExpression" /> that represents a call to a static method.
        /// </summary>
        /// <param name="method">The <see cref="MethodInfo" /> that represents the target method.</param>
        /// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="ParameterAssignment" /> objects to use to populate the <see cref="P:Microsoft.CSharp.Expressions.MethodCallCSharpExpression.Arguments" /> collection.</param>
        ///<returns>A <see cref="MethodCallCSharpExpression" /> that has the <see cref="P:Microsoft.CSharp.Expressions.CSharpExpression.CSharpNodeType" /> property equal to <see cref="F:Microsoft.CSharp.Expressions.CSharpExpressionType.Call" /> and the <see cref="P:Microsoft.CSharp.Expressions.MethodCallCSharpExpression.Object" />, <see cref="P:Microsoft.CSharp.Expressions.MethodCallCSharpExpression.Method" />, and <see cref="P:Microsoft.CSharp.Expressions.MethodCallCSharpExpression.Arguments" /> properties set to the specified values.</returns>
        public static MethodCallCSharpExpression Call(MethodInfo method, IEnumerable<ParameterAssignment> arguments)
        {
            return Call(null, method, (IEnumerable<ParameterAssignment>)arguments);
        }

        /// <summary>
        /// Creates a <see cref="MethodCallCSharpExpression" /> that represents a method call.
        /// </summary>
        /// <param name="instance">An <see cref="Expression" /> that specifies the instance for an instance call. (pass null for a static method).</param>
        /// <param name="method">The <see cref="MethodInfo" /> that represents the target method.</param>
        /// <param name="arguments">An array of one or more of <see cref="ParameterAssignment" /> that represents the call arguments.</param>
        ///<returns>A <see cref="MethodCallCSharpExpression" /> that has the <see cref="P:Microsoft.CSharp.Expressions.CSharpExpression.CSharpNodeType" /> property equal to <see cref="F:Microsoft.CSharp.Expressions.CSharpExpressionType.Call" /> and the <see cref="P:Microsoft.CSharp.Expressions.MethodCallCSharpExpression.Object" />, <see cref="P:Microsoft.CSharp.Expressions.MethodCallCSharpExpression.Method" />, and <see cref="P:Microsoft.CSharp.Expressions.MethodCallCSharpExpression.Arguments" /> properties set to the specified values.</returns>
        public static MethodCallCSharpExpression Call(Expression instance, MethodInfo method, params ParameterAssignment[] arguments)
        {
            return Call(instance, method, (IEnumerable<ParameterAssignment>)arguments);
        }

        /// <summary>
        /// Creates a <see cref="MethodCallCSharpExpression" /> that represents a method call.
        /// </summary>
        /// <param name="instance">An <see cref="Expression" /> that specifies the instance for an instance call. (pass null for a static method).</param>
        /// <param name="method">The <see cref="MethodInfo" /> that represents the target method.</param>
        /// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="ParameterAssignment" /> objects to use to populate the <see cref="P:Microsoft.CSharp.Expressions.MethodCallCSharpExpression.Arguments" /> collection.</param>
        ///<returns>A <see cref="MethodCallCSharpExpression" /> that has the <see cref="P:Microsoft.CSharp.Expressions.CSharpExpression.CSharpNodeType" /> property equal to <see cref="F:Microsoft.CSharp.Expressions.CSharpExpressionType.Call" /> and the <see cref="P:Microsoft.CSharp.Expressions.MethodCallCSharpExpression.Object" />, <see cref="P:Microsoft.CSharp.Expressions.MethodCallCSharpExpression.Method" />, and <see cref="P:Microsoft.CSharp.Expressions.MethodCallCSharpExpression.Arguments" /> properties set to the specified values.</returns>
        public static MethodCallCSharpExpression Call(Expression instance, MethodInfo method, IEnumerable<ParameterAssignment> arguments)
        {
            throw new NotImplementedException();
        }
    }

    partial class CSharpExpressionVisitor
    {
        /// <summary>
        /// Visits the children of the <see cref="MethodCallCSharpExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected internal virtual Expression VisitMethodCall(MethodCallCSharpExpression node)
        {
            return node.Update(Visit(node.Object), Visit(node.Arguments, VisitParameterAssignment));
        }
    }
}
