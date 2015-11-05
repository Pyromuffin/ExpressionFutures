﻿// Prototyping extended expression trees for C#.
//
// bartde - October 2015

using Microsoft.CSharp.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Playground.ReflectionUtils;

namespace Playground
{
    class Program
    {
        static void Main()
        {
            Call();
            Invoke();
            New();
            Index();
            NewMultidimensionalArrayInit();
            AsyncLambda();
            While();
            DoWhile();
            Using();
            ForEach();
            For();
            ConditionalMember();
            ConditionalCall();
            ConditionalIndex();
        }

        static void Call()
        {
            Call1();
            Call2();
            Call3();
            Call4();
            Call5();
            Call6();
            Call7();
            Call8();
        }

        static void Call1()
        {
            var mtd = MethodInfoOf(() => Math.Min(default(int), default(int)));

            var val1 = mtd.GetParameters()[0];
            var val2 = mtd.GetParameters()[1];

            var arg0 = CSharpExpression.Bind(val1, Log(Expression.Constant(1), "A"));
            var arg1 = CSharpExpression.Bind(val2, Log(Expression.Constant(2), "B"));

            var call = CSharpExpression.Call(mtd, arg1, arg0);

            var res = Expression.Lambda<Func<int>>(call).Compile()();

            Console.WriteLine(res);
        }

        static void Call2()
        {
            var mtd = MethodInfoOf(() => Math.Min(default(int), default(int)));

            var val1 = mtd.GetParameters()[0];
            var val2 = mtd.GetParameters()[1];

            var arg0 = CSharpExpression.Bind(val1, Log(Expression.Constant(1), "A"));
            var arg1 = CSharpExpression.Bind(val2, Log(Expression.Constant(2), "B"));

            var call = CSharpExpression.Call(mtd, arg0, arg1);

            var res = Expression.Lambda<Func<int>>(call).Compile()();

            Console.WriteLine(res);
        }

        static void Call3()
        {
            var mtd = MethodInfoOf(() => F(default(int), default(int), default(int)));

            var val1 = mtd.GetParameters()[0];
            var val2 = mtd.GetParameters()[1];

            var arg0 = CSharpExpression.Bind(val1, Log(Expression.Constant(1), "A"));
            var arg1 = CSharpExpression.Bind(val2, Log(Expression.Constant(2), "B"));

            var call = CSharpExpression.Call(mtd, arg1, arg0);

            var res = Expression.Lambda<Func<int>>(call).Compile()();

            Console.WriteLine(res);
        }

        static void Call4()
        {
            var mtd = MethodInfoOf(() => F(default(int), default(int), default(int)));

            var val1 = mtd.GetParameters()[0];
            var val2 = mtd.GetParameters()[1];

            var arg0 = CSharpExpression.Bind(val1, Log(Expression.Constant(1), "A"));
            var arg1 = CSharpExpression.Bind(val2, Log(Expression.Constant(2), "B"));

            var call = CSharpExpression.Call(mtd, arg0, arg1);

            var res = Expression.Lambda<Func<int>>(call).Compile()();

            Console.WriteLine(res);
        }

        static void Call5()
        {
            var mtd = MethodInfoOf(() => F(default(int), default(int), default(int)));

            var val1 = mtd.GetParameters()[0];
            var val2 = mtd.GetParameters()[1];
            var val3 = mtd.GetParameters()[2];

            var arg0 = CSharpExpression.Bind(val1, Log(Expression.Constant(1), "A"));
            var arg1 = CSharpExpression.Bind(val2, Log(Expression.Constant(2), "B"));
            var arg2 = CSharpExpression.Bind(val3, Log(Expression.Constant(3), "C"));

            var call = CSharpExpression.Call(mtd, arg2, arg0, arg1);

            var res = Expression.Lambda<Func<int>>(call).Compile()();

            Console.WriteLine(res);
        }

        static void Call6()
        {
            var mtd = MethodInfoOf((Bar b) => b.F(default(int), default(int), default(int)));

            var val1 = mtd.GetParameters()[0];
            var val2 = mtd.GetParameters()[1];
            var val3 = mtd.GetParameters()[2];

            var obj = Log(Expression.Constant(new Bar()), "O");
            var arg0 = CSharpExpression.Bind(val1, Log(Expression.Constant(1), "A"));
            var arg1 = CSharpExpression.Bind(val2, Log(Expression.Constant(2), "B"));
            var arg2 = CSharpExpression.Bind(val3, Log(Expression.Constant(3), "C"));

            var call = CSharpExpression.Call(obj, mtd, arg2, arg0, arg1);

            var res = Expression.Lambda<Func<int>>(call).Compile()();

            Console.WriteLine(res);
        }

