.assembly UnsafeIL {
	.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilationRelaxationsAttribute::.ctor(int32) = (
		01 00 08 00 00 00 00 00
	)
	.custom instance void [mscorlib]System.Runtime.CompilerServices.ExtensionAttribute::.ctor() = (
		01 00 00 00
	)
	.custom instance void [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::.ctor() = (
		01 00 01 00 54 02 16 57 72 61 70 4e 6f 6e 45 78
		63 65 70 74 69 6f 6e 54 68 72 6f 77 73 01
	)
	.custom instance void [mscorlib]System.Diagnostics.DebuggableAttribute::.ctor(valuetype [mscorlib]System.Diagnostics.DebuggableAttribute/DebuggingModes) = (
		01 00 02 00 00 00 00 00
	)
	.hash algorithm 0x00008004 // SHA1
	.ver 6:0:0:0
}

.namespace UnsafeIL {
	.class public auto ansi abstract sealed beforefieldinit Unsafe extends [mscorlib]System.Object {
		.method public hidebysig static 
			!!T Read<T> (
				void* source
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x2114
			// Header size: 12
			// Code size: 7 (0x7)
			.maxstack 1

			IL_0000: ldarg.0
			IL_0001: ldobj !!T
			IL_0006: ret
		} // end of method Unsafe::Read

		.method public hidebysig static 
			!!T ReadUnaligned<T> (
				void* source
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x2128
			// Header size: 12
			// Code size: 10 (0xa)
			.maxstack 1

			IL_0000: ldarg.0
			IL_0001: unaligned. 1
			IL_0004: ldobj !!T
			IL_0009: ret
		} // end of method Unsafe::ReadUnaligned

		.method public hidebysig static 
			!!T ReadUnaligned<T> (
				uint8& source
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x2140
			// Header size: 12
			// Code size: 10 (0xa)
			.maxstack 1

			IL_0000: ldarg.0
			IL_0001: unaligned. 1
			IL_0004: ldobj !!T
			IL_0009: ret
		} // end of method Unsafe::ReadUnaligned

		.method public hidebysig static 
			void Write<T> (
				void* destination,
				!!T 'value'
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x2158
			// Header size: 12
			// Code size: 8 (0x8)
			.maxstack 2

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: stobj !!T
			IL_0007: ret
		} // end of method Unsafe::Write

		.method public hidebysig static 
			void WriteUnaligned<T> (
				void* destination,
				!!T 'value'
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x216c
			// Header size: 12
			// Code size: 11 (0xb)
			.maxstack 2

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: unaligned. 1
			IL_0005: stobj !!T
			IL_000a: ret
		} // end of method Unsafe::WriteUnaligned

		.method public hidebysig static 
			void WriteUnaligned<T> (
				uint8& destination,
				!!T 'value'
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x2184
			// Header size: 12
			// Code size: 11 (0xb)
			.maxstack 2

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: unaligned. 1
			IL_0005: stobj !!T
			IL_000a: ret
		} // end of method Unsafe::WriteUnaligned

		.method public hidebysig static 
			void Copy<T> (
				void* destination,
				!!T& source
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x219c
			// Header size: 12
			// Code size: 13 (0xd)
			.maxstack 2

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: ldobj !!T
			IL_0007: stobj !!T
			IL_000c: ret
		} // end of method Unsafe::Copy

		.method public hidebysig static 
			void Copy<T> (
				!!T& destination,
				void* source
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x21b8
			// Header size: 12
			// Code size: 13 (0xd)
			.maxstack 2

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: ldobj !!T
			IL_0007: stobj !!T
			IL_000c: ret
		} // end of method Unsafe::Copy

		.method public hidebysig static 
			void* AsPointer<T> (
				!!T& 'value'
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x21d4
			// Header size: 12
			// Code size: 3 (0x3)
			.maxstack 1

			IL_0000: ldarg.0
			IL_0001: conv.u
			IL_0002: ret
		} // end of method Unsafe::AsPointer

		.method public hidebysig static 
			void SkipInit<T> (
				[out] !!T& 'value'
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x21e4
			// Header size: 12
			// Code size: 1 (0x1)
			.maxstack 0

			IL_0000: ret
		} // end of method Unsafe::SkipInit

		.method public hidebysig static 
			int32 SizeOf<T> () cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x21f4
			// Header size: 12
			// Code size: 7 (0x7)
			.maxstack 1

			IL_0000: sizeof !!T
			IL_0006: ret
		} // end of method Unsafe::SizeOf

		.method public hidebysig static 
			void CopyBlock (
				void* destination,
				void* source,
				uint32 byteCount
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x2208
			// Header size: 12
			// Code size: 6 (0x6)
			.maxstack 3

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: ldarg.2
			IL_0003: cpblk
			IL_0005: ret
		} // end of method Unsafe::CopyBlock

		.method public hidebysig static 
			void CopyBlock (
				uint8& destination,
				uint8& source,
				uint32 byteCount
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x221c
			// Header size: 12
			// Code size: 6 (0x6)
			.maxstack 3

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: ldarg.2
			IL_0003: cpblk
			IL_0005: ret
		} // end of method Unsafe::CopyBlock

		.method public hidebysig static 
			void CopyBlockUnaligned (
				void* destination,
				void* source,
				uint32 byteCount
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x2230
			// Header size: 12
			// Code size: 9 (0x9)
			.maxstack 3

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: ldarg.2
			IL_0003: unaligned. 1
			IL_0006: cpblk
			IL_0008: ret
		} // end of method Unsafe::CopyBlockUnaligned

		.method public hidebysig static 
			void CopyBlockUnaligned (
				uint8& destination,
				uint8& source,
				uint32 byteCount
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x2248
			// Header size: 12
			// Code size: 9 (0x9)
			.maxstack 3

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: ldarg.2
			IL_0003: unaligned. 1
			IL_0006: cpblk
			IL_0008: ret
		} // end of method Unsafe::CopyBlockUnaligned

		.method public hidebysig static 
			void InitBlock (
				void* startAddress,
				uint8 'value',
				uint32 byteCount
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x2260
			// Header size: 12
			// Code size: 6 (0x6)
			.maxstack 3

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: ldarg.2
			IL_0003: initblk
			IL_0005: ret
		} // end of method Unsafe::InitBlock

		.method public hidebysig static 
			void InitBlock (
				uint8& startAddress,
				uint8 'value',
				uint32 byteCount
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x2274
			// Header size: 12
			// Code size: 6 (0x6)
			.maxstack 3

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: ldarg.2
			IL_0003: initblk
			IL_0005: ret
		} // end of method Unsafe::InitBlock

		.method public hidebysig static 
			void InitBlockUnaligned (
				void* startAddress,
				uint8 'value',
				uint32 byteCount
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x2288
			// Header size: 12
			// Code size: 9 (0x9)
			.maxstack 3

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: ldarg.2
			IL_0003: unaligned. 1
			IL_0006: initblk
			IL_0008: ret
		} // end of method Unsafe::InitBlockUnaligned

		.method public hidebysig static 
			void InitBlockUnaligned (
				uint8& startAddress,
				uint8 'value',
				uint32 byteCount
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x22a0
			// Header size: 12
			// Code size: 9 (0x9)
			.maxstack 3

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: ldarg.2
			IL_0003: unaligned. 1
			IL_0006: initblk
			IL_0008: ret
		} // end of method Unsafe::InitBlockUnaligned

		.method public hidebysig static 
			!!T As<class T> (
				object o
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x22b8
			// Header size: 12
			// Code size: 2 (0x2)
			.maxstack 1

			IL_0000: ldarg.0
			IL_0001: ret
		} // end of method Unsafe::As

		.method public hidebysig static 
			!!T& AsRef<T> (
				void* source
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x22c8
			// Header size: 12
			// Code size: 2 (0x2)
			.maxstack 1

			IL_0000: ldarg.0
			IL_0001: ret
		} // end of method Unsafe::AsRef

		.method public hidebysig static 
			!!T& AsRef<T> (
				!!T& source
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x22d8
			// Header size: 12
			// Code size: 2 (0x2)
			.maxstack 1

			IL_0000: ldarg.0
			IL_0001: ret
		} // end of method Unsafe::AsRef

		.method public hidebysig static 
			!!TTo& As<TFrom, TTo> (
				!!TFrom& source
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x22e8
			// Header size: 12
			// Code size: 2 (0x2)
			.maxstack 1

			IL_0000: ldarg.0
			IL_0001: ret
		} // end of method Unsafe::As

		.method public hidebysig static 
			!!T& Unbox<valuetype .ctor ([System.Runtime]System.ValueType) T> (
				object 'box'
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x22f8
			// Header size: 12
			// Code size: 7 (0x7)
			.maxstack 1

			IL_0000: ldarg.0
			IL_0001: unbox !!T
			IL_0006: ret
		} // end of method Unsafe::Unbox

		.method public hidebysig static 
			!!T& Add<T> (
				!!T& source,
				int32 elementOffset
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x230c
			// Header size: 12
			// Code size: 12 (0xc)
			.maxstack 3

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: sizeof !!T
			IL_0008: conv.i
			IL_0009: mul
			IL_000a: add
			IL_000b: ret
		} // end of method Unsafe::Add

		.method public hidebysig static 
			void* Add<T> (
				void* source,
				int32 elementOffset
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x2324
			// Header size: 12
			// Code size: 12 (0xc)
			.maxstack 3

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: sizeof !!T
			IL_0008: conv.i
			IL_0009: mul
			IL_000a: add
			IL_000b: ret
		} // end of method Unsafe::Add

		.method public hidebysig static 
			!!T& Add<T> (
				!!T& source,
				native int elementOffset
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x233c
			// Header size: 12
			// Code size: 11 (0xb)
			.maxstack 3

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: sizeof !!T
			IL_0008: mul
			IL_0009: add
			IL_000a: ret
		} // end of method Unsafe::Add

		.method public hidebysig static 
			!!T& Add<T> (
				!!T& source,
				native uint elementOffset
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x2354
			// Header size: 12
			// Code size: 11 (0xb)
			.maxstack 3

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: sizeof !!T
			IL_0008: mul
			IL_0009: add
			IL_000a: ret
		} // end of method Unsafe::Add

		.method public hidebysig static 
			!!T& AddByteOffset<T> (
				!!T& source,
				native int byteOffset
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x236c
			// Header size: 12
			// Code size: 4 (0x4)
			.maxstack 2

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: add
			IL_0003: ret
		} // end of method Unsafe::AddByteOffset

		.method public hidebysig static 
			!!T& AddByteOffset<T> (
				!!T& source,
				native uint byteOffset
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x237c
			// Header size: 12
			// Code size: 4 (0x4)
			.maxstack 2

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: add
			IL_0003: ret
		} // end of method Unsafe::AddByteOffset

		.method public hidebysig static 
			!!T& Subtract<T> (
				!!T& source,
				int32 elementOffset
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x238c
			// Header size: 12
			// Code size: 12 (0xc)
			.maxstack 3

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: sizeof !!T
			IL_0008: conv.i
			IL_0009: mul
			IL_000a: sub
			IL_000b: ret
		} // end of method Unsafe::Subtract

		.method public hidebysig static 
			void* Subtract<T> (
				void* source,
				int32 elementOffset
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x23a4
			// Header size: 12
			// Code size: 12 (0xc)
			.maxstack 3

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: sizeof !!T
			IL_0008: conv.i
			IL_0009: mul
			IL_000a: sub
			IL_000b: ret
		} // end of method Unsafe::Subtract

		.method public hidebysig static 
			!!T& Subtract<T> (
				!!T& source,
				native int elementOffset
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x23bc
			// Header size: 12
			// Code size: 11 (0xb)
			.maxstack 3

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: sizeof !!T
			IL_0008: mul
			IL_0009: sub
			IL_000a: ret
		} // end of method Unsafe::Subtract

		.method public hidebysig static 
			!!T& Subtract<T> (
				!!T& source,
				native uint elementOffset
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x23d4
			// Header size: 12
			// Code size: 11 (0xb)
			.maxstack 3

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: sizeof !!T
			IL_0008: mul
			IL_0009: sub
			IL_000a: ret
		} // end of method Unsafe::Subtract

		.method public hidebysig static 
			!!T& SubtractByteOffset<T> (
				!!T& source,
				native int byteOffset
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x23ec
			// Header size: 12
			// Code size: 4 (0x4)
			.maxstack 2

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: sub
			IL_0003: ret
		} // end of method Unsafe::SubtractByteOffset

		.method public hidebysig static 
			!!T& SubtractByteOffset<T> (
				!!T& source,
				native uint byteOffset
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x23fc
			// Header size: 12
			// Code size: 4 (0x4)
			.maxstack 2

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: sub
			IL_0003: ret
		} // end of method Unsafe::SubtractByteOffset

		.method public hidebysig static 
			native int ByteOffset<T> (
				!!T& origin,
				!!T& target
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x240c
			// Header size: 12
			// Code size: 4 (0x4)
			.maxstack 2

			IL_0000: ldarg.1
			IL_0001: ldarg.0
			IL_0002: sub
			IL_0003: ret
		} // end of method Unsafe::ByteOffset

		.method public hidebysig static 
			bool AreSame<T> (
				!!T& left,
				!!T& right
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x241c
			// Header size: 12
			// Code size: 5 (0x5)
			.maxstack 2

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: ceq
			IL_0004: ret
		} // end of method Unsafe::AreSame

		.method public hidebysig static 
			bool IsAddressGreaterThan<T> (
				!!T& left,
				!!T& right
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x2430
			// Header size: 12
			// Code size: 5 (0x5)
			.maxstack 2

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: cgt.un
			IL_0004: ret
		} // end of method Unsafe::IsAddressGreaterThan

		.method public hidebysig static 
			bool IsAddressLessThan<T> (
				!!T& left,
				!!T& right
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x2444
			// Header size: 12
			// Code size: 5 (0x5)
			.maxstack 2

			IL_0000: ldarg.0
			IL_0001: ldarg.1
			IL_0002: clt.un
			IL_0004: ret
		} // end of method Unsafe::IsAddressLessThan

		.method public hidebysig static 
			bool IsNullRef<T> (
				!!T& source
			) cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x2458
			// Header size: 12
			// Code size: 6 (0x6)
			.maxstack 2

			IL_0000: ldarg.0
			IL_0001: ldc.i4.0
			IL_0002: conv.u
			IL_0003: ceq
			IL_0005: ret
		} // end of method Unsafe::IsNullRef

		.method public hidebysig static 
			!!T& NullRef<T> () cil managed aggressiveinlining 
		{
			// Method begins at RVA 0x246c
			// Header size: 12
			// Code size: 3 (0x3)
			.maxstack 1

			IL_0000: ldc.i4.0
			IL_0001: conv.u
			IL_0002: ret
		} // end of method Unsafe::NullRef
	}
}