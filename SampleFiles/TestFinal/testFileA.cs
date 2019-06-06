using System;

namespace NamespaceA
{
	class classInferfaceA
	{
		public interface interfaceA
		{
			void doThis(Action action);
		}
	}
	class classA
	{
		A()
		{
			int i=0;
		}
		A(int q)
		{
			q=10;
		}
		struct structA
		{
			int a;
			char b;
		}
		delegate void Delegate1();
		delegate void Delegate2();

		static void method(Delegate1 d, Delegate2 e, System.Delegate f)
		{
		// Sample comment
		System.Console.WriteLine(d == f);
		}
	}
	static void Main(string[] args)
	{
		classA objA = new classA(7);
		enum days { Monday, Tuesday, Wednesday }
	}
}