        static void Call7()
        {
            var x = default(int);
            var mtd = MethodInfoOf((string s) => int.TryParse(s, out x));

            var val1 = mtd.GetParameters()[0];
            var val2 = mtd.GetParameters()[1];

            var i = Expression.Parameter(typeof(int));
            var arg0 = CSharpExpression.Bind(val1, Log(Expression.Constant("42"), "A"));
            var arg1 = CSharpExpression.Bind(val2, Log(i, "B"));

            var call = CSharpExpression.Call(mtd, arg1, arg0);

            var res = Expression.Lambda<Func<int>>(Expression.Block(new[] { i }, call, i)).Compile()();

            Console.WriteLine(res);
        }

        static void Call8()
        {
            var x = default(int);
            var mtd = MethodInfoOf((string s) => int.TryParse(s, out x));

            var val1 = mtd.GetParameters()[0];
            var val2 = mtd.GetParameters()[1];

            var i = Expression.Parameter(typeof(int));
            var arg0 = CSharpExpression.Bind(val1, Log(Expression.Constant("42"), "A"));
            var arg1 = CSharpExpression.Bind(val2, i);

            var call = CSharpExpression.Call(mtd, arg1, arg0);

            var res = Expression.Lambda<Func<int>>(Expression.Block(new[] { i }, call, i)).Compile()();

            Console.WriteLine(res);
        }

        static void Invoke()
        {
            Invoke1();
            Invoke2();
        }

        static void Invoke1()
        {
            var f = new Func<int, int, int>((a, b) => a + b);
            var mtd = MethodInfoOf(() => f.Invoke(default(int), default(int)));

            var val1 = mtd.GetParameters()[0];
            var val2 = mtd.GetParameters()[1];

            var obj = Log(Expression.Constant(f), "O");
            var arg0 = CSharpExpression.Bind(val1, Log(Expression.Constant(1), "A"));
            var arg1 = CSharpExpression.Bind(val2, Log(Expression.Constant(2), "B"));

            var invoke = CSharpExpression.Invoke(obj, arg1, arg0);

            var res = Expression.Lambda<Func<int>>(invoke).Compile()();

            Console.WriteLine(res);
        }

        static void Invoke2()
        {
            var f = new Func<int, int, int>((a, b) => a + b);
            var mtd = MethodInfoOf(() => f.Invoke(default(int), default(int)));

            var val1 = mtd.GetParameters()[0];
            var val2 = mtd.GetParameters()[1];

            var obj = Log(Expression.Constant(f), "O");
            var arg0 = CSharpExpression.Bind(val1, Log(Expression.Constant(1), "A"));
            var arg1 = CSharpExpression.Bind(val2, Log(Expression.Constant(2), "B"));

            var invoke = CSharpExpression.Invoke(obj, arg0, arg1);

            var res = Expression.Lambda<Func<int>>(invoke).Compile()();

            Console.WriteLine(res);
        }

        static void New()
        {
            New1();
            New2();
        }

        static void New1()
        {
            var ctor = ConstructorInfoOf(() => new TimeSpan(default(int), default(int), default(int)));

            var val1 = ctor.GetParameters()[0];
            var val2 = ctor.GetParameters()[1];
            var val3 = ctor.GetParameters()[2];

            var arg0 = CSharpExpression.Bind(val1, Log(Expression.Constant(1), "A"));
            var arg1 = CSharpExpression.Bind(val2, Log(Expression.Constant(2), "B"));
            var arg2 = CSharpExpression.Bind(val3, Log(Expression.Constant(3), "C"));

            var @new = CSharpExpression.New(ctor, arg2, arg0, arg1);

            var res = Expression.Lambda<Func<TimeSpan>>(@new).Compile()();

            Console.WriteLine(res);
        }

        static void New2()
        {
            var ctor = ConstructorInfoOf(() => new TimeSpan(default(int), default(int), default(int)));

            var val1 = ctor.GetParameters()[0];
            var val2 = ctor.GetParameters()[1];
            var val3 = ctor.GetParameters()[2];

            var arg0 = CSharpExpression.Bind(val1, Log(Expression.Constant(1), "A"));
            var arg1 = CSharpExpression.Bind(val2, Log(Expression.Constant(2), "B"));
            var arg2 = CSharpExpression.Bind(val3, Log(Expression.Constant(3), "C"));

            var @new = CSharpExpression.New(ctor, arg0, arg1, arg2);

            var res = Expression.Lambda<Func<TimeSpan>>(@new).Compile()();

            Console.WriteLine(res);
        }

