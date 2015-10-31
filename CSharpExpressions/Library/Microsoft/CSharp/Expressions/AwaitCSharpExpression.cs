﻿// Prototyping extended expression trees for C#.
//
// bartde - October 2015

using System;
using System.Dynamic.Utils;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using static System.Linq.Expressions.ExpressionStubs;
using LinqError = System.Linq.Expressions.Error;

namespace Microsoft.CSharp.Expressions
{
    /// <summary>
    /// Represents an expression that awaits an asynchronous operation.
    /// </summary>
    public sealed class AwaitCSharpExpression : UnaryCSharpExpression
    {
        internal AwaitCSharpExpression(Expression operand, MethodInfo getAwaiterMethod, Type type)
            : base(operand)
        {
            GetAwaiterMethod = getAwaiterMethod;
            Type = type;
        }

        /// <summary>
        /// Returns the node type of this <see cref="CSharpExpression" />. (Inherited from <see cref="CSharpExpression" />.)
        /// </summary>
        /// <returns>The <see cref="CSharpExpressionType"/> that represents this expression.</returns>
        public sealed override CSharpExpressionType CSharpNodeType => CSharpExpressionType.Await;

        /// <summary>
        /// Gets the static type of the expression that this <see cref="Expression" /> represents. (Inherited from <see cref="Expression"/>.)
        /// </summary>
        /// <returns>The <see cref="Type"/> that represents the static type of the expression.</returns>
        public override Type Type { get; }

        /// <summary>
        /// Gets the GetAwaiter method used to await the asynchronous operation.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Awaiter", Justification = "Get a waiter :-)")]
        public MethodInfo GetAwaiterMethod { get; }

        /// <summary>
        /// Dispatches to the specific visit method for this node type.
        /// </summary>
        /// <param name="visitor">The visitor to visit this node with.</param>
        /// <returns>The result of visiting this node.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Following the visitor pattern from System.Linq.Expressions.")]
        protected internal override Expression Accept(CSharpExpressionVisitor visitor)
        {
            return visitor.VisitAwait(this);
        }

        /// <summary>
        /// Creates a new expression that is like this one, but using the supplied children. If all of the children are the same, it will return this expression.
        /// </summary>
        /// <param name="operand">The <see cref="UnaryCSharpExpression.Operand" /> property of the result.</param>
        /// <returns>This expression if no children changed, or an expression with the updated children.</returns>
        public AwaitCSharpExpression Update(Expression operand)
        {
            if (operand == Operand)
            {
                return this;
            }

            return CSharpExpression.Await(operand, GetAwaiterMethod);
        }

        /// <summary>
        /// Reduces the call expression node to a simpler expression.
        /// </summary>
        /// <returns>The reduced expression.</returns>
        public override Expression Reduce()
        {
#if DONE
            // NB: Await nodes can only occur in AsyncLambda expressions and should not get compiled
            //     via the normal expression compiler path of reducing extension nodes. The closest
            //     enclosing AsyncLambda node is responsible for rewriting those.
            throw new NotSupportedException();
#else
            // NB: In order to unblock initial experimentation, the Reduce method will emit a blocking
            //     invocation of GetAwaiter and GetResult.

            var getAwaiterCall = default(Expression);
            if (GetAwaiterMethod.IsStatic)
            {
                getAwaiterCall = Expression.Call(GetAwaiterMethod, Operand);
            }
            else
            {
                getAwaiterCall = Expression.Call(Operand, GetAwaiterMethod);
            }

            var getResultMethod = getAwaiterCall.Type.GetMethod("GetResult", BindingFlags.Public | BindingFlags.Instance, null, Array.Empty<Type>(), null);
            var getResultCall = Expression.Call(getAwaiterCall, getResultMethod);

            return getResultCall;
#endif
        }
    }

    partial class CSharpExpression
    {
        /// <summary>
        /// Creates an <see cref="AwaitCSharpExpression"/> that represents awaiting an asynchronous operation.
        /// </summary>
        /// <param name="operand">An <see cref="Expression" /> that specifies the asynchronous operation to await.</param>
        /// <returns>An instance of the <see cref="AwaitCSharpExpression"/>.</returns>
        public static AwaitCSharpExpression Await(Expression operand)
        {
            return Await(operand, null);
        }

