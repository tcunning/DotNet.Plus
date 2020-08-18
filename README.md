# DotNet.Plus
 ![license](https://img.shields.io/github/license/martijn00/ExoPlayerXamarin.svg) [![Build Status](https://ci.appveyor.com/api/projects/status/github/tcunning/dotnet.plus?branch=master&svg=true)](https://ci.appveyor.com/project/tcunning/dotnet.plus) [![NuGet](https://img.shields.io/nuget/v/Tod.DotNet.Plus.svg)](https://www.nuget.org/packages/Tod.DotNet.Plus.svg) 

A cross platform .NET Standard Library that has a foundational set of additions and extensional.  See the [API Documentation](http://dotnetplusdocs.todcunningham.com/title--.html) for more detailed information.

### Endianness 

Conversion to and from Big and Little [endian format](https://en.wikipedia.org/wiki/Endianness) is supported.  

```c#
var buffer = new byte[] { 0xFF, 0x01, 0x02, 0x83, 0x04 };
buffer.ToInt32(startOffset: 1, endian: EndianFormat.Big);    // Results in 0x01028304 
buffer.ToInt32(startOffset: 1, endian: EndianFormat.Little); // Results in 0x04830201 
buffer.ToInt16(startOffset: 3, endian: EndianFormat.Big);    // Results in 0x8304 or -31996
buffer.ToInt16(startOffset: 3, endian: EndianFormat.Little); // Results in 0x0483 or 1155

Int32 value = 0x01028304;
buffer.Clear();                                                      // Results in { 0x00, 0x00, 0x00, 0x00, 0x00 }
value.ToBuffer(buffer, startOffset: 1, endian: EndianFormat.Big);    // Results in { 0x00, 0x01, 0x02, 0x83, 0x04 }
value.ToBuffer(buffer, startOffset: 1, endian: EndianFormat.Little); // Results in { 0x00, 0x04, 0x83, 0x02, 0x01 }
```

Support for 16, 32, and 64 bit both sign and unsigned are supported.  

### Fixed Point Numbers

Support for [fixed point](https://andybargh.com/fixed-and-floating-point-binary/) numbers up to 63 bits.  

```c#
FixedPoint.MakeFixedPoint<byte>(14.5f, 4, 4);                                // Results in 0xE8
FixedPoint.MakeFixedPoint<short>(14.5f, wholeBits: 8, fractionalBits: 4);    // Results in 0x00E8
FixedPoint.MakeFixedPoint<short>(-14.5f, wholeBits: 8, fractionalBits: 4);   // Results in 0x0F18 (high nibble not used because only 12 bits)

FixedPoint.MakeDouble<Int32>(0x00FFFFFD, wholeBits: 23, fractionalBits: 1);  // Results in -1.5
FixedPoint.MakeDouble<UInt32>(0x00FFFFFD, wholeBits: 23, fractionalBits: 1); // Results in 8388606.5
```

It can be common to combine the reading/writing of fixed point values to/from a buffer with a particular endianness.

```c#
var buffer = new byte[] { 0xFD, 0xFF };
buffer.ToInt16(endian: EndianFormat.Little).MakeFloat(wholeBits: 15, fractionalBits: 1);          // Results in -1.5f

var value = 100f;
value.MakeFixedPoint<UInt16>(wholeBits: 10, fractionalBits: 6).ToBufferNew(EndianFormat.Little);  // Results in { 0x00, 0x19 }
```

## Installation

DotNet.Plus can be [found here on NuGet]([https://www.nuget.org/packages/Tod.DotNet.Plus/](https://www.nuget.org/packages/Tod.DotNet.Plus/)) and can be installed by copying and pasting the following command into your Package Manager Console within Visual Studio (Tools > NuGet Package Manager > Package Manager Console).

```bash
Install-Package Tod.DotNet.Plus
```

Alternatively if you're using .NET Core then you can install DotNet.Plus via the command line interface with the following command:

```bash
dotnet add package Tod.DotNet.Plus
```

## Contributing
Contributions to DotNet.Plus are welcome.
-   [Open an issue]([https://github.com/tcunning/DotNet.Plus/issues](https://github.com/tcunning/DotNet.Plus/issues))  if you encounter a bug or have a suggestion for improvements/features
-  Be sure to add tests with 100% test coverage if you do submit a pull request.

## Currently maintained by
 - [Tod Cunningham](https://github.com/tcunning)

If you are interested in helping out, jump on [Gitter]([https://gitter.im/DotNet-Plus](https://gitter.im/DotNet-Plus)) and have a chat.