        static void Index()
        {
            Index1();
            Index2();
        }

        static void Index1()
        {
            var get = MethodInfoOf((Field f) => f[default(int), default(int), default(int)]);
            var idx = get.DeclaringType.GetProperty(get.Name.Substring("get_".Length));

            var val1 = idx.GetIndexParameters()[0];
            var val2 = idx.GetIndexParameters()[1];
            var val3 = idx.GetIndexParameters()[2];

            var obj = Log(Expression.Constant(new Field()), "O");
            var arg0 = CSharpExpression.Bind(val1, Log(Expression.Constant(1), "A"));
            var arg1 = CSharpExpression.Bind(val2, Log(Expression.Constant(2), "B"));
            var arg2 = CSharpExpression.Bind(val3, Log(Expression.Constant(3), "C"));

            var index = CSharpExpression.Index(obj, idx, arg2, arg0, arg1);

            var res = Expression.Lambda<Func<int>>(index).Compile()();

            Console.WriteLine(res);
        }

        static void Index2()
        {
            var get = MethodInfoOf((Field f) => f[default(int), default(int), default(int)]);
            var idx = get.DeclaringType.GetProperty(get.Name.Substring("get_".Length));

            var val1 = idx.GetIndexParameters()[0];
            var val2 = idx.GetIndexParameters()[1];
            var val3 = idx.GetIndexParameters()[2];

            var obj = Log(Expression.Constant(new Field()), "O");
            var arg0 = CSharpExpression.Bind(val1, Log(Expression.Constant(1), "A"));
            var arg1 = CSharpExpression.Bind(val2, Log(Expression.Constant(2), "B"));
            var arg2 = CSharpExpression.Bind(val3, Log(Expression.Constant(3), "C"));

            var index = CSharpExpression.Index(obj, idx, arg0, arg1, arg2);

            var res = Expression.Lambda<Func<int>>(index).Compile()();

            Console.WriteLine(res);
        }

