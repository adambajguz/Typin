# Typin

<p align="center">

[![Build](https://github.com/adambajguz/Typin/workflows/CI/badge.svg?branch=master)](https://github.com/adambajguz/Typin/actions)
[![Coverage](https://codecov.io/gh/adambajguz/Typin/branch/master/graph/badge.svg)](https://codecov.io/gh/adambajguz/Typin)
[![Version](https://img.shields.io/nuget/v/Typin.svg)](https://nuget.org/packages/Typin)
[![Downloads](https://img.shields.io/nuget/dt/Typin.svg)](https://nuget.org/packages/Typin)

</p>

<table>
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

 </tr>
</table>



**Typin** is a simple to use, yet powerful framework for building both interactive command line applications and command line tools. 

> **Etymology:** Typin is made out of "Typ" for "Type" and "in" for "interactively". It's pronounced as "Type in".

It is build based on the code of [CliFx](https://github.com/Tyrrrz/CliFx). Its primary goal is to completely take over the user input layer, letting you forget about the infrastructure and instead focus on writing your application.
This framework uses a declarative class-first approach for defining commands, avoiding excessive boilerplate code and complex configurations.

An important property of Typin, when compared to some other libraries, is that it's not just a parser -- it's a complete application framework.
The main goal of the library is to provide a consistent and enjoyable development experience when building command line applications.
At its core, Typin is highly opinionated, giving preference to convention over configuration, strictness over extensibility, consistency over ambiguity, and so on.

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
  Job-YRJDTM : .NET Core 3.1.6 (CoreCLR 4.700.20.26901, CoreFX 4.700.20.31603), X64 RyuJIT
```

|                               Method |         Mean |      Error |     StdDev | Ratio | RatioSD | Rank |
|------------------------------------- |-------------:|-----------:|-----------:|------:|--------:|-----:|
|                    CommandLineParser |     2.876 us |  0.0631 us |  0.1812 us |  0.04 |    0.00 |    1 |
|                                CliFx |    51.839 us |  1.0130 us |  1.6643 us |  0.70 |    0.04 |    2 |
|                  'Typin - 1 command' |    73.327 us |  1.4587 us |  3.0449 us |  1.00 |    0.00 |    3 |
|                 'Typin - 2 commands' |    94.696 us |  1.8546 us |  2.3455 us |  1.27 |    0.05 |    4 |
|                                Clipr |   132.516 us |  1.4895 us |  1.3933 us |  1.79 |    0.07 |    5 |
| McMaster.Extensions.CommandLineUtils |   134.784 us |  2.6727 us |  2.6250 us |  1.82 |    0.08 |    5 |
|                   System.CommandLine |   190.910 us |  3.8168 us |  4.5436 us |  2.56 |    0.12 |    6 |
|                'Typin - 10 commands' |   270.005 us |  5.3565 us |  5.5007 us |  3.64 |    0.15 |    7 |
|                            PowerArgs |   275.738 us |  3.5869 us |  3.3552 us |  3.72 |    0.17 |    7 |
|                'Typin - 22 commands' |   564.404 us | 11.2229 us | 10.4979 us |  7.62 |    0.30 |    8 |
|                               Cocona | 1,326.940 us | 29.2181 us | 85.6915 us | 18.23 |    1.51 |    9 |
