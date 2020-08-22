# Typin

<p align="center">

[![Build](https://github.com/adambajguz/Typin/workflows/CI/badge.svg?branch=master)](https://github.com/adambajguz/Typin/actions)
[![Coverage](https://codecov.io/gh/adambajguz/Typin/branch/master/graph/badge.svg)](https://codecov.io/gh/adambajguz/Typin)
[![Version](https://img.shields.io/nuget/v/Typin.svg)](https://nuget.org/packages/Typin)
[![Downloads](https://img.shields.io/nuget/dt/Typin.svg)](https://nuget.org/packages/Typin)

</p>

<table border="0">
 <tr>
    <td>
      <p align="center">
        <img src="https://raw.githubusercontent.com/adambajguz/Typin/master/.img/typin-logo-256px.png" width="75%">
      </p>
    </td>
    <td>

<b>Table of contents</b>

- [Typin](#typin)
  - [Features](#features)
  - [Installing Typin](#installing-typin)
  - [Documentation](#documentation)
  - [Screenshots](#screenshots)
  - [Benchmarks](#benchmarks)
  - [Etymology](#etymology)
  
  </td>
 </tr>
</table>



**Typin** is a simple to use, yet powerful framework for building both interactive command line applications and command line tools. 

> **Etymology:** Typin is made out of "Typ" for "Type" and "in" for "interactively". It's pronounced as "Type in".

It is build based on the code of [CliFx](https://github.com/Tyrrrz/CliFx). Its primary goal is to completely take over the user input layer, letting you forget about the infrastructure and instead focus on writing your application.
This framework uses a declarative class-first approach for defining commands, avoiding excessive boilerplate code and complex configurations.

An important property of Typin, when compared to some other libraries, is that it's not just a parser -- it's a complete application framework.
The main goal of the library is to provide a consistent and enjoyable development experience when building command line applications.

## Features

- Complete application framework, not just an argument parser
- Requires minimal amount of code to get started
- Configuration via attributes
- Handles conversions to various types, including custom types
- Supports multi-level command hierarchies
- Supports interactive mode
- Intuitive auto-completion (Tab / Shift + Tab) in interactive mode.
- Intuitive command history (Up and Down arrows) in interactive mode, accessible also from user code.
- Exposes raw input, output, error streams to handle binary data
- Allows graceful command cancellation
- Prints errors and routes exit codes on exceptions
- Provides comprehensive and colorful auto-generated help text
- Highly testable and easy to debug
- Comes with built-in analyzers to help catch common mistakes
- Targets .NET Standard 2.0+
- Uses `Microsoft.Extensions.DependencyInjection` but no other external dependencies

## Installing Typin

You should install [Typin with NuGet](https://www.nuget.org/packages/Typin):

    Install-Package Typin
    
Or via the .NET Core command line interface:

    dotnet add package Typin

Either commands, from Package Manager Console or .NET Core CLI, will download and install Typin and all required dependencies.

## Documentation

See [wiki](https://github.com/adambajguz/Typin/wiki) for detailed instructions and documentation.

## Screenshots

![help screen](.screenshots/help.png)

## Benchmarks

Here's how Typin's execution overhead compares to that of other libraries.

```ini
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18363
Intel Core i7-4790 CPU 3.60GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.302
  [Host]     : .NET Core 3.1.6 (CoreCLR 4.700.20.26901, CoreFX 4.700.20.31603), X64 RyuJIT
  DefaultJob : .NET Core 3.1.6 (CoreCLR 4.700.20.26901, CoreFX 4.700.20.31603), X64 RyuJIT
```

|                               Method |         Mean |      Error |     StdDev |       Median | Ratio | RatioSD | Rank |
|------------------------------------- |-------------:|-----------:|-----------:|-------------:|------:|--------:|-----:|
|                    CommandLineParser |     2.459 us |  0.0416 us |  0.0369 us |     2.455 us |  0.01 |    0.00 |    1 |
|                                CliFx |    52.064 us |  0.6804 us |  0.6032 us |    52.090 us |  0.24 |    0.02 |    2 |
|                                Clipr |   133.519 us |  2.6883 us |  4.5650 us |   133.679 us |  0.64 |    0.09 |    3 |
| McMaster.Extensions.CommandLineUtils |   134.350 us |  1.7527 us |  1.5537 us |   134.076 us |  0.61 |    0.07 |    3 |
|                                Typin |   204.328 us | 11.0929 us | 31.2877 us |   196.450 us |  1.00 |    0.00 |    4 |
|                   System.CommandLine |   205.282 us |  4.1898 us |  5.5932 us |   202.731 us |  0.97 |    0.13 |    4 |
|                            PowerArgs |   265.280 us |  3.1048 us |  2.9042 us |   264.349 us |  1.18 |    0.14 |    5 |
|                               Cocona | 1,282.893 us | 25.5067 us | 74.8066 us | 1,303.211 us |  6.42 |    1.10 |    6 |