        static void NewMultidimensionalArrayInit()
        {
            var expr = CSharpExpression.NewMultidimensionalArrayInit(typeof(int), new[] { 2, 3, 5 }, Enumerable.Range(0, 30).Select(i => Expression.Constant(i)));

            var res = Expression.Lambda<Func<int[,,]>>(expr).Compile()();

            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    for (var k = 0; k < 5; k++)
                    {
                        var e = expr.GetExpression(i, j, k);
                        var v = res.GetValue(i, j, k);
                        Console.WriteLine(e + " = " + v);
                    }
                }
            }
        }

        static void AsyncLambda()
        {
            AsyncLambda1();
            AsyncLambda2();
            AsyncLambda3();
            AsyncLambda4();
            AsyncLambda5();
            AsyncLambda6();
        }

        static void AsyncLambda1()
        {
            var async = CSharpExpression.AsyncLambda<Func<Task<int>>>(Expression.Constant(42));
            var res = async.Compile()();
            Console.WriteLine(res.Result);
        }

        static void AsyncLambda2()
        {
            var async = CSharpExpression.AsyncLambda(Expression.Constant(42));
            var res = (Task<int>)async.Compile().DynamicInvoke();
            Console.WriteLine(res.Result);
        }

        static void AsyncLambda3()
        {
            var await = CSharpExpression.Await(Expression.Constant(Task.FromResult(42)));
            var async = CSharpExpression.AsyncLambda(await);
            var res = (Task<int>)async.Compile().DynamicInvoke();
            Console.WriteLine(res.Result);
        }

        static void AsyncLambda4()
        {
            var delay = (Expression<Action>)(() => Task.Delay(100));
            var async = CSharpExpression.AsyncLambda<Func<Task<int>>>(
                Expression.Block(
                    CSharpExpression.Await(delay.Body),
                    CSharpExpression.Await(Expression.Constant(Task.FromResult(42)))
                )
            );
            var res = async.Compile()();
            Console.WriteLine(res.Result);
        }

        static void AsyncLambda5()
        {
            var i = Expression.Parameter(typeof(int));
            var delay = (Expression<Action>)(() => Task.Delay(100));
            var cout = MethodInfoOf(() => Console.WriteLine(default(int)));
            var brk = Expression.Label();
            var cnt = Expression.Label();
            var async = CSharpExpression.AsyncLambda<Func<Task>>(
                Expression.Block(
                    new[] { i },
                    Expression.Assign(i, Expression.Constant(0)),
                    Expression.Loop(
                        Expression.Block(
                            Expression.IfThen(Expression.Equal(i, Expression.Constant(10)), Expression.Break(brk)),
                            CSharpExpression.Await(delay.Body),
                            Expression.Call(cout, i),
                            Expression.PostIncrementAssign(i)
                        ), brk, cnt
                    )
                )
            );
            var res = async.Compile()();
            res.Wait();
        }

        static void AsyncLambda6()
        {
            var delay = (Expression<Action>)(() => Task.Delay(1000));
            var async = CSharpExpression.AsyncLambda<Func<Task<int>>>(
                Expression.Block(
                    Expression.TryCatch(
                        CSharpExpression.Await(delay.Body),
                        Expression.Catch(
                            Expression.Parameter(typeof(Exception)),
                            Expression.Empty()
                        )
                    ),
                    CSharpExpression.Await(Expression.Constant(Task.FromResult(42)))
                )
            );
            var res = async.Compile()();
            Console.WriteLine(res.Result);
        }

        static void While()
        {
            var i = Expression.Parameter(typeof(int));
            var cout = MethodInfoOf(() => Console.WriteLine(default(int)));
            var loop = Expression.Lambda<Action>(
                Expression.Block(
                    new[] { i },
                    Expression.Assign(i, Expression.Constant(0)),
                    CSharpExpression.While(
                        Expression.LessThan(i, Expression.Constant(10)),
                        Expression.Block(
                            Expression.Call(cout, i),
                            Expression.PostIncrementAssign(i)
                        )
                    )
                )
            );
            loop.Compile()();
        }

        static void DoWhile()
        {
            var i = Expression.Parameter(typeof(int));
            var cout = MethodInfoOf(() => Console.WriteLine(default(int)));
            var loop = Expression.Lambda<Action>(
                Expression.Block(
                    new[] { i },
                    Expression.Assign(i, Expression.Constant(0)),
                    CSharpExpression.Do(
                        Expression.Block(
                            Expression.Call(cout, i),
                            Expression.PostIncrementAssign(i)
                        ),
                        Expression.LessThan(i, Expression.Constant(10))
                    )
                )
            );
            loop.Compile()();
        }

        static void Using()
        {
            Using1();
            Using2();
            Using3();
            Using4();
            Using5();
            Using6();
            Using7();
        }

        static void Using1()
        {
            var ctor = ConstructorInfoOf(() => new RC(default(string)));
            var cout = MethodInfoOf(() => Console.WriteLine(default(string)));
            var @using = Expression.Lambda<Action>(
                CSharpExpression.Using(
                    Expression.New(ctor, Expression.Constant("X")),
                    Expression.Call(cout, Expression.Constant("B"))
                )
            );
            @using.Compile()();
        }

        static void Using2()
        {
            var ctor = ConstructorInfoOf(() => new RC(default(string)));
            var prnt = MethodInfoOf((RC r) => r.Print());
            var resv = Expression.Parameter(typeof(RC));
            var @using = Expression.Lambda<Action>(
                CSharpExpression.Using(
                    resv,
                    Expression.New(ctor, Expression.Constant("B")),
                    Expression.Call(resv, prnt)
                )
            );
            @using.Compile()();
        }

        static void Using3()
        {
            var ctor = ConstructorInfoOf(() => new RC(default(string)));
            var cout = MethodInfoOf(() => Console.WriteLine(default(string)));
            var resv = Expression.Parameter(typeof(IDisposable));
            var @using = Expression.Lambda<Action>(
                CSharpExpression.Using(
                    resv,
                    Expression.New(ctor, Expression.Constant("X")),
                    Expression.Call(cout, Expression.Constant("B"))
                )
            );
            @using.Compile()();
        }

        static void Using4()
        {
            var ctor = ConstructorInfoOf(() => new RC(default(string)));
            var cout = MethodInfoOf(() => Console.WriteLine(default(string)));
            var resv = Expression.Parameter(typeof(IDisposable));
            var @using = Expression.Lambda<Action>(
                CSharpExpression.Using(
                    resv,
                    Expression.New(ctor, Expression.Constant("X")),
                    Expression.Block(
                        Expression.Call(cout, Expression.Constant("N")),
                        Expression.Assign(resv, Expression.Constant(null, resv.Type))
                    )
                )
            );
            @using.Compile()();
        }

        static void Using5()
        {
            var ctor = ConstructorInfoOf(() => new RV(default(string)));
            var prnt = MethodInfoOf((RV r) => r.Print());
            var resv = Expression.Parameter(typeof(RV));
            var @using = Expression.Lambda<Action>(
                CSharpExpression.Using(
                    resv,
                    Expression.New(ctor, Expression.Constant("B")),
                    Expression.Call(resv, prnt)
                )
            );
            @using.Compile()();
        }

        static void Using6()
        {
            var ctor = ConstructorInfoOf(() => new RV(default(string)));
            var cout = MethodInfoOf(() => Console.WriteLine(default(string))); ;
            var @using = Expression.Lambda<Action>(
                CSharpExpression.Using(
                    Expression.New(ctor, Expression.Constant("X")),
                    Expression.Call(cout, Expression.Constant("B"))
                )
            );
            @using.Compile()();
        }

        static void Using7()
        {
            var ctor = ConstructorInfoOf(() => new RV(default(string)));
            var prnt = MethodInfoOf((RV r) => r.Print());
            var resv = Expression.Parameter(typeof(RV?));
            var @using = Expression.Lambda<Action>(
                CSharpExpression.Using(
                    resv,
                    Expression.Convert(Expression.New(ctor, Expression.Constant("B")), typeof(RV?)),
                    Expression.Call(Expression.Property(resv, "Value"), prnt)
                )
            );
            @using.Compile()();
        }

        static void ForEach()
        {
            ForEach1();
            ForEach2();
            ForEach3();
            ForEach4();
            ForEach5();
        }

        static void ForEach1()
        {
            var x = Expression.Parameter(typeof(int));
            var xs = Expression.Constant(new[] { 2, 3, 5 });
            var cout = MethodInfoOf(() => Console.WriteLine(default(int)));
            var loop = Expression.Lambda<Action>(
                CSharpExpression.ForEach(x, xs,
                    Expression.Call(cout, x)
                )
            );
            loop.Compile()();
        }

        static void ForEach2()
        {
            var x = Expression.Parameter(typeof(int?));
            var xs = Expression.Constant(new[] { 2, 3, 5 });
            var cout = MethodInfoOf(() => Console.WriteLine(default(int)));
            var loop = Expression.Lambda<Action>(
                CSharpExpression.ForEach(x, xs,
                    Expression.Call(cout, Expression.Property(x, "Value"))
                )
            );
            loop.Compile()();
        }

        static void ForEach3()
        {
            var x = Expression.Parameter(typeof(int));
            var xs = Expression.Constant(new int?[] { 2, 3, 5 });
            var cout = MethodInfoOf(() => Console.WriteLine(default(int)));
            var loop = Expression.Lambda<Action>(
                CSharpExpression.ForEach(x, xs,
                    Expression.Call(cout, x)
                )
            );
            loop.Compile()();
        }

        static void ForEach4()
        {
            var x = Expression.Parameter(typeof(string));
            var xs = Expression.Constant(new object[] { "bar", "foo", "qux" });
            var cout = MethodInfoOf(() => Console.WriteLine(default(string)));
            var loop = Expression.Lambda<Action>(
                CSharpExpression.ForEach(x, xs,
                    Expression.Call(cout, x)
                )
            );
            loop.Compile()();
        }

        static void ForEach5()
        {
            var x = Expression.Parameter(typeof(string));
            var xs = Expression.Constant(new List<string> { "bar", "foo", "qux" });
            var cout = MethodInfoOf(() => Console.WriteLine(default(string)));
            var loop = Expression.Lambda<Action>(
                CSharpExpression.ForEach(x, xs,
                    Expression.Call(cout, x)
                )
            );
            loop.Compile()();
        }

        static void For()
        {
            For1();
        }

        static void For1()
        {
            var i = Expression.Parameter(typeof(int));
            var init = Expression.Assign(i, Expression.Constant(0));
            var test = Expression.LessThan(i, Expression.Constant(5));
            var iterate = Expression.PostIncrementAssign(i);
            var cout = MethodInfoOf(() => Console.WriteLine(default(int)));
            var body = Expression.Call(cout, i);
            var loop = Expression.Lambda<Action>(
                CSharpExpression.For(new[] { init }, test, new[] { iterate },
                    body
                )
            );
            loop.Compile()();
        }

        static void ConditionalMember()
        {
            ConditionalMember1();
            ConditionalMember2();
            ConditionalMember3();
        }

        static void ConditionalMember1()
        {
            var p = Expression.Parameter(typeof(TimeSpan?));
            var e = Expression.Lambda<Func<TimeSpan?, int?>>(CSharpExpression.ConditionalProperty(p, "Seconds"), p);
            var f = e.Compile();
            Console.WriteLine(f(null));
            Console.WriteLine(f(TimeSpan.FromSeconds(42)));
        }

        static void ConditionalMember2()
        {
            var p = Expression.Parameter(typeof(string));
            var e = Expression.Lambda<Func<string, int?>>(CSharpExpression.ConditionalProperty(p, "Length"), p);
            var f = e.Compile();
            Console.WriteLine(f(null));
            Console.WriteLine(f("bar"));
        }

        static void ConditionalMember3()
        {
            var p = Expression.Parameter(typeof(DateTimeOffset?));
            var e = Expression.Lambda<Func<DateTimeOffset?, int?>>(CSharpExpression.ConditionalProperty(CSharpExpression.ConditionalProperty(p, "Offset"), "Hours"), p);
            var f = e.Compile();
            Console.WriteLine(f(null));
            Console.WriteLine(f(DateTimeOffset.Now));
        }

        static void ConditionalCall()
        {
            ConditionalCall1();
            ConditionalCall2();
            ConditionalCall3();
        }

        static void ConditionalCall1()
        {
            var addYears = MethodInfoOf((DateTime dt) => dt.AddYears(default(int)));
            var p0 = addYears.GetParameters()[0];

            var p = Expression.Parameter(typeof(DateTime?));
            var e = Expression.Lambda<Func<DateTime?, DateTime?>>(CSharpExpression.ConditionalCall(p, addYears, CSharpExpression.Bind(p0, Expression.Constant(1))), p);
            var f = e.Compile();
            Console.WriteLine(f(null));
            Console.WriteLine(f(DateTime.Now));
        }

        static void ConditionalCall2()
        {
            var toString = MethodInfoOf((DateTime dt) => dt.ToString());

            var p = Expression.Parameter(typeof(DateTime?));
            var e = Expression.Lambda<Func<DateTime?, string>>(CSharpExpression.ConditionalCall(p, toString), p);
            var f = e.Compile();
            Console.WriteLine(f(null));
            Console.WriteLine(f(DateTime.Now));
        }

        static void ConditionalCall3()
        {
            var toUpper = MethodInfoOf((string s) => s.ToUpper());
            var toLower = MethodInfoOf((string s) => s.ToLower());

            var p = Expression.Parameter(typeof(string));
            var e = Expression.Lambda<Func<string, string>>(CSharpExpression.ConditionalCall(CSharpExpression.ConditionalCall(p, toLower), toUpper), p);
            var f = e.Compile();
            Console.WriteLine(f(null));
            Console.WriteLine(f("bar"));
        }

        static void ConditionalIndex()
        {
            ConditionalIndex1();
        }

        static void ConditionalIndex1()
        {
            var index = PropertyInfoOf((List<int> xs) => xs[default(int)]);
            var p0 = index.GetIndexParameters()[0];

            var p = Expression.Parameter(typeof(List<int>));
            var e = Expression.Lambda<Func<List<int>, int?>>(CSharpExpression.ConditionalIndex(p, index, CSharpExpression.Bind(p0, Expression.Constant(0))), p);
            var f = e.Compile();
            Console.WriteLine(f(null));
            Console.WriteLine(f(new List<int> { 42 }));
        }

        static int F(int x, int y, int z = 42)
        {
            return x * y - z;
        }

        static Expression Log(Expression expression, string log)
        {
            var cout = MethodInfoOf(() => Console.WriteLine(default(string)));
            return Expression.Block(Expression.Call(cout, Expression.Constant(log, typeof(string))), expression);
        }
    }

    class Bar
    {
        public int F(int x, int y, int z = 42)
        {
            return x * y - z;
        }
    }

    class Field
    {
        public int this[int x, int y, int z]
        {
            get
            {
                return x + y + z;
            }
        }
    }

    class RC : IDisposable
    {
        private readonly string _message;

        public RC(string message)
        {
            _message = message;
        }

        public void Print()
        {
            Console.WriteLine(_message);
        }

        public void Dispose()
        {
            Console.WriteLine("D");
        }
    }

    struct RV : IDisposable
    {
        private readonly string _message;

        public RV(string message)
        {
            _message = message;
        }

        public void Print()
        {
            Console.WriteLine(_message);
        }

        public void Dispose()
        {
            Console.WriteLine("D");
        }
    }
}