        /// <summary>
        /// Creates an <see cref="AwaitCSharpExpression"/> that represents awaiting an asynchronous operation.
        /// </summary>
        /// <param name="operand">An <see cref="Expression" /> that specifies the asynchronous operation to await.</param>
        /// <param name="getAwaiterMethod">The GetAwaiter method used to await the asynchronous operation.</param>
        /// <returns>An instance of the <see cref="AwaitCSharpExpression"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Awaiter", Justification = "Get a waiter :-)")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Done by helper method.")]
        public static AwaitCSharpExpression Await(Expression operand, MethodInfo getAwaiterMethod)
        {
            ContractUtils.RequiresNotNull(operand, nameof(operand));

            RequiresCanRead(operand, nameof(operand));

            var resultType = typeof(Type);
            ValidateAwaitPattern(operand.Type, ref getAwaiterMethod, out resultType);

            return new AwaitCSharpExpression(operand, getAwaiterMethod, resultType);
        }

        private static void ValidateAwaitPattern(Type operandType, ref MethodInfo getAwaiterMethod, out Type resultType)
        {
            if (getAwaiterMethod == null)
            {
                getAwaiterMethod = operandType.GetMethod("GetAwaiter", BindingFlags.Public | BindingFlags.Instance, null, Array.Empty<Type>(), null);
            }

            ContractUtils.RequiresNotNull(getAwaiterMethod, nameof(getAwaiterMethod));

            ValidateGetAwaiterMethod(operandType, getAwaiterMethod);
            ValidateAwaiterType(getAwaiterMethod.ReturnType, out resultType);
        }

        private static void ValidateGetAwaiterMethod(Type operandType, MethodInfo getAwaiterMethod)
        {
            ValidateMethodInfo(getAwaiterMethod);

            var getAwaiterParams = getAwaiterMethod.GetParametersCached();

            if (getAwaiterMethod.IsStatic)
            {
                if (getAwaiterParams.Length != 1)
                {
                    throw Error.GetAwaiterShouldTakeZeroParameters();
                }

                var firstParam = getAwaiterParams[0];
                if (!TypeUtils.AreReferenceAssignable(firstParam.ParameterType, operandType))
                {
                    throw LinqError.ExpressionTypeDoesNotMatchParameter(operandType, firstParam.ParameterType);
                }
            }
            else
            {
                if (getAwaiterParams.Length != 0)
                {
                    throw Error.GetAwaiterShouldTakeZeroParameters();
                }

                if (getAwaiterMethod.IsGenericMethod)
                {
                    throw Error.GetAwaiterShouldNotBeGeneric();
                }
            }

            var returnType = getAwaiterMethod.ReturnType;

            if (returnType == typeof(void) || returnType.IsByRef || returnType.IsPointer)
            {
                throw Error.GetAwaiterShouldReturnAwaiterType();
            }
        }

        private static void ValidateAwaiterType(Type awaiterType, out Type resultType)
        {
            if (!typeof(INotifyCompletion).IsAssignableFrom(awaiterType))
            {
                throw Error.AwaiterTypeShouldImplementINotifyCompletion(awaiterType);
            }

            var isCompleted = awaiterType.GetProperty("IsCompleted", BindingFlags.Public | BindingFlags.Instance);
            if (isCompleted == null || isCompleted.GetMethod == null)
            {
                throw Error.AwaiterTypeShouldHaveIsCompletedProperty(awaiterType);
            }

            if (isCompleted.PropertyType != typeof(bool))
            {
                throw Error.AwaiterIsCompletedShouldReturnBool(awaiterType);
            }

            if (isCompleted.GetIndexParameters().Length != 0)
            {
                throw Error.AwaiterIsCompletedShouldNotBeIndexer(awaiterType);
            }

            var getResult = awaiterType.GetMethod("GetResult", BindingFlags.Public | BindingFlags.Instance, null, Array.Empty<Type>(), null);
            if (getResult == null || getResult.IsGenericMethodDefinition)
            {
                throw Error.AwaiterTypeShouldHaveGetResultMethod(awaiterType);
            }

            var returnType = getResult.ReturnType;

            if (returnType.IsByRef || returnType.IsPointer)
            {
                throw Error.AwaiterGetResultTypeInvalid(awaiterType);
            }

            resultType = returnType;
        }
    }

    partial class CSharpExpressionVisitor
    {
        /// <summary>
        /// Visits the children of the <see cref="AwaitCSharpExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Following the visitor pattern from System.Linq.Expressions.")]
        protected internal virtual Expression VisitAwait(AwaitCSharpExpression node)
        {
            return node.Update(Visit(node.Operand));
        }
    }
}