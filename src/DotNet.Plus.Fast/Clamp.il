
.assembly extern mscorlib {}


.class public abstract sealed beforefieldinit DotNet.Plus.Fast.Clamp
	extends [netstandard]System.Object
{
	.method public hidebysig static int32 Value(int32 number, int32 min, int32 max) cil managed aggressiveinlining
	{
		.maxstack 2

		ldarg max
		ldarg min
		blt.s BADARG
		
		ldarg number
		ldarg min
		clt				// Push 1 if value number < min
		brtrue USEMIN

		ldarg number
		ldarg max
		cgt				// Push 1 if value number > max
		brtrue USEMAX

		ldarg number
		ret

		BADARG: 
		ldstr "max"
		ldstr "Max value needs to be larger then min value"
		newobj instance void [netstandard]System.ArgumentOutOfRangeException::.ctor(string, string)
		throw

		USEMIN:
		ldarg min
		ret

		USEMAX:
		ldarg max
		ret
	}

	.method public hidebysig static !!TValue Value<valuetype .ctor ([mscorlib]System.ValueType) TValue>(!!TValue number, !!TValue min, !!TValue max) cil managed aggressiveinlining
	{
		.maxstack 2

		ldarg max
		ldarg min
		blt.s BADARG
		
		ldarg number
		ldarg min
		clt				// Push 1 if value number < min
		brtrue USEMIN

		ldarg number
		ldarg max
		cgt				// Push 1 if value number > max
		brtrue USEMAX

		ldarg number
		ret

		BADARG: 
		ldstr "max"
		ldstr "Max value needs to be larger then min value"
		newobj instance void [netstandard]System.ArgumentOutOfRangeException::.ctor(string, string)
		throw

		USEMIN:
		ldarg min
		ret

		USEMAX:
		ldarg max
		ret
	}

}
