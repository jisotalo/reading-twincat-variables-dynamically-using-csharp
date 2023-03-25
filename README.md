# Reading TwinCAT variables dynamically using C#

This is an example project for blog post at [https://jisotalo.github.io/reading-twincat-variables-dynamically-using-charp/](https://jisotalo.github.io/reading-twincat-variables-dynamically-using-charp/).

Minimal example how to read TwinCAT 3 variables dynamically from C# without providing data type definitions. Uses [Beckhoff.TwinCAT.Ads 6.x](https://www.nuget.org/packages/Beckhoff.TwinCAT.Ads).

## Requirements
You need a local PLC project that has the following data types and variables:
```
TYPE ST_Structure :
STRUCT
	TextValue : STRING(50) := 'some string data';
	RealValue : REAL := 20.23;
	DateValue : DT := DT#2023-3-18-19:33:05;
	SubStruct : ST_SubStructure;
END_STRUCT
END_TYPE
```

```
TYPE ST_SubStructure :
STRUCT
	SubTextValue	: STRING := 'This is substruct';
	SubArrayValue	: ARRAY[0..4] OF INT := [1, 2, 3, 4, 5];
END_STRUCT
END_TYPE
```
An instance of `ST_Structure` is created under GVL `GVL_Test`.
```
//GVL_Test
{attribute 'qualified_only'}
VAR_GLOBAL
	StructValue : ST_Structure;
END_VAR
```
Therefore the instance path is `GVL_Test.StructValue`.

## Running
Make sure you have .NET 7 SDK installed. 

Run the following command:
```
dotnet run
```

Now the server runs at port 9000.

## Endpoints 

The directory `REST requests` includes requests that can be used with Visual Studio Code [REST client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client).

**Reading the struct (GET)**

[http://localhost:9000/](http://localhost:9000/)

**Reading any variable (GET)**

[http://localhost:9000/read-value-by-name?variableName=GVL_CSharpTest.StructValue](http://localhost:9000/read-value-by-name?variableName=GVL_CSharpTest.StructValue)

**Writing string variable (POST)**

[http://localhost:9000/write-string-value](http://localhost:9000/write-string-value) and string value as JSON payload


# License

Licensed under [MIT License](http://www.opensource.org/licenses/MIT).


Copyright (c) Jussi Isotalo <<j.isotalo91@gmail.com>